using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EnemyVisionCone2D : MonoBehaviour
{
    [Header("Vision Cone Settings")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public int rayCount = 50;
    public LayerMask obstacleMask;
    public GameObject player;
    public DetectionManager detectionManager;

    private PolygonCollider2D visionCollider;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material visionMaterial;
    private Transform playerTransform;
    private Transform parentTransform; // Reference to the Guard parent
    private bool previousFacingRight; // Tracks the previous facing direction
    private bool isPlayerInTriggerZone = false; // Tracks if the player is in the trigger zone

    void Start()
    {
        visionCollider = GetComponent<PolygonCollider2D>();
        visionCollider.isTrigger = true;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Initialize the material for the vision cone
        visionMaterial = new Material(Shader.Find("Sprites/Default"))
        {
            color = new Color(1, 0, 0, 0.25f) // Semi-transparent red
        };
        visionMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        meshRenderer.material = visionMaterial;
        meshRenderer.sortingLayerName = "Foreground";
        meshRenderer.sortingOrder = 10;

        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("Player GameObject not assigned in EnemyVisionCone2D!");

        parentTransform = transform.parent;
        previousFacingRight = parentTransform.localScale.x < 0;
    }

    void LateUpdate()
    {
        UpdateFacingDirection(); // Flip the vision cone if needed
        UpdateVisionCone();
        DetectPlayer();
    }

    void UpdateFacingDirection()
    {
        bool currentFacingRight = parentTransform.localScale.x < 0;

        if (currentFacingRight != previousFacingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            previousFacingRight = currentFacingRight;
        }
    }

    void UpdateVisionCone()
    {
        float angleIncrement = viewAngle / rayCount;
        float startAngle = -viewAngle / 2f;
        Vector2 origin = transform.position;

        bool facingRight = parentTransform.localScale.x < 0;
        Vector2[] points = new Vector2[rayCount + 2];
        points[0] = Vector2.zero; // Cone origin (local space)

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + angleIncrement * i;
            Vector2 dir = DirectionFromAngle(angle, facingRight);

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, obstacleMask);

            // If ray hits an obstacle, use the hit point; otherwise, use max distance
            points[i + 1] = hit ? transform.InverseTransformPoint(hit.point) : dir * viewDistance;
        }

        visionCollider.pathCount = 1;
        visionCollider.SetPath(0, points);

        UpdateMesh(points);
    }

    void UpdateMesh(Vector2[] points)
    {
        Vector3[] vertices = new Vector3[points.Length];
        int[] triangles = new int[(points.Length - 2) * 3];

        for (int i = 0; i < points.Length; i++)
        {
            vertices[i] = new Vector3(points[i].x, points[i].y, 0);
        }

        for (int i = 0; i < points.Length - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        Mesh visionMesh = new Mesh();
        visionMesh.vertices = vertices;
        visionMesh.triangles = triangles;
        visionMesh.RecalculateNormals();
        meshFilter.mesh = visionMesh;
    }

    void DetectPlayer()
    {
        if (playerTransform == null || detectionManager == null) return;

        Vector3 origin = transform.position;
        Vector3 directionToPlayer = (playerTransform.position - origin).normalized;
        float distanceToPlayer = Vector3.Distance(origin, playerTransform.position);
        float angleToPlayer = Vector3.Angle(transform.up, directionToPlayer);

        bool isVisible = distanceToPlayer <= viewDistance && angleToPlayer <= viewAngle / 2f && !Physics2D.Raycast(origin, directionToPlayer, distanceToPlayer, obstacleMask);

        detectionManager.SetPlayerDetected(isVisible || isPlayerInTriggerZone);

        visionMaterial.color = isVisible ? new Color(1, 0, 0, 0.5f) : new Color(1, 0, 0, 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTriggerZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTriggerZone = false;
        }
    }

    Vector2 DirectionFromAngle(float angleInDegrees, bool facingRight)
    {
        float radians = angleInDegrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        return facingRight ? new Vector2(x, y) : new Vector2(-x, y);
    }
}