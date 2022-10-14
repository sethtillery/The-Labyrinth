using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Troll : MonoBehaviour
{
    // General References
    public int bossState;
    public Animator animator;
    public float maxHP;
    public float hp;
    public Transform player;
    public Image healthBar;
    public GameObject healthUI;

    // Stage 1 Stuff
    public NavMeshAgent navMeshAgent;
    public float sight;
    public bool seesPlayer;
    public bool isAttacking;
    public LayerMask playerLayer;
    public float attackRange;


    // Stage 2 Stuff
    public Transform[] platforms;
    public int currentPlatform;
    public float timeChecker;
    public float jumpSpeed;
    public GameObject slimeBall;
    public bool isJumping;


    private void Start()
    {
        bossState = 0;
        animator = GetComponent<Animator>();
        hp = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        isAttacking = false;
        currentPlatform = 0;
        timeChecker = 0;
        isJumping = false;
        StartCoroutine(CheckForPlayer());
        StartCoroutine(CheckToAttack());
        StartCoroutine(CheckToEnterState2());
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = hp / maxHP;
    }

    private void FixedUpdate()
    {
        if (bossState == 0)
        {
            if (seesPlayer && !isAttacking)
            {
                navMeshAgent.SetDestination(player.position);
            }
        }
    }

    // Stage 1 Methods

    public IEnumerator CheckForPlayer()
    {
        while (!seesPlayer)
        {
            yield return new WaitForSeconds(1.0f);
            RaycastHit[] player = Physics.SphereCastAll(transform.position, sight, Vector3.forward, 0.1f, playerLayer);
            if (player.Length > 0)
            {
                seesPlayer = true;
                healthUI.SetActive(true);
                animator.SetTrigger("Run");
            }
        }
    }

    public IEnumerator CheckToAttack()
    {
        while (bossState == 0)
        {
            RaycastHit[] player = Physics.SphereCastAll(transform.position, attackRange, Vector3.forward, 0.1f, playerLayer);
            if (player.Length > 0)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
                yield return new WaitForSeconds(0.5f);
                player[0].collider.gameObject.GetComponent<Player>().hitPoints -= 1;
                StartCoroutine(player[0].collider.gameObject.GetComponent<Player>().ShowDamageEffect());
                isAttacking = false;
                Debug.Log("Player was damaged");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator CheckToEnterState2()
    {
        while (bossState == 0)
        {
            yield return new WaitForSeconds(1.0f);
            if (hp < maxHP / 2)
            {
                bossState = 1;
                EnterSecondStage();
            }
        }
    }

    // STAGE 2 METHODS

    public void EnterSecondStage()
    {
        Debug.Log("Entering second stage");
        navMeshAgent.enabled = false;
        StopAllCoroutines();
        player.gameObject.GetComponent<Player>().damageEffect.SetActive(false);
        StartCoroutine(JumpToPlatform());
    }

    public IEnumerator JumpToPlatform()
    {
        if(!isJumping)
        {
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.audioList[9].Play(); 
            isJumping = true;
            int choosenPlatform = Random.Range(0, platforms.Length);
            while (choosenPlatform == currentPlatform)
            {
                choosenPlatform = Random.Range(0, platforms.Length);
            }
            Transform target = platforms[choosenPlatform];
            currentPlatform = choosenPlatform;
            Vector3 newtarget = target.position;
            newtarget.y = transform.position.y;
            transform.LookAt(newtarget);
            animator.StopPlayback();
            animator.SetTrigger("Jump");
            yield return new WaitForSeconds(1.0f);
            float distance = Vector3.Distance(this.transform.position, target.position);
            Vector3 point1 = this.transform.position + new Vector3(0, 6, 0); ;
            Vector3 point2 = this.transform.position + new Vector3(0, distance/2, 0);
            Vector3 point3 = target.position + new Vector3(0, distance/2, 0);
            Vector3 point4 = target.position;

            while (timeChecker < 1)
            {
                timeChecker += Time.deltaTime * jumpSpeed;
                this.transform.position = Mathf.Pow(1 - timeChecker, 3) * point1 +
                    3 * Mathf.Pow(1 - timeChecker, 2) * timeChecker * point2 +
                    3 * (1 - timeChecker) * Mathf.Pow(timeChecker, 2) * point3 +
                    Mathf.Pow(timeChecker, 3) * point4;
                yield return new WaitForEndOfFrame();
            }
            timeChecker = 0.0f;
            //This is where the Troll lands
            isJumping = false;
            jumpSpeed = .75f;
            StartCoroutine(LookAtPlayer());
            StartCoroutine(ThrowSlimeRepeating());
        }
    }

    public IEnumerator LookAtPlayer()
    {
        while (true)
        {
            Vector3 newtarget = player.position;
            newtarget.y = transform.position.y;
            transform.LookAt(newtarget);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator ThrowSlime()
    {
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(0.3f);
        Instantiate(slimeBall, new Vector3(this.transform.position.x, this.transform.position.y + 10, this.transform.position.z), Quaternion.identity);
    }

    public IEnumerator ThrowSlimeRepeating()
    {
        while (true)
        {
            StartCoroutine(ThrowSlime());
            yield return new WaitForSeconds(Random.Range(3, 6));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "FireBall")
        {
            if(hp == maxHP)
            {
                GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gameManager.audioList[9].Play();
                healthUI.SetActive(true);
                seesPlayer = true;
                animator.SetTrigger("Run");
                Debug.Log("HIT FOR THE FIRST TIME!");
            }
            Destroy(collision.gameObject);
            hp -= 2;
            if (hp <= 0)
            {
                Debug.Log("YOU KILLED THE TROLL!");
                StopAllCoroutines();
                StartCoroutine(KillTroll());
            }
            else if (bossState == 1 && !isJumping)
            {
                StopAllCoroutines();
                StartCoroutine(JumpToPlatform());
            }
        }
        else if(collision.gameObject.name == "heavyFireBall")
        {
            if (hp == maxHP)
            {
                GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gameManager.audioList[9].Play();
                healthUI.SetActive(true);
                seesPlayer = true;
                animator.SetTrigger("Run");
                Debug.Log("HIT FOR THE FIRST TIME!");
            }
            Destroy(collision.gameObject);
            hp -= 8;
            if (hp <= 0)
            {
                Debug.Log("YOU KILLED THE TROLL!");
                StopAllCoroutines();
                StartCoroutine(KillTroll());
            }
            else if (bossState == 1 && !isJumping)
            {
                StopAllCoroutines();
                StartCoroutine(JumpToPlatform());
            }
        }
        else if (collision.gameObject.name == "IceBall")
        {
            if (hp == maxHP)
            {
                GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gameManager.audioList[9].Play();
                healthUI.SetActive(true);
                seesPlayer = true;
                animator.SetTrigger("Run");
                Debug.Log("HIT FOR THE FIRST TIME!");
            }
            hp -= 1;
            StartCoroutine(SlowTroll(4));
            Destroy(collision.gameObject);
            if (hp <= 0)
            {
                Debug.Log("YOU KILLED THE TROLL!");
                StopAllCoroutines();
                StartCoroutine(KillTroll());
            }
            else if (bossState == 1 && !isJumping)
            {
                StopAllCoroutines();
                StartCoroutine(JumpToPlatform());
            }
        }
        else if (collision.gameObject.name == "heavyIceBall")
        {
            if (hp == maxHP)
            {
                GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gameManager.audioList[9].Play();
                healthUI.SetActive(true);
                seesPlayer = true;
                animator.SetTrigger("Run");
                Debug.Log("HIT FOR THE FIRST TIME!");
            }
            hp -= 1;
            StartCoroutine(SlowTroll(8));
            Destroy(collision.gameObject);
            if (hp <= 0)
            {
                Debug.Log("YOU KILLED THE TROLL!");
                StopAllCoroutines();
                StartCoroutine(KillTroll());
            }
            else if (bossState == 1 && !isJumping)
            {
                StopAllCoroutines();
                StartCoroutine(JumpToPlatform());
            }
        }
    }

    public IEnumerator KillTroll()
    {
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(3.0f);
        Destroy(healthBar);
        yield return new WaitForSeconds(7.0f);
        Destroy(this.gameObject);
    }

    public IEnumerator SlowTroll(int seconds)
    {
        if(bossState == 0)
        {
            this.gameObject.GetComponent<NavMeshAgent>().speed = 2;
            yield return new WaitForSeconds(seconds);
            this.gameObject.GetComponent<NavMeshAgent>().speed = 5;
        }
        else
        {
            if(seconds == 2)
            {
                jumpSpeed = .6f;
            }
            else
            {
                jumpSpeed = .3f;
            }
            yield return new WaitForSeconds(seconds);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }
}
