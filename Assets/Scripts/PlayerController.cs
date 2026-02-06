using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runMultiplier = 1.5f;

    [Header("Footsteps")]
    [SerializeField] private AudioClip footstepWalk;
    [SerializeField] private AudioClip footstepRun;
    [SerializeField] private float walkStepInterval = 0.45f;
    [SerializeField] private float runStepInterval = 0.28f;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    private Vector2 input;
    private float originalScaleX;
    private bool isRunning;
    private float footstepTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        audioSource.loop = false;

        originalScaleX = Mathf.Abs(transform.localScale.x);
    }

    private void Update()
    {
        // INPUT
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        isRunning = Input.GetKey(KeyCode.LeftShift);

        // FLIP
        if (input.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = originalScaleX * Mathf.Sign(input.x);
            transform.localScale = scale;
        }

        // ANIMATOR
        bool isMoving = input.sqrMagnitude > 0.01f;
        animator.SetBool("Walk", isMoving && !isRunning);
        animator.SetBool("Run", isMoving && isRunning);

        // FOOTSTEPS
        HandleFootsteps(isMoving);
    }

    private void FixedUpdate()
    {
        float speed = moveSpeed * (isRunning ? runMultiplier : 1f);
        rb.linearVelocity = input * speed; // ✅ CORRECTO
    }

    private void HandleFootsteps(bool isMoving)
    {
        if (!isMoving)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= Time.deltaTime;
        if (footstepTimer > 0f) return;

        AudioClip clip = isRunning ? footstepRun : footstepWalk;
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
        footstepTimer = isRunning ? runStepInterval : walkStepInterval;
    }
}




