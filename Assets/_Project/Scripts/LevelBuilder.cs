using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class LevelBuilder : MonoBehaviour
{
    public int BaseGridSize;
    public float BaseTileSize;    

    public GameObject Prefab_Wall;
    public GameObject Prefab_WallCorner;
    public GameObject Prefab_Floor;

    public GameObject LevelRoot;

    private BaseWall[,] _wallGrid;
    private BaseFloor[,] _floorGrid;
    private bool _initialized;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(BuildBaseLevel());
        }
    }

    public IEnumerator BuildBaseLevel()
    {
        if (_initialized)
        {
            int childcount = LevelRoot.transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                Destroy(LevelRoot.transform.GetChild(0).gameObject);
                yield return null;
            }            
        }

        yield return null;

        _wallGrid = new BaseWall[BaseGridSize + 2, BaseGridSize + 2];
        _floorGrid = new BaseFloor[BaseGridSize, BaseGridSize];

        GameObject tempRoot = new GameObject();

        #region Walls
        int wallGridMax = _wallGrid.GetLength(0) - 1;
        for (int y = 0; y < _wallGrid.GetLength(0); y++)
        {
            for (int x = 0; x < _wallGrid.GetLength(0); x++)
            {
                if(x == 0 || x == wallGridMax || y == 0 || y == wallGridMax)
                {                    
                    Vector3 wallDir = -Vector3.forward;
                    float wallRot = 0;
                    string wallName = "Wall_";
                    BaseWallType type = BaseWallType.WALL;

                    //Corner North-West
                    if(x == 0 && y == 0)
                    {
                        type = BaseWallType.CORNER;
                        wallName += "Corner_NW";
                        wallDir = new Vector3(1f, 0, -1f);
                    }
                    //Corner North-East
                    else if (x == wallGridMax && y == 0)
                    {
                        type = BaseWallType.CORNER;
                        wallName += "Corner_NE";
                        wallDir = new Vector3(-1f, 0, -1f);
                    }
                    //Corner South-West
                    else if (x == 0 && y == wallGridMax)
                    {
                        type = BaseWallType.CORNER;
                        wallName += "Corner_SW";
                        wallDir = new Vector3(1f, 0, 1f);
                    }
                    //Corner South-East
                    else if (x == wallGridMax && y == wallGridMax)
                    {
                        type = BaseWallType.CORNER;
                        wallName += "Corner_SE";
                        wallDir = new Vector3(-1f, 0, 1f);
                    }


                    //North
                    else if (y == 0)
                    {
                        //Is default                        
                        wallName += "_N_" + x + "" + y;
                    }
                    //West
                    else if (x == 0)
                    {                        
                        wallRot = 90;
                        wallDir = Vector3.right;
                        wallName += "_W_" + x + "" + y;
                    }
                    //East
                    else if (x == wallGridMax)
                    {                        
                        wallRot = -90;
                        wallDir = Vector3.left;
                        wallName += "_E_" + x + "" + y;
                    }
                    //South
                    else if (y == wallGridMax)
                    {                        
                        wallRot = 180;
                        wallDir = Vector3.forward;
                        wallName += "_S_" + x + "" + y;
                    }

                    var pos = new Vector3((x + 1) * BaseTileSize, 0.1f, -(y + 1) * BaseTileSize);
                    
                    
                    _wallGrid[x, y] = CreateWall(tempRoot.transform, pos, wallRot, wallDir, type, wallName);
                }
            }
        }
        #endregion

        #region Floor
        for (int y = 0; y < _floorGrid.GetLength(0); y++)
        {
            for (int x = 0; x < _floorGrid.GetLength(0); x++)
            {
                var floor = Instantiate(Prefab_Floor, tempRoot.transform);
                floor.name = "Floor_" + x + "" + y;
                floor.transform.localPosition = new Vector3((x + 2) * BaseTileSize, -0.05f, - (y + 2) * BaseTileSize);
                var baseFloor = floor.GetComponent<BaseFloor>();
                baseFloor.InitFloor(BaseTileSize, Vector3.zero);
                _floorGrid[x, y] = baseFloor;                
            }
        }
        #endregion
        var midPoint = BaseTileSize * (((float)BaseGridSize+2)/2) + BaseTileSize/2;
        LevelRoot.transform.position = new Vector3(midPoint, 0, -midPoint);
        foreach (var item in _wallGrid)
        {
            if(item != null)
                item.transform.SetParent(LevelRoot.transform, true);
        }
        foreach (var item in _floorGrid)
        {
            if(item != null)
                item.transform.SetParent(LevelRoot.transform, true);
        }
        LevelRoot.transform.position = Vector3.zero;
        Destroy(tempRoot);
        _initialized = true;
    }

    public BaseWall CreateWall(Transform root, Vector3 position, float rotation, Vector3 wallDir, BaseWallType type, string name)
    {
        var baseWallObj = Instantiate<GameObject>(type == BaseWallType.WALL ? Prefab_Wall : Prefab_WallCorner, root);
        baseWallObj.transform.localPosition = position;
        baseWallObj.name = name;
        
        var baseWall = baseWallObj.GetComponent<BaseWall>();
        baseWall.InitWall(BaseTileSize, wallDir, rotation);

        return baseWall;
    }
}
