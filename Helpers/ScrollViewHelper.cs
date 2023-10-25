using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollViewHelper
{
	public static Vector2 CalculateFocusedScrollPosition(ScrollRect scrollView, Vector2 focusPoint)
	{
		Vector2 contentSize = scrollView.content.rect.size;
		Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
		Vector2 contentScale = scrollView.content.localScale;

		contentSize.Scale(contentScale);
		focusPoint.Scale(contentScale);

		Vector2 scrollPosition = scrollView.normalizedPosition;
		if (scrollView.horizontal && contentSize.x > viewportSize.x)
			scrollPosition.x = Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
		if (scrollView.vertical && contentSize.y > viewportSize.y)
			scrollPosition.y = Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));

		return scrollPosition;
	}

	public static Vector2 CalculateFocusedScrollPosition(ScrollRect scrollView, RectTransform item)
	{
		Vector2 itemCenterPoint = scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));

		Vector2 contentSizeOffset = scrollView.content.rect.size;
		contentSizeOffset.Scale(scrollView.content.pivot);

		return CalculateFocusedScrollPosition(scrollView,itemCenterPoint + contentSizeOffset);
	}

	public static void FocusAtPoint(ScrollRect scrollView, Vector2 focusPoint)
	{
		scrollView.normalizedPosition = CalculateFocusedScrollPosition(scrollView,focusPoint);
	}

	public static void FocusOnItem(ScrollRect scrollView, RectTransform item)
	{
		scrollView.normalizedPosition = CalculateFocusedScrollPosition(scrollView,item);
	}

	private static IEnumerator LerpToScrollPositionCoroutine(ScrollRect scrollView, Vector2 targetNormalizedPos, float speed)
	{
		Vector2 initialNormalizedPos = scrollView.normalizedPosition;

		float t = 0f;
		while (t < 1f)
		{
			scrollView.normalizedPosition = Vector2.LerpUnclamped(initialNormalizedPos, targetNormalizedPos, 1f - (1f - t) * (1f - t));

			yield return null;
			t += speed * Time.unscaledDeltaTime;
		}

		scrollView.normalizedPosition = targetNormalizedPos;
	}

	public static IEnumerator FocusAtPointCoroutine(ScrollRect scrollView, Vector2 focusPoint, float speed)
	{
		yield return LerpToScrollPositionCoroutine(scrollView,CalculateFocusedScrollPosition(scrollView,focusPoint), speed);
	}

	public static IEnumerator FocusOnItemCoroutine(ScrollRect scrollView, RectTransform item, float speed)
	{
		yield return LerpToScrollPositionCoroutine(scrollView,CalculateFocusedScrollPosition(scrollView,item), speed);
	}
}
