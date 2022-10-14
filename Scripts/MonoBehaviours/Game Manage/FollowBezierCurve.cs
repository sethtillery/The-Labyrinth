using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBezierCurve : MonoBehaviour
{
    public GameObject target;
    public float speed;
    Vector3 point1;
    Vector3 point2;
    Vector3 point3;
    Vector3 point4;

    public bool towardsPlayer;

    public float timeChecker;


    // Start is called before the first frame update
    void Start()
    {
        timeChecker = 0.0f;
        speed = 0.5f;
        if (towardsPlayer)
        {
            target = GameObject.Find("Player");
        }

        if (target != null && this.CompareTag("Projectile"))
        {
            point1 = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
            point2 = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 10, this.transform.localPosition.z);
            point3 = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y + 10, target.transform.localPosition.z);
            point4 = new Vector3(target.transform.localPosition.x, -1.0f, target.transform.localPosition.z);
            StartCoroutine(FollowPath());
        }
        else if(target != null)
        {
            point1 = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
            point2 = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 10, this.transform.localPosition.z);
            point3 = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y + 10, target.transform.localPosition.z);
            point4 = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, target.transform.localPosition.z);
        }
    }

    public IEnumerator FollowPath()
    {
        while (timeChecker < 1)
        {
            timeChecker += Time.deltaTime * speed;
            this.transform.position = Mathf.Pow(1 - timeChecker, 3) * point1 +
                3 * Mathf.Pow(1 - timeChecker, 2) * timeChecker * point2 +
                3 * (1 - timeChecker) * Mathf.Pow(timeChecker, 2) * point3 +
                Mathf.Pow(timeChecker, 3) * point4;
            yield return new WaitForEndOfFrame();
        }
        timeChecker = 0.0f;
    }
}
