using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 movement;
    

    [Header("Attack Cone")]
    [SerializeField] private GameObject coneObject;
    [SerializeField] private float coneRadius = 2f;
    [SerializeField, Range(10, 360)] private float coneAngle = 60f;
    [SerializeField, Range(3, 100)] private int coneSegments = 30;
    [SerializeField] private float coneVisibleTime = 0.5f;
    [SerializeField] private float waveWidth = 0.5f;

    [Header("Attack")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackDelay;
    private float attackTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coneObject.SetActive(false);
    }

    private void Update()
    {
        HandleMovementInput();
        HandleSpriteFlip();
        IncrementAttackTimer();
        ManageAttack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void HandleSpriteFlip()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)(mouseWorldPos - transform.position);

        spriteRenderer.flipX = direction.x < 0;
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void ManageAttack()
    {
        if (attackTimer >= Mathf.Max(attackDelay, coneVisibleTime))
        {
            Attack();
            attackTimer = 0f;
        }
    }


    private void Attack()
    {
        ShowCone();  
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, coneRadius, enemyLayer);
        if (enemies.Length < 1)
            return;

        foreach (var enemy in enemies)
        {
            Vector2 toEnemy = enemy.transform.position - transform.position;
            float angleToEnemy = Vector2.Angle(GetConeDirection(), toEnemy);

            if (angleToEnemy <= coneAngle / 2f)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

   

    private Vector2 GetConeDirection()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return ((Vector2)(mouseWorldPos - transform.position)).normalized;
    }

    private void ShowCone()
    {
        MeshFilter mf = coneObject.GetComponent<MeshFilter>();
        MeshRenderer mr = coneObject.GetComponent<MeshRenderer>();

        Mesh mesh = GenerateConeMesh(coneAngle, coneRadius, coneSegments);
        mf.mesh = mesh;

        Material coneMaterial = mr.material;

        coneMaterial.SetFloat("_WaveRadius", 0f);
        coneMaterial.SetFloat("_WaveWidth", waveWidth); 

        Vector2 direction = GetConeDirection();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        coneObject.transform.position = transform.position;
        coneObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        coneObject.SetActive(true);

        StartCoroutine(AnimateConeWave(coneMaterial));
        StartCoroutine(HideConeAfterDelay(coneVisibleTime));
    }


    private IEnumerator AnimateConeWave(Material material)
    {
        float elapsedTime = 0f;

        while (elapsedTime < coneVisibleTime)
        {
            float radius = Mathf.Lerp(0f, 1f, elapsedTime / coneVisibleTime);
            material.SetFloat("_WaveRadius", radius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.SetFloat("_WaveRadius", 1f);
    }

    private IEnumerator HideConeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        coneObject.SetActive(false);
    }

    private Mesh GenerateConeMesh(float angleDeg, float radius, int segments)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3> { Vector3.zero };
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2> { new Vector2(0.5f, 0.5f) };

        float angleRad = Mathf.Deg2Rad * angleDeg;
        float step = angleRad / segments;

        for (int i = 0; i <= segments; i++)
        {
            float theta = -angleRad / 2 + step * i;
            Vector3 point = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * radius;
            vertices.Add(point);

            Vector2 uv = new Vector2(point.x / (radius * 2f) + 0.5f, point.y / (radius * 2f) + 0.5f);
            uvs.Add(uv);

            if (i > 0)
            {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneRadius);
    }
}
