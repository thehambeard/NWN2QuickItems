using Kingmaker.UI.MVVM._VM.Utility;
using NWN2QuickItems.UI.Extensions;
using NWN2QuickItems.UI.MVVM.Views.Elements;
using NWN2QuickItems.UI.MVVM.VMs.Elements;
using NWN2QuickItems.UI.MVVM.VMs.Panels;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.VirtualListSystem;
using UnityEngine;
using TMPro;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.PubSubSystem;
using NWN2QuickItems.UI.MVVM.Events;
using Steamworks;
using System.Diagnostics;
using System;
using System.Collections;

namespace NWN2QuickItems.UI.MVVM.Views.Panels
{ 
    internal class SpellPanelPCView : ViewBase<SpellPanelVM>, IInitializable
    {
        [SerializeField]
        private VirtualListVertical _spellVirtualList;

        [SerializeField]
        private ClassHeaderElementPCView _classHeaderPrefab;

        [SerializeField]
        private SpellLevelCollectionElementPCView _spellLevelCollectionPrefab;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TMP_InputField _searchField;

        private float _previousScrollValue;

        public override void BindViewImplementation()
        {
            base.AddDisposable(_spellVirtualList.Subscribe(ViewModel.Elements));
            base.AddDisposable(_searchField.onValueChanged.AsObservable().Subscribe(x => ViewModel.SearchText.Value = x));
            base.AddDisposable(EventBus.Subscribe(this));

            _classHeaderPrefab.gameObject.FixTMPMaterialShader();
            _spellLevelCollectionPrefab.gameObject.FixTMPMaterialShader();
            gameObject.SetActive(true);
        }

        public override void DestroyViewImplementation()
        {
        }

        public void Initialize()
        {
            InitializeVirtualList(_spellVirtualList);
        }

        private void InitializeVirtualList(VirtualListComponent virtualListComponent)
        {
            virtualListComponent.Initialize(new IVirtualListElementTemplate[]
            {
                    new VirtualListElementTemplate<ClassHeaderElementVM>(_classHeaderPrefab),
                    new VirtualListElementTemplate<SpellLevelCollectionElementVM>(_spellLevelCollectionPrefab),
            });
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void ToggleHideShow()
        {
            if (_canvasGroup.alpha == 0f)
                Show();
            else
                Hide();
        }

        public void SetVisible(bool state)
        {
            if (state)
                Show();
            else
                Hide();
        }

        
    }
}
