using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Gun Variables
    private GameObject gun;
    private SpriteRenderer gunSprite;
    private Vector3 originalGunScale = new Vector3(0.05f, 0.05f, 1);
    [SerializeField] private float gunSizeGrowthFactor = 0.1f;
    [SerializeField] private float gunFloatMinRadius = 1f;
    [SerializeField] private float gunFloatMaxRadius = 2f;
    [SerializeField] private float baseRecoilForce = 1000f;
    [SerializeField] private float recoilGrowthFactor = 0.1f;
    [SerializeField] private Vector3 maxGunSize = new Vector3(0.10f, 0.10f, 1);
    [SerializeField] private float timeHeld = 0f;

    // Player Variables
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;

    void Start()
    {
        gun = transform.Find("Gun").gameObject;
        gunSprite = gun.GetComponent<SpriteRenderer>();
        playerSprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if not game over, handle player input
        if (!GameManager.instance.getGameOverStatus())
        {
            GunMovementHandler();
            InputHandler();
        }
    }

    void LateUpdate() {
        OutOfBoundsHandler();
    }

    void OutOfBoundsHandler() {
        // Keep the player within the screen bounds
        Vector2 screenBounds = GameManager.instance.getScreenBounds();

        Vector3 playerPos = transform.position;

        playerPos.x = Mathf.Clamp(playerPos.x, -screenBounds.x, screenBounds.x);
        playerPos.y = Mathf.Clamp(playerPos.y, -screenBounds.y, screenBounds.y);

        transform.position = playerPos;
    }

    void GunMovementHandler() {
        // Move the gun as close to the mouse as possible within gunFloatMaxRadius of the player 
        // while keeping the gun outside of gunFloatMinRadius of the player
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.position;
        Vector3 gunPos = gun.transform.position;

        Vector3 direction = mousePos - playerPos;
        float distance = Vector3.Distance(mousePos, playerPos);

        if (distance > gunFloatMaxRadius) {
            gun.transform.position = playerPos + direction.normalized * gunFloatMaxRadius;
        } else if (distance < gunFloatMinRadius) {
            gun.transform.position = playerPos + direction.normalized * gunFloatMinRadius;
        } else {
            gun.transform.position = mousePos;
        }

        // Rotate gun to face the mouse, flip gun if mouse is on the right side of the player
        Vector3 gunDirection = mousePos - gunPos;
        float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        if (mousePos.x < playerPos.x) {
            gunSprite.flipY = true;
        } else {
            gunSprite.flipY = false;
        }
    }

    void InputHandler() {
        if (Input.GetMouseButtonDown(0) && gun.GetComponent<PlayerGunScript>().getAmmo() > 0) {
            // The longer the player holds the mouse button, the more powerful the recoil
            
            // calculate the time the player has held the mouse button
            timeHeld = 0f;

            // Start the shooting coroutine
            StartCoroutine(ShootCoroutine());
        }
    }

    void ShootHandler() {
        // Shoot and apply recoil force
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.position;

        // Calculate the direction of the recoil (opposite to the shooting direction)
        Vector3 direction = playerPos - mousePos;
        direction.Normalize(); // Normalize the direction to apply consistent recoil

        float recoilForce = baseRecoilForce + (timeHeld * baseRecoilForce * recoilGrowthFactor);

        // Add the recoil force to the player's Rigidbody
        rb.AddForce(direction * recoilForce);

        // Reset the gun scale
        gun.transform.localScale = originalGunScale;

        // Shoot the bullet
        gun.GetComponent<PlayerGunScript>().ShootBullet(gun.transform.position, gun.transform.localScale, gun.transform.right);
    }

    IEnumerator ShootCoroutine() {
        while (Input.GetMouseButton(0) && gun.transform.localScale.x < maxGunSize.x) {
            timeHeld += Time.deltaTime;
            // increase the scale of the gun while the player holds the mouse button
            gun.transform.localScale = originalGunScale + new Vector3(timeHeld * gunSizeGrowthFactor, timeHeld * gunSizeGrowthFactor, 0);
            yield return null;
        }

        // once the player releases the mouse button, fire the gun
        ShootHandler();
    }
}
