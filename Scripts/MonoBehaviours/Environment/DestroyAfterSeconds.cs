using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float secondsAlive;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTimer());
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(secondsAlive);
        Destroy(this.gameObject);
    }
}
