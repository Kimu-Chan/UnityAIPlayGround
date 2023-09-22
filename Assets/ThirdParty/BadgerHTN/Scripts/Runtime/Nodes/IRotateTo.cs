using UnityEngine;

namespace BadgerHTN
{
    public interface IRotateTo
    {
        void RotateTo(Vector3 target);
        void RotateTo(GameObject target);
        bool IsDone();
    }
}