using UnityEngine;


// public class WeaponAttributes : MonoBehaviour
// {
//     public AttributesManager atm;


//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.transform.root == transform.root) return;


//         // Player đánh Enemy
//         if (other.CompareTag("Enemy"))
//         {
//             atm.DealDamage(other.gameObject);
//         }


//         // Enemy đánh Player
//         if (other.CompareTag("Player"))
//         {
//             Debug.Log("Enemy chem Player");


//             atm.DealDamage(other.gameObject); //  DÙNG CHUNG
//         }
//     }
// }


public class WeaponAttributes : MonoBehaviour
{
    private AttributesManager atm;


    private void Awake()
    {
        atm = GetComponentInParent<AttributesManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == transform.root) return;


        if (other.CompareTag("Enemy"))
        {
            atm.DealDamage(other.gameObject);
        }


        if (other.CompareTag("Player"))
        {
            atm.DealDamage(other.gameObject);
            Debug.Log("Enemy chem Player");
        }
    }
}
