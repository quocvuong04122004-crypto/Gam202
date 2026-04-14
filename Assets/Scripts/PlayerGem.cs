using UnityEngine;


public class PlayerGem : MonoBehaviour
{
    public int totalGem = 0;


    public void AddGem(int amount)
    {
        totalGem += amount;
        Debug.Log("Gem: " + totalGem);
    }
}
