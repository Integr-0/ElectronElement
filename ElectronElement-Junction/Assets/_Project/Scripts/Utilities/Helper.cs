using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helper
{
    private static Camera _cam;
    /// <summary>
    /// A better version of Camera.main with better performance
    /// </summary>
    public static Camera MainCamera
    {
        get
        {
            if (_cam == null)
                _cam = Camera.main;
            return _cam;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    /// <summary>
    /// A better version of WaitForSeconds with less GarbageCollection
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static WaitForSeconds GetWait(float seconds)
    {
        if (WaitDictionary.TryGetValue(seconds, out var wait))
            return wait;

        WaitDictionary[seconds] = new WaitForSeconds(seconds);
        return WaitDictionary[seconds];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    /// <summary>
    /// Returns true if the mouse is over any type of UI (works for mobile)
    /// </summary>
    /// <returns></returns>
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    /// <summary>
    /// Returns the WorldPosition of any RectTransform
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, MainCamera, out var result);
        return result;
    }
}