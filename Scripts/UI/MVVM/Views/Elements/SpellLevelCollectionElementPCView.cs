using NWN2QuickItems.UI.MVVM.VMs.Elements;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using Owlcat.Runtime.UniRx;
using System.Linq;


namespace NWN2QuickItems.UI.MVVM.Views.Elements
{
    class SpellLevelCollectionElementPCView : ElementBasePCView<SpellLevelCollectionElementVM>
    {
        [SerializeField]
        private TextMeshProUGUI _levelText;

        [SerializeField]
        private SpellElementPCView _spellElementPrefab;

        [SerializeField]
        private RectTransform _spellContainerTransform;

        [SerializeField]
        private int _spellContainerMaxWidth = 400;

        [SerializeField]
        private int _spellElementSize = 40;

        [SerializeField]
        private int _spacing = 3;

        private RectTransform _rectTransform;
        private readonly List<SpellElementPCView> _spellElements = new List<SpellElementPCView>();

        public override void BindViewImplementation()
        {
            _rectTransform = (RectTransform)transform;

            base.AddDisposable(ViewModel.Level.Subscribe(x => _levelText.text = x.ToString()));
            base.AddDisposable(ViewModel.SpellElements.ObserveAnyCollectionChange().Subscribe(x =>
            {
                FillElements();
                ArrangeElements();
            }));
            base.AddDisposable(Observable.EveryLateUpdate().Subscribe(x =>
            {
                if (_spellElements.Count > 0 && !_spellElements.FirstOrDefault().IsBinded)
                {
                    FillElements();
                    ArrangeElements();
                }
            }));
        }

        public override void DestroyViewImplementation()
        {
        }

        

        public void FillElements()
        {
            int index;
            for (index = 0; index < ViewModel.SpellElements.Count; index++)
            {
                if (index < _spellElements.Count)
                    _spellElements[index].Bind(ViewModel.SpellElements[index]);
                else
                {
                    var go = GameObject.Instantiate(_spellElementPrefab, _spellContainerTransform, false);
                    go.Bind(ViewModel.SpellElements[index]);
                    _spellElements.Add(go);
                }
            }

            for (int x = _spellElements.Count - 1; x >= index; x--)
            {
                _spellElements[x].DestroyView();
                GameObject.DestroyImmediate(_spellElements[x].gameObject);
                _spellElements.RemoveAt(x);
            }

        }

        public void ArrangeElements()
        {
            int currentX = 0;
            int currentY = 0;
            int rowHeight = _spellElementSize + _spacing;

            foreach (Transform child in _spellContainerTransform)
            {
                var childRect = child as RectTransform;

                if (currentX + _spellElementSize > _spellContainerMaxWidth)
                {
                    currentX = 0;
                    currentY -= rowHeight;
                }

                childRect.anchoredPosition = new Vector2(currentX, currentY);
                currentX += _spellElementSize + _spacing;
            }

            int totalHeight = Mathf.Abs(currentY) + rowHeight;
            _layoutSettings.Height = totalHeight;
            _spellContainerTransform.sizeDelta = new Vector2(_spellContainerTransform.sizeDelta.x, totalHeight);
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, totalHeight);
        }
    }
}
