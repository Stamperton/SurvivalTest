using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRandomiser : MonoBehaviour
{
    public GameObject[] models;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }

        models[Random.Range(0, models.Length)].SetActive(true);
    }

}
