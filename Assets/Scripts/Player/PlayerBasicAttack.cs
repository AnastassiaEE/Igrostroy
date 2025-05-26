using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Attack State")]
    private float attackTimer;
    private bool isAttackEnabled;
    private bool isAttacking;

    [Header("Attack UI & FX")]
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private AudioClip attackSound;
    private AudioSource audioSource;

    [Header("Cone Visualization")]
    [SerializeField] private GameObject coneObject;
    [SerializeField] private float coneRadius = 2f;
    [SerializeField, Range(10, 360)] private float coneAngle = 60f;
    [SerializeField, Range(3, 100)] private int coneSegments = 30;
    [SerializeField] private float waveWidth = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool gizmos;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coneObject.SetActive(false);
        EnableAttack(true); 
    }

    // Update is called once per frame
    void Update()
    {
        ManageAttack();
        HandleAttackToggle();
    }

    private void ManageAttack()
    {
        if (!isAttackEnabled) return;
        if (isAttacking) return;
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void Attack()
    {
        isAttacking = true;
        ShowCone();
        DetectEnemies();
        audioSource.PlayOneShot(attackSound);
    }

    private void DetectEnemies()
    {
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

    private void HandleAttackToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EnableAttack(!isAttackEnabled);
        }
    }

    private void EnableAttack(bool isEnabled)
    {
        attackIndicator.SetActive(isEnabled);
        isAttackEnabled = isEnabled;
        attackTimer = 0;
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

        StartCoroutine(AnimateCone(coneMaterial, attackDuration));
        StartCoroutine(HideConeAfter(attackDuration));
    }

    private IEnumerator AnimateCone(Material material, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float radius = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            material.SetFloat("_WaveRadius", radius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.SetFloat("_WaveRadius", 1f);
    }

    private IEnumerator HideConeAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        coneObject.SetActive(false);
        isAttacking = false;
    }

    private Mesh GenerateConeMesh(float angleDeg, float radius, int segments)
    {
        Mesh mesh = new();
        List<Vector3> vertices = new() { Vector3.zero };
        List<int> triangles = new();
        List<Vector2> uvs = new() { new Vector2(0.5f, 0.5f) };

        float angleRad = Mathf.Deg2Rad * angleDeg;
        float step = angleRad / segments;

        for (int i = 0; i <= segments; i++)
        {
            float theta = -angleRad / 2 + step * i;
            Vector3 point = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * radius;
            vertices.Add(point);

            Vector2 uv = new(point.x / (radius * 2f) + 0.5f, point.y / (radius * 2f) + 0.5f);
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
        if (!gizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneRadius);
    }

}
