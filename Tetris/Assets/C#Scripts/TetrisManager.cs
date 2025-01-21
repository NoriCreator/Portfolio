using UnityEngine;
using Commons;
using static MinoManagement.Mino;

namespace MinoManagement
{
    public class MinoManager : MonoBehaviour
    {
        private GameObject[] minoCubeTypeArray = new GameObject[7];
        [SerializeField] private GameObject I_Mino_Cube;
        [SerializeField] private GameObject O_Mino_Cube;
        [SerializeField] private GameObject T_Mino_Cube;
        [SerializeField] private GameObject S_Mino_Cube;
        [SerializeField] private GameObject Z_Mino_Cube;
        [SerializeField] private GameObject J_Mino_Cube;
        [SerializeField] private GameObject L_Mino_Cube;

        public Vector3 SpawnOrigin = new(3.0f, 16.0f, 0.0f);

        TetriMino playTetriMino;
        public GameObject[] PlayMinoObjectArray { get; private set; } = new GameObject[4];

        TetrisField tetrisField = new();
        public GameObject[] TetrisFieldObjectArray { get; private set; } = new GameObject[1000];

        [SerializeField] float downSpeedNomal = 1.0f;
        [SerializeField] float downSpeedFast = 0.2f;

        private float timeDownSpeedCount = 0f;
        private float timeDownStopCount = 0f;

        void Awake()
        {
            minoCubeTypeArray[0] = I_Mino_Cube;
            minoCubeTypeArray[1] = O_Mino_Cube;
            minoCubeTypeArray[2] = T_Mino_Cube;
            minoCubeTypeArray[3] = S_Mino_Cube;
            minoCubeTypeArray[4] = Z_Mino_Cube;
            minoCubeTypeArray[5] = J_Mino_Cube;
            minoCubeTypeArray[6] = L_Mino_Cube;
        }
        void Start()
        {
            InstantTetriMino();
        }

        void Update()
        {
            TetriMinoController();

            if(IsCheckReflectOnField())
            {
                ReflectPlayTetriMinoOnField();
                SpawnFieldObject();
                InstantTetriMino();
            }
        }

        private void InstantTetriMino()
        {
            playTetriMino = new TetriMino((MinoType)Random.Range(0, 7));
            PlayMinoObjectArray = SpawnTetriMinoObject();
        }

        private void NaturalMoveDown()
        {
            timeDownSpeedCount += Time.deltaTime;
            if(timeDownSpeedCount >= downSpeedNomal)
            {
                timeDownSpeedCount = 0f;
                MoveTetriMino(new(0, -1), 0);
            }
        }

        private void TetriMinoController()
        {
            NaturalMoveDown();
            
            int rotate = 0;
            IntVector2 moveVector;
            moveVector.x = 0;
            moveVector.y = 0;

            bool isMove = false;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                rotate += 1;
                isMove = true;
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                rotate += -1;
                isMove = true;
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                moveVector.x += -1;
                isMove = true;
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                moveVector.x += 1;
                isMove = true;
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                if(timeDownSpeedCount > downSpeedFast)
                {
                    timeDownSpeedCount = 0f;

                    moveVector.y += -1;
                    isMove = true;
                }
            }

            if(isMove)
            {
                MoveTetriMino(moveVector, rotate);
            }
        }

        private GameObject SpawnMinoObject(Vector3 position, MinoType minoType)
        {
            GameObject spawnObject = Instantiate(minoCubeTypeArray[(int)minoType], position, Quaternion.identity);

            return spawnObject;
        }

        private GameObject[] spawnCubeArray = new GameObject[4];

        private GameObject[] SpawnTetriMinoObject()
        {
            int count = 0;

            for(int raw = 0; raw < playTetriMino.PlayTetriMino.GetLength(0); raw++)
            {
                for(int col = 0; col < playTetriMino.PlayTetriMino.GetLength(1); col++)
                {
                    if(playTetriMino.PlayTetriMino[raw, col].ThisMinoType != MinoType.None)
                    {
                        spawnCubeArray[count] = SpawnMinoObject(new Vector3(-4.5f + raw, -10.0f + col) + new Vector3(playTetriMino.PlayTetriMinoPosition.x, playTetriMino.PlayTetriMinoPosition.y, 0), playTetriMino.ThisTetriMinoType);
                        count++;
                        if(count == 4)
                        {
                            break;
                        }
                    }
                }
                
                if(count == 4)
                {
                    break;
                }
            }

            return spawnCubeArray;
        }

        private void MoveTetriMino(IntVector2 moveVector, int rotation)
        {
            DestroyObjectArray(PlayMinoObjectArray);
            playTetriMino.TetriMinoReflectValue(moveVector, rotation, tetrisField.TetrisFieldArray);
            PlayMinoObjectArray = SpawnTetriMinoObject();
        }

        private int lastYPosition = int.MinValue;

        private bool IsCheckReflectOnField()
        {
            int currentYPosition = playTetriMino.PlayTetriMinoPosition.y;
            if(lastYPosition == currentYPosition)
            {
                timeDownStopCount += Time.deltaTime;
                if(timeDownStopCount >= 2.0f)
                {
                    return true;
                }
            }
            else {timeDownStopCount = 0f;}

            lastYPosition = currentYPosition;
            return false;
        }

        private void ReflectPlayTetriMinoOnField()
        {
            DestroyObjectArray(PlayMinoObjectArray);
            Mino[,] shape = playTetriMino.PlayTetriMino;
            int posX = playTetriMino.PlayTetriMinoPosition.x;
            int posY = playTetriMino.PlayTetriMinoPosition.y;
            int height = shape.GetLength(0);
            int width = shape.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (shape[y, x].ThisMinoType != MinoType.None)
                    {
                        int fieldY = posY + y;
                        int fieldX = posX + x;
                        if (fieldY >= 0 && fieldY < tetrisField.TetrisFieldArray.GetLength(1) && fieldX >= 0 && fieldX < tetrisField.TetrisFieldArray.GetLength(0))
                        {
                            tetrisField.TetrisFieldArray[fieldX, fieldY].ThisMinoType = shape[y, x].ThisMinoType;
                        }
                    }
                }
            }
        }

        private void DestroyObjectArray(GameObject[] destroyObjectArray)
        {
            foreach(GameObject obj in destroyObjectArray)
            {
                if(obj != null)
                {
                    Destroy(obj);
                }
            }
        }

        private void SpawnFieldObject()
        {
            if(TetrisFieldObjectArray != null)
            {
                DestroyObjectArray(TetrisFieldObjectArray);
            }

            int count = 0;

            for(int raw = 0; raw < tetrisField.TetrisFieldArray.GetLength(0); raw++)
            {
                for(int col = 0; col < tetrisField.TetrisFieldArray.GetLength(1); col++)
                {
                    if(tetrisField.TetrisFieldArray[raw, col].ThisMinoType != MinoType.None)
                    {
                        TetrisFieldObjectArray[count] = SpawnMinoObject(new Vector3(-4.5f + raw, -10.0f + col), tetrisField.TetrisFieldArray[raw, col].ThisMinoType);
                        count++;
                    }
                }
            }

            return;
        }
    }
}