using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float speed;
    private float[] speeds = { 20f, 25f, 30f, 35f, 40f, 45f, 50f, 55f, 60f };

    [SerializeField] private float sideSpeedMultiplier = 0.5f; // Controls the proportion of side movement speed
    [SerializeField] private float detectionRange = 10f; // Range to detect slower cars
    [SerializeField] private LayerMask carLayerMask; // Layer to filter which objects are considered cars

    private Vector3 sideMovementDirection; // Direction for side movement
    private bool isAvoiding = false; // Whether the car is actively avoiding another car

    void Start()
    {
        // Set the initial speed randomly from the speeds array
        speed = speeds[Random.Range(0, speeds.Length)];
        Debug.Log(Physics.gravity);
    }

    void Update()
    {
        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check for slower cars and adjust lateral movement
        DetectAndAvoidSlowerCar();

        // Apply side movement if avoiding
        if (isAvoiding)
        {
            transform.Translate(sideMovementDirection * speed * sideSpeedMultiplier * Time.deltaTime, Space.World);
        }
    }

    private void DetectAndAvoidSlowerCar()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f; // Adjust height to avoid ground interference
        Vector3 rayDirection = transform.forward;

        // Check if there's a slower car in front
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, detectionRange, carLayerMask))
        {
            CarController otherCar = hit.collider.GetComponent<CarController>();

            // Ensure the detected object is a car and is slower
            if (otherCar != null && otherCar.speed < speed)
            {
                // Calculate side movement direction (left or right based on proximity to edges, or choose randomly)
                if (!isAvoiding)
                {
                    sideMovementDirection = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;
                    isAvoiding = true;
                }
            }
        }
        else
        {
            // Stop side movement if no slower car is detected
            isAvoiding = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the raycast for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 1f, transform.forward * detectionRange);
    }
}
