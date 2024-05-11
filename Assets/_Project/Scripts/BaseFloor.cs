using UnityEngine;

public class BaseFloor : MonoBehaviour
{
    public Transform Origin;

    void Start()
    {

    }

    public void InitFloor(float tileSize, Vector3 dir)
    {
        Origin.localPosition = dir * tileSize;
        Origin.transform.localScale = new Vector3(tileSize, Origin.transform.localScale.y, tileSize);
    }
}
