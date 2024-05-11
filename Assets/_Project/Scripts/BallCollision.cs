using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public bool DEBUG;

    public int TargetCount;

    private Vector3[] _colTargets;

    private int _wallLayerMask;

    public struct CollisionResult { public bool HasHit; public RaycastHit Hit; public int HitIndex; public BaseCollisionInteraction CollisionInteraction; public void Clear() { HasHit = false; CollisionInteraction = null; } }
    private CollisionResult _colResult;

    void Start()
    {
        _colResult = new CollisionResult();
        _colResult.HasHit = false;


        _wallLayerMask = LayerMask.NameToLayer("Wall");

       // SetupCollisionTargets();
    }
    /*
        private void Update()
        {
            if (DEBUG)
            {
                if (TargetCount != _colTargets.Length)
                    SetupCollisionTargets();            
            }
        }

        private void SetupCollisionTargets()
        {
            _colTargets = new Vector3[TargetCount];
            float angleIncrement = 360f / TargetCount;

            for (int i = 0; i < TargetCount; i++)
            {
                float angle = i * angleIncrement;
                float x = transform.position.x + (transform.localScale.x/2) * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = transform.position.z + (transform.localScale.x/2) * Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 point = new Vector3(x, 0, z);
                _colTargets[i] = point;
            }
        }

        public CollisionResult CheckCollision()
        {
            _colResult.HasHit = false;

            for (int i = 0; i < _colTargets.Length; i++)
            {
                if (Physics.Raycast(transform.position, _colTargets[i].normalized, out _colResult.Hit, transform.localScale.x/2, 1 << _wallLayerMask))
                {
                    _colResult.HasHit = true;
                    _colResult.HitIndex = i;
                    Debug.DrawLine(transform.position, _colResult.Hit.point, Color.red, 0);                

                    return _colResult;
                }
                Debug.DrawLine(transform.position, transform.position + _colTargets[i], Color.green, 0);
            }

            _colResult.HasHit = false;
            _colResult.HitIndex = -1;
            return _colResult;
        }
    */
    public CollisionResult CheckFutureCollision(Vector3 velocity)
    {
        velocity *= Time.fixedDeltaTime;
        _colResult.Clear();
        if(Physics.SphereCast(transform.position, transform.localScale.x/2, velocity.normalized, out _colResult.Hit, velocity.magnitude, 1 << _wallLayerMask))
        {
            _colResult.HasHit = true;
            _colResult.CollisionInteraction = _colResult.Hit.collider.GetComponent<BaseCollisionInteraction>();
            Debug.DrawLine(transform.position, _colResult.Hit.point, Color.red, 3);
        }
        return _colResult;
    }
/*
    private void OnDrawGizmos()
    {
        if (DEBUG)
        {
            if (_colTargets == null)
                return;

            for (int i = 0; i < _colTargets.Length; i++)
            {                
                Gizmos.color = _colResult.HitIndex == i ? Color.red : Color.green;
                Gizmos.DrawWireSphere(transform.position + _colTargets[i], 0.01f);
            }
        }
    }*/
}
