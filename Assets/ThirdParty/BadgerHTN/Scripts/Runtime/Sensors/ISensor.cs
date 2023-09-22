using System.Collections.Generic;

namespace BadgerHTN
{
    public interface IAttributeSensor
    {
        List<Effect> EffectsOnAdd { get; }
        List<Effect> EffectsOnRemove { get; }

        void SetDirty();
    }
}