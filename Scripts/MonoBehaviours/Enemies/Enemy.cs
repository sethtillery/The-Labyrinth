using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : character
{

    public NavMeshAgent enemy;
    public Transform Player;
    public float distanceRange = 5.0f;
    private Animator character;
    public bool beenHit;
    private Player playerScript;
    public int strength;
    public bool isAttack;
    public bool attacking;
    public AudioClip walking;
    public AudioClip runnung;
    public AudioSource enemyRoar;

    public bool playSound = true;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyRoar = GameObject.Find("Enemy Roar").GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        character = GetComponent<Animator>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        isAttack = false;
        attacking = false;
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TowardsPlayer();
    }

    public void AdjustHitPoints(int damage)
    {
        if (hitPoints >= 0 && !beenHit)
        {
            Debug.Log("Hit by Player");
            beenHit = true;
            Vector3 dirToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position - dirToPlayer;
            enemy.SetDestination(newPos);
        }

        hitPoints -= damage;
        StartCoroutine(Hit());
        if (hitPoints <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            enemy.SetDestination(this.transform.position);
            isDead = true;
            character.SetTrigger("death");
            Destroy(gameObject, 10);
        }
    }

    public void TowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance < distanceRange && !isDead)
        {
            if(playSound)
            {
                playSound = false;
                enemyRoar.Play();
            }
            Debug.Log("Closer than Player");
            Vector3 dirToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position - dirToPlayer;
            enemy.SetDestination(newPos);

            if(distance > 2 && distance < distanceRange) 
            {
                character.SetBool("Run", true);
                character.SetBool("Idle", false);
            }
            
            else if (distance <= 2)
            {
                character.SetBool("Run", false);
                isAttack = true;
                if (isAttack && !attacking)
                {
                    StartCoroutine(Attack());
                    attacking = true;
                    isAttack = false;
                }
            }
        }
        else if (beenHit)
        {
            character.SetBool("Run", true);
            character.SetBool("Idle", false);
        }
        else if (distance > distanceRange || !beenHit)
        {
           character.SetBool("Run", false);
           StartCoroutine(Walk());
        }
    }

    public IEnumerator Hit()
    {
        if (beenHit)
        {
            Debug.Log("HIT");
            yield return new WaitForSeconds(5);
            beenHit = false;
        }
    }

    public IEnumerator Attack()
    {
       if(Player != null)
        {
            enemy.SetDestination(Player.position);
            if(enemy.remainingDistance <= 5.5f)
            {
                enemyRoar.Play();
                GetComponent<Animator>().SetTrigger("isAttacking");
                yield return new WaitForSeconds(1.1f);
                if (enemy.remainingDistance <= 2.0f)
                {
                    isAttack = true;
                    Debug.Log("Damage Taken");
                    playerScript.hitPoints = playerScript.hitPoints - strength;
                    StartCoroutine(playerScript.ShowDamageEffect());
                }
                yield return new WaitForSeconds(1);
                attacking = false;
            }
        }
    }

    public IEnumerator Walk()
    {
        character.SetBool("Walk", true);
        yield return new WaitForSeconds(1);
        character.SetBool("Walk", false);
        character.SetBool("Idle", true);
    }

    public override void ResetCharacter()
    {
        throw new System.NotImplementedException();
    }
}