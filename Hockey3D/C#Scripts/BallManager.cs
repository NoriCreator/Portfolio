using UnityEngine;

namespace GameManager
{
    public class BallManager : MonoBehaviour
    {
        Rigidbody rb;
        private float velocity;
        private Vector3 moveDirection;
        private const float resistvity = 0.9f;

        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.linearDamping = 0.3f;
            rb.angularDamping = 1.0f;
        }
    }
}