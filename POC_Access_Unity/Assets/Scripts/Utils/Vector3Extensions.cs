using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 WhereX(this Vector3 v3, float xValue)
    {
        v3.x = xValue;
        return v3;
    }
    public static Vector3 WhereY(this Vector3 v3, float yValue)
    {
        v3.y = yValue;
        return v3;
    }
    public static Vector3 WhereZ(this Vector3 v3, float zValue)
    {
        v3.z = zValue;
        return v3;
    }
}
