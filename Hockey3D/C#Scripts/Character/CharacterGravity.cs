using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterGravity : MonoBehaviour
{
    private List<GravityObject> activeGravities = new List<GravityObject>();
    private Vector3 currentGravityDirection;
    private Rigidbody rb;
    
    [SerializeField] private float rotationSpeed = 1.0f;

    private void Start()
    {
        this.tag = "Character";
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        currentGravityDirection = Vector3.down;
    }

    private void FixedUpdate()
    {
        UpdateGravity();
        ApplyGravityForce();
        SmoothRotateToGravity();
    }

    private void UpdateGravity()
    {
        if (activeGravities.Count > 0)
        {
            var highestPriorityGravity = activeGravities.OrderByDescending(g => g.priority).First();
            currentGravityDirection = highestPriorityGravity.GetGravityDirection(transform.position);
        }
        else
        {
            currentGravityDirection = Vector3.down;
        }
    }

    private void ApplyGravityForce()
    {
        rb.AddForce(currentGravityDirection * rb.mass, ForceMode.Acceleration);
    }

    private void SmoothRotateToGravity()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, -currentGravityDirection), -currentGravityDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void AddGravity(GravityObject gravityObject)
    {
        if (!activeGravities.Contains(gravityObject))
        {
            activeGravities.Add(gravityObject);
        }
    }

    public void RemoveGravity(GravityObject gravityObject)
    {
        activeGravities.Remove(gravityObject);
    }
}