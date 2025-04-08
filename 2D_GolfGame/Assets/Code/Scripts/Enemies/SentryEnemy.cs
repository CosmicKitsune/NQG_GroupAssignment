using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class SentryEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform bulletSpawnPoint;
    [SerializeField] public GameObject bulletPrefab;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed; // bullet speed
    [SerializeField] private float damage = 1f; // bullet damage
    [SerializeField]  private float bulletTimer; //how many bullets can spawn at once

    public AudioManager audioManager;

    private void FixedUpdate()
    {
        if (bulletTimer == 0)
        {
            FireBullet(); 
        } else { bulletTimer -= 1; }
    }

    private void FireBullet()
    {
        audioManager.PlaySFX(audioManager.bulletFire);
        bulletTimer = 50f;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.up * bulletSpeed;
    }
}
