using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BadgerHTN
{
    [Serializable]
    public abstract class Agent<T> : IAgent
        where T : BaseContext, new()
    {
        public GraphAsset Asset { get; set; }

        [field: SerializeField] public bool AsyncSearch { get; set; }

        public Blackboard Blackboard { get; set; }

        public T Context { get; set; }

        public string Name { get; set; }

        [field: SerializeField] public Plan Plan { get; set; }

        public ulong PlanHandle { get; set; }

        public List<DebugInfo> SearchDebugPath { get; set; }

        [field: SerializeField] public SearchState State { get; set; }

        public delegate void SearchHandler(SearchResult result);

        public Agent()
        {
            Name = "Agent";

            SearchDebugPath = new List<DebugInfo>();
            Blackboard = new Blackboard();
            State = new SearchState();
            Plan = new Plan();

            Context = new T
            {
                Blackboard = Blackboard,
                Agent = this
            };

            Blackboard.AttributeChanged += OnBlackboardAttributeChanged;
            Plan.StateChanged += OnPlanStateChanged;
        }

        public void OnDraw()
        {
            foreach (var planNode in Plan.nodes)
            {
                if (planNode?.node is IDraw<T> d)
                {
                    d.OnDraw(Context);
                }
            }
        }

        public void OnDrawSelected()
        {
            foreach (var planNode in Plan.nodes)
            {
                if (planNode?.node is IDrawSelected<T> d)
                {
                    d.OnDrawSelected(Context);
                }
            }
        }

        public SearchResult Search()
        {
            var result = new SearchResult();
            var root = Asset?.Data?.GetRootNode();
            if (root == null)
            {
                return result;
            }

            var search = new Search(Blackboard, Context);
            result = search.Run(root);

            SearchDebugPath = result.debugPath;
            AgentEvents.SearchComplete?.Invoke(this);

            return result;
        }

        public void SearchAsync()
        {
            var root = Asset?.Data?.GetRootNode();
            if (root == null)
            {
                return;
            }

            var id = ++PlanHandle;
            var syncContext = SynchronizationContext.Current;

            var callback = new SearchHandler(searchResult =>
                syncContext.Post(_ => { OnSearchComplete(searchResult); }, null));

            var search = new Search(Blackboard, Context);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    var result = search.Run(root);
                    result.handle = id;

                    callback.Invoke(result);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }


        /// <summary>
        /// Set the graph asset for the agent.
        /// </summary>
        public void SetAsset(GraphAsset asset)
        {
            Asset = asset;
        }

        public void SetGameObject(GameObject gameObject)
        {
            Context.GameObject = gameObject;
        }

        public void SetName(string agentName)
        {
            if (string.IsNullOrEmpty(agentName))
            {
                return;
            }

            Name = agentName;
        }

        public void SetPlan(SearchResult searchResult)
        {
            if (searchResult == null)
            {
                return;
            }

            var current = Plan.GetCurrent()?.node;
            if (current != null && Plan.GetCurrent().state is BadgerHTN.State.Running)
            {
                current.Interrupt(Context);
            }

            Plan.Set(searchResult);
            State.hasPlanChanged = true;

            OnPlanSet();
        }

        public void Tick()
        {
            UpdateSearch();
            UpdatePlan();
        }

        public override string ToString()
        {
            return $"Agent: {Name}";
        }

        protected virtual void OnPlanSet()
        {
            AgentEvents.PlanSet?.Invoke(this);
        }

        private void OnBlackboardAttributeChanged(string key, object value)
        {
            AgentEvents.BlackboardChanged?.Invoke(this);
            State.isDirty = true;
            State.blackboardChanged = true;
        }

        private void OnPlanStateChanged(PlanNode planNode, State planState)
        {
            switch (planState)
            {
                case BadgerHTN.State.Failed:
                    State.isDirty = true;
                    State.planFailed = true;
                    break;

                case BadgerHTN.State.Success:
                    if (Plan.IsComplete())
                    {
                        State.isDirty = true;
                        State.planDone = true;
                    }

                    break;
            }

            State.hasPlanChanged = true;

            AgentEvents.PlanStateChanged?.Invoke(this, planNode, planState);
        }

        private void OnSearchComplete(SearchResult searchResult)
        {
            if (searchResult.handle != PlanHandle)
            {
                // Ignore stale results
                return;
            }

            State.isSearchingAsync = false;
            SearchDebugPath = searchResult.debugPath;
            AgentEvents.SearchComplete?.Invoke(this);

            if (searchResult.success)
            {
                ReplacePlan(searchResult);
            }
        }

        private void ReplacePlan(SearchResult searchResult)
        {
            bool isComplete = Plan.IsComplete();
            var isPartial = Plan.IsPartial(searchResult.plan);
            if (!isComplete && isPartial)
            {
                // Don't replace plan if new one is partial of the previous one.
                return;
            }

            bool isCurrentPlanValid = Plan.Verify(Blackboard);
            var hasNewPlanLowerMtr = searchResult.methodTraversalRecord.CompareTo(Plan.mtr) < 0;
            if (Plan.HasFailed() || isComplete || !isCurrentPlanValid || hasNewPlanLowerMtr)
            {
                SetPlan(searchResult);
            }
        }

        private void UpdatePlan()
        {
            Plan?.Tick(Context);
        }

        /// <summary>
        /// Searches for a plan if search state is dirty.
        /// </summary>
        private void UpdateSearch()
        {
            if (!State.isDirty || State.isSearchingAsync)
            {
                // No need to search for a plan.
                return;
            }

            // Clear search state.
            State.Clear();

            if (!AsyncSearch)
            {
                // Search for a plan syncronously.
                var searchResult = Search();
                if (searchResult.success)
                {
                    // Replace the current plan if search successful.
                    ReplacePlan(searchResult);
                }
            }
            else
            {
                // Search for a plan on a different thread.
                State.isSearchingAsync = true;
                SearchAsync();
            }
        }
    }
}