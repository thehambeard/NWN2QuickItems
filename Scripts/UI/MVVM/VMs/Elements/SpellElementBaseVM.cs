using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.Items;
using Kingmaker.UI.UnitSettings;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Kingmaker.Items.Slots;
using Kingmaker.Blueprints.Items.Equipment;
using HarmonyLib;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.PubSubSystem;
using NWN2QuickItems.UI.MVVM.Events;

namespace NWN2QuickItems.UI.MVVM.VMs.Elements
{
    public abstract class SpellElementBaseVM : VirtualListElementVMBase
    {
        public readonly ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();
        public readonly ReactiveProperty<TooltipBaseTemplate> Tooltip = new ReactiveProperty<TooltipBaseTemplate>();
        public readonly IntReactiveProperty ResourceValue = new IntReactiveProperty();

        public readonly ItemEntity ItemEntity;
        public readonly MechanicActionBarSlotItem ItemSlot;

        public SpellElementBaseVM(ItemEntity itemEntity)
        {
            ItemEntity = itemEntity;

            ItemSlot = new MechanicActionBarSlotItem()
            {
                Item = (ItemEntityUsable)itemEntity
            };

            Tooltip.Value = ItemSlot.GetTooltipTemplate();
            ResourceValue.Value = ItemEntity.Count;

            base.AddDisposable(MainThreadDispatcher
                .UpdateAsObservable()
                .Subscribe(_ => OnUpdateHandler()));
        }

        protected virtual void OnUpdateHandler()
        {
            try
            {
                var resource = ItemSlot.GetResource();

                if (resource != ResourceValue.Value)
                    ResourceValue.Value = resource;
            }
            catch
            {
                Dispose();
            }
        }

        private void Click(bool convert)
        {
            var wielder = Game.Instance.UI.SelectionManager.SelectedUnits.FirstOrDefault<UnitEntityData>();
            wielder.Body.QuickSlots.LastOrDefault().InsertItem(ItemSlot.Item);

            ItemSlot.Unit = wielder;
            ItemSlot.Item = wielder.Body.QuickSlots.LastOrDefault().Item;
            ItemSlot.Item.OnDidEquipped(wielder);

            if (convert && (ItemSlot.Item.Blueprint as BlueprintItemEquipmentUsable).Type == UsableItemType.Potion)
            {
                var ability = ItemSlot.GetConvertedAbilityData()?.m_AbilityData?.FirstOrDefault();

                if (ability == null)
                    return;

                var abilitySlot = new MechanicActionBarSlotAbility()
                {
                    Ability = ability,
                    Unit = wielder
                };

                abilitySlot.OnClick();

                return;
            }

            if (ItemSlot.Item.ActivatableAbility == null)
                ItemSlot.OnClick();
            else
                ItemSlot.Item.ActivatableAbility.Activate();
        }

        public virtual void OnClick() => Click(false);

        public virtual void OnRightClick() => Click(true);

        public virtual void OnHover(bool state)
        {
            if (ItemSlot == null)
                return;

            ItemSlot.OnHover(state);
        }

        public override void DisposeImplementation()
        {
        }
    }
}
