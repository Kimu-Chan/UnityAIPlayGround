using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Covers
{
    public class CoverManager : MonoBehaviour
    {
        private List<CoverTag> _covers;
        public static CoverManager Instance { get; set; }

        public List<CoverTag> GetAvailableCovers(IAgent agent)
        {
            var validCovers = _covers
                .Where(coverSpot => coverSpot != null
                                    && coverSpot.valid
                                    && (coverSpot.agent == agent
                                        || coverSpot.agent == null))
                .ToList();

            return validCovers;
        }

        public bool HasAvailableCover(IAgent agent)
        {
            var covers = GetAvailableCovers(agent);
            return covers.Any();
        }

        public CoverTag Reserve(IAgent agent, Transform agentTransform)
        {
            var agentPosition = agentTransform.position;

            var cover = GetAvailableCovers(agent)
                .OrderBy(coverSpot => Vector3.Distance(agentPosition, coverSpot.transform.position))
                .FirstOrDefault();

            if (cover != null)
            {
                cover.agent = agent;
            }

            return cover;
        }

        private void Awake()
        {
            _covers = FindObjectsOfType<CoverTag>().ToList();
            Instance = this;
        }
    }
}