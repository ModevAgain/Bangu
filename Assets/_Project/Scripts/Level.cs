using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Current 
    { 
        get; 
        set; 
    }

    public static Ball StartingBall
    {
        get;
        set;
    }

    public bool LevelIsRunning;

    public Action LevelStart;
    public Action Reset;



    private void Awake()
    {
        Current = this;
    }

    void Start()
    {
        OnReset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnReset();
        }
    }

    private void OnReset()
    {
        Reset?.Invoke();
        LevelIsRunning = false;
    }

    private void OnStart()
    {
        LevelStart?.Invoke();
        LevelIsRunning = true;
    }

    public void ShootOff(Vector3 direction, float force)
    {
        StartingBall.ShootOff(direction, force);        
        OnStart();
    }
}
