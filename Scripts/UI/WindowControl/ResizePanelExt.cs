using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.UI;
using Kingmaker.UI.Log;
using O3DWB;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

namespace NWN2QuickItems.UI.WindowControl
{
    public class ResizePanelExt : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected RectTransform _parentRectTransform;

        [SerializeField]
        private GameObject _targetGameObject;

        [SerializeField]
        private Texture2D _editorCursor;

        [SerializeField]
        private PivotModifier.Alignment _pivotAlignment;

        [SerializeField]
        private AnchorModifier.Alignment _anchorAlignment;

        [SerializeField]
        private float _maxHeight = -1f;

        [SerializeField]
        private float _maxWidth = -1f;

        [SerializeField]
        private float _minHeight = -1f;

        [SerializeField]
        private float _minWidth = 100f;

        [SerializeField]
        private float _padding = 0f;

        private bool _isDrag;
        private Vector2 _originalLocalPointerPosition;
        private Vector2 _originalSizeDelta;
        private PivotModifier _pivotModifier;
        private AnchorModifier _anchorModifier;
        private IResizeElement _targetResizePanel;

        private IResizeElement Target
        {
            get
            {
                IResizeElement obj = _targetResizePanel ?? _targetGameObject.GetComponent<IResizeElement>();
                IResizeElement result = obj;
                _targetResizePanel = obj;
                return result;
            }
        }

        public void OnDrag(PointerEventData data)
        {
            _isDrag = true;
            ShowCursor();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Target.GetTransform(), data.position, data.pressEventCamera, out var localPoint);
            var vector = UIUtility.EnforceResizeBounds(_originalSizeDelta + GetOffset(localPoint), _padding, new UIUtility.Bounds(_maxHeight, _minHeight, _maxWidth, _minWidth), _parentRectTransform, Target.GetTransform());
            Target.SetSizeDelta(vector);
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (_isDrag)
            {
                _isDrag = false;
                _pivotModifier.Reset();
                _anchorModifier.Reset();
                HideCursor();
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            _pivotModifier = new PivotModifier(Target.GetTransform());
            _pivotModifier.Set(_pivotAlignment);

            _anchorModifier = new AnchorModifier(Target.GetTransform());
            _anchorModifier.Set(_anchorAlignment);

            _originalSizeDelta = Target.GetSize();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Target.GetTransform(), data.position, data.pressEventCamera, out _originalLocalPointerPosition);
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
#if !UNITY_EDITOR
            if (!CursorController.IsResizeCursor)
            {
                Game.Instance.CursorController.SetCustomCursor(GetCursor(), new Vector2(32f, 32f));
                CursorController.IsResizeCursor = true;
            }
#else
            var hotspot = new Vector2(_editorCursor.width / 2, _editorCursor.height / 2);
            Cursor.SetCursor(_editorCursor, hotspot, CursorMode.Auto);
#endif
        }

        private void HideCursor()
        {
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

        private Vector2 GetOffset(Vector2 localPointerPosition)
        {
            Vector3 vector = localPointerPosition - _originalLocalPointerPosition;
            float x = 0f;
            float y = 0f;
            switch (_pivotAlignment)
            {
                case PivotModifier.Alignment.BottomCenter:
                    y = vector.y;
                    break;
                case PivotModifier.Alignment.BottomLeft:
                    x = vector.x;
                    y = vector.y;
                    break;
                case PivotModifier.Alignment.BottomRight:
                    x = -vector.x;
                    y = vector.y;
                    break;
                case PivotModifier.Alignment.MiddleLeft:
                    x = vector.x;
                    break;
                case PivotModifier.Alignment.MiddleRight:
                    x = -vector.x;
                    break;
                case PivotModifier.Alignment.TopCenter:
                    y = -vector.y;
                    break;
                case PivotModifier.Alignment.TopLeft:
                    x = vector.x;
                    y = 0f - vector.y;
                    break;
                case PivotModifier.Alignment.TopRight:
                    x = -vector.x;
                    y = -vector.y;
                    break;
            }
            return new Vector2(x, y);
        }
    }
}
