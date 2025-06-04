using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] TextMeshPro healthText;

    private void Start()
    {
        health = maxHealth;
        healthText.text = maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        healthText.text = health.ToString();
        if (health <= 0)
        {
            PassAway();
        }
    }

    private void PassAway()
    {
        Destroy(gameObject);
    }

    private void Attack()
    {

    }
}
