using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Spawning
{
    public class EnemyManager : MonoBehaviour
    {
        public GameObject enemyPrefab;

        private readonly int _maxCorpses = 5;
        private List<GameObject> _corpses;
        private List<GameObject> _spawnLocations;

        private void Awake()
        {
            _spawnLocations = FindObjectsOfType<SpawnTag>()
                .Select(t => t.gameObject)
                .ToList();

            _corpses = new List<GameObject>();

            ActorEvents.HealthChanged += OnHealthChanged;
        }

        private void HandleCorpse(GameObject obj)
        {
            _corpses.Add(obj);
            if (_corpses.Count > 0 && _corpses.Count > _maxCorpses)
            {
                var corpse = _corpses[0];
                Destroy(corpse);
                _corpses.RemoveAt(0);
            }
        }

        private void OnDestroy()
        {
            ActorEvents.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(Actor actor, CombatInfo info)
        {
            if (actor.health.IsDead && actor.faction is Faction.Enemy)
            {
                SpawnEnemy();
                HandleCorpse(actor.gameObject);
            }
        }

        private void SpawnEnemy()
        {
            if (_spawnLocations.Count > 0)
            {
                var spawn = _spawnLocations[Random.Range(0, _spawnLocations.Count)];
                if (spawn != null)
                {
                    var zombie = Instantiate(enemyPrefab);
                    zombie.transform.position = spawn.transform.position;
                }
            }
        }
    }
}