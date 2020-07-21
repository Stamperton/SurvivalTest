using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    //Components
    Camera cam;
    RaycastHit rayHit;

    public Text tooltipTextBox;

    //Timer Variables
    float timer;
    float crosshairUpdateDelay = .1f;

    //Player Variables
    public float interactionRange = 5f;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        //ToggleInventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerInventory.instance.ToggleInventoryAndEquipment();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (GameManager.instance.currentState == PlayerState.FPS)
            {
                GameManager.instance.EnterBuildMode();
            }
            else if (GameManager.instance.currentState == PlayerState.Build)
            {
                GameManager.instance.ExitBuildMode();
            }
        }

        if (Input.GetMouseButtonDown(0) && GameManager.instance.currentState != PlayerState.Canvas)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, interactionRange))
            {
                Debug.Log(rayHit.collider.name);

                IInteractable currentInteractable = rayHit.collider.GetComponent<IInteractable>();
                if (currentInteractable != null)
                {
                    currentInteractable.Interact();
                }
            }
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
                tooltipTextBox.text = tooltip.toolTipText;
            }
            else
                tooltipTextBox.text = null;
        }
    }
}
