using UnityEngine;

namespace GameManager
{
    public class GoalFlag : MonoBehaviour
    {
        [SerializeField] private GameObject gameManeger;
        public string GoalColor;
        public bool GoalFlagActive { get; set; } = false;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball")) // タグが "Ball" の場合のみ処理を実行
            {
                Debug.Log($"Goal in {GoalColor}");
                gameManeger.GetComponent<GamaManager>().BallSpawn();
                GoalFlagActive = true;
                Destroy(collision.gameObject);
            }
        }
    }
}