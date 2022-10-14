using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // public Transform target will be used as the destination for the projectile.
    public Vector3 target;

    // public float speed will be used to set the projectile speed.
    public float speed;

    // public float damage will be used to set the projectiles damage.
    public float damage;

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed);
    }

}
