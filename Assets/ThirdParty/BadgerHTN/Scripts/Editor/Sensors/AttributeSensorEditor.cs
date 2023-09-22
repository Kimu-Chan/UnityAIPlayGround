using BadgerEditor.Data;
using BadgerEditor.DependencyInjection;
using BadgerEditor.Ioc;
using BadgerEditor.Views;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BadgerHTN.Editor.Sensors
{
    [CustomEditor(typeof(AttributeSensor<>), true)]
    public class AttributeSensorEditor : UnityEditor.Editor
    {
        private BlackboardProvider _blackboardProvider;
        private Component _component;
        private GraphContainer _container;
        [Inject] private ResourceService _resourceService;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var xml = _resourceService.GetSensorXml();
            xml.CloneTree(root);

            var viewAdd = root.Q<EffectView>("add");
            var viewRemove = root.Q<EffectView>("remove");

            if (target is Component component and IAttributeSensor sensor)
            {
                var agentComponent = component.GetComponentInParent<IAgentComponent>();
                _blackboardProvider.GraphAsset = agentComponent?.Asset;

                _container.Resolve(viewAdd);
                _container.Resolve(viewRemove);

                viewAdd.Source = sensor.EffectsOnAdd;
                viewAdd.Object = component;
                viewAdd.Initialize();
                viewAdd.KeyValueChanged = () => sensor.SetDirty();

                viewRemove.Source = sensor.EffectsOnRemove;
                viewRemove.Object = component;
                viewRemove.Initialize();
                viewRemove.KeyValueChanged = () => sensor.SetDirty();
            }

            return root;
        }

        private void OnEnable()
        {
            _container = new GraphContainer();
            _container.Resolve(this);
            _blackboardProvider = _container.Get<BlackboardProvider>();
        }
    }
}