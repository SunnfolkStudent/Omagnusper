using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public PlayerController player;
    public Image[] hearts;
    
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
    }
}