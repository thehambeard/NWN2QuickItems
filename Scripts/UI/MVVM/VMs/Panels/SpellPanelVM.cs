using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker;
using NWN2QuickItems.UI.MVVM.VMs.Elements;
using Owlcat.Runtime.UI.MVVM;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.PubSubSystem;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Owlcat.Runtime.Core;
using Owlcat.Runtime.UI.Utility;
using Kingmaker.Blueprints.Items.Equipment;
using System;

namespace NWN2QuickItems.UI.MVVM.VMs.Panels
{
    public class SpellPanelVM : BaseDisposable,
        IViewModel,
        ISelectionHandler,
        IItemsCollectionHandler
    {
        public readonly ReactiveCollection<VirtualListElementVMBase> Elements = new ReactiveCollection<VirtualListElementVMBase>();
        public readonly ReactiveProperty<UnitEntityData> SelectedUnit = new ReactiveProperty<UnitEntityData>();
        public readonly StringReactiveProperty SearchText = new StringReactiveProperty("");
        
        public UnitEntityData SelectedUnitValue => SelectedUnit.Value;

        private bool _needUpdateSelection;
        private bool _needsReset;
        private List<ItemEntity> _items = new List<ItemEntity>();
        private readonly Dictionary<string, bool> _headersExpanded = new Dictionary<string, bool>();
        
        public SpellPanelVM()
        {
            base.AddDisposable(MainThreadDispatcher.UpdateAsObservable().Subscribe(_ => OnUpdateHandler()));
            base.AddDisposable(EventBus.Subscribe(this));
            base.AddDisposable(SearchText.Subscribe(x => _needsReset = true));

            BuildElements();
            base.AddDisposable(Game.Instance.SelectionCharacter.SelectionCharacterUpdated.Subscribe(_ =>
                this._needUpdateSelection = Game.Instance.SelectionCharacter.SelectedUnit?.Value.Value != SelectedUnitValue));
            base.AddDisposable(Game.Instance.SelectionCharacter.SelectedUnit.Subscribe(x =>
                this._needUpdateSelection = x != SelectedUnitValue));

            _needsReset = true;
        }

        public override void DisposeImplementation()
        {
            DisposeElements();
            _items = null;
        }

        private void OnUnitChanged(UnitEntityData unit)
        {
        }

        private void DisposeElements()
        {
            Elements.ForEach(x => x.Dispose());
            Elements.Clear();
        }

        private void BuildElements()
        {
            var _root = new ClassHeaderElementVM("root");
            DisposeElements();

            foreach (var itemEntity in _items
                .GroupBy(x => (x.Blueprint as BlueprintItemEquipmentUsable).Type))
            {
                var itemCollection = new List<ElementBaseVM>();

                foreach (var itemByLevel in itemEntity
                    .GroupBy(x => x.Blueprint.GetSpellLevel())
                    .OrderBy(x => x.Key))
                {
                    var itemList = new List<SpellElementVM>();

                    foreach (var item in itemByLevel)
                    {
                        itemList.Add(new SpellElementVM(item));
                    }

                    if (itemList.Count > 0)
                        itemCollection.Add(new SpellLevelCollectionElementVM((int)itemByLevel.Key, itemList));
                }

                var headerText = Enum.GetName(typeof(UsableItemType), itemEntity.Key);

                if (!_headersExpanded.ContainsKey(headerText))
                    _headersExpanded.Add(headerText, false);

                var header = new ClassHeaderElementVM(headerText, _headersExpanded[headerText], itemCollection, (isExpanded) =>
                {
                    if (_headersExpanded.ContainsKey(headerText))
                        _headersExpanded[headerText] = isExpanded;
                });

                _root.AddChild(header);
            }
            _root.Flatten(false).ForEach(x => Elements.Add(x));
        }

        private void OnUpdateHandler()
        {
            if (!LoadingProcess.Instance.IsLoadingInProcess)
            {
                if (_needUpdateSelection)
                {
                    UpdateSelection();
                    _needUpdateSelection = false;
                    _needsReset = true;
                }
                if ((_needsReset || SelectedUnitValue != null && SelectedUnitValue.UISettings.Dirty)
                    && (Game.Instance.CurrentMode == GameModeType.Default || Game.Instance.CurrentMode == GameModeType.Pause)
                    && SelectedUnitValue != null
                    && SelectedUnitValue.IsInGame)
                {
                    OnUnitChanged(SelectedUnitValue);
                    CollectItems();
                    BuildElements();
                    _needsReset = false;
                }
                else if (_needsReset)
                {
                    DisposeElements();
                }
            }
        }

        private void CollectItems()
        {
            _items = Game.Instance.Player.Inventory
                .Where(item => item.Blueprint.Name.IndexOf(SearchText.Value, StringComparison.OrdinalIgnoreCase) >= 0)
                .Where(item => item.Blueprint is BlueprintItemEquipmentUsable usable)
                .ToList();
        }

        private void UpdateSelection() => SelectedUnit.Value =
            Game.Instance.SelectionCharacter.IsSingleSelected
            && Game.Instance.SelectionCharacter.CurrentSelectedCharacter.IsDirectlyControllable
                ? Game.Instance.SelectionCharacter.CurrentSelectedCharacter
                : null;

        void ISelectionHandler.OnUnitSelectionAdd(UnitEntityData selected) => _needUpdateSelection = selected != SelectedUnitValue;
        void ISelectionHandler.OnUnitSelectionRemove(UnitEntityData selected) => _needUpdateSelection = true;

        void IItemsCollectionHandler.HandleItemsAdded(ItemsCollection collection, ItemEntity item, int count)
        {
            _needsReset = true;
        }

        void IItemsCollectionHandler.HandleItemsRemoved(ItemsCollection collection, ItemEntity item, int count)
        {
            _needsReset = true;
        }
    }
}
