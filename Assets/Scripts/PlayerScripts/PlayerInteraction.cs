using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    //Components
    Camera cam;
    RaycastHit rayHit;

    //UI Variables
    public Text tooltipTextBox;
    public GameObject canBuildIndicator;

    //Timer Variables
    float timer;
    float crosshairUpdateDelay = .1f;

    //Player Variables
    public float interactionRange = 5f;
    bool canBuild = false;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();

        EnterBuildArea(false);
    }

    private void Update()
    {
        //ToggleInventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerInventory.instance.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!canBuild)
                return;

            if (GameManager.instance.currentState == E_PlayerState.FPS)
            {
                GameManager.instance.EnterBuildMode();
            }
            else if (GameManager.instance.currentState == E_PlayerState.Build)
            {
                GameManager.instance.ExitBuildMode();
            }
        }

        if (Input.GetMouseButtonDown(0) && GameManager.instance.currentState != E_PlayerState.Canvas)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, interactionRange))
            {
                //Debug.Log(rayHit.collider.name);

                IInteractable currentInteractable = rayHit.collider.GetComponent<IInteractable>();
                if (currentInteractable != null)
                {
                    currentInteractable.Interact();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInventory.instance.CloseInventoryButton();
        }

        timer += Time.deltaTime;
        if (timer >= crosshairUpdateDelay)
        {
            timer = 0;
            CheckForToolTip();
        }


    }

    void CheckForToolTip()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, interactionRange))
        {
            CrosshairTooltip tooltip = rayHit.collider.GetComponent<CrosshairTooltip>();
            if (tooltip != null)
            {
                tooltip.UpdateText();
                tooltipTextBox.text = tooltip.toolTipText;
            }
            else
                tooltipTextBox.text = null;
        }
        else
            tooltipTextBox.text = null;
    }

    public void EnterBuildArea(bool _state)
    {
        canBuild = _state;
        canBuildIndicator.SetActive(_state);
        if (_state == false)
        {
            GameManager.instance.ExitBuildMode();
        }
    }

}
