using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform player;
    public PlayerMovement playerMovement;
    public float speed = .01f;
    public GameObject ghostScreen;
    public int currentSpawn;
    public Transform[] spawnPoints;
    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        currentSpawn = 2;
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, player.position, speed);
        this.transform.LookAt(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(playerMovement.SlowPlayer());
            StartCoroutine(GhostScreen());
        }
    }

    public IEnumerator GhostScreen()
    {
        ghostScreen.SetActive(true);
        manager.audioList[12].Play();
        MoveGhost();
        yield return new WaitForSeconds(5.0f);
        ghostScreen.SetActive(false);
    }

    public void MoveGhost()
    {
        int newSpawn = Random.Range(0, spawnPoints.Length);
        while(newSpawn == currentSpawn)
        {
            newSpawn = Random.Range(0, spawnPoints.Length);
        }
        this.transform.position = spawnPoints[newSpawn].position;
    }

}
