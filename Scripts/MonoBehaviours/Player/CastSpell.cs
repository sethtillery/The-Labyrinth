using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    [HideInInspector]
    public Player character;

    public PlayerMovement movement;
    
    //cooldown time for weapon
    public float coolDownTime = 1;
    public float heavyCoolDownTime = 2;

    //next time player can fire weapon
    public float nextFireTime = 0;
    public float nextHeavyFireTime = 0;

    //spawn point for magic spells
    public Transform magicSpawn;

    public GameManager manager;

    //reference to a spell
    Spell spell;
    public bool useGravity = false;
    public bool canCastFire = false;
    public bool canCastIce = false;
    public bool canCastHeal = false;
    public bool iceAble = false;
    public bool healAble = false;

    //list to hold all spells
    public List<Spell> spellList = new List<Spell>();

    public ParticleSystem healingSpell;

    public int spellIndex = 1;

    public AudioSource healing;

    private void Start()
    {

        movement = this.gameObject.GetComponent<PlayerMovement>(); 
        character = GetComponent<Player>();

        spell = (Spell)Resources.Load("Spells/" + gameObject.name, typeof(Spell));

        List<Spell> spellDatabase = GameObject.Find("SpellManager").GetComponent<SpellManager>().spellList;

        for (int i = 0; i < spellDatabase.Count; i++)
        {
            spellList.Add(spellDatabase[i]);
        }

        manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    IEnumerator spellFireCast()
    {
        yield return new WaitForSeconds(2);
        CastMagic(spellList[2], magicSpawn);
    }


    IEnumerator spellIceCast()
    {
        yield return new WaitForSeconds(2);
        CastMagic(spellList[3], magicSpawn);
    }

    void Update()
    {
        if(!manager.isPaused)
        {
            if(movement.canMove)
            {
                if(Time.time > nextFireTime)
                {
                    if (Input.GetMouseButtonDown(0) && character.currentMana >= 0)
                    {
                        nextFireTime = Time.time + coolDownTime;

                        if(spellIndex == 1 && character.currentMana >= 1 && canCastFire)
                        {
                            useGravity = false;
                            CastMagic(spellList[0], magicSpawn);
                            character.currentMana = character.currentMana - spellList[0].spellManaCost;
                        }
                        else if(spellIndex == 2 && character.currentMana >= 1 && canCastIce)
                        {
                            Debug.Log("casting ice spell");
                            useGravity = false;
                            CastMagic(spellList[1], magicSpawn);
                            character.currentMana = character.currentMana - spellList[1].spellManaCost;
                        }
                        else if (spellIndex == 3 && character.currentMana >= 5 && character.hitPoints < character.maxHitPoints && canCastHeal)
                        {
                            healingSpell.Play();
                            StartCoroutine("healingCo");
                            character.currentMana = character.currentMana - 5;
                            character.AdjustHitPoints(4);
                        }
                        /*
                        if (spell != null)
                        {
                            CastMagic(spell, magicSpawn);
                        }
                        */
                    }

                }
                if (Time.time > nextFireTime)
                {
                    nextHeavyFireTime = Time.time + coolDownTime;

                    if (Input.GetMouseButtonDown(1) && character.currentMana >= 0)
                    {
                        if (spellIndex == 1 && character.currentMana >= 3 && canCastFire)
                        {
                            useGravity = false;
                            StartCoroutine(spellFireCast());
                            //CastMagic(spellList[2]);
                            character.currentMana = character.currentMana - spellList[2].spellManaCost;
                            useGravity = true;
                        }
                        else if (spellIndex == 2 && character.currentMana >= 3 && canCastIce)
                        {
                            useGravity = false;
                            StartCoroutine(spellIceCast());
                           // CastMagic(spellList[3]);
                            character.currentMana = character.currentMana - spellList[3].spellManaCost;
                            useGravity = true;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    spellIndex = 1;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2) && iceAble)
                {
                    spellIndex = 2;
                }

                if (Input.GetKeyDown(KeyCode.Alpha3) && healAble)
                {
                    spellIndex = 3;
                }
            }
        }
    }
   


    public void CastMagic(Spell spell, Transform position)
    {
        if(spell.spellPrefab == null)
        {
            Debug.LogWarning("Spell prefab is equal to null! Must assign a spell prefab!");
            return;
        }
        else if(!useGravity)
        {
            GameObject spellObject = Instantiate(spell.spellPrefab, position.position, Camera.main.GetComponent<Transform>().rotation);
            spellObject.AddComponent<Rigidbody>();
            spellObject.GetComponent<Rigidbody>().useGravity = true;
            spellObject.GetComponent<Rigidbody>().velocity = spellObject.transform.forward * spell.projectileSpeed;
            spellObject.name = spell.spellName;
            spellObject.transform.parent = GameObject.Find("SpellManager").transform;
            manager.audioList[0].Play();

            Destroy(spellObject, 3);
        }
        else
        {
            GameObject spellObject = Instantiate(spell.spellPrefab, position.position, Camera.main.GetComponent<Transform>().rotation);
            spellObject.AddComponent<Rigidbody>();
            spellObject.GetComponent<Rigidbody>().useGravity = false;
            spellObject.GetComponent<Rigidbody>().velocity = spellObject.transform.forward * spell.projectileSpeed;
            spellObject.name = spell.spellName;
            spellObject.transform.parent = GameObject.Find("SpellManager").transform;
            manager.audioList[1].Play();
            Destroy(spellObject, 6);
        }




    }

    IEnumerator healingCo()
    {
        yield return new WaitForSeconds(.2f);

        GetComponent<AudioSource>().time = GetComponent<AudioSource>().clip.length * .5f;

        healing.Play();

    }
}
