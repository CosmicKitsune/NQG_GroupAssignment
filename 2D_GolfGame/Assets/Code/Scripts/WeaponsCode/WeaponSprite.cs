using UnityEngine;

public class WeaponSprite : MonoBehaviour, IWeaponUsable  
{
    private SpriteRenderer weaponSpriteRenderer;
    private Ball playerBall;

    public void Test(string test)
    {

    }

    void Start()
    {
        // Get the SpriteRenderer component
        weaponSpriteRenderer = GetComponent<SpriteRenderer>();

        // Find the Ball script (assuming it's on the same GameObject or its parent)
        playerBall = GetComponentInParent<Ball>();

        if (playerBall != null)
        {
            UpdateWeaponSprite();
        }
    }

    void Update()
    {
        // Ensure the sprite updates when the weapon changes
        if (playerBall != null && weaponSpriteRenderer.sprite != playerBall.weaponSprite)
        {
            UpdateWeaponSprite();
        }
    }

    private void UpdateWeaponSprite()
    {
        if (playerBall.currentWeapon != null)
        {
            weaponSpriteRenderer.sprite = playerBall.weaponSprite;
        }
    }
}
