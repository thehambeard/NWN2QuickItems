using UnityEngine;
using UnityEngine.EventSystems;


namespace NWN2QuickItems.UI
{
    public class AnchorModifier
    {
        public enum Alignment
        {
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        private const float TOP = 1f;
        private const float MIDDLE = .5f;
        private const float BOTTOM = 0f;
        private const float RIGHT = 1f;
        private const float LEFT = 0f;

        public Alignment CurrentAlignment { get; private set; }
        public Vector2 AnchorMin { get; private set; }
        public Vector2 AnchorMax { get; private set; }
        private readonly RectTransform _rectTransform;

        private Vector2 _originalAnchorMin;
        private Vector2 _originalAnchorMax;

        public AnchorModifier(RectTransform transform)
        {
            _rectTransform = transform;
            _originalAnchorMin = transform.anchorMin;
            _originalAnchorMax = transform.anchorMax;
        }

        private void Set(Vector2 anchorMin, Vector2 anchorMax)
        {
            AnchorMin = anchorMin;
            AnchorMax = anchorMax;
            SetAnchorWithoutMoving(anchorMin, anchorMax);
        }

        public void Set(Alignment alignment)
        {
            CurrentAlignment = alignment;

            switch (alignment)
            {
                case Alignment.TopLeft:
                    Set(new Vector2(LEFT, TOP), new Vector2(LEFT, TOP));
                    break;

                case Alignment.TopCenter:
                    Set(new Vector2(MIDDLE, TOP), new Vector2(MIDDLE, TOP));
                    break;

                case Alignment.TopRight:
                    Set(new Vector2(RIGHT, TOP), new Vector2(RIGHT, TOP));
                    break;

                case Alignment.MiddleLeft:
                    Set(new Vector2(LEFT, MIDDLE), new Vector2(LEFT, MIDDLE));
                    break;

                case Alignment.MiddleCenter:
                    Set(new Vector2(MIDDLE, MIDDLE), new Vector2(MIDDLE, MIDDLE));
                    break;

                case Alignment.MiddleRight:
                    Set(new Vector2(RIGHT, MIDDLE), new Vector2(RIGHT, MIDDLE));
                    break;

                case Alignment.BottomLeft:
                    Set(new Vector2(LEFT, BOTTOM), new Vector2(LEFT, BOTTOM));
                    break;

                case Alignment.BottomCenter:
                    Set(new Vector2(MIDDLE, BOTTOM), new Vector2(MIDDLE, BOTTOM));
                    break;

                case Alignment.BottomRight:
                    Set(new Vector2(RIGHT, BOTTOM), new Vector2(RIGHT, BOTTOM));
                    break;

            }
        }

        public void Reset()
        {
            AnchorMin = _originalAnchorMin;
            AnchorMax = _originalAnchorMax;
            SetAnchorWithoutMoving(_originalAnchorMin, _originalAnchorMax);
        }

        private void SetAnchorWithoutMoving(Vector2 anchorMin, Vector2 anchorMax)
        {
            var OriginalPosition = _rectTransform.localPosition;
            var OriginalSize = _rectTransform.sizeDelta;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, OriginalSize.x);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, OriginalSize.y);
            _rectTransform.localPosition = OriginalPosition;
        }
    }
}