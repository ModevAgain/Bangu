using System.Collections.Generic;
using UnityEngine;

public class BaseCollisionInteraction : MonoBehaviour
{
    [SerializeReference]
    public List<BaseCollisionInteractionType> CI_Types;

    [Header("Add Content")]
    public bool AddCI_Score;
    public bool AddCI_Boost;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void OnBallHit(Ball ball)
    {
        foreach (var item in CI_Types)
        {
            item.Apply(ball);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AddCI_Score)
        {
            AddCI_Score = false;
            CI_Types.Add(new CI_Score());
        }

        if (AddCI_Boost)
        {
            AddCI_Boost = false;
            CI_Types.Add(new CI_Boost());
        }
    }
}
