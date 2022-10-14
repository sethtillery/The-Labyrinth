using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffAnimation : MonoBehaviour
{
    public Player character;

    public PlayerMovement movement;

    //cooldown time for weapon
    public float coolDownTime = 1;

    //next time player can fire weapon
    public float nextFireTime = 0;

    //reference to animator
    public Animator animator;

    public SwitchStaff staffSwitcher;

    public GameManager gm;

    // Start is called before the first frame update
    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
       animator = gameObject.GetComponent<Animator>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        character = GameObject.Find("Player").GetComponent<Player>();

        staffSwitcher = GameObject.Find("staffSwitchManager").GetComponent<SwitchStaff>();
    }


    // Update is called once per frame
    void Update()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = gameObject.GetComponent<Animator>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        character = GameObject.Find("Player").GetComponent<Player>();

        staffSwitcher = GameObject.Find("staffSwitchManager").GetComponent<SwitchStaff>();
        if (!gm.isPaused)
        {
                if (Time.time > nextFireTime)
                {
                    if (Input.GetButtonDown("Fire1") && character.currentMana >= 1)
                    {
                        nextFireTime = Time.time + coolDownTime;

                        if (staffSwitcher.staffIndex == 1 && character.currentMana >= 1 || staffSwitcher.staffIndex == 2 && character.currentMana >= 1)
                            animator.SetTrigger("attack");

                        else //if (staffSwitcher.staffIndex == 3 && character.currentMana >= 5 && character.hitPoints < character.maxHitPoints)
                            animator.SetTrigger("heal");
                    }

                    if(Input.GetButtonDown("Fire2") && character.currentMana >= 3)
                    { 

                        if (staffSwitcher.staffIndex == 1 && character.currentMana >= 3 || staffSwitcher.staffIndex == 2 && character.currentMana >= 3)
                            animator.SetTrigger("heavy");

                    }
                }      
            
        }
    }

}
