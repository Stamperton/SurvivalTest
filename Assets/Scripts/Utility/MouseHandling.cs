using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseHandling
{

    public static void MouseToFPSMode()
    {
        ItemInteractionPanel.instance.CloseItemPanel();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        GameManager.instance.currentState = GameManager.instance.lastState;
    }

    public static void MouseToCanvasMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = Mathf.Epsilon;

        if (GameManager.instance.lastState != GameManager.instance.currentState && GameManager.instance.currentState != E_PlayerState.Canvas)
            GameManager.instance.lastState = GameManager.instance.currentState;

        GameManager.instance.currentState = E_PlayerState.Canvas;

    }

}
