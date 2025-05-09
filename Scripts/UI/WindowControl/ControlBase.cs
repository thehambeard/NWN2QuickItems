using NWN2QuickItems.Utility.Helpers;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using UnityEngine.Events;

namespace NWN2QuickItems.UI.WindowControl
{
    public abstract class ControlBase : MonoBehaviour, 
        IEventSystemHandler, 
        IPointerUpHandler,
        IPointerDownHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        protected bool _moveMode;
        protected Vector2 _mouseStartPos;
        protected Vector2 _lastMousePos;
        protected Vector2 _currentScale;
        protected Vector2 _containerStartPos;
        protected float _scaleFactor = 1f;
        private AnchorModifier _anchorModifier;
        private PivotModifier _pivotModifier;
        private bool _isDrag;

        [SerializeField]
        protected RectTransform _ownRectTransform;

        [SerializeField]
        protected RectTransform _parentRectTransform;

        [SerializeField]
        private AnchorModifier.Alignment _anchorAlignment;

        [SerializeField]
        private PivotModifier.Alignment _pivotAlignment;

        [SerializeField]
        [CanBeNull]
        private Texture2D _editorCursor;

        [SerializeField]
        private bool _changeCursor;

        [SerializeField]
        [CanBeNull]
        private UnityEvent<Vector2, Vector2, Vector3> _saveAction;

        public abstract void MoveAction(Vector2 vector);

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _anchorModifier.Set(_anchorAlignment);
                _pivotModifier.Set(_pivotAlignment);
                _moveMode = true;
                _mouseStartPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                _containerStartPos = _ownRectTransform.anchoredPosition;
                _currentScale = _ownRectTransform.localScale;
#if !UNITY_EDITOR
                _scaleFactor = WrathHelpers.GetStaticCanvas().GetComponent<Canvas>().scaleFactor;
#endif
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _moveMode = false;
                _mouseStartPos = default;
                _anchorModifier.Reset();
                _pivotModifier.Reset();
                _saveAction?.Invoke(
                    _ownRectTransform.anchoredPosition, 
                    _ownRectTransform.sizeDelta,
                    _ownRectTransform.localScale);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isDrag)
                HideCursor();
        }

        private void ShowCursor()
        {
            if (!_changeCursor)
                return;

#if !UNITY_EDITOR
            if (!CursorController.IsResizeCursor)
            {
                Game.Instance.CursorController.SetCustomCursor(GetCursor(), new Vector2(32f, 32f));
                CursorController.IsResizeCursor = true;
            }
#else
            var hotspot = new Vector2(_editorCursor.width / 2, _editorCursor.height / 2);

            if (_editorCursor != null)
                Cursor.SetCursor(_editorCursor, hotspot, CursorMode.Auto);
#endif
        }

        private void HideCursor()
        {
            if (!_changeCursor)
                return;

#if !UNITY_EDITOR
            if (CursorController.IsResizeCursor)
            {
                CursorController.IsResizeCursor = false;
                Game.Instance.CursorController.ClearCursor();
                Game.Instance.CursorController.SetCustomCursor(CursorRoot.CursorType.None, Vector2.zero);
            }
#else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
#endif
        }

        private CursorRoot.CursorType GetCursor()
        {
            switch (_pivotAlignment)
            {
                case PivotModifier.Alignment.TopCenter:
                case PivotModifier.Alignment.BottomCenter:
                    return CursorRoot.CursorType.ArrowVerticalCursor;
                case PivotModifier.Alignment.TopRight:
                case PivotModifier.Alignment.BottomLeft:
                    return CursorRoot.CursorType.ArrowDiagonally01Cursor;
                case PivotModifier.Alignment.TopLeft:
                case PivotModifier.Alignment.BottomRight:
                    return CursorRoot.CursorType.ArrowDiagonally02Cursor;
                case PivotModifier.Alignment.MiddleLeft:
                case PivotModifier.Alignment.MiddleRight:
                    return CursorRoot.CursorType.ArrowHorizontalCursor;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Start()
        {
            _anchorModifier = new AnchorModifier(_ownRectTransform);
            _pivotModifier = new PivotModifier(_ownRectTransform);
        }

        public void LateUpdate()
        {
            if (!_moveMode) return;

            Vector2 vector = new Vector2((Input.mousePosition.x - _mouseStartPos.x) / _scaleFactor, (Input.mousePosition.y - _mouseStartPos.y) / _scaleFactor);

            if (_lastMousePos != vector)
                MoveAction(vector);
        }
    }
}
