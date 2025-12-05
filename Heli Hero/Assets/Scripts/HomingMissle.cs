using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class HomingMissile : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 220f;
    public float lifeTime = 3f;

    [Header("Effects")]
    public GameObject explosionPrefab;
    [Tooltip("Enable to make missiles explode on ground. Requires 'Ground' tag to be created in Unity.")]
    public bool explodeOnGround = false;

    private Rigidbody2D rb;
    private Transform target;
    private bool hasExploded = false;
    private Coroutine lifetimeCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (Lander.Instance != null)
            target = Lander.Instance.transform;

        // Start lifetime coroutine
        lifetimeCoroutine = StartCoroutine(LifetimeCountdown());
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            rb.linearVelocity = transform.right * speed;
            return;
        }

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        float rotateAmount = Vector3.Cross(dir, transform.right).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        Lander lander = other.GetComponent<Lander>();
        if (lander != null)
        {
            lander.CrashFromHazard();
            Explode();
            return;
        }

        // Optional: Check for Ground tag (only if enabled and tag exists)
        if (explodeOnGround)
        {
            try
            {
                if (other.CompareTag("Ground"))
                {
                    Explode();
                }
            }
            catch
            {
                // Ground tag doesn't exist - disable this feature to avoid errors
                explodeOnGround = false;
                Debug.LogWarning("HomingMissile: 'Ground' tag not found. Ground explosion disabled. Create the tag in Edit > Project Settings > Tags and Layers to enable.", this);
            }
        }
    }

    private IEnumerator LifetimeCountdown()
    {
        yield return new WaitForSeconds(lifeTime);
        
        // Time's up - explode
        if (!hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Stop the lifetime coroutine if it's still running
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }

        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
            Debug.Log($"[HomingMissile] Exploded at position {transform.position}. Explosion prefab was assigned.", this);
        }
        else
        {
            Debug.LogWarning($"[HomingMissile] Exploded at position {transform.position}, but explosionPrefab is not assigned in Inspector! No visual effect will appear.", this);
        }

        Destroy(gameObject);
    }
}
