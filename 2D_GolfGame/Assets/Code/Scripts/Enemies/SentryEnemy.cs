using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SentryEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 10f; // bullet speed
    [SerializeField] private float damage = 1f; // bullet damage
    [SerializeField] private float time_between_fire = 100f;

    

    private void FixedUpdate()
    {
        if (time_between_fire == 0 && bulletPrefab != null)
        {
            FireBullet();
            time_between_fire = 100f;
        }
        else { time_between_fire -= 1; }
    }

    private void FireBullet()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = bulletSpawnPoint.up * bulletSpeed;
    }
}
