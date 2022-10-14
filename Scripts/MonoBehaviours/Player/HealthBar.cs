using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Reference to the current Player object to get hit point fields
    // Will be set programmatically, instead of through the Unity Editor, so it is hidden in the Inspector window
    [HideInInspector]
    public Player character;

    // For convenience, a direct reference to the health bar meter; set through the Unity Editor
    public Image meterImage;
    
    // Update is called once per frame
    void Update()
    {
        if (character != null)
        {
            // set the meter's fill amount; must be a value between 0 and 1
            meterImage.fillAmount = character.hitPoints / character.maxHitPoints;
        }
    }
}
