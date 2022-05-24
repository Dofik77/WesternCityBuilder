using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.UiExtensions
{
    public static partial class UiExtensions
    {
        // Shared array used to receive result of RectTransform.GetWorldCorners
        private static readonly Vector3[] Corners = new Vector3[4];

        /// <summary>
        /// Transform the bounds of the current rect transform to the space of another transform.
        /// </summary>
        /// <param name="source">The rect to transform</param>
        /// <param name="target">The target space to transform to</param>
        /// <returns>The transformed bounds</returns>
        public static Bounds TransformBoundsTo(this RectTransform source, Transform target)
        {
            // Based on code in ScrollRect's internal GetBounds and InternalGetBounds methods
            var bounds = new Bounds();
            if (source == null) return bounds;

            source.GetWorldCorners(Corners);

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var matrix = target.worldToLocalMatrix;
            for (var j = 0; j < 4; j++)
            {
                Vector3 v = matrix.MultiplyPoint3x4(Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        /// <summary>
        /// Normalize a distance to be used in verticalNormalizedPosition or horizontalNormalizedPosition.
        /// </summary>
        /// <param name="scrollRect">Scroll rect to scroll</param>
        /// <param name="axis">Scroll axis, 0 = horizontal, 1 = vertical</param>
        /// <param name="distance">The distance in the scroll rect's view's coordiante space</param>
        /// <returns>The normalized scoll distance</returns>
        public static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
        {
            // Based on code in ScrollRect's internal SetNormalizedPosition method
            var viewport = scrollRect.viewport;
            var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
            var rect = viewRect.rect;
            var viewBounds = new Bounds(rect.center, rect.size);

            var content = scrollRect.content;
            var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

            var hiddenLength = contentBounds.size[axis] - viewBounds.size[axis];
            return distance / hiddenLength;
        }

        /// <summary>
        /// Scroll the target element to the vertical center of the scroll rect's viewport.
        /// Assumes the target element is part of the scroll rect's contents.
        /// </summary>
        /// <param name="scrollRect">Scroll rect to scroll</param>
        /// <param name="target">Element of the scroll rect's content to center vertically</param>
        /// <param name="duration">animation duration</param>
        /// <param name="axis">Scroll direction</param>
        public static void ScrollToCenter(this ScrollRect scrollRect, RectTransform target, float duration = 0,
            RectTransform.Axis axis = RectTransform.Axis.Vertical)
        {
            // The scroll rect's view's space is used to calculate scroll position
            var view = scrollRect.viewport ?? scrollRect.GetComponent<RectTransform>();

            // Calculate the scroll offset in the view's space
            var viewRect = view.rect;
            var elementBounds = target.TransformBoundsTo(view);

            // Normalize and apply the calculated offset
            if (axis == RectTransform.Axis.Vertical)
            {
                var offset = viewRect.center.y - elementBounds.center.y;
                var scrollPos = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, offset);
                scrollPos = Mathf.Clamp01(scrollPos);
                scrollRect.DOVerticalNormalizedPos(scrollPos, duration);
            }
            else
            {
                var offset = viewRect.center.x - elementBounds.center.x;
                var scrollPos = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(0, offset);
                scrollPos = Mathf.Clamp01(scrollPos);
                scrollRect.DOHorizontalNormalizedPos(scrollPos, duration);
            }
        }
    }
}