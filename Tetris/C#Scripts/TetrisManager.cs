using UnityEngine;
using UnityEngine.SceneManagement;
using Commons;
using static MinoManagement.Mino;
using System.Collections.Generic;

namespace MinoManagement
{
    public class MinoManager : MonoBehaviour
    {
        // Constants ---------------------------------------------------------
        private const int FieldWidth = 10;
        private const int FieldHeight = 20;
        private const int MinoSize = 4;

        private const float DefaultDownSpeed = 1.0f;
        private const float FastDownSpeed = 0.2f;
        private const float StopTimeThreshold = 2.0f;

        private const float MinoOffsetX = -4.5f;
        private const float MinoOffsetY = -10.0f;
        private const float HoldOffsetX = -9.5f;
        private const float HoldOffsetY = -6.9f;
        private const float NextOffsetX = -4.5f;
        private const float NextOffsetY = -6.9f;
        private const float Next2OffsetX = -0.0f;
        private const float Next2OffsetY = -6.9f;

        // その他変数宣言 -------------------------------------------------
        private GameObject[] minoCubeTypeArray = new GameObject[7];
        [SerializeField] private GameObject I_Mino_Cube;
        [SerializeField] private GameObject O_Mino_Cube;
        [SerializeField] private GameObject T_Mino_Cube;
        [SerializeField] private GameObject S_Mino_Cube;
        [SerializeField] private GameObject Z_Mino_Cube;
        [SerializeField] private GameObject J_Mino_Cube;
        [SerializeField] private GameObject L_Mino_Cube;

        private TetriMino currentTetriMino;
        public GameObject[] CurrentMinoObjects { get; private set; } = new GameObject[MinoSize];

        private TetrisField tetrisField = new();
        public GameObject[] FieldObjects { get; private set; } = new GameObject[FieldWidth * FieldHeight];

        private float downSpeedNomal = DefaultDownSpeed;
        private float downSpeedFast = FastDownSpeed;

        private float downSpeedCounter = 0f;
        private float stopCounter = 0f;

        private TetriMino holdTetriMino;
        private TetriMino nextTetriMino;
        private TetriMino next2TetriMino;

        private GameObject[] HoldTetriMinoObjects { get; set; } = new GameObject[MinoSize];
        private GameObject[] NextTetriMinoObjects { get; set; } = new GameObject[MinoSize];
        private GameObject[] Next2TetriMinoObjects { get; set; } = new GameObject[MinoSize];

        private int[] spawnCount = new int[7]; // 各ミノの出現回数を追跡
        private const int MinAppearanceGap = 4; // 出現回数の差

        // Unity Methods ------------------------------------------------------
        void Awake()
        {
            InitializeMinoCubeTypeArray();
        }

        void Start()
        {
            SetNextTetriMino();
            SpawnNewTetriMino();
        }

        void Update()
        {
            UnityCommonsFunc.EndGame();

            HandleTetriMinoMovement();

            if (ShouldApplyTetriMinoOnField())
            {
                ApplyTetriMinoOnField();
                SpawnFieldObjects();

                if(IsFinishGame())
                {
                    SceneManager.LoadScene("StartScene");
                }
                
                SpawnNewTetriMino();
            }
        }

        void OnApplicationQuit()
        {
            Application.Quit(); // アプリを終了する
        }

        // Private Methods ----------------------------------------------------
        private void InitializeMinoCubeTypeArray()
        {
            minoCubeTypeArray[0] = I_Mino_Cube;
            minoCubeTypeArray[1] = O_Mino_Cube;
            minoCubeTypeArray[2] = T_Mino_Cube;
            minoCubeTypeArray[3] = S_Mino_Cube;
            minoCubeTypeArray[4] = Z_Mino_Cube;
            minoCubeTypeArray[5] = J_Mino_Cube;
            minoCubeTypeArray[6] = L_Mino_Cube;
        }

        private void InitializeTetrisField()
        {
            tetrisField = new TetrisField();

            DestroyObjects(FieldObjects);
        }

