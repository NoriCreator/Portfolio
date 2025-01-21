using UnityEngine;
using Commons;
using static MinoManagement.Mino;

namespace MinoManagement
{
    public class MinoManager : MonoBehaviour
    {
        private const int FieldWidth = 10;
        private const int FieldHeight = 20;
        private const int MinoSize = 4;
        private const float DefaultDownSpeed = 1.0f;
        private const float FastDownSpeed = 0.2f;
        private const float StopTimeThreshold = 2.0f;
        private const float SpawnOriginX = 3.0f;
        private const float SpawnOriginY = 16.0f;
        private const float SpawnOriginZ = 0.0f;
        private const float MinoOffsetX = -4.5f;
        private const float MinoOffsetY = -10.0f;

        private GameObject[] minoCubeTypeArray = new GameObject[7];
        [SerializeField] private GameObject I_Mino_Cube;
        [SerializeField] private GameObject O_Mino_Cube;
        [SerializeField] private GameObject T_Mino_Cube;
        [SerializeField] private GameObject S_Mino_Cube;
        [SerializeField] private GameObject Z_Mino_Cube;
        [SerializeField] private GameObject J_Mino_Cube;
        [SerializeField] private GameObject L_Mino_Cube;

        public Vector3 SpawnOrigin { get; private set; } = new(SpawnOriginX, SpawnOriginY, SpawnOriginZ);

        private TetriMino currentTetriMino;
        public GameObject[] CurrentMinoObjects { get; private set; } = new GameObject[MinoSize];

        private TetrisField tetrisField = new();
        public GameObject[] FieldObjects { get; private set; } = new GameObject[FieldWidth * FieldHeight];

        private float normalDownSpeed = DefaultDownSpeed;
        private float fastDownSpeed = FastDownSpeed;

        private float downSpeedCounter = 0f;
        private float stopCounter = 0f;

        void Awake()
        {
            InitializeMinoCubeTypeArray();
        }

        void Start()
        {
            SpawnNewTetriMino();
        }

        void Update()
        {
            HandleTetriMinoMovement();

            if (ShouldApplyTetriMinoOnField())
            {
                ApplyTetriMinoOnField();
                SpawnFieldObjects();
                SpawnNewTetriMino();
            }
        }

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

        private void SpawnNewTetriMino()
        {
            currentTetriMino = new TetriMino((MinoType)Random.Range(0, 7));
            CurrentMinoObjects = SpawnTetriMinoObjects();
        }

        private void HandleTetriMinoMovement()
        {
            downSpeedCounter += Time.deltaTime;
            if (downSpeedCounter >= normalDownSpeed)
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
                if (downSpeedCounter > fastDownSpeed)
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

        private GameObject[] SpawnTetriMinoObjects()
        {
            GameObject[] spawnCubeArray = new GameObject[MinoSize];
            int count = 0;

            for (int row = 0; row < currentTetriMino.PlayTetriMino.GetLength(0); row++)
            {
                for (int col = 0; col < currentTetriMino.PlayTetriMino.GetLength(1); col++)
                {
                    if (currentTetriMino.PlayTetriMino[row, col].ThisMinoType != MinoType.None)
                    {
                        spawnCubeArray[count] = SpawnMinoObject(new Vector3(MinoOffsetX + row, MinoOffsetY + col) + new Vector3(currentTetriMino.PlayTetriMinoPosition.x, currentTetriMino.PlayTetriMinoPosition.y, 0), currentTetriMino.ThisTetriMinoType);
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
            CurrentMinoObjects = SpawnTetriMinoObjects();
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
    }
}