using UnityEngine;

[System.Serializable]
public class CI_Score : BaseCollisionInteractionType
{
    public float ScoreValue;

    public override void Apply(Ball ball)
    {
        ScoreManager.Instance.AddScore(ScoreValue);
    }
}
