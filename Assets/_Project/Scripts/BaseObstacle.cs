using Sirenix.OdinInspector;
using UnityEngine;

public class BaseObstacle : SerializedMonoBehaviour
{
    public Transform Visual;
    public Transform Physical;
    public Transform Origin;
    public int GridX;
    public int GridY;
    public string NamePrefix;


    public virtual void InitObstacle(int x, int y)
    {
        GridX = x;
        GridY = y;
        name = NamePrefix + "" + x + "" + y;
    }
}
