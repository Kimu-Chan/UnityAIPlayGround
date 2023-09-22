using System.Linq;
using BadgerHTN.Examples.Zombies.Scripts.Guns;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    public class DeathAnimation : MonoBehaviour
    {
        public bool isEnabled = true;
        public GameObject normal;
        public GameObject fractured;
        public GameObject mesh;
        private Actor _actor;
        private float _average;
        private MeshCollider[] _colliders;
        private float _delayCheckForSleep = 0.5f;

        private bool _isActive;
        private Rigidbody[] _rigidbodies;

        private float _threshold = 0.05f;
        private float _timestamp;

        private void Awake()
        {
            _actor = GetComponent<Actor>();
            if (_actor)
            {
                ActorEvents.HealthChanged += OnHealthChanged;
            }
        }

        private void Update()
        {
            if (!isEnabled)
            {
                return;
            }

            CheckForInactive();
        }

        private void OnDestroy()
        {
            ActorEvents.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(Actor actor, CombatInfo info)
        {
            if (actor != _actor)
            {
                return;
            }

            if (actor.health.hitPoints <= 0)
            {
                DisableComponents();
                Activate();
                ApplyRagdoll(info.physicsInfo);
            }
        }

        // public void OnHealthChange(CombatInfo info)
        // {
        //     if (info.health.hitPoints <= 0)
        //     {
        //         DisableComponents();
        //
        //         Activate();
        //         ApplyRagdoll(info.physicsInfo);
        //     }
        // }

        private void ApplyRagdoll(PhysicsInfo info)
        {
            if (!isEnabled)
            {
                return;
            }

            if (info.force <= 0)
            {
                return;
            }

            foreach (var rb in _rigidbodies)
            {
                rb.AddExplosionForce(info.force, info.point, 1f, 0f);
            }
        }

        private void DisableComponents()
        {
            foreach (var component in GetComponentsInChildren<Behaviour>())
            {
                if (component == null || component == this)
                {
                    continue;
                }

                component.enabled = false;
            }

            foreach (var component in GetComponentsInChildren<CharacterController>())
            {
                component.enabled = false;
            }
        }

        public void Activate()
        {
            if (!isEnabled)
            {
                gameObject.SetActive(false);
                return;
            }

            _rigidbodies = mesh?.GetComponentsInChildren<Rigidbody>(true);
            _colliders = mesh?.GetComponentsInChildren<MeshCollider>(true);

            if (normal != null)
            {
                normal?.SetActive(false);
            }

            foreach (var gun in GetComponentsInChildren<GunComponent>())
            {
                gun?.Ragdoll();
            }

            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = false;
            }

            foreach (var c in fractured?.GetComponentsInChildren<MeshCollider>(true))
            {
                c.enabled = true;
            }

            fractured?.SetActive(true);

            _timestamp = Time.time;
            _isActive = true;
        }

        private void CheckForInactive()
        {
            if (!_isActive)
            {
                return;
            }

            if (Time.time > _timestamp + _delayCheckForSleep)
            {
                var size = _rigidbodies.Length != 0 ? _rigidbodies.Length : 1;
                _average = _rigidbodies.Sum(t => t.velocity.magnitude) / size;
                if (_average < _threshold)
                {
                    Sleep();
                }
            }
        }

        private void Sleep()
        {
            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = true;
            }

            foreach (var c in _colliders)
            {
                c.enabled = false;
                c.gameObject.isStatic = true;
            }

            _isActive = false;
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(DeathAnimation))]
    public class DeathAnimationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var obj = (DeathAnimation)target;

            if (GUILayout.Button("Activate"))
            {
                if (Application.isPlaying)
                {
                    obj.Activate();
                }
            }
        }
    }

#endif
}