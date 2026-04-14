using UnityEngine;


public class GemPickup : MonoBehaviour
{
    public int gemValue = 1;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player nhặt gem");


            PlayerGem playerGem = other.GetComponentInParent<PlayerGem>();
            if (playerGem != null)
            {
                playerGem.AddGem(gemValue);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy PlayerGem!");
            }


            Destroy(gameObject);
        }
    }
}
