using System.Collections;
using UnityEngine;

public class TurretAmbushTrigger : MonoBehaviour
{
    public TurretController turret;
    public float delay = 0.4f;
    public bool onlyOnce = true;

    [Header("Safety")]
    [Tooltip("Ignore triggers for the first X seconds after the level loads")]
    public float minActivationTime = 0.5f;
    [Tooltip("Require some horizontal movement so we don't trigger when the lander is basically stationary in the zone")]
    public float minHorizontalSpeed = 0.1f;

    private bool hasFired;
    private bool isFiring;

    private void Awake()
    {
        // Auto-assign turret if not set - look for TurretController in parent or siblings
        if (turret == null)
        {
            turret = GetComponentInParent<TurretController>();
            if (turret == null)
            {
                // Try to find it in the same GameObject's siblings (if trigger is a child)
                Transform parent = transform.parent;
                if (parent != null)
                {
                    turret = parent.GetComponentInChildren<TurretController>();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onlyOnce && hasFired) return;

        Lander lander = other.GetComponent<Lander>();
        if (lander == null) return;

        // Don't trigger while still on start screen or immediately on spawn
        if (!lander.IsInNormalState()) return;
        if (Time.timeSinceLevelLoad < minActivationTime) return;

        // Make sure the player is actually moving through the zone, not just sitting/spawning in it
        if (Mathf.Abs(lander.GetSpeedX()) < minHorizontalSpeed) return;

        if (!isFiring)
        {
            StartCoroutine(FireAfterDelay());
        }
    }

    private IEnumerator FireAfterDelay()
    {
        isFiring = true;
        yield return new WaitForSeconds(delay);

        // Double-check game is still running before firing
        if (turret == null)
        {
            Debug.LogError($"TurretAmbushTrigger on {gameObject.name}: turret reference is null! Make sure the turret is assigned in the Inspector.", this);
            isFiring = false;
            yield break;
        }

        if (Lander.Instance != null && Lander.Instance.IsInNormalState())
        {
            turret.Fire();
            hasFired = true;
        }

        isFiring = false;
    }
}
