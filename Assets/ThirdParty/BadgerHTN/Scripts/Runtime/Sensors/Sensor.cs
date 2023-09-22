using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadgerHTN
{
    /*
        A sensor for detecting gameobjects using a trigger collider.
        Only detects gameobjects that have a component of type T
        attached to it.
    */
    public abstract class Sensor<T> : MonoBehaviour
        where T : Component
    {
        public bool ignoreTriggerColliders = true;
        [SerializeField, TextArea(10, 10)] private string debugText;
        public readonly Dictionary<int, T> components = new Dictionary<int, T>();

        private T _selfComponent;
        public IAgentComponent agentComponent;


        private void OnTriggerEnter(Collider other)
        {
            if (ignoreTriggerColliders && other.isTrigger)
            {
                // Exit if we should ignore trigger colliders 
                return;
            }

            // Ignore our own component if any.
            _selfComponent ??= GetComponentInParent<T>();

            var component = other.GetComponentInParent<T>();
            if (component != null && component != _selfComponent)
            {   
                OnEnter(component);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (ignoreTriggerColliders && other.isTrigger)
            {
                return;
            }

            // Ignore our own component if any.
            _selfComponent ??= GetComponentInParent<T>();

            var component = other.GetComponentInParent<T>();
            if (component != null && component != _selfComponent)
            {
                OnExit(component);
            }
        }

        public int Count()
        {
            return components.Count;
        }

        /// <summary>
        /// Callback for when a gameobject with component
        /// T entered our trigger collider.
        /// </summary>
        private void OnEnter(T component)
        {
            var id = component.GetInstanceID();
            if (components.TryAdd(id, component))
            {
                OnAdd(component);
                OnComponentsChanged();
            }
        }

        protected virtual void OnAdd(T component)
        {
        }

        private void OnExit(T component)
        {
            var id = component.GetInstanceID();
            if (components.TryGetValue(id, out var t))
            {
                components.Remove(id);
                OnRemove(component);
                OnComponentsChanged();
            }
        }

        protected virtual void OnRemove(T component)
        {
        }

        protected virtual void OnComponentsChanged()
        {
            UpdateDebugText();
        }

        // Updates the debug text in editor
        // Prints every detected object.
        protected void UpdateDebugText()
        {
#if UNITY_EDITOR
            var names = components.Values
                .Select(t => t.gameObject.name)
                .OrderBy(t => t)
                .ToList();

            debugText = string.Join("\n", names);
#endif
        }
    }
}