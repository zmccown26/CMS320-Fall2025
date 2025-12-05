using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject missilePrefab;

    [Header("Firing")]
    public bool canFire = true;      // you can toggle this if needed

    public void Fire()
    {
        if (!canFire) return;

        if (missilePrefab != null && firePoint != null)
        {
            Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("Turret missing missilePrefab or firePoint", this);
        }
    }
}
