using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollSlimeBall : FollowBezierCurve
{
    public GameObject slimePuddle;
    public AudioSource splat;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
        else if(other.CompareTag("Floor"))
        {
            Debug.Log("Hit floor!");
            Instantiate(slimePuddle, new Vector3(this.transform.position.x, 0.01f, this.transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
            splat.Play();
            Destroy(this.gameObject);
        }
        else if(other.gameObject.name == "Troll")
        {
            Debug.Log("Slime Contacting Troll");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
