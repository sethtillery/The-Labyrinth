using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrollBoss : MonoBehaviour
{
    public GameObject player;
    public GameObject bossArena;
    public GameManager gm;
    public GameObject rain;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            rain.SetActive(true);
            gm.currentCheckpoint = 5;
            Debug.Log("Player has entered Troll Boss Arena");
            //Instead of loading the boss scene, we are going to go with
            //implementing the boss into the main scene and
            bossArena.SetActive(true);
            //disabling all of the rest of the map
            //to save on CPU usage.
            GameObject.Find("Environment").SetActive(false);
            //Pause Time and put up a loading screen

            //Teleport player to start of Troll Boss
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(23.6f, 1.2f, 21f);
            player.GetComponent<CharacterController>().enabled = true;
            RenderSettings.fog = false;
            gm.redLighting.SetActive(true);
            gm.audioList[3].volume = 0;
            gm.audioList[7].Play();
            
        }
    }
}
