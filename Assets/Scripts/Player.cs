using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Gun Variables
    private GameObject gun;
    private SpriteRenderer gunSprite;
    private Vector3 originalGunScale = new Vector3(0.05f, 0.05f, 1);
    public float gunSizeGrowthFactor = 0.1f;
    public float gunFloatMinRadius = 1f;
    public float gunFloatMaxRadius = 2f;
    public float baseRecoilForce = 100f;
    public float recoilGrowthFactor = 0.1f;
    public float timeHeld = 0f;

    // Player Variables
    private Rigidbody2D rb;

    void Start()
    {
        gun = transform.Find("Gun").gameObject;
        gunSprite = gun.GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GunMovementHandler();
        InputHandler();
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

        if (mousePos.x < playerPos.x) {
            gunSprite.flipY = true;
        } else {
            gunSprite.flipY = false;
        }
    }

    void InputHandler() {
        if (Input.GetMouseButtonDown(0)) {
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

        Debug.Log("Recoil Force: " + recoilForce);

        // Add the recoil force to the player's Rigidbody
        rb.AddForce(direction * recoilForce);

        // Reset the gun scale
        gun.transform.localScale = originalGunScale;
    }

    IEnumerator ShootCoroutine() {
        while (Input.GetMouseButton(0)) {
            timeHeld += Time.deltaTime;
            // increase the scale of the gun while the player holds the mouse button
            gun.transform.localScale = originalGunScale + new Vector3(timeHeld * gunSizeGrowthFactor, timeHeld * gunSizeGrowthFactor, 0);
            yield return null;
        }

        // once the player releases the mouse button, fire the gun
        ShootHandler();
    }
}
