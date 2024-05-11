using UnityEngine;

public class BaseWall : MonoBehaviour
{
    public Transform Origin;
    public BaseWallType Type;

    void Start()
    {
        
    }

    public void InitWall(int x, int y, string wallNamePrefix, float tileSize, Vector3 dir, float rotation)
    {
        Origin.localPosition = dir * (tileSize/2);
        name = wallNamePrefix;
        if (Type == BaseWallType.WALL)
        {
            Origin.transform.localScale = new Vector3(tileSize, Origin.transform.localScale.y, Origin.transform.localScale.z);
            Origin.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            name += x + "" + y;
        }        
    }
}

public enum BaseWallType
{
    WALL,
    CORNER
}
