using UnityEngine;

[System.Serializable]
public class CI_Boost : BaseCollisionInteractionType
{
    public float BoostValue;

    public override void Apply(Ball ball)
    {
        ball.ApplyVelocityBoost(BoostValue);
    }
}
