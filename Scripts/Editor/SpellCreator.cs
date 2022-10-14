using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpellCreator : EditorWindow
{
    [MenuItem("Spell Maker/Spell Wizard")]

    static void Init()
    {
        SpellCreator spellWindow = (SpellCreator)CreateInstance(typeof(SpellCreator));
        spellWindow.Show();
    }

    Spell tempSpell = null;
    SpellManager spellManager = null;

    void OnGUI()
    {
        if(spellManager == null)
        {
            spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();
        }

        if(tempSpell)
        {
            tempSpell.spellName = EditorGUILayout.TextField("Spell Name", tempSpell.spellName);
            tempSpell.spellPrefab = (GameObject)EditorGUILayout.ObjectField("Spell Prefab", tempSpell.spellPrefab, typeof(GameObject), false);
            tempSpell.spellCollisionParticle = (GameObject)EditorGUILayout.ObjectField("Spell Collision Effect", tempSpell.spellCollisionParticle, typeof(GameObject), false);
            tempSpell.spellManaCost = EditorGUILayout.IntField("Mana Cost", tempSpell.spellManaCost);
            tempSpell.spellDamage = EditorGUILayout.IntField("Damage", tempSpell.spellDamage);
            tempSpell.projectileSpeed = EditorGUILayout.IntField("Projectile Speed", tempSpell.projectileSpeed);
        }

        EditorGUILayout.Space();

        if(tempSpell == null)
        {
            if(GUILayout.Button("Create Spell"))
            {
                tempSpell = CreateInstance<Spell>();
            }
        }
        else
        {
            if(GUILayout.Button("Create Scriptable Object"))
            {
                AssetDatabase.CreateAsset(tempSpell, "Assets/Prefabs/Spells/" + tempSpell.spellName + ".asset");
                AssetDatabase.SaveAssets();
                spellManager.spellList.Add(tempSpell);
                Selection.activeObject = tempSpell;

                tempSpell = null;
            }

            if(GUILayout.Button("Reset"))
            {
                Reset();
            }
        }
    }

    void Reset()
    {
        if(tempSpell)
        {
            tempSpell.spellName = "";
            tempSpell.spellDamage = 0;
            tempSpell.spellManaCost = 0;
            tempSpell.spellPrefab = null;
            tempSpell.spellCollisionParticle = null;
        }
    }

}
