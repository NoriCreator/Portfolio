using UnityEngine;

namespace GameManager
{
    public class GamaManager : MonoBehaviour
    {
        public GameObject GameBall;

        void Start()
        {
            if (GameBall != null)
            {
                BallSpawn();
            }
            else if (GameBall == null)
            {
                Debug.LogError("GameBallObject is null");
            }
        }

        public void BallSpawn()
        {
            Instantiate(GameBall, new Vector3(-2, 0, 0), Quaternion.identity);
            Debug.Log("GameBall is Spawned");
        }
    }
}