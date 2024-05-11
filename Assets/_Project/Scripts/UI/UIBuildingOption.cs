using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildingOption : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public BaseObstacle Building;
    private LevelBuilder _levelBuilder;
    private bool _dragging;

    void Start()
    {
        _levelBuilder = FindAnyObjectByType<LevelBuilder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _levelBuilder.ActivateObstacleBuildingMode(Building);
        _dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var pos = MathHelper.RayIntersectionWithYZero(ray.origin, ray.direction);
        _levelBuilder.UpdateObstacleBuildingMode(pos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragging)
        {
            _levelBuilder.FinishObstacleBuildingMode(true);
            _dragging = false;
        }
    }


    private void Update()
    {
        if(_dragging && Input.GetMouseButtonDown(1))
        {
            _levelBuilder.FinishObstacleBuildingMode(false);
            _dragging = false;
        }
    }


}
