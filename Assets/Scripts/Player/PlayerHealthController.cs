using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public PlayerController player;
    public PickUpMushroom playerPickUp;
    public Image[] hearts;
    
    public Slider mushroomSlider;
    
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.playerHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        mushroomSlider.value = playerPickUp.mushroomsCollected;
    }
}