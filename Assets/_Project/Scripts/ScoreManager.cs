using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float Score;

    public TMP_Text TMP_Score;

    public static ScoreManager Instance
    {
        get;
        set;
    }

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Level.Current.Reset += ResetScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float score)
    {
        Score += score;

        TMP_Score.text = Score.ToString("0000");
    }

    public void ResetScore() 
    {
        Score = 0;
        TMP_Score.text = "0000";
    }
}
