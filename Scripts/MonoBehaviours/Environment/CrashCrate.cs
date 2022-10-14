namespace ArionDigital
{
    using UnityEngine;

    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Create")]
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        [Header("Fractured Create")]
        public GameObject fracturedCrate;
        [Header("Audio")]
        public AudioSource crashAudioClip;

        public bool canBeBroken;
        public bool lavaCrate;

        private void OnTriggerEnter(Collider other)
        {
            if(canBeBroken && lavaCrate)
            {
                wholeCrate.enabled = false;
                boxCollider.enabled = false;
                fracturedCrate.SetActive(true);
                crashAudioClip.Play();
                Destroy(gameObject, 0.5f);
            }

            if (other.CompareTag("Projectile") && !lavaCrate)
            {
                wholeCrate.enabled = false;
                boxCollider.enabled = false;
                fracturedCrate.SetActive(true);
                crashAudioClip.Play();
                Destroy(gameObject, 3.0f);
            }
            /*
            else
            {
                wholeCrate.enabled = false;
                boxCollider.enabled = false;
                fracturedCrate.SetActive(true);
                crashAudioClip.Play();
                Destroy(gameObject, 0.5f);
            }
            */
        }

        [ContextMenu("Test")]
        public void Test()
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
        }
    }
}