using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Transform player;
    private float originalScaleX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Guardamos el tamaño original en X (evita deformaciones)
        originalScaleX = Mathf.Abs(transform.localScale.x);
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("[EnemyChase] No se encontró un objeto con tag 'Player'");
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        // MOVIMIENTO
        rb.linearVelocity = direction * moveSpeed;

        // FLIP (solo en X)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = originalScaleX * Mathf.Sign(direction.x);
            transform.localScale = scale;
        }
    }
}


