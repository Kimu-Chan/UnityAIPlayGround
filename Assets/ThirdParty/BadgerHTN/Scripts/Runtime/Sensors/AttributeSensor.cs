using System.Collections.Generic;
using UnityEngine;

namespace BadgerHTN
{
    public abstract class AttributeSensor<T> : Sensor<T>, IAttributeSensor
        where T : Component
    {
        [SerializeField, HideInInspector] private JsonObject<List<Effect>> effectsOnAdd;
        [SerializeField, HideInInspector] private JsonObject<List<Effect>> effectsOnRemove;
        public List<Effect> EffectsOnAdd => effectsOnAdd.data;

        public List<Effect> EffectsOnRemove => effectsOnRemove.data;

        public void AddEffects()
        {
            foreach (var effect in EffectsOnAdd)
            {
                agentComponent?.Agent.Blackboard.Set(effect.Key, effect.Value);
            }
        }

        public void RemoveEffects()
        {
            foreach (var effect in EffectsOnRemove)
            {
                agentComponent?.Agent.Blackboard.Set(effect.Key, effect.Value);
            }
        }

        public void SetDirty()
        {
            effectsOnAdd.IsDirty = true;
            effectsOnRemove.IsDirty = true;
        }

        protected override void OnComponentsChanged()
        {
            UpdateEffects();
        }

        protected void UpdateEffects()
        {
            var any = Count() > 0;
            if (any)
            {
                AddEffects();
            }
            else
            {
                RemoveEffects();
            }
        }

        private void OnEnable()
        {
            agentComponent ??= GetComponentInParent<IAgentComponent>();
        }

        private void OnValidate()
        {
            agentComponent ??= GetComponentInParent<IAgentComponent>();
        }
    }
}