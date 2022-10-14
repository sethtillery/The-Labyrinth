using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : character
{
    // Used to get a reference to the prefab
    public HealthBar healthBarObject;
    public manaBar ManaBar;

    // A copy of the health bar prefab
    HealthBar healthBar;
    manaBar manaBarObject;

    public float manaRegenRate = 2;
    public float manaToBeAdded = 1;
    
    //coroutine for mana regen
    Coroutine manaRegen;

    float damageRate = .05f;
    float damageCooldown;

    [HideInInspector]
    public string textChar;

    public GameManager gm;

    public AudioSource footsteps;

    public GameObject damageEffect;
    public GameObject slimeEffect;

    // Start is called before the first frame update
    private void Awake()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ResetCharacter();
    }

    private void FixedUpdate()
    {
        //checks if the manaRegen is not active and the current mana is less than max
        if (manaRegen == null && currentMana < maxMana)
        {

            //starts the mana regen
            manaRegen = StartCoroutine(regenMana());

        }

        //checks if mana is max
        else if (currentMana >= maxMana)
        {

            //if the manaRegen is active stops it
            if (manaRegen != null)
            {

                StopCoroutine(manaRegen);
                manaRegen = null;

            }

        }

    }


    // Called when player's collider touches an "Is Trigger" collider
    private void OnTriggerEnter(Collider collision)
    {
        //checks if collision with enemy and that the collision isn't a trigger(enemy sight) and that damage is not on cooldown
        if (collision.gameObject.CompareTag("Enemy") && !collision.isTrigger && Time.time > damageCooldown)
        {

            //float damage = collision.gameObject.GetComponent<Enemy>().damageStrength;

            if (damageCoroutine == null)
            {
                // Start the coroutine to inflict damage to the player every 1 second
                damageCoroutine = StartCoroutine(DamageCharacter(2, 1.0f));

            }

            //sets cooldown
            damageCooldown = Time.time + damageRate;

        }

        //checks if the collision is with a projectile and the damage cooldown is not active
        else if (collision.gameObject.CompareTag("Projectile") && Time.time > damageCooldown)
        {

            //destroys the projectile damages the player and sets damage cooldown
            Destroy(collision.gameObject);
           // damageCoroutine = StartCoroutine(DamageCharacter(collision.gameObject.GetComponent<projectiles>().projectilesScript.damage, 0));
            damageCooldown = Time.time + damageRate;
        }

        //if the collision is with a sign
        else if (collision.gameObject.CompareTag("Sign"))
        {

            //sets the text to show on screen
            //textChar = collision.GetComponent<sign>().text;

        }

        else if(collision.gameObject.CompareTag("Harm"))
        {
            StartCoroutine(DamageCharacter(1, 1));
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        // See if the enemy has just stopped colliding with the player
        if (collision.gameObject.CompareTag("Enemy") )
        {

            // If coroutine is currently executing
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

        }
        else if (collision.gameObject.CompareTag("Harm"))
        {
            StopAllCoroutines();
            StartCoroutine(regenMana());
        }

    }

    public bool AdjustHitPoints(int amount)
    {
        // Don't increase above the max amount
        if (hitPoints < maxHitPoints)
        {
            hitPoints = hitPoints + amount;
            if(hitPoints > maxHitPoints)
            {
                hitPoints = maxHitPoints;
            }
            //print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);
            return true;
        }

        // Return false if hit points is at max and can't be adjusted
        return false;
    }

    public override void ResetCharacter()
    {
        gm.isPaused = false;
        // Start the player off with the starting hit point value
        hitPoints = startingHitPoints;

        // Get a copy of the health bar prefab and store a reference to it
        healthBar = Instantiate(healthBarObject);

        manaBarObject = Instantiate(ManaBar);

        textChar = null;


        // Set the healthBar's character property to this character so it can retrieve the maxHitPoints
        healthBar.character = this;
        manaBarObject.character = this;
    }

    //regens mana every 2 seconds
    public IEnumerator regenMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaRegenRate);
            currentMana += manaToBeAdded;
        }
    }

    public IEnumerator ShowDamageEffect()
    {
        damageEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        damageEffect.SetActive(false);
    }

    public IEnumerator ShowSlimeEffect()
    {
        slimeEffect.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        slimeEffect.SetActive(false);
    }

}
