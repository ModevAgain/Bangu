using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
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
        InitData();
    }

    [OnValueChanged(nameof(CreateBaseMatrix))]
    [FoldoutGroup("Dimension")]
    public bool USE_DIMENSION = true;
    [FoldoutGroup("Dimension")]
    public bool LOG_DIMENSION_ROTATION = true;
    [FoldoutGroup("Dimension"), ValueDropdown(nameof(GetPossibleDimensionSizes))]
    public int DimensionSize = 3;
    [FoldoutGroup("Dimension")]
    [ReadOnly]
    public Vector2 CurrentPivotOffset;
    [FoldoutGroup("Dimension")]
    [Indent(5)]
    [TableMatrix(DrawElementMethod = "DrawCell", RespectIndentLevel = true, ResizableColumns = false, SquareCells = true), OnValueChanged(nameof(SetupDimensionBlocks))]
    public bool[,] DimensionMatrix;
    [FoldoutGroup("Dimension"), SerializeField, ReadOnly]
    private List<ObstacleGridIndex> _dimensionBlocks_0;
    [FoldoutGroup("Dimension"), SerializeField, ReadOnly]
    private List<ObstacleGridIndex> _dimensionBlocks_90;
    [FoldoutGroup("Dimension"), SerializeField, ReadOnly]
    private List<ObstacleGridIndex> _dimensionBlocks_180;
    [FoldoutGroup("Dimension"), SerializeField, ReadOnly]
    private List<ObstacleGridIndex> _dimensionBlocks_270;

    void Start()
    {

    }

    public void InitData()
    {
    }

    [OnInspectorInit]
    private void CreateBaseMatrix()
    {
        if(DimensionSize == 3)
        {
            CurrentPivotOffset = new Vector2(-1, -1);
        }
        else if(DimensionSize == 5)
        {
            CurrentPivotOffset = new Vector2(-2, -2);
        }

        if (DimensionMatrix == null)
        {
            DimensionMatrix = new bool[DimensionSize, DimensionSize];
            return;
        }

        if (DimensionMatrix.GetLength(0) != DimensionSize)
        {
            DimensionMatrix = new bool[DimensionSize, DimensionSize];
            return;
        }
    }

    private List<int> GetPossibleDimensionSizes()
    {
        return new List<int> { 3, 5 };
    }

    public List<ObstacleGridIndex> GetDimensionBlocks(int rotation = 0)
    {
        if (rotation == 0)
            return _dimensionBlocks_0;
        else if (rotation == 90)
            return _dimensionBlocks_90;
        else if (rotation == 180)
            return _dimensionBlocks_180;
        else if (rotation == 270)
            return _dimensionBlocks_270;
        else
        {
            throw new System.Exception();           
        }
    }

    private void SetupDimensionBlocks()
    {    
        List< ObstacleGridIndex> blockedDimensions = new List<ObstacleGridIndex>();
        string logOutput = "";

        //Default
        logOutput = "0 Degrees:\n";
        for (int y = 0; y < DimensionMatrix.GetLength(1); y++)
        {
            for (int x = 0; x < DimensionMatrix.GetLength(0); x++)
            {
                if (DimensionMatrix[x, y])
                {
                    blockedDimensions.Add(new ObstacleGridIndex((int)CurrentPivotOffset.x + x, (int)CurrentPivotOffset.y + y));
                    logOutput += "o";
                }
                else logOutput += "-";
            }
            logOutput += "\n";
        }
        if(LOG_DIMENSION_ROTATION)Debug.Log(logOutput);
        _dimensionBlocks_0 = new List<ObstacleGridIndex>(blockedDimensions);
        blockedDimensions.Clear();

        //Rotated 90 degrees
        logOutput = "90 Degrees:\n";
        var rotatedMatrix = MathHelper.RotateBitMatrixByKTimes(DimensionMatrix, 3);
        for (int y = 0; y < rotatedMatrix.GetLength(1); y++)
        {
            for (int x = 0; x < rotatedMatrix.GetLength(0); x++)
            {
                if (rotatedMatrix[x, y])
                {
                    blockedDimensions.Add(new ObstacleGridIndex((int)CurrentPivotOffset.x + x, (int)CurrentPivotOffset.y + y));
                    logOutput += "o";
                }
                else logOutput += "-";
            }
            logOutput += "\n";
        }
        if (LOG_DIMENSION_ROTATION) Debug.Log(logOutput);
        _dimensionBlocks_90 = new List<ObstacleGridIndex>(blockedDimensions);
        blockedDimensions.Clear();

        //Rotated 180 degrees
        logOutput = "180 Degrees:\n";
        rotatedMatrix = MathHelper.RotateBitMatrixByKTimes(DimensionMatrix, 2);
        for (int y = 0; y < rotatedMatrix.GetLength(1); y++)
        {
            for (int x = 0; x < rotatedMatrix.GetLength(0); x++)
            {
                if (rotatedMatrix[x, y])
                {
                    blockedDimensions.Add(new ObstacleGridIndex((int)CurrentPivotOffset.x + x, (int)CurrentPivotOffset.y + y));
                    logOutput += "o";
                }
                else logOutput += "-";
            }
            logOutput += "\n";
        }
        if (LOG_DIMENSION_ROTATION) Debug.Log(logOutput);
        _dimensionBlocks_180 = new List<ObstacleGridIndex>(blockedDimensions);
        blockedDimensions.Clear();

        //Rotated 270 degrees
        logOutput = "270 Degrees:\n";
        rotatedMatrix = MathHelper.RotateBitMatrixByKTimes(DimensionMatrix, 1);
        for (int y = 0; y < rotatedMatrix.GetLength(1); y++)
        {
            for (int x = 0; x < rotatedMatrix.GetLength(0); x++)
            {
                if (rotatedMatrix[x, y])
                {
                    blockedDimensions.Add(new ObstacleGridIndex((int)CurrentPivotOffset.x + x, (int)CurrentPivotOffset.y + y));
                    logOutput += "o";
                }
                else logOutput += "-";
            }
            logOutput += "\n";
        }
        if (LOG_DIMENSION_ROTATION) Debug.Log(logOutput);
        _dimensionBlocks_270 = new List<ObstacleGridIndex>(blockedDimensions);
        blockedDimensions.Clear();
    }

#if UNITY_EDITOR
    private static bool DrawCell(Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }

        UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
        return value;
    }
#endif
}

[System.Serializable]
public struct ObstacleGridIndex
{
    public int X;
    public int Y;

    public ObstacleGridIndex(int x, int y)
    {
        X = x;
        Y = y;
    }
}
