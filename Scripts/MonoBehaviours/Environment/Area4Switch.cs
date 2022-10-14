using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area4Switch : MonoBehaviour
{
    public GameManager manager;
    public CastSpell spell;
    public bool isLavaLevel = true;
    public bool isIceLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile") && spell.spellIndex == 2)
        {
            isLavaLevel = false;
            isIceLevel = true;
            manager.audioList[4].Play();
        }
    }


}
