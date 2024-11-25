using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 0f;
    private const float maxSpeed = 100f;
    private const float acceleration = 10f;
    private const float deceleration = 50f;
    private const float lateralSpeedFactor = 0.2f;

    private TextMeshProUGUI speedText; // TMP text pre zobrazenie rýchlosti

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        // Lock all rotations to prevent spinning
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Nájdeme TMP Text v scéne podľa mena alebo tagu
        GameObject speedTextObject = GameObject.Find("SpeedText"); // Zmeň meno podľa názvu TMP objektu
        if (speedTextObject != null)
        {
            speedText = speedTextObject.GetComponent<TextMeshProUGUI>();
        }

        // Kontrola, či bol TMP nájdený
        if (speedText == null)
        {
            Debug.LogError("Speed Text (TMP) not found! Ensure the TMP Text is named 'SpeedText' or set it manually.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle forward and backward movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            speed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speed -= deceleration * Time.deltaTime;
        }

        // Clamp the speed to be between 0 and maxSpeed
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        // Calculate movement direction
        Vector3 moveDirection = Vector3.forward * speed;

        // Calculate lateral speed based on forward speed
        float lateralSpeed = speed * lateralSpeedFactor;

        // Handle left and right movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection += Vector3.left * lateralSpeed;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector3.right * lateralSpeed;
        }

        // Apply the movement to the Rigidbody
        rb.velocity = moveDirection;

        // Update the speed text on the screen
        if (speedText != null)
        {
            speedText.text = $"Speed: {speed:F2} km/h";
        }
    }

    // On collision detection, end the game if the tag is not "road"
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "road")
        {
            GameManager._gameOver = true;
            Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        }
    }
}
