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
    
    /// <summary>
    /// Returns a formatted time as a string
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <returns></returns>
    public static string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}";
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
    
    /// <summary>
    /// Gives the vertical distance of 2 rect transform
    /// </summary>
    /// <param name="zone"></param>
    /// <param name="pointer"></param>
    /// <returns></returns>
    public static float VerticalDistanceToRect(RectTransform zone, RectTransform pointer)
    {
        Rect zr = GetWorldRect(zone);
        Rect pr = GetWorldRect(pointer);

        float py = pr.center.y;

        if (py < zr.yMin) 
            return py - zr.yMin;

        if (py > zr.yMax) 
            return py - zr.yMax;
        
        return 0f;
    }
}
