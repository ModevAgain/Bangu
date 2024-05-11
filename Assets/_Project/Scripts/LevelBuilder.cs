using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class LevelBuilder : MonoBehaviour
{
    public int BaseGridSize;
    public float BaseTileSize;

    public bool BuildLevelOnStart;

    public GameObject Prefab_Wall;
    public GameObject Prefab_WallCorner;
    public GameObject Prefab_Floor;
    public GameObject Prefab_Obstacle;

    public GameObject LevelRoot;

    [Header("Obstacle Building")]
    public Material Mat_ObstacleCanBuild;
    public Material Mat_ObstacleCannotBuild;

    private BaseWall[,] _wallGrid;
    private BaseFloor[,] _floorGrid;
    private BaseObstacle[,] _obstacleGrid;
    private bool[,] _blockingGrid;

    private bool _initialized;
    
    private GameObject _tempGOToBuild;
    private BaseObstacle _tempObstacleToBuild;
    private BaseObstacle _currentSelectedObstacle;
    private bool _canBuildAtCurrentPosition;

    void Start()
    {
        StartCoroutine(BuildBaseLevel());
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
        _obstacleGrid = new BaseObstacle[BaseGridSize * 2 - 1, BaseGridSize * 2 - 1];

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
                        wallName += "_N_";
                    }
                    //West
                    else if (x == 0)
                    {                        
                        wallRot = 90;
                        wallDir = Vector3.right;
                        wallName += "_W_";
                    }
                    //East
                    else if (x == wallGridMax)
                    {                        
                        wallRot = -90;
                        wallDir = Vector3.left;
                        wallName += "_E_";
                    }
                    //South
                    else if (y == wallGridMax)
                    {                        
                        wallRot = 180;
                        wallDir = Vector3.forward;
                        wallName += "_S_";
                    }

                    var pos = new Vector3((x + 1) * BaseTileSize, 0.1f * BaseTileSize/2, -(y + 1) * BaseTileSize);
                    
                    
                    _wallGrid[x, y] = CreateWall(x, y, wallName, tempRoot.transform, pos, wallRot, wallDir, type, wallName);
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
                floor.transform.localPosition = new Vector3((x + 2) * BaseTileSize, -0.05f, - (y + 2) * BaseTileSize);
                var baseFloor = floor.GetComponent<BaseFloor>();
                baseFloor.InitFloor(x, y, BaseTileSize, Vector3.zero);
                _floorGrid[x, y] = baseFloor;                
            }
        }
        #endregion

        #region Obstacle        
        for (int y = 0; y < _obstacleGrid.GetLength(0); y++)
        {
            for (int x = 0; x < _obstacleGrid.GetLength(0); x++)
            {
                var obstacleObj = Instantiate(Prefab_Obstacle, tempRoot.transform);                
                obstacleObj.transform.localPosition = new Vector3(BaseTileSize + ((x + 2) * (BaseTileSize/2)), 0, -(BaseTileSize + (y + 2) * (BaseTileSize/2)));
                var baseObstacle = obstacleObj.GetComponent<BaseObstacle>();                
                baseObstacle.InitObstacle(x, y);
                _obstacleGrid[x, y] = baseObstacle;
            }
        }
        _blockingGrid = new bool[_obstacleGrid.GetLength(0) + 2, _obstacleGrid.GetLength(0) + 2]; // +2 since we also want the borders to be included
        for (int x = 0; x < _blockingGrid.GetLength(0); x++)
        {
            for (int y = 0; y < _blockingGrid.GetLength(0); y++)
            {
                if (x == 0 || y == 0 || x == _blockingGrid.GetLength(0) - 1 || y == _blockingGrid.GetLength(1) - 1)
                    _blockingGrid[x, y] = true;
            }
        }
        #endregion


        #region Finalize
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
        foreach (var item in _obstacleGrid)
        {
            if (item != null)
                item.transform.SetParent(LevelRoot.transform, true);
        }
        LevelRoot.transform.position = Vector3.zero;
        Destroy(tempRoot);
        #endregion
        _initialized = true;
    }

    public BaseWall CreateWall(int x, int y, string wallName, Transform root, Vector3 position, float rotation, Vector3 wallDir, BaseWallType type, string name)
    {
        var baseWallObj = Instantiate<GameObject>(type == BaseWallType.WALL ? Prefab_Wall : Prefab_WallCorner, root);
        baseWallObj.transform.localPosition = position;
        baseWallObj.name = name;
        
        var baseWall = baseWallObj.GetComponent<BaseWall>();
        baseWall.InitWall(x, y, wallName, BaseTileSize, wallDir, rotation);

        return baseWall;
    }

    public void ActivateObstacleBuildingMode(BaseObstacle obstacle)
    {
        _tempGOToBuild = Instantiate(obstacle.Visual.gameObject);
        var scale = obstacle.Origin.localScale;
        scale.x *= BaseTileSize / 2;
        scale.z *= BaseTileSize / 2;
        _tempGOToBuild.transform.localScale = scale;
        
        _tempObstacleToBuild = obstacle;
    }

    public void UpdateObstacleBuildingMode(Vector3 position)
    {
        _currentSelectedObstacle = position.GetClosestTransform(_obstacleGrid.Cast<BaseObstacle>().ToArray());
        _tempGOToBuild.transform.position = _currentSelectedObstacle.transform.position;
        _tempGOToBuild.transform.position += (_tempObstacleToBuild.Origin.position * BaseTileSize/2);     
        _canBuildAtCurrentPosition = CheckifCanBuildObstacle();
        _tempGOToBuild.GetComponentInChildren<MeshRenderer>().material = _canBuildAtCurrentPosition ? Mat_ObstacleCanBuild : Mat_ObstacleCannotBuild;
    }

    public void FinishObstacleBuildingMode(bool tryBuild)
    {
        //IF CAN BUILD        
        if (tryBuild && _canBuildAtCurrentPosition)
        {
            var targetOb = _obstacleGrid.Cast<BaseObstacle>().Where(o => o == _currentSelectedObstacle).FirstOrDefault();
            var newOb = Instantiate(_tempObstacleToBuild, LevelRoot.transform);
            newOb.transform.position = targetOb.transform.position;
            newOb.Origin.localPosition *= BaseTileSize / 2;
            var scale = newOb.Origin.localScale;
            scale.x *= BaseTileSize / 2;
            scale.z *= BaseTileSize / 2;
            newOb.Origin.localScale = scale;
            newOb.InitObstacle(targetOb.GridX, targetOb.GridY);
            _obstacleGrid[targetOb.GridX, targetOb.GridY] = newOb;
            UpdateBlockingGrid(newOb);
        }
        
        if(_tempGOToBuild != null)
            Destroy(_tempGOToBuild.gameObject);
        _tempGOToBuild = null;
        _currentSelectedObstacle = null;        
    }

    private bool CheckifCanBuildObstacle()
    {
        var blocks = _tempObstacleToBuild.GetDimensionBlocks();
        int xOffset = _currentSelectedObstacle.GridX;
        int yOffset = _currentSelectedObstacle.GridY;

        foreach (var block in blocks)
        {
            //+1 for the borders
            int x = 1 + xOffset + block.Item1; 
            int y = 1 + yOffset + block.Item2;

            if (x < 0 || x >= _blockingGrid.GetLength(0))
                return false;
            else if (y < 0 || y >= _blockingGrid.GetLength(1))
                return false;
            else if (_blockingGrid[x, y])
                return false;
        }
        return true;
    }

    private void UpdateBlockingGrid(BaseObstacle obstacle)
    {
        foreach (var block in obstacle.GetDimensionBlocks())
        {
            _blockingGrid[1 + obstacle.GridX + block.Item1, 1 + obstacle.GridY + block.Item2] = true;
        }
    }
}
