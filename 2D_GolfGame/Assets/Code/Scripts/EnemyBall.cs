using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform playerTransform; // Reference to the player's transform

    [SerializeField] private GameObject deathFx;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f; // Maximum power applied
    [SerializeField] private float power = 2f; // Power applied
    [SerializeField] private float activationDistance = 5f; // Distance to activate flick
    [SerializeField] private float health = 2f; // Maximum power applied

    private bool inMotion;
    private bool takenDamage;
    private bool playerFlicked = false; // Track if the player has flicked
    private void Update()
    {
        // Trigger movement if player moves and NPC is within range
        if (Vector2.Distance(transform.position, playerTransform.position) <= activationDistance && !inMotion && playerFlicked)
        {
            StartFlick();
            playerFlicked = false;
        }

        // Update LineRenderer during motion
        if (inMotion)
        {
            UpdateLineRenderer();
        }
    }

    private void StartFlick()
    {
        lr.positionCount = 2;

        // Calculate direction towards player
        Vector2 dir = (Vector2)playerTransform.position - (Vector2)transform.position;

        // Set initial LineRenderer positions
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude(dir * power / 2, maxPower / 2));

        // Apply force to flick towards the player
        rb.linearVelocity = Vector2.ClampMagnitude(dir * power, maxPower);

        inMotion = true;
        StartCoroutine(StopFlickAfterMotion());
    }

    public void TakeDamage(float damage) 
    { 
        if (!takenDamage)
        { health -= damage;
        takenDamage = true;
        Debug.Log($"Take damage {health} at {takenDamage}");
        }
        
        if (health <= 0f) { 
            GameObject fx = Instantiate(deathFx, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
            Destroy(gameObject); }
    }

    private IEnumerator StopFlickAfterMotion()
    {
        yield return new WaitForSeconds(0.8f); // Adjust time as needed
        inMotion = false;
        takenDamage = false;
        lr.positionCount = 0;
    }

    private void UpdateLineRenderer()
    {
        Vector2 dir = (Vector2)playerTransform.position - (Vector2)transform.position;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude(dir * power / 2, maxPower / 2));
    }

    // Method to be called when the player flicks
    public void OnPlayerFlick()
    {
        playerFlicked = true; // Allow NPC to flick after the player flicks
    }
}
