using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameManager manager;
    public GameObject winScreen;
    public Troll troll;
    public bool playSound = true;
    public CastSpell spell;
    public bool playCoroutine = true;

    // Update is called once per frame
    void Update()
    {
        if (troll.hp <= 0 && playCoroutine)
        {
            StartCoroutine(enableScreen());
            playCoroutine = false;
        }
    }

    IEnumerator enableScreen()
    {
        
            manager.audioList[10].Play();

        
        yield return new WaitForSeconds(2f);
        spell.canCastFire = false;
        spell.canCastIce = false;
        spell.canCastHeal = false;

        
            manager.audioList[7].Stop();
            manager.audioList[11].Play();
            playSound = false;
        
        Time.timeScale = 0;
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
