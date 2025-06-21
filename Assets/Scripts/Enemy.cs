using System;
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

    [Header("Attack")]
    [SerializeField] private int damage;

    void Start()
    {
        health = maxHealth;
        healthText.text = maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
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
        player.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null) return;
        if (collision.gameObject == player.gameObject && !player.IsInvisible)
        {
            Attack();
        }
    }
}
