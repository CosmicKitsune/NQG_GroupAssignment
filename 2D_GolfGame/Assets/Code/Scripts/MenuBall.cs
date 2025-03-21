//cutos to MuddyWolf on YT for this code

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuBall : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr; 
    [SerializeField] private GameObject goalFx;
    [SerializeField] private GameObject playerSprite;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f; //max power applied
    [SerializeField] private float power = 2f; //power applied

    private  bool isPlayerDragging;
    private bool inHole;

    // Update is called once per frame
    private void Update()
    {
        PlayerInput();

        if(!isPlayerDragging && rb.linearVelocity.magnitude > 0.1f)
        {   
            playerSprite.GetComponent<FollowBall>().SetPlayAnimation(); }
    }

    private void PlayerInput ()
    {
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (Input.GetMouseButtonDown(0) && distance <= 0.5f) DragStart(); //checks if first held down
        if (Input.GetMouseButton(0) && isPlayerDragging) DragChange(inputPos); //checks while holding down
        if (Input.GetMouseButtonUp(0) && isPlayerDragging) DragRelease(inputPos); //checks when released
    }

    private void DragStart() {
        isPlayerDragging = true;
        lr.positionCount = 2;
    }
    private void DragChange(Vector2 pos) {
        Vector2 dir = (Vector2)transform.position - pos;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude(dir * power /2, maxPower / 2)); //line isn't 100% accurate 
    }
    private void DragRelease(Vector2 pos) {
        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isPlayerDragging = false;
        lr.positionCount = 0;

        if (distance < 1f) { //cancel drag if not far enough
            return;
        }

        Vector2 dir = (Vector2)transform.position - pos;

        rb.linearVelocity = Vector2.ClampMagnitude(dir * power, maxPower); //clamps max power of the drag
   
         // Notify NPC that the player has flicked
        //FindObjectByType<EnemyBall>()?.OnPlayerFlick(); // Call the NPC's OnPlayerFlick method
        FindObjectsByType<EnemyBall>(FindObjectsSortMode.None).FirstOrDefault()?.OnPlayerFlick();
    }

    private void CheckWinState() {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
        playerSprite.SetActive(false);

        GameObject fx = Instantiate(goalFx, transform.position, Quaternion.identity);
        Destroy(fx, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Level") CheckWinState();
    }
}
