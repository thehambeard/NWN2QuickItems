using UnityEngine;
using UnityEngine.EventSystems;


namespace NWN2QuickItems.UI
{
    public class PivotModifier
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

        private readonly RectTransform _rectTransform;

        private Vector2 _originalPivot;

        public PivotModifier(RectTransform transform)
        {
            _rectTransform = transform;
            _originalPivot = transform.pivot;
        }

        private void Set(Vector2 pivot)
        {
            SetPivotWithoutMoving(pivot);
        }

        public void Set(Alignment alignment)
        {
            CurrentAlignment = alignment;

            switch (alignment)
            {
                case Alignment.TopLeft:
                    Set(new Vector2(LEFT, TOP));
                    break;

                case Alignment.TopCenter:
                    Set(new Vector2(MIDDLE, TOP));
                    break;

                case Alignment.TopRight:
                    Set(new Vector2(RIGHT, TOP));
                    break;

                case Alignment.MiddleLeft:
                    Set(new Vector2(LEFT, MIDDLE));
                    break;

                case Alignment.MiddleCenter:
                    Set(new Vector2(MIDDLE, MIDDLE));
                    break;

                case Alignment.MiddleRight:
                    Set(new Vector2(RIGHT, MIDDLE));
                    break;

                case Alignment.BottomLeft:
                    Set(new Vector2(LEFT, BOTTOM));
                    break;

                case Alignment.BottomCenter:
                    Set(new Vector2(MIDDLE, BOTTOM));
                    break;

                case Alignment.BottomRight:
                    Set(new Vector2(RIGHT, BOTTOM));
                    break;

            }
        }

        public void Reset()
        {
            SetPivotWithoutMoving(_originalPivot);
        }

        private void SetPivotWithoutMoving(Vector2 pivot)
        {
            Vector2 pivotDelta = pivot - _rectTransform.pivot;

            Vector2 pivotOffset = new Vector2(
                -pivotDelta.x * _rectTransform.rect.width * _rectTransform.localScale.x,
                -pivotDelta.y * _rectTransform.rect.height * _rectTransform.localScale.y);

            _rectTransform.anchoredPosition -= pivotOffset;

            _rectTransform.pivot = pivot;
        }
    }
}