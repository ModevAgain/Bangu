using UnityEngine;

[System.Serializable]
public class BaseCollisionInteractionType
{
    public virtual void Apply(Ball ball) 
    {
        throw new System.Exception("Did not override BaseCollisionInteractionType.Apply(Ball)!");
    }
}
