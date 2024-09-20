using UnityEngine;
public class PickUpMushroom : MonoBehaviour
{
    public int mushroomsCollected = 0;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            mushroomsCollected++;
            Destroy(other.gameObject);
        }
    }
}