using Kingmaker.Items;
using Kingmaker.UI.UnitSettings;
using UniRx;

namespace NWN2QuickItems.UI.MVVM.VMs.Elements
{
    class SpellElementVM : SpellElementBaseVM
    {
        public SpellElementVM(ItemEntity spellSlot) : base(spellSlot)
        {
            Icon.Value = ItemSlot.GetIcon();
        }

        public override void OnClick() =>
            base.OnClick();

        public override void OnRightClick() =>
            base.OnRightClick();

        public override void OnHover(bool state) =>
            base.OnHover(state);

        public override void DisposeImplementation() =>
            base.DisposeImplementation();
    }
}
