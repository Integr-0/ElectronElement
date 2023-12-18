using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Destroy all children of a transform
    /// </summary>
    /// <param name="t"></param>
    public static void DestroyChildren(this Transform t)
    {
        while (t.childCount > 0)
            Object.DestroyImmediate(t.GetChild(0).gameObject);
    }

    /// <summary>
    /// Deactivate/Activate all children of a transform
    /// </summary>
    /// <param name="t"></param>
    public static void SetChildrenActive(this Transform t, bool active)
    {
        foreach (Transform child in t)
        {
            child.gameObject.SetActive(active);
        }
    }

    /// <summary>
    /// Set a layer for a GameObject and all the children and all their children and so on
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="layer"></param>
    public static void SetLayersRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetLayersRecursively(layer);
        }
    }

    /// <summary>
    /// Convert a vector3 to a vector2
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(this Vector3 input)
    {
        return new Vector2(input.x, input.y);
    }

    /// <summary>
    /// Convert a vector3 to its flat version (y-value set to 0)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Vector3 Flat(this Vector3 input)
    {
        return new Vector3(input.x, 0, input.z);
    }

    /// <summary>
    /// Convert a vector3 to a vector3Int
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector3Int ToVector3Int(this Vector3 vec3)
    {
        return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
    }

    /// <summary>
    /// returns a Vector with the specified params changed. Does not change the original Vector!
    /// </summary>
    /// <param name="vec3"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Vector3 With(this Vector3 vec3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vec3.x, y ?? vec3.y, z ?? vec3.z);
    }

    /// <summary>
    /// returns the normalized direction from the calling Vector to the target Vector
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 DirectionTo(this Vector3 origin, Vector3 target)
    {
        return (target - origin).normalized;
    }

    /// <summary>
    /// returns the normalized direction from the position of the calling Transfrom to the position of the target Transform
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 DirectionTo(this Transform origin, Transform target)
    {
        return origin.position.DirectionTo(target.position);
    }
}