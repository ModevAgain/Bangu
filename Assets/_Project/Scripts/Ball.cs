using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool Active;
    public float MaxVelocityMagnitude;
    public Vector3 Velocity;
    public float CurrentVelocityMagnitude;
    public float CurrentDrag;
    public float MaxCollisionDrag;

    [Header("DEBUG")]
    public bool DEBUG_USE_FUTURE_STEP;

    private RaycastHit _hit;
    private RaycastHit _debugHit;
    private int _wallLayerMask;
    private BallCollision _ballCollision;

    void Start()
    {
        Level.Current.Reset += OnReset;
        Level.StartingBall = this;

        _ballCollision = GetComponent<BallCollision>();

        _wallLayerMask = LayerMask.NameToLayer("Wall");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Active)
        {
            Step();    
        }
    }

    private void Step()
    {
        var result = _ballCollision.CheckFutureCollision(Velocity);
        if (result.HasHit && DEBUG_USE_FUTURE_STEP)
        {
            //Future Safe Step
            var dir = transform.position - result.Hit.point;
            transform.position = result.Hit.point + dir.normalized * transform.localScale.x / 2;

            //Wall reflection vector adjustment
            Velocity -= 2 * (Vector3.Dot(Velocity, result.Hit.normal)) * result.Hit.normal;
            Velocity.y = 0;

            //Hit Wall dampening
            float dot = Vector3.Dot(Velocity.normalized, result.Hit.normal);
            Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, dot * MaxCollisionDrag);

            //Apply Collision Interaction
            result.CollisionInteraction?.OnBallHit(this);
        }
        //Normal step along velocity
        else transform.position += Velocity * Time.fixedDeltaTime;
        
        //Normal velocity dampening
        Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, CurrentDrag * Time.fixedDeltaTime);
        Velocity = Vector3.ClampMagnitude(Velocity, MaxVelocityMagnitude);
        CurrentVelocityMagnitude = Velocity.magnitude;

        if (Velocity.sqrMagnitude < 0.02f)
        {
            Velocity = Vector3.zero;
            Active = false;
        }
    }

    public void ShootOff(Vector3 direction, float force)
    {
        ApplyForce(direction, force);
        Active = true;
    }

    public void ApplyForce(Vector3 direction, float force)
    {
        direction.y = 0;
        Velocity = direction * force;       
    }

    public void ApplyVelocityBoost(float boostValue)
    {
        Velocity *= boostValue;
    }

    private void OnReset()
    {
        Active = false;                
        Velocity = Vector3.zero;
        transform.position = Vector3.up * 0.15f;
    }
}
