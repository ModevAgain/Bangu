using TMPro;
using UnityEngine;

public class ShootOffController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LineRenderer _lineRen;
    [SerializeField]
    private TMP_Text _forceTMP;

    [Header("Data")]
    [SerializeField]
    private float _maxDragRange;
    [SerializeField]
    private float _maxShootOffForce;
    [SerializeField]
    private float _currentForce;
    [SerializeField]
    private Vector3 _currentDirection;


    private Ball _ball;
    private bool _indicatorActive;    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag("Ball"))
                {
                    ShowIndicator();
                }
            }
        }
        
        if (Input.GetMouseButton(0) && _indicatorActive)
        {
            UpdateIndicator();
        }

        if(Input.GetMouseButtonUp(0) && _indicatorActive)
        {
            ShootOff();
            HideIndicator();
        }
    }

    

    private void ShowIndicator()
    {
        _ball = Level.StartingBall;

        _lineRen.gameObject.SetActive(true);
        _forceTMP.gameObject.SetActive(true);

        _lineRen.SetPosition(0, _ball.transform.position);
        _forceTMP.transform.position = _ball.transform.position + Vector3.right * 0.2f;
        var camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _forceTMP.transform.forward = -(camPos - _forceTMP.transform.position).normalized;

        _indicatorActive = true;
    }

    private void UpdateIndicator()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var pos = MathHelper.RayIntersectionWithYZero(ray.origin, ray.direction);

            _currentDirection = pos - _ball.transform.position;
            _currentDirection = Vector3.ClampMagnitude(_currentDirection, _maxDragRange);
            _lineRen.SetPosition(1, _ball.transform.position + _currentDirection);

            float px = _currentDirection.magnitude / _maxDragRange;
            _currentForce = px * _maxShootOffForce;

            _forceTMP.text = (px * 100).ToString("0.0") + "%";            
        //if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, 1 << LayerMask.NameToLayer("Floor")))
        //{
        //    var pos = hit.point;

        //}
    }

    private void HideIndicator()
    {
        _lineRen.gameObject.SetActive(false);
        _forceTMP.gameObject.SetActive(false);

        _indicatorActive = false;
    }

    private void ShootOff()
    {
        _currentDirection *= -1;

        Debug.DrawLine(_ball.transform.position, _ball.transform.position + (_currentDirection.normalized * Mathf.Lerp(1, 2, _currentForce / _maxShootOffForce)), Color.blue, 1);
        Level.Current.ShootOff(_currentDirection, _currentForce);
    }    
}
