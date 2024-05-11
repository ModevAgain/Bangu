using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildingOption : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public BaseObstacle Building;
    private LevelBuilder _levelBuilder;

    void Start()
    {
        _levelBuilder = FindAnyObjectByType<LevelBuilder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _levelBuilder.ActivateObstacleBuildingMode(Building);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var pos = MathHelper.RayIntersectionWithYZero(ray.origin, ray.direction);
        _levelBuilder.UpdateObstacleBuildingMode(pos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _levelBuilder.FinishObstacleBuildingMode(true);
    }
    
    

    

}
