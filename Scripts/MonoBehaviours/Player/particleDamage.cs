using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //collision.gameObject.GetComponent<character>().damageCoroutine = StartCoroutine(collision.gameObject.GetComponent<character>().DamageCharacter(5, 0));
            collision.gameObject.GetComponent<Enemy>().AdjustHitPoints(damage);
            Destroy(gameObject);
            
        }
       
    }
}
