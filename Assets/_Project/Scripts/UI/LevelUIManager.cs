using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUIManager : MonoBehaviour
{
    [Header("Speed")]
    public CanvasGroup CG_Speed;
    public Image Image_Speed;
    

    void Start()
    {
        Level.Current.LevelStart += OnShootBall;
        Level.Current.Reset += OnResetLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Level.Current.LevelIsRunning)
            Image_Speed.fillAmount = Level.StartingBall.CurrentVelocityMagnitude / Level.StartingBall.MaxVelocityMagnitude;
    }

    public void OnShootBall()
    {
        CG_Speed.DOFade(1, 0.2f);
    }

    public void OnResetLevel()
    {
        CG_Speed.alpha = 0;
        Image_Speed.fillAmount = 0;
    }
}