        private void SpawnNewTetriMino()
        {
            downSpeedCounter = 0f;
            stopCounter = 0f;
            
            currentTetriMino = nextTetriMino;
            CurrentMinoObjects = SpawnTetriMinoObjects(currentTetriMino, MinoOffsetX, MinoOffsetY);

            nextTetriMino = next2TetriMino;
            DestroyObjects(NextTetriMinoObjects);
            NextTetriMinoObjects = SpawnTetriMinoObjects(nextTetriMino, NextOffsetX, NextOffsetY);

            next2TetriMino = GetRandomTetriMino();
            DestroyObjects(Next2TetriMinoObjects);
            Next2TetriMinoObjects = SpawnTetriMinoObjects(next2TetriMino, Next2OffsetX, Next2OffsetY);
        }

        private void HoldTetriMino()
        {
            DestroyObjects(CurrentMinoObjects);

            if (holdTetriMino == null)
            {
                holdTetriMino = currentTetriMino;
                currentTetriMino = nextTetriMino;
                SpawnNewTetriMino();
                holdTetriMino = new TetriMino(holdTetriMino.ThisTetriMinoType);
            }
            else
            {
                DestroyObjects(HoldTetriMinoObjects);

                TetriMino tempTetriMino = currentTetriMino;
                currentTetriMino = holdTetriMino;
                holdTetriMino = new TetriMino(tempTetriMino.ThisTetriMinoType);

                CurrentMinoObjects = SpawnTetriMinoObjects(currentTetriMino, MinoOffsetX, MinoOffsetY);
            }

            HoldTetriMinoObjects = SpawnTetriMinoObjects(holdTetriMino, HoldOffsetX, HoldOffsetY);
        }

        private void HandleTetriMinoMovement()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) HoldTetriMino();

            if (Input.GetKeyDown(KeyCode.W))
            {
                SetTetriMinoImmediately();
            }

            downSpeedCounter += Time.deltaTime;
            if (downSpeedCounter >= downSpeedNomal)
            {
                downSpeedCounter = 0f;
                MoveTetriMino(new IntVector2(0, -1), 0);
            }

            IntVector2 moveVector = GetMoveVector();
            int rotation = GetRotation();

