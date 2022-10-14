using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int limitOfObjects;
    public string tagToCheckFor;
    public void SpawnObject()
    {
        GameObject[] objectsFound = GameObject.FindGameObjectsWithTag(tagToCheckFor);
        if(objectsFound.Length <= limitOfObjects)
        {
            Instantiate(objectToSpawn, this.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("LIMIT ALREADY REACHED");
        }
    }
}
