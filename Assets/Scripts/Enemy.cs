using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float moveSpeed;

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







}
