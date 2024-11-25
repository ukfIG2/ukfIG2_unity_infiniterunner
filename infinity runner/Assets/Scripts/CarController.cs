using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private float speed;
    private float[] speeds = { 20f, 25f, 30f, 35f, 40f, 45f, 50f, 55f, 60f };

    [SerializeField] private float sideSpeedMultiplier = 0.5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask carLayerMask;
    [SerializeField] private float maxSideDistance = 2f;

    private Vector3 sideMovementDirection;
    private bool isAvoiding = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Ignorovať kolízie medzi autami vo vrstve "Cars"
        int carLayer = LayerMask.NameToLayer("Cars");
        Physics.IgnoreLayerCollision(carLayer, carLayer);

        // Nastaviť náhodnú rýchlosť
        speed = speeds[Random.Range(0, speeds.Length)];
    }

    void FixedUpdate()
    {
        // Resetovať vertikálnu rýchlosť
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // Pohyb dopredu
        Vector3 forwardMovement = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Detekcia pomalších áut
        DetectAndAvoidSlowerCar();

        // Bočný pohyb pri vyhýbaní sa
        if (isAvoiding)
        {
            Vector3 lateralMovement = sideMovementDirection * speed * sideSpeedMultiplier * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + lateralMovement);
        }
    }

    private void DetectAndAvoidSlowerCar()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, detectionRange, carLayerMask))
        {
            CarController otherCar = hit.collider.GetComponent<CarController>();

            if (otherCar != null && otherCar.speed < speed)
            {
                if (!isAvoiding)
                {
                    sideMovementDirection = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;

                    float currentX = transform.position.x;
                    if (currentX > maxSideDistance) sideMovementDirection = Vector3.left;
                    else if (currentX < -maxSideDistance) sideMovementDirection = Vector3.right;

                    isAvoiding = true;
                }
            }
        }
        else
        {
            isAvoiding = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CarController otherCar = collision.collider.GetComponent<CarController>();

        if (otherCar != null)
        {
            // Znížiť rýchlosť pri kontakte s pomalším autom
            if (otherCar.speed < speed)
            {
                speed -= 5f;
                if (speed < 20f) speed = 20f; // Minimálna rýchlosť
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 1f, transform.forward * detectionRange);
    }
}
