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

    public static BaseObstacle GetClosestTransform(this Vector3 position, BaseObstacle[] obstacles)
    {
        BaseObstacle bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (var potentialTarget in obstacles)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    public static bool[,] RotateBitMatrixByKTimes(bool[,] matrix, int numberOftimes)
    {
        int n = matrix.GetLength(0);
        bool[,] outputMatrix = new bool[n,n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                outputMatrix[i, j] = matrix[i, j];
            }
        }

        for (int k = 0; k < numberOftimes; k++)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = i; j < n - i - 1; j++)
                {
                    bool top = outputMatrix[i, j];
                    //Move left to top
                    outputMatrix[i, j] = outputMatrix[n - 1 - j, i];
                    //Move bottom to left
                    outputMatrix[n - 1 - j, i] = outputMatrix[n - i - 1, n - 1 - j];
                    //Move right to bottom
                    outputMatrix[n - i - 1, n - 1 - j] = outputMatrix[j, n - i - 1];
                    //Move top to right
                    outputMatrix[j, n - i - 1] = top;
                }
            }
        }

        //Check for center flag
        var centerIndex = (n - 1) / 2;
        if(centerIndex % 1 == 0)
        {
            outputMatrix[centerIndex, centerIndex] = matrix[centerIndex, centerIndex];
        }

        return outputMatrix;
    }
}
