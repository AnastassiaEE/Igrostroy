using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private int maxDashes = 2;
    [SerializeField] private float dashesResetDelay = 10f; 
    [SerializeField] private float invisibilityDuration = 0.15f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dashesText;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite dashSprite;

    private int dashes;
    private bool isDashing = false;

    private bool isInvisible;
    public bool IsInvisible
    {
        get => isInvisible;         
        private set                  
        {
            isInvisible = value;                           
        }
    }

    private Rigidbody2D rb;
    private Vector2 movement;


    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start() {
        spriteRenderer.sprite = normalSprite;
        dashes = maxDashes;
        UpdateDashUI();
        StartCoroutine(ResetDashes());
    }


    void Update() {
        HandleMovementInput();
        HandleSpriteFlip();
        HandleDashInput();
    }

    void FixedUpdate() {
        if (!isDashing) Move();
    }

    private void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void HandleSpriteFlip()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - transform.position;
        spriteRenderer.flipX = direction.x < 0;
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleDashInput()
    {
        if (CanDash() && Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 dashDirection = movement == Vector2.zero
                ? (spriteRenderer.flipX ? Vector2.left : Vector2.right)
                : movement.normalized;

            StartCoroutine(Dash(dashDirection));
        }
    }

    private IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        dashes = Mathf.Max(0, dashes - 1);
        UpdateDashUI();

        StartCoroutine(ApplyInvincibility());

        spriteRenderer.sprite = dashSprite;

        float startTime = Time.time;

        Vector2 start = rb.position;
        Vector2 end = start + direction * dashDistance;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            rb.MovePosition(Vector2.Lerp(start, end, t));
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(end);
        isDashing = false;

        spriteRenderer.sprite = normalSprite;
    }

    private IEnumerator ApplyInvincibility()
    {
        IsInvisible = true;
        yield return new WaitForSeconds(invisibilityDuration);
        IsInvisible = false;
    }

    private void UpdateDashUI()
    {
        dashesText.text = $"Dashes: {dashes}";
    }

    private IEnumerator ResetDashes()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashesResetDelay);
            if (dashes < maxDashes)
            {
                dashes = maxDashes;
                UpdateDashUI();
            }
        }
    }

    private bool CanDash()
    {
        return !isDashing && dashes > 0;
    }
}
