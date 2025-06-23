using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
   

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
       
    }

    void Update()
    {
  
    }

    public bool IsInvisible => playerMovement.IsInvisible;

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

}
