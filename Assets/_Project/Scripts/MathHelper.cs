using UnityEngine;

public static class MathHelper
{
    public static Vector3 RayIntersectionWithYZero(Vector3 origin, Vector3 dir)
    {
        float denom = Vector3.Dot(Vector3.up, dir);
        if (Mathf.Abs(denom) > 0.0001f)
        {
            // negate normal instead of rayOrigin
            float t = Vector3.Dot(origin, Vector3.up * -1) / denom;
            return origin + dir * t;
        }
        return Vector3.zero;
    }


    public static Transform GetClosestTransform(this Vector3 position, Transform[] transforms)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Transform potentialTarget in transforms)
        {
            Vector3 directionToTarget = potentialTarget.position - position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
