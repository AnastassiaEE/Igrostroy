using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update() {
        HandleMovementInput();
        HandleSpriteFlip();
    }

    void FixedUpdate() => Move();

    private void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    private void HandleSpriteFlip()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - transform.position);
        spriteRenderer.flipX = direction.x < 0;
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
