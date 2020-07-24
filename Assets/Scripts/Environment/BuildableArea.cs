using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableArea : MonoBehaviour
{
    BoxCollider entryCollider;
   
    private void Start()
    {
        entryCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInteraction>().EnterBuildArea(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInteraction>().EnterBuildArea(false);
        }
    }
}
