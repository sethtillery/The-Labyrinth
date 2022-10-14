using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //reference to players ccharacter controller
    public CharacterController controller;

    public Player player;

    //players movement speed
    public float playerSpeed = 12f;

    //gravitty modifier
    public float gravity = -9.81f;

    //players velocity
    Vector3 velocity;

    //checks to see if player is on ground
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpForce = 3f;
    bool isGrounded;
    public SwitchStaff switchStaff;
    public CastSpell castSpell;

    public Text fireStaffPickupText;
    public Text iceStaffPickupText;
    public Text healStaffPickupText;
    public Text keyPickupText;
    public Text newAreaUnlockedText;
    public Text healUseText;
    public Text iceUseText;

    public GameObject slimeBallEffect;
    public GameObject damageEffect;
    public GameManager manager;

    public Transform checkpoint1;
    public bool canMove = true;

    public AudioSource footsteps;
    public GameObject ghostTrigger;
    public GameObject ghost;

    public GameObject area3TrapTrigger;

    public GameObject[] Area2Enemies;

    [SerializeField] Footsteps soundGenerator;
    bool isWalking = false;
    public float footStepTimer;

    private void Awake()
    {
        canMove = true;
    }


    private void Start()
    {
        soundGenerator = GetComponent<Footsteps>();
    }
    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * playerSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
            
            if(move.x < 0 || move.x > 0)
            {
                if (isGrounded)
                {
                    if (!isWalking)
                    {
                        PlayFootSound();
                    }
                }
                
            }
   


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TrollSlimeBall(Clone)" || other.gameObject.name == "SlimePuddle(Clone)")
        {
            StartCoroutine(SlowPlayer());
            StartCoroutine(ShowSlimeEffect());
        }

        if (other.CompareTag("fireStaffPickup"))
        {
            fireStaffPickupText.gameObject.SetActive(true);
        }

        if (other.CompareTag("iceStaffPickup"))
        {
            iceStaffPickupText.gameObject.SetActive(true);
        }

        if (other.CompareTag("healStaffPickup"))
        {
            healStaffPickupText.gameObject.SetActive(true);
        }

        if (other.CompareTag("key"))
        {
            keyPickupText.gameObject.SetActive(true);
        }

        if(other.CompareTag("Projectile"))
        {
            player.hitPoints -= 2;
        }

        if(other.CompareTag("lava"))
        {
            player.hitPoints = 0;
        }

        if(other.gameObject.name == "Soundtrigger")
        {
            manager.audioList[6].Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("fireStaffPickup") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("picked up");
            fireStaffPickupText.gameObject.SetActive(false);
            Destroy(other.gameObject);
            castSpell.canCastFire = true;
            switchStaff.fireStaff.SetActive(true);
            switchStaff.hasFireStaff = true;
            manager.audioList[5].Play();
        }

        else if (other.CompareTag("healStaffPickup") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("picked up");
            healStaffPickupText.gameObject.SetActive(false);
            Destroy(other.gameObject);
            castSpell.canCastHeal = true;
            switchStaff.fireStaff.SetActive(false);
            switchStaff.healStaff.SetActive(true);
            switchStaff.hasHealStaff = true;
            castSpell.spellIndex = 3;
            castSpell.healAble = true;
            StartCoroutine(showHealText());
            switchStaff.staffIndex = 3;
            ghostTrigger.SetActive(true);
            manager.audioList[5].Play();
        }

        else if (other.CompareTag("iceStaffPickup") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("picked up");
            iceStaffPickupText.gameObject.SetActive(false);
            Destroy(other.gameObject);
            castSpell.canCastIce = true;
            switchStaff.fireStaff.SetActive(false);
            switchStaff.iceStaff.SetActive(true);
            switchStaff.hasIceStaff = true;
            castSpell.iceAble = true;
            StartCoroutine(showIceText());
            castSpell.spellIndex = 2;
            switchStaff.staffIndex = 2;
            manager.audioList[5].Play();
        }

        else if (other.gameObject.name == "Area 3 Key" && Input.GetKeyDown(KeyCode.E))
        {
            manager.audioList[5].Play();
            manager.Area3.SetActive(false);
            keyPickupText.gameObject.SetActive(false);
            Destroy(other.gameObject);
            manager.currentCheckpoint = 1;
            StartCoroutine(showText());
            foreach(GameObject enemy in Area2Enemies)
            {
                enemy.SetActive(true);
            }

            Debug.Log("PICKED UP AREA 3 KEY!! Current Checkpoint == " + manager.currentCheckpoint.ToString());
        }

        else if (other.gameObject.name == "Area 4 Key" && Input.GetKeyDown(KeyCode.E))
        {
            manager.audioList[5].Play();
            manager.Area4.SetActive(false);
            manager.checkpoint3.gameObject.SetActive(true);
            keyPickupText.gameObject.SetActive(false);
            Destroy(other.gameObject);
            manager.currentCheckpoint = 2;
            StartCoroutine(showText());
            area3TrapTrigger.SetActive(true);
            Debug.Log("PICKED UP AREA 4 KEY!! Current Checkpoint == " + manager.currentCheckpoint.ToString());
        }

        else if(other.gameObject.name == "checkpoint3")
        {
            manager.currentCheckpoint = 3;
            Debug.Log("Hit checkpoint3! Current Checkpoint == " + manager.currentCheckpoint.ToString());
        }

        else if (other.gameObject.name == "Area 5 Key" && Input.GetKeyDown(KeyCode.E))
        {
            manager.audioList[5].Play();
            manager.Area5.SetActive(false);
            keyPickupText.gameObject.SetActive(false);
            manager.checkpoint4.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(showText());
            Debug.Log("PICKED UP AREA 5 KEY!! Current Checkpoint == " + manager.currentCheckpoint.ToString());
        }

        else if (other.gameObject.name == "checkpoint4")
        {
            manager.checkpoint3.gameObject.SetActive(false);
            manager.currentCheckpoint = 4;
            Debug.Log("Hit checkpoint4! Current Checkpoint == " + manager.currentCheckpoint.ToString());
        }

        else if (other.gameObject.name == "GhostTrigger")
        {
            ghost.SetActive(true);
        }

        else if(other.gameObject.name == "lava sound trigger")
        {
            manager.audioList[3].volume = 0.015f;
            manager.audioList[2].volume = 1;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("fireStaffPickup"))
        {
            fireStaffPickupText.gameObject.SetActive(false);
        }

        if (other.CompareTag("iceStaffPickup"))
        {
            iceStaffPickupText.gameObject.SetActive(false);
        }

        if (other.CompareTag("healStaffPickup"))
        {
            healStaffPickupText.gameObject.SetActive(false);
        }

        if (other.CompareTag("key"))
        {
            keyPickupText.gameObject.SetActive(false);
        }

        if (other.gameObject.name == "lava sound trigger")
        {
            manager.audioList[2].volume = 0;
            manager.audioList[3].volume = 0.193f;
        }
    }

    void PlayFootSound()
    {
        StartCoroutine("playStepSound", footStepTimer);
    }

    public IEnumerator showText()
    {
        newAreaUnlockedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        newAreaUnlockedText.gameObject.SetActive(false);
    }

    public IEnumerator showIceText()
    {
        iceUseText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        iceUseText.gameObject.SetActive(false);
    }

    public IEnumerator showHealText()
    {
        healUseText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        healUseText.gameObject.SetActive(false);
    }

    public IEnumerator SlowPlayer()
    {
        playerSpeed = 1;
        yield return new WaitForSeconds(3.0f);
        playerSpeed = 4;
    }

    public IEnumerator ShowSlimeEffect()
    {
        slimeBallEffect.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        slimeBallEffect.SetActive(false);
    }

    public IEnumerator playStepSound(float timer)
    {
        var randomIndex = Random.Range(0, 5);
        soundGenerator.stepAudio.clip = soundGenerator.footStepSounds[randomIndex];

        soundGenerator.stepAudio.Play();
        
        isWalking = true;

        yield return new WaitForSeconds(timer);

        isWalking = false;

    }

}
