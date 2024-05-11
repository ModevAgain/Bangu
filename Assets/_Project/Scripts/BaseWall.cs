using UnityEngine;

public class BaseWall : MonoBehaviour
{
    public Transform Origin;
    public BaseWallType Type;

    void Start()
    {
        
    }

    public void InitWall(float tileSize, Vector3 dir, float rotation)
    {
        Origin.localPosition = dir * (tileSize/2);
        if (Type == BaseWallType.WALL)
        {
            Origin.transform.localScale = new Vector3(tileSize, Origin.transform.localScale.y, Origin.transform.localScale.z);
            Origin.transform.localRotation = Quaternion.Euler(0, rotation, 0);
        }        
    }
}

public enum BaseWallType
{
    WALL,
    CORNER
}
