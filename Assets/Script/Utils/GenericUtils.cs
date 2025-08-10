using UnityEngine;
using MEC;
using System;

public class GenericUtils
{
    
    /// <summary>
    /// Check if 2 rects overlap, converting in world position first
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    public static bool RectOverlaps(RectTransform rect1, RectTransform rect2)
    {

        Rect worldRect1 = GetWorldRect(rect1);
        Rect worldRect2 = GetWorldRect(rect2);

        return worldRect1.Overlaps(worldRect2);
    }
    
    /// <summary>
    /// Starts a timer and perform an action when at zero
    /// </summary>
    public static CoroutineHandle StartTimer(float seconds, Action onComplete)
    {
        return Timing.CallDelayed(seconds, () => onComplete?.Invoke());
    }
    
    
    
    private static Rect GetWorldRect(RectTransform rectTransform)
    {
        var localRect = rectTransform.rect;

        return new Rect
        {
            min = rectTransform.TransformPoint(localRect.min),
            max = rectTransform.TransformPoint(localRect.max)
        };
    }
}
