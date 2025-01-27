using static MinoManagement.Mino;
using Commons;

namespace MinoManagement
{
    public class Mino
    {
        public enum MinoType
        {
            None = -1,
            I_Mino = 0,
            O_Mino = 1,
            T_Mino = 2,
            S_Mino = 3,
            Z_Mino = 4,
            J_Mino = 5,
            L_Mino = 6
        }
        public MinoType ThisMinoType { get; set; }
        
        public Mino()
        {
            ThisMinoType = MinoType.None;
        }
    }

    public class TetriMino
    {
        public MinoType ThisTetriMinoType { get; set; }

        public int MinoRotation { get; set; }

        public Mino[,] PlayTetriMino { get; set; }

        public IntVector2 PlayTetriMinoPosition { get; set; }

        public Mino[,] PlayTetriMinoAboveField { get; set; } = new Mino[10, 20];

        public TetriMino(MinoType MinoType)
        {
            ThisTetriMinoType = MinoType;
            MinoRotation = 0;
            SetSpawnPosition();
            PlayTetriMino = ReflectPlayTetriMino(MinoRotation);
            PlayTetriMinoAboveField = ReflectPlayFieldFunc(PlayTetriMino, PlayTetriMinoPosition);
        }

        private int[,] MinoArray { get; } = new int[7, 4]
        {
            {
                0b1_1111_0000,
                0b10_0010_0010_0010,
                0b1_1111_0000,
                0b10_0010_0010_0010
            },
            {
                0b11_0011,
                0b11_0011,
                0b11_0011,
                0b11_0011
            },
            {
                0b10_0111_0000,
                0b10_0011_0010,
                0b111_0010,
                0b10_0110_0010
            },
            {
                0b11_0110,
                0b10_0011_0001,
                0b11_0110,
                0b10_0011_0001
            },
            {
                0b110_0011,
                0b1_0011_0010,
                0b110_0011,
                0b1_0011_0010
            },
            {
                0b100_0111_0000,
                0b11_0010_0010,
                0b111_0001,
                0b10_0010_0110
            },
            {
                0b1_0111_0000,
                0b10_0010_0011,
                0b111_0100,
                0b110_0010_0010
            }
        };

        public int GetMinoNum()
        {
            return MinoArray[(int) ThisTetriMinoType, MinoRotation];
        }

        private void SetSpawnPosition()
        {
            if(ThisTetriMinoType == MinoType.I_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(3, 17);
            }
            else if(ThisTetriMinoType == MinoType.O_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(4, 18);
            }
            else if(ThisTetriMinoType == MinoType.T_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(4, 18);
            }
            else if(ThisTetriMinoType == MinoType.S_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(4, 18);
            }
            else if(ThisTetriMinoType == MinoType.Z_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(4, 18);
            }
            else if(ThisTetriMinoType == MinoType.J_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(4, 17);
            }
            else if(ThisTetriMinoType == MinoType.L_Mino)
            {
                PlayTetriMinoPosition = new IntVector2(4, 17);
            }
        }

        private Mino[,] ReflectPlayTetriMino(int rotate)
        {
            Mino[,] playTetriMino = new Mino[4, 4];
            int minoNum = GetMinoNum();
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    playTetriMino[i, j] = new Mino();
                }
            }

            int count = 0;
            for(int n = 0; n < 16; n++)
            {
                if((MinoArray[(int) ThisTetriMinoType, rotate] & (1 << n)) != 0)
                {
                    playTetriMino[n % 4, n / 4].ThisMinoType = ThisTetriMinoType;
                    count++;
                    if(count == 4)
                    {
                        break;
                    }
                }
            }
            return playTetriMino;
        }

        private Mino[,] reflectPlayField = new Mino[10, 20];

        private Mino[,] ReflectPlayFieldFunc(Mino[,] playTetriMino, IntVector2 position)
        {
            int countReflect = 0;
            for(int row = 0; row < reflectPlayField.GetLength(0); row++)
            {
                for(int col = 0; col < reflectPlayField.GetLength(1); col++)
                {
                    reflectPlayField[row, col] = new();
                }
            }

            for(int row = 0; row < playTetriMino.GetLength(0); row++)
            {
                for(int col = 0; col < playTetriMino.GetLength(1); col++)
                {
                    if(playTetriMino[row, col].ThisMinoType != MinoType.None)
                    {
                        if(position.x + row >= 0 && position.x + row < reflectPlayField.GetLength(0) && position.y + col >= 0 && position.y + col < reflectPlayField.GetLength(1))
                        {
                            reflectPlayField[row + position.x, col + position.y].ThisMinoType = ThisTetriMinoType;
                            
                            if(ThisTetriMinoType != MinoType.None)
                            {
                                countReflect++;
                            }

                            if(countReflect == 4)
                            {
                                return reflectPlayField;
                            }
                        }
                    }
                }
            }

            return reflectPlayField;
        }

        private int tempMinoRotation;
        private Mino[,] tempPlayTetriMino = new Mino[4, 4];
        private Mino[,] tempPlayField = new Mino[10, 20];
        private IntVector2 tempPlayTetriMinoPosition = new();

        public void TetriMinoReflectValue(IntVector2 moveVector, int rotation, Mino[,] tetrisField)
        {
            tempMinoRotation = MinoRotation;
            tempPlayTetriMinoPosition = PlayTetriMinoPosition;

            tempPlayTetriMinoPosition.x += moveVector.x;
            tempPlayTetriMinoPosition.y += moveVector.y;
            tempMinoRotation += rotation;

            if(tempMinoRotation == 4)
            {
                tempMinoRotation = 0;
            }
            else if(tempMinoRotation == -1)
            {
                tempMinoRotation = 3;
            }

            tempPlayTetriMino = ReflectPlayTetriMino(tempMinoRotation);
            tempPlayField = ReflectPlayFieldFunc(tempPlayTetriMino, tempPlayTetriMinoPosition);

            int count = 0;

            for(int row = 0; row < tempPlayField.GetLength(0); row++)
            {
                for(int col = 0; col < tempPlayField.GetLength(1); col++)
                {
                    if(tempPlayField[row, col].ThisMinoType != MinoType.None && tetrisField[row, col].ThisMinoType != MinoType.None)
                    {
                        count++;
                    }

                    if(count == 4)
                    {
                        PlayTetriMinoPosition = tempPlayTetriMinoPosition;
                        MinoRotation = tempMinoRotation;
                        PlayTetriMino = tempPlayTetriMino;
                        PlayTetriMinoAboveField = tempPlayField;
                        return;
                    }
                }
            }

            return;
        }
    }
}