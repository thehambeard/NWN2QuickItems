using System.Collections.Generic;
using UniRx;

namespace NWN2QuickItems.UI.MVVM.VMs.Elements
{
    class SpellLevelCollectionElementVM : ElementBaseVM
    {
        
        public readonly IntReactiveProperty Level = new IntReactiveProperty();
        public readonly ReactiveCollection<SpellElementVM> SpellElements = new ReactiveCollection<SpellElementVM>();
        
        public SpellLevelCollectionElementVM(int level, List<SpellElementVM> spellElementVMs = null)
        {
            Level.Value = level;

            if (spellElementVMs != null)
                SpellElements = new ReactiveCollection<SpellElementVM>(spellElementVMs);
        }

        public override void DisposeImplementation()
        {
        }

        
    }
}
