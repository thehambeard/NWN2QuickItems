using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NWN2QuickItems.UI
{
    public static class UIUtility
    {
        public class Bounds
        {
            public float MaxHeight;
            public float MinHeight;
            public float MaxWidth;
            public float MinWidth;

            public Bounds(float maxHeight, float minHeight, float maxWidth, float minWidth)
            {
                MaxHeight = maxHeight;
                MinHeight = minHeight;
                MaxWidth = maxWidth;
                MinWidth = minWidth;
            }
        }

        public static Vector2 LimitPositionRectInRect(Vector2 nPos, RectTransform parent, RectTransform child)
        {
            float width = parent.rect.width;
            float height = parent.rect.height;
            float width2 = child.rect.width;
            float height2 = child.rect.height;
            Vector2 scale = child.localScale;
            Vector2 pivot = child.pivot;

            if (nPos.x + width / 2f - pivot.x * width2 * scale.x <= 0f)
            {
                nPos.x = -width / 2f + pivot.x * width2 * scale.x;
            }
            else if (nPos.x + width / 2f + (1f - pivot.x) * width2 * scale.x >= width)
            {
                nPos.x = width / 2f - (1f - pivot.x) * width2 * scale.x;
            }
            if (nPos.y + height / 2f - pivot.y * height2 * scale.y <= 0f)
            {
                nPos.y = -height / 2f + pivot.y * height2 * scale.y;
            }
            else if (nPos.y + height / 2f + (1f - pivot.y) * height2 * scale.y >= height)
            {
                nPos.y = height / 2f - (1f - pivot.y) * height2 * scale.y;
            }
            return nPos;
        }

        public static Vector3 LimitScaleRectInRect(Vector3 newScale, RectTransform parent, RectTransform child)
        {
            float parentWidth = parent.rect.width;
            float parentHeight = parent.rect.height;
            float childWidth = child.rect.width;
            float childHeight = child.rect.height;
            Vector2 childPosition = child.localPosition;
            Vector2 pivot = child.pivot;

            float scaledWidth = childWidth * newScale.x;
            float scaledHeight = childHeight * newScale.y;

            if (childPosition.x + parentWidth / 2f - pivot.x * scaledWidth < 0f)
            {
                newScale.x = (childPosition.x + parentWidth / 2f) / (pivot.x * childWidth + .001f);
            }
            else if (childPosition.x + parentWidth / 2f + (1f - pivot.x) * scaledWidth > parentWidth)
            {
                newScale.x = (parentWidth - childPosition.x - parentWidth / 2f) / ((1f - pivot.x) * childWidth + .001f);
            }

            if (childPosition.y + parentHeight / 2f - pivot.y * scaledHeight < 0f)
            {
                newScale.y = (childPosition.y + parentHeight / 2f) / (pivot.y * childHeight + .001f);
            }
            else if (childPosition.y + parentHeight / 2f + (1f - pivot.y) * scaledHeight > parentHeight)
            {
                newScale.y = (parentHeight - childPosition.y - parentHeight / 2f) / ((1f - pivot.y) * childHeight + .001f);
            }

            if (newScale.x == float.NaN || newScale.y == float.NaN || newScale.x == 0f || newScale.y == 0f)
                Main.Logger.Debug("NAN in LimitScaleRectInRect");

            return newScale.x < newScale.y && (newScale.y != float.NaN || newScale.y != 0f) && (newScale.x != float.NaN || newScale.x != 0f)
                ? new Vector3(newScale.x, newScale.x, newScale.x) 
                : new Vector3(newScale.y, newScale.y, newScale.y);
        }

        public static bool AreRectTransformsEdgeToEdge(RectTransform rect1, RectTransform rect2, float tolerance = 7f)
        {
            Vector3[] corners1 = new Vector3[4];
            Vector3[] corners2 = new Vector3[4];

            rect1.GetWorldCorners(corners1);
            rect2.GetWorldCorners(corners2);

            bool edgeToEdgeHorizontal =
                Mathf.Abs(corners1[2].x - corners2[0].x) <= tolerance || 
                Mathf.Abs(corners1[0].x - corners2[2].x) <= tolerance;  

            bool edgeToEdgeVertical =
                Mathf.Abs(corners1[1].y - corners2[3].y) <= tolerance || 
                Mathf.Abs(corners1[3].y - corners2[1].y) <= tolerance;  

            return edgeToEdgeHorizontal || edgeToEdgeVertical;
        }


        public static Vector2 LimitSizeDeltaRectInRect(Vector2 nSizeDelta, float padding, RectTransform parent, RectTransform child)
        {
            float parentWidth = parent.rect.width;
            float parentHeight = parent.rect.height;
            float childWidth = child.rect.width;
            float childHeight = child.rect.height;
            Vector2 childPosition = child.localPosition;
            Vector2 pivot = child.pivot;

            float scaledWidth = childWidth * child.localScale.x;
            float scaledHeight = childHeight * child.localScale.y;

            float offset;

            if ((offset = childPosition.x + parentWidth / 2f - pivot.x * scaledWidth - padding) <= 0f && nSizeDelta.x > child.sizeDelta.x)
            {
                nSizeDelta.x = child.sizeDelta.x + offset;
            }
            else if ((offset = childPosition.x + parentWidth / 2f + (1f - pivot.x) * scaledWidth + padding) > parentWidth && nSizeDelta.x > child.sizeDelta.x)
            {
                nSizeDelta.x = child.sizeDelta.x + parentWidth - offset;
            }

            if ((offset = childPosition.y + parentHeight / 2f - pivot.y * scaledHeight - padding) <= 0f && nSizeDelta.y > child.sizeDelta.y)
            {
                nSizeDelta.y = child.sizeDelta.y + offset;
            }
            else if ((offset = childPosition.y + parentHeight / 2f + (1f - pivot.y) * scaledHeight + padding) > parentHeight && nSizeDelta.y > child.sizeDelta.y)
            {
                nSizeDelta.y = child.sizeDelta.y + parentHeight - offset;
            }

            return nSizeDelta;
        }


        public static Vector2 EnforceResizeBounds(Vector2 inputChange, float padding, Bounds bounds, RectTransform parent, RectTransform child)
        {
            if (bounds.MinWidth != -1f)
                inputChange.x = Mathf.Max(inputChange.x, bounds.MinWidth);

            if (bounds.MaxWidth != -1f)
                inputChange.x = Mathf.Min(inputChange.x, bounds.MaxWidth);

            if (bounds.MinHeight != -1f)
                inputChange.y = Mathf.Max(inputChange.y, bounds.MinHeight);

            if (bounds.MaxHeight != -1f)
                inputChange.y = Mathf.Min(inputChange.y, bounds.MaxHeight);

            return LimitSizeDeltaRectInRect(inputChange, padding, parent, child);
        }


        public static Vector3 MapValueVector(float a0, float a1, float b0, float b1, float a)
        {
            float v = b0 + (b1 - b0) * ((a - a0) / (a1 - a0 +.01f));

            if (v == float.NaN || v == 0f)
                Main.Logger.Debug("NaN in MapValueVector");
            return new Vector3(v, v, v);
        }
    }
}
