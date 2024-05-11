using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObstacle : BaseObstacle
{
    [OnValueChanged(nameof(CreateBaseMatrix))]
    [FoldoutGroup("Dimension"), ValueDropdown(nameof(GetPossibleDimensionSizes))]
    public int DimensionSize = 3;
    [FoldoutGroup("Dimension")]
    [Indent(5)]
    [TableMatrix(DrawElementMethod = "DrawCell", RespectIndentLevel = true,ResizableColumns = false, SquareCells = true)]
    public bool[,] DimensionMatrix;

    
    void Start()
    {
        
    }


    [OnInspectorInit]
    private void CreateBaseMatrix()
    {        
        if(DimensionMatrix == null)
        {
            DimensionMatrix = new bool[DimensionSize, DimensionSize];
            return;
        }

        if(DimensionMatrix.GetLength(0) != DimensionSize)
        {
            DimensionMatrix = new bool[DimensionSize, DimensionSize];
            return;
        }
    }

    private List<int> GetPossibleDimensionSizes()
    {
        return new List<int> { 3, 5 };
    }

#if UNITY_EDITOR
    private static bool DrawCell(Rect rect, bool value)
    {
        if(Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
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
