using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGameObjectActive : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
        }
    }
}
