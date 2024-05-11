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
    }

    [OnValueChanged(nameof(CreateBaseMatrix))]
    [FoldoutGroup("Dimension"), ValueDropdown(nameof(GetPossibleDimensionSizes))]
    public int DimensionSize = 3;
    [FoldoutGroup("Dimension")]
    [ReadOnly]
    public Vector2 CurrentPivotOffset;
    [FoldoutGroup("Dimension")]
    [Indent(5)]
    [TableMatrix(DrawElementMethod = "DrawCell", RespectIndentLevel = true, ResizableColumns = false, SquareCells = true)]
    public bool[,] DimensionMatrix;
    private List<(int, int)> _dimensionBlocks;

    void Start()
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

    public List<(int, int)> GetDimensionBlocks()
    {
        if(_dimensionBlocks == null)
        {
            List<(int, int)> blockedDimensions = new List<(int, int)>();
            for (int x = 0; x < DimensionMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < DimensionMatrix.GetLength(1); y++)
                {
                    if (DimensionMatrix[x, y])
                    {
                        blockedDimensions.Add(((int)CurrentPivotOffset.x + x, (int)CurrentPivotOffset.y + y));
                    }
                }
            }
            _dimensionBlocks = blockedDimensions;
        }
        return _dimensionBlocks;
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
