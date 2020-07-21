using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameManager in Scene");
            Destroy(this.gameObject);
        }
    }
    #endregion

    //Globally Accessable Bits
    public Sprite blankIcon;


    public PlayerState lastState;
    public PlayerState currentState = PlayerState.FPS;

    public event Action onEnterBuildMode;
    public event Action onExitBuildMode;

    public void EnterBuildMode()
    {
        Debug.Log("Entered Build Mode");
        currentState = PlayerState.Build;
        onEnterBuildMode?.Invoke();
    }

    public void ExitBuildMode()
    {
        Debug.Log("Exited Build Mode");
        currentState = PlayerState.FPS;
        onExitBuildMode?.Invoke();
    }
}
