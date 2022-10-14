using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStaff : MonoBehaviour
{

    //lights on each staff
    public GameObject fireStaff;
    public GameObject iceStaff;
    public GameObject healStaff;
    public bool hasFireStaff = false;
    public bool hasIceStaff = false;
    public bool hasHealStaff = false;


    //assigns a value to each staff
    public int staffIndex;

    // Start is called before the first frame update
    void Start()
    {
        staffIndex = 1;

        fireStaff.SetActive(false);
        iceStaff.SetActive(false);
        healStaff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasFireStaff)
        {
            fireStaff.SetActive(true);
            iceStaff.SetActive(false);
            healStaff.SetActive(false);


            staffIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasIceStaff)
        {
            fireStaff.SetActive(false);
            iceStaff.SetActive(true);
            healStaff.SetActive(false);

            staffIndex = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && hasHealStaff)
        {
            fireStaff.SetActive(false);
            iceStaff.SetActive(false);
            healStaff.SetActive(true);

            staffIndex = 3;
        }

    }
}
