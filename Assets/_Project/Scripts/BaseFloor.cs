using UnityEngine;

public class BaseFloor : MonoBehaviour
{
    public Transform Origin;
    public int GridX;
    public int GridY;

    void Start()
    {

    }

    public void InitFloor(int x, int y, float tileSize, Vector3 dir)
    {
        Origin.localPosition = dir * tileSize;
        Origin.transform.localScale = new Vector3(tileSize, Origin.transform.localScale.y, tileSize);
        name = "Floor_" + x + "" + y;
    }
}
