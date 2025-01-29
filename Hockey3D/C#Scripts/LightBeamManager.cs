using UnityEngine;

public class LightBeamManager : MonoBehaviour
{
    private const float hitForce = 10.0f;

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがBallタグを持っている場合のみ処理
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                // バットの進行方向に力を加える
                Vector3 hitDirection = transform.forward;  // バットの進行方向
                ballRigidbody.AddForce(hitDirection * hitForce, ForceMode.Impulse);
            }
        }
    }
}
