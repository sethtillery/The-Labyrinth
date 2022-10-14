using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdAreaTrap : MonoBehaviour
{
    public CastSpell cast;
    public Transform thirdTrapMagicSpawn;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        thirdTrapMagicSpawn = this.transform;
        cast = GameObject.Find("Player").GetComponent<CastSpell>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine(Area3Trap());
    }

    IEnumerator Area3Trap()
    {
        int counter = 0;

        while (counter < 6)
        {
            GameObject spellObject = Instantiate(cast.spellList[5].spellPrefab, thirdTrapMagicSpawn.position, thirdTrapMagicSpawn.transform.rotation);
            spellObject.AddComponent<Rigidbody>();
            spellObject.GetComponent<Rigidbody>().useGravity = false;
            spellObject.GetComponent<Rigidbody>().velocity = spellObject.transform.forward * cast.spellList[5].projectileSpeed;
            spellObject.name = cast.spellList[5].spellName;
            spellObject.transform.parent = GameObject.Find("SpellManager").transform;
            counter++;
            gm.audioList[1].Play();

            Destroy(spellObject, 5);
            yield return new WaitForSeconds(1f);
        }
    }
}
