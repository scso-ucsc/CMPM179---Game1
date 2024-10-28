using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Gun Variables
    private GameObject gun;
    private SpriteRenderer gunSprite;
    private Vector3 originalGunScale = new Vector3(0.05f, 0.05f, 1);
    [SerializeField] private float gunSizeGrowthFactor = 0.1f;
    [SerializeField] private float gunFloatMaxRadius = 2f;
    [SerializeField] private float baseRecoilForce = 700f;
    [SerializeField] private float recoilGrowthFactor = 0.1f;
    [SerializeField] private Vector3 maxGunSize = new Vector3(0.10f, 0.10f, 1);
    [SerializeField] private float timeHeld = 0f;

    // Time Slowdown Variables
    [SerializeField] private float slowdownFactor = 0.25f;
    [SerializeField] private VignetteController vignetteController;
    [SerializeField] private float slowdownDuration = 3f;
    [SerializeField] private float slowdownCooldown = 5f;
    private bool isSlowedDown = false;
    private bool canSlowdown = true;

    [SerializeField] private Image slowDownCooldownImage;
    [SerializeField] private TextMeshProUGUI timeSlowText;

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

        // Clamp by 95% of the screen bounds to keep the player within the screen
        playerPos.x = Mathf.Clamp(playerPos.x, -screenBounds.x * 0.95f, screenBounds.x * 0.95f);
        playerPos.y = Mathf.Clamp(playerPos.y, -screenBounds.y * 0.95f, screenBounds.y * 0.70f);


        transform.position = playerPos;
    }

    void GunMovementHandler() {
        // Move the gun as close to the mouse as possible within gunFloatMaxRadius of the player 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.position;
        Vector3 gunPos = gun.transform.position;

        Vector3 direction = mousePos - playerPos;
        float distance = Vector3.Distance(mousePos, playerPos);

        if (distance > gunFloatMaxRadius) {
            gun.transform.position = playerPos + direction.normalized * gunFloatMaxRadius;
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

        // if the player presses E, tweens the time to slowdownFactor in a coroutine
        if (Input.GetKeyDown(KeyCode.E) && canSlowdown && !isSlowedDown) {
            vignetteController.FadeIn(0.4f, 0.5f, slowdownFactor);
            isSlowedDown = true;
            canSlowdown = false;

            StartCoroutine(SlowdownCoroutine());
        }
    }

    void ShootHandler() {
        // Shoot and apply recoil force
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.position;
        Vector3 gunPos = gun.transform.position;

        // Calculate the direction of the recoil (directly behind the player)

        Vector3 direction = -gun.transform.right;

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

    IEnumerator SlowdownCoroutine() {
        yield return new WaitForSeconds(slowdownDuration);

        vignetteController.FadeOut(0.5f);

        Time.timeScale = 1;

        isSlowedDown = false;

        // begin cooldown for slowdown, fill cooldown image to represent the cooldown
        StartCoroutine(SlowdownCooldownCoroutine());
    }

    IEnumerator SlowdownCooldownCoroutine() {
        float timeElapsed = 0f;
        timeSlowText.color = Color.white;
        timeSlowText.fontSize = 48;

        while (timeElapsed < slowdownCooldown) {
            timeElapsed += Time.deltaTime;
            float fillAmount = timeElapsed / slowdownCooldown;
            slowDownCooldownImage.fillAmount = fillAmount;
            timeSlowText.text = (slowdownCooldown - timeElapsed).ToString("F1");
            yield return null;
        }

        slowDownCooldownImage.fillAmount = 1;
        timeSlowText.text = "PRESS E";
        timeSlowText.color = Color.green;
        timeSlowText.fontSize = 40;
        canSlowdown = true;
    }
}