            if (moveVector.x != 0 || moveVector.y != 0 || rotation != 0)
            {
                MoveTetriMino(moveVector, rotation);
            }
        }

        private IntVector2 GetMoveVector()
        {
            IntVector2 moveVector = new(0, 0);

            if (Input.GetKeyDown(KeyCode.A))
            {
                moveVector.x = -1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveVector.x = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (downSpeedCounter > downSpeedFast)
                {
                    downSpeedCounter = 0f;
                    moveVector.y = -1;
                }
            }

            return moveVector;
        }

        private int GetRotation()
        {
            int rotation = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rotation = -1;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                rotation = 1;
            }

            return rotation;
        }

        private GameObject SpawnMinoObject(Vector3 position, MinoType minoType)
        {
            return Instantiate(minoCubeTypeArray[(int)minoType], position, Quaternion.identity);
        }

        private GameObject[] SpawnTetriMinoObjects(TetriMino tetriMino, float offsetX, float offsetY)
        {
            GameObject[] spawnCubeArray = new GameObject[MinoSize];
            int count = 0;

            for (int row = 0; row < tetriMino.PlayTetriMino.GetLength(0); row++)
            {
                for (int col = 0; col < tetriMino.PlayTetriMino.GetLength(1); col++)
                {
                    if (tetriMino.PlayTetriMino[row, col].ThisMinoType != MinoType.None)
                    {
                        spawnCubeArray[count] = SpawnMinoObject(new Vector3(offsetX + row, offsetY + col) + new Vector3(tetriMino.PlayTetriMinoPosition.x, tetriMino.PlayTetriMinoPosition.y, 0), tetriMino.ThisTetriMinoType);
                        count++;
                        if (count == MinoSize)
                        {
                            break;
                        }
                    }
                }

                if (count == MinoSize)
                {
                    break;
                }
            }

            return spawnCubeArray;
        }

        private void MoveTetriMino(IntVector2 moveVector, int rotation)
        {
            DestroyObjects(CurrentMinoObjects);
            currentTetriMino.ApplyTetriMino(moveVector, rotation, tetrisField.FieldArray);
            CurrentMinoObjects = SpawnTetriMinoObjects(currentTetriMino, MinoOffsetX, MinoOffsetY);
        }

        private int lastYPosition = int.MinValue;

        private bool ShouldApplyTetriMinoOnField()
        {
            int currentYPosition = currentTetriMino.PlayTetriMinoPosition.y;
            if (lastYPosition == currentYPosition)
            {
                stopCounter += Time.deltaTime;
                if (stopCounter >= StopTimeThreshold)
                {
                    return true;
                }
            }
            else
            {
                stopCounter = 0f;
            }

            lastYPosition = currentYPosition;
            return false;
        }

        private void ApplyTetriMinoOnField()
        {
            DestroyObjects(CurrentMinoObjects);
            int posX = currentTetriMino.PlayTetriMinoPosition.x;
            int posY = currentTetriMino.PlayTetriMinoPosition.y;

            for (int row = 0; row < currentTetriMino.PlayTetriMino.GetLength(0); row++)
            {
                for (int col = 0; col < currentTetriMino.PlayTetriMino.GetLength(1); col++)
                {
                    if (currentTetriMino.PlayTetriMino[row, col].ThisMinoType != MinoType.None)
                    {
                        int fieldX = posX + row;
                        int fieldY = posY + col;
                        if (fieldX >= 0 && fieldX < FieldWidth && fieldY >= 0 && fieldY < FieldHeight)
                        {
                            tetrisField.FieldArray[fieldX, fieldY].ThisMinoType = currentTetriMino.PlayTetriMino[row, col].ThisMinoType;
                        }
                    }
                }
            }

            tetrisField.ClearFullLines();
        }

        private void DestroyObjects(GameObject[] objects)
        {
            foreach (GameObject obj in objects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }

        private void SpawnFieldObjects()
        {
            if (FieldObjects != null)
            {
                DestroyObjects(FieldObjects);
            }

            int count = 0;

            for (int row = 0; row < FieldWidth; row++)
            {
                for (int col = 0; col < FieldHeight; col++)
                {
                    if (tetrisField.FieldArray[row, col].ThisMinoType != MinoType.None)
                    {
                        FieldObjects[count] = SpawnMinoObject(new Vector3(MinoOffsetX + row, MinoOffsetY + col), tetrisField.FieldArray[row, col].ThisMinoType);
                        count++;
                    }
                }
            }
        }

        private bool IsFinishGame()
        {
            for (int row = 3; row < 7; row++)
            {
                for (int col = 18; col < 20; col++)
                {
                    if (tetrisField.FieldArray[row, col].ThisMinoType != MinoType.None)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private TetriMino GetRandomTetriMino()
        {
            int minCount = int.MaxValue;
            for (int i = 0; i < spawnCount.Length; i++)
            {
                if (spawnCount[i] < minCount)
                {
                    minCount = spawnCount[i];
                }
            }

            List<int> eligibleIndices = new();
            for (int i = 0; i < spawnCount.Length; i++)
            {
                if (spawnCount[i] <= minCount + MinAppearanceGap)
                {
                    eligibleIndices.Add(i);
                }
            }

            int chosenIndex = eligibleIndices[Random.Range(0, eligibleIndices.Count)];
            spawnCount[chosenIndex]++;

            return new TetriMino((MinoType)chosenIndex);
        }

        private void SetNextTetriMino()
        {
            if (nextTetriMino == null)
            {
                nextTetriMino = GetRandomTetriMino();
            }

            if (next2TetriMino == null)
            {
                next2TetriMino = GetRandomTetriMino();
            }
        }

        private void SetTetriMinoImmediately()
        {
            while (!ShouldApplyTetriMinoOnField())
            {
                MoveTetriMino(new IntVector2(0, -1), 0);
            }
        }
    }
}