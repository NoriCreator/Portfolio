using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class GamePointManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> goalObjects = new();
        private List<GamePoint> gamePoints = new();
        
        void Start()
        {
            if (goalObjects == null || goalObjects.Count == 0)
            {
                Debug.LogError("goalObjects is not set or empty.");
                return; // 処理を中断
            }

            foreach (var goalObject in goalObjects)
            {
                var goalFlag = goalObject.GetComponent<GoalFlag>();
                if (goalFlag == null)
                {
                    Debug.LogError($"GoalFlag component is missing on {goalObject.name}");
                    continue;
                }

                gamePoints.Add(new GamePoint
                {
                    GoalColor = goalFlag.GoalColor,
                    gamePoint = 0
                });
            }

            gamePoints.ForEach(gamePoint =>
            {
                Debug.Log($"Goal in {gamePoint.GoalColor} : {gamePoint.gamePoint}");
            });
        }

        void Update()
        {
            foreach (var goalObject in goalObjects)
            {
                if (goalObject.GetComponent<GoalFlag>().GoalFlagActive)
                {
                    PointIterator(goalObject.GetComponent<GoalFlag>().GoalColor);
                    goalObject.GetComponent<GoalFlag>().GoalFlagActive = false;
                }
            }
        }

        
        private void PointIterator(string goalColor)
        {
            gamePoints.ForEach(gamePoint =>
            {
                if (gamePoint.GoalColor == goalColor)
                {
                    gamePoint.gamePoint++;

                    Debug.Log($"Goal in {goalColor} : {gamePoint.gamePoint}"); // デバッグ用
                }
            });
        }
    }

    // 得点計算用のクラス
    public class GamePoint
    {
        public string GoalColor;
        public int gamePoint;
    }
}