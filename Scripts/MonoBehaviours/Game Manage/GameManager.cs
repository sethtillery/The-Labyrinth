using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Gates to unlock different areas
    public GameObject Area3;
    public GameObject Area4;
    public GameObject Area5;

    //Keys to unlock each of the above areas
    public GameObject area3Key;
    public GameObject area4Key;
    public GameObject area5Key;

    public CastSpell cast;

    public PlayerMovement movement;
    public Player player;
    public GameObject playerObject;

    public Canvas deathScreen;

    public AudioSource[] audioList;

    public int currentCheckpoint = 0;

    public GameObject lavaLevel;
    public GameObject iceLevel;

    // Each instantiable area of the level
    // to be used with the respawn system.
    public GameObject areaToSpawn2;
    public GameObject spawnArea2;
    public Transform area2Location;

    public GameObject areaToSpawn3;
    public GameObject spawnArea3;
    public Transform area3Location;

    public GameObject areaToSpawn4;
    public GameObject spawnArea4;
    public Transform area4Location;

    public GameObject areaToSpawn5;
    public GameObject spawnArea5;
    public Transform area5Location;

    public GameObject areaToSpawnTroll;
    public GameObject spawnAreaTroll;
    public Transform areaTrollLocation;

    // Checkpoints where player will be moved
    // to on respawn
    public Transform checkpoint1;
    public Transform checkpoint2;
    public Transform checkpoint3;
    public Transform checkpoint4;
    public Transform checkpoint5;

    public GameObject pauseMenu;
    public bool gameOver = false;
    public bool isPaused = false;
    public Area4Switch area4Switch;

    public GameObject firstTrapBox;
    public Transform firstTrapMagicSpawn;
    bool shouldSpawnfirstTrapSpell = true;

    public GameObject secondTrapBox;
    public Transform secondTrapMagicSpawn;
    bool shouldSpawnsecondTrapSpell = true;

    public Transform thirdTrapMagicSpawn;
    bool shouldSpawnThirdTrapSpell = true;

    public GameObject redLighting;

    public bool stopsound = true;

    public WinScreen winScreen;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        // If it is the main level scene
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Deep_Tunnel_set"))
        {
            lavaLevel.SetActive(true);
            iceLevel.SetActive(false);
        }

        redLighting.SetActive(false);
        RenderSettings.fog = true;

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) && !gameOver)
        {
            audioList[3].volume = 0.015f;
            isPaused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (player.hitPoints <= 0)
        {
            // Dying behaviour on every scene
            if(stopsound)
            {
                audioList[3].volume = 0.015f;
                audioList[7].Stop();
                stopsound = false;
            }
            deathScreen.gameObject.SetActive(true);
            isPaused = true;
            gameOver = true;
            movement.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // If it is the main level scene
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Deep_Tunnel_set"))
        {
            if (!firstTrapBox && shouldSpawnfirstTrapSpell)
            {
                GameObject spellObject = Instantiate(cast.spellList[4].spellPrefab, firstTrapMagicSpawn.position, firstTrapMagicSpawn.transform.rotation);
                spellObject.AddComponent<Rigidbody>();
                spellObject.GetComponent<Rigidbody>().useGravity = false;
                spellObject.GetComponent<Rigidbody>().velocity = spellObject.transform.forward * cast.spellList[4].projectileSpeed;
                spellObject.name = cast.spellList[4].spellName;
                spellObject.transform.parent = GameObject.Find("SpellManager").transform;
                audioList[1].Play();

                Destroy(spellObject, 3);
                shouldSpawnfirstTrapSpell = false;
            }

            if (!secondTrapBox && shouldSpawnsecondTrapSpell)
            {
                GameObject spellObject = Instantiate(cast.spellList[4].spellPrefab, secondTrapMagicSpawn.position, secondTrapMagicSpawn.transform.rotation);
                spellObject.AddComponent<Rigidbody>();
                spellObject.GetComponent<Rigidbody>().useGravity = false;
                spellObject.GetComponent<Rigidbody>().velocity = spellObject.transform.forward * cast.spellList[4].projectileSpeed;
                spellObject.name = cast.spellList[4].spellName;
                spellObject.transform.parent = GameObject.Find("SpellManager").transform;
                audioList[1].Play();

                Destroy(spellObject, 3);
                shouldSpawnsecondTrapSpell = false;
            }

            if (area4Switch.isIceLevel && !area4Switch.isLavaLevel)
            {
                lavaLevel.SetActive(false);
                iceLevel.SetActive(true);
               // audioList[4].Play();
                audioList[2].volume = 0;
            }
        }
    }
    public void respawn()
    {
        // If it is the main level scene
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Deep_Tunnel_set"))
        {
            if (currentCheckpoint == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (currentCheckpoint == 1)
            {
                if(spawnArea2 != null)
                    Destroy(spawnArea2);
                spawnArea2 = Instantiate(areaToSpawn2);
                spawnArea2.transform.position = area2Location.transform.position;
                if (spawnArea3 != null)
                    Destroy(spawnArea3);
                spawnArea3 = Instantiate(areaToSpawn3);
                spawnArea3.transform.position = area3Location.transform.position;
                playerObject.GetComponent<CharacterController>().enabled = false;
                playerObject.transform.position = checkpoint1.position;
                playerObject.GetComponent<CharacterController>().enabled = true;
            }
            else if (currentCheckpoint == 2)
            {
                if (spawnArea3 != null)
                    Destroy(spawnArea3);

                spawnArea3 = Instantiate(areaToSpawn3);
                spawnArea3.transform.position = area3Location.transform.position;
                playerObject.GetComponent<CharacterController>().enabled = false;
                playerObject.transform.position = checkpoint2.position;
                playerObject.GetComponent<CharacterController>().enabled = true;
            }
            else if (currentCheckpoint == 3)
            {
                if (spawnArea4 != null)
                    Destroy(spawnArea4);

                spawnArea4 = Instantiate(areaToSpawn4);
                spawnArea4.transform.position = area4Location.transform.position;
                playerObject.GetComponent<CharacterController>().enabled = false;
                playerObject.transform.position = checkpoint3.position;
                playerObject.GetComponent<CharacterController>().enabled = true;
            }
            else if (currentCheckpoint == 4)
            {
                if (spawnArea5 != null)
                    Destroy(spawnArea5);

                spawnArea5 = Instantiate(areaToSpawn5);
                spawnArea5.transform.position = area5Location.transform.position;
                playerObject.GetComponent<CharacterController>().enabled = false;
                playerObject.transform.position = checkpoint4.position;
                playerObject.GetComponent<CharacterController>().enabled = true;
            }
            else if (currentCheckpoint == 5)
            {
                if (spawnAreaTroll != null)
                    Destroy(spawnAreaTroll);

                spawnAreaTroll = Instantiate(areaToSpawnTroll);
                spawnAreaTroll.transform.position = areaTrollLocation.transform.position;
                playerObject.GetComponent<CharacterController>().enabled = false;
                playerObject.transform.position = checkpoint5.position;
                playerObject.GetComponent<CharacterController>().enabled = true;
                audioList[7].Play();
                winScreen.troll = GameObject.FindGameObjectWithTag("Troll").GetComponent<Troll>();
            }
        }
        else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TrollBossScene"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Continue Playing behavior for each scene
        player.hitPoints = 10;
        player.currentMana = 10;
        player.gameObject.GetComponent<PlayerMovement>().playerSpeed = 5;
        player.gameObject.GetComponent<Player>().damageEffect.SetActive(false);
        isPaused = false;
        deathScreen.gameObject.SetActive(false);
        movement.canMove = true;
        gameOver = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
