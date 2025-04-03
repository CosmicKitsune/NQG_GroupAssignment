//cutos to MuddyWolf on YT for this code

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr; 
    [SerializeField] private GameObject goalFx;
    [SerializeField] private GameObject waterFx;
    [SerializeField] private GameObject ballFx;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private Weapon[] weaponList;

    public float maxPower; //max power applied
    public float power; //power applied
    public float maxGoalSpeed;
    public float maxVelocity;
    public int maxBounceCount;
    public float meleeSpeed; //weapon stat
    public float damage; //weapon stat
    

    private bool isPlayerDragging;
    private bool isGameOver;
    private bool inHole;
    private bool inBunker;
    private bool isWeapPutter;
    private Vector3 originalPos;
    private int collisionCount;
   
    public Weapon currentWeapon;
    public Sprite weaponSprite;
    public string weaponName;
    int swordSwingCount; //weapon stat
    float timeUntilMelee; //weapon stat

    private void Start()
    {
        InitializePlayerData(); 
        LoadPlayerData();
        LoadWeaponData();
        //Debug.Log($"{maxPower} {power} {maxGoalSpeed} {maxVelocity} {maxBounceCount} {meleeSpeed} {damage}");
        originalPos = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void LoadPlayerData()
    {
        maxPower = PlayerData.maxPower;
        power = PlayerData.power;
        maxGoalSpeed = PlayerData.maxGoalSpeed;
        maxVelocity = PlayerData.maxVelocity;
        maxBounceCount = PlayerData.maxBounceCount;
        meleeSpeed = PlayerData.meleeSpeed;
        damage = PlayerData.damage;
    }

    public void LoadWeaponData()
    {
        weaponName = currentWeapon.name;
        weaponSprite = currentWeapon.icon;
        Debug.Log($"Is Weapon Putter? {isWeapPutter}; Current weapon name: {weaponName}"); 
    }

    private void InitializePlayerData()
    {
        currentWeapon = weaponList[0]; 
        isWeapPutter = true; 
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.instance.AttackInput) { Debug.Log("Attack button pressed!"); }
        PlayerInput();

        if(!isPlayerDragging && rb.linearVelocity.magnitude > 0.1f)
        {   
            playerSprite.GetComponent<FollowBall>().SetPlayAnimation(); }
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {   currentWeapon = weaponList[0];
            LoadWeaponData();
            isWeapPutter = true; 
          } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {   currentWeapon = weaponList[1];
            LoadWeaponData();
            isWeapPutter = false;  }

        /* fix this for dynamic rotation
        if(!isPlayerDragging)// && rb.velocity.magnitude > 0.1f)
        {
            float dynamicRotationSpeed = rotationSpeed * rb.velocity.magnitude;
            transform.Rotate(Vector3.forward, -dynamicRotationSpeed * Time.deltaTime);       
            // ^^^ could potentially be used to make some curveball kind of mechanic
        }*/ 

        if(LevelManager.main.outOfStrokes && rb.linearVelocity.magnitude <= 0.2f && !LevelManager.main.levelCompleted) {
            LevelManager.main.GameOver(); //will finish the game
            isGameOver = true;
        }
        
        TimeUntilMelee();
    }

    private void TimeUntilMelee() //will need to be on each weapon
    {
        if (timeUntilMelee <= 0f)
        {
            if(InputManager.instance.AttackInput && weaponList[0]) //&& swordSwingCount == 0 if (Input.GetMouseButtonDown(1) && swordSwingCount == 0 && weaponList[0]) //
            {
                Debug.Log($"Current weapon name: {weaponName}");

                // Get mouse position in world space
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Ensure the Z value is 0 for 2D

                // Calculate direction from player to mouse
                Vector3 direction = (mousePosition - transform.position).normalized;

                // Determine the angle of the sword swing
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                // Set the sword swing animation to rotate towards the mouse
                Transform swordTransform = anim.transform; // Ensure the sword is a child of the animation
                swordTransform.eulerAngles = new Vector3(0, 0, angle);

                // Play the animation
                anim.SetTrigger("Attack");
                timeUntilMelee = meleeSpeed;
                swordSwingCount += 1;
            } 

            if(Input.GetMouseButtonDown(1) && swordSwingCount == 0 && weaponList[1])
            {
                Debug.Log($"Current weapon name: {weaponName}");
            }
        } else { timeUntilMelee -= Time.deltaTime; }
    }

    private void FixedUpdate()
    {
        CapVelocity();
    }

     private void CapVelocity()
    {
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }
    }

    private bool IsReady() { return rb.linearVelocity.magnitude <= 0.1f; } //once ball is slow enough, allow click and drag again

    private void PlayerInput ()
    {
        if(!IsReady() || isGameOver) return;
        
        Vector2 inputPos;
        
        //if (InputManager.instance.IsUsingMouseKeyboard())
        //{
        inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //else
        //{
        //    inputPos = (Vector2)transform.position + InputManager.instance.DragPosition * 2f;
        //    Debug.Log($"Current joystick position {inputPos}");
        //}
        
        float distance = Vector2.Distance(transform.position, inputPos);
        
        if (InputManager.instance.DragStart)
        {
            Debug.Log("Click/trigger input received");
        }

        if (InputManager.instance.DragStart && distance <= 0.5f)  DragStart();//Input.GetMouseButtonDown(0) && distance <= 0.5f) DragStart(); //checks if first held down 
        if (InputManager.instance.DragStart && isPlayerDragging) DragChange(inputPos); //checks while holding down
        if (InputManager.instance.DragStart && isPlayerDragging) DragRelease(inputPos); //checks when released
    }

    private void DragStart() {
        isPlayerDragging = true;
        lr.positionCount = 2;
    }
    private void DragChange(Vector2 pos) {
        Vector2 dir = (Vector2)transform.position - pos;

        float maxLineModifier = inBunker ? 4f: 2f; //initializes modifier with a value

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude(dir * power / maxLineModifier, maxPower / maxLineModifier)); //line isn't 100% accurate 
    }
    private void DragRelease(Vector2 pos) {
        float distance = Vector2.Distance((Vector2)transform.position, pos);

        isPlayerDragging = false;
        lr.positionCount = 0;
        swordSwingCount = 0;
        Debug.Log($"{swordSwingCount}");

        if (distance < 1f) { //cancel drag if not far enough
            return;
        }

        LevelManager.main.IncreaseStroke(); //increase stroke every release

        float powerModifier = inBunker ? 2f: 1f; //initializes ball speed modifier

        Vector2 dir = (Vector2)transform.position - pos;
        rb.linearVelocity = Vector2.ClampMagnitude(dir * power, maxPower / powerModifier); //clamps max power of the drag

        // Notify NPC that the player has flicked
        //FindObjectOfType<EnemyBall>()?.OnPlayerFlick(); // Call the NPC's OnPlayerFlick method, depreciated method
        FindObjectsByType<EnemyBall>(FindObjectsSortMode.None).FirstOrDefault()?.OnPlayerFlick();
    }

    private void CheckWinState() {
        if (inHole) return;

        if (rb.linearVelocity.magnitude <= maxGoalSpeed) {
            inHole = true;

            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
            playerSprite.SetActive(false);

            GameObject fx = Instantiate(goalFx, transform.position, Quaternion.identity);
            Destroy(fx, 2f);

            //LevelComplete function
            LevelManager.main.LevelComplete();
        }
    }

    private void CheckWaterBall() {
        if (rb.linearVelocity.magnitude * 3f <= maxGoalSpeed) {
            
            //resets ball velocity to zero and hides it to play fx particles
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);

            //spawns ball falling and water splash particle fx
            GameObject bfx = Instantiate(ballFx, transform.position, Quaternion.identity);
            Destroy(bfx, 1.5f);
            GameObject fx = Instantiate(waterFx, transform.position, Quaternion.identity);
            Destroy(fx, 2f);

            //resets ball position without resetting the level
            transform.position = originalPos;
            playerSprite.GetComponent<FollowBall>().ResetSpritePosition(originalPos);
            gameObject.SetActive(true);
        }
    }

    private void CheckSandPit() { 
        if (inBunker) return;
        
        if (rb.linearVelocity.magnitude * 1.5 <= maxGoalSpeed) {
            inBunker = true;
            rb.linearVelocity = Vector2.zero;
        }
    }

    //handles entering Collider2D tags
    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Goal") CheckWinState();
        else if (other.tag == "Bunker") CheckSandPit(); 
        else if (other.tag == "Enemy") {
            other.GetComponent<EnemyBall>().TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        //following caps the amount of times you can bounce 
        if(collision.gameObject.CompareTag("Bouncy"))
        {   
            if (collisionCount < maxBounceCount)
            {
                collisionCount++;
                Debug.Log($"Bouncy collision count: {collisionCount}");
            }
            else
            {
                collisionCount = 0;
                rb.linearVelocity = Vector2.zero;
                Debug.Log($"Bounce reset");
            }
        }
    }

    //handles staying within a Collider2D tags
    private void OnTriggerStay2D(Collider2D other){
        if (other.tag == "Goal") CheckWinState();
        else if (other.tag == "Water") CheckWaterBall();
        else if (other.tag == "Bunker") CheckSandPit();
    }

    //handles exiting Collider2D tags
    private void OnTriggerExit2D(Collider2D other) { 
        if (other.tag == "Bunker") { 
            inBunker = false;
        }
    }
}
