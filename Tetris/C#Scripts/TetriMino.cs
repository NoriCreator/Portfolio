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
        private const int FieldWidth = 10;
        private const int FieldHeight = 20;
        private const int MinoSize = 4;
        private const int MaxRotation = 4;
        private const int MinRotation = 0;
        private const int InitialRotation = 0;
        private const int MaxApplyCount = 4;

        public MinoType ThisTetriMinoType { get; set; }

        public int MinoRotation { get; set; }

        public Mino[,] PlayTetriMino { get; set; }

        public IntVector2 PlayTetriMinoPosition { get; set; }

        public Mino[,] PlayTetriMinoAboveField { get; set; } = new Mino[FieldWidth, FieldHeight];

        public TetriMino(MinoType minoType)
        {
            ThisTetriMinoType = minoType;
            MinoRotation = InitialRotation;
            SetSpawnPosition();
            PlayTetriMino = ApplyPlayTetriMino(MinoRotation);
            PlayTetriMinoAboveField = ApplyPlayField(PlayTetriMino, PlayTetriMinoPosition);
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
            PlayTetriMinoPosition = ThisTetriMinoType switch
            {
                MinoType.I_Mino => new IntVector2(3, 17),
                MinoType.O_Mino => new IntVector2(4, 18),
                MinoType.T_Mino => new IntVector2(4, 17),
                MinoType.S_Mino => new IntVector2(4, 18),
                MinoType.Z_Mino => new IntVector2(4, 18),
                MinoType.J_Mino => new IntVector2(4, 17),
                MinoType.L_Mino => new IntVector2(4, 17),
                _ => new IntVector2(0, 0)
            };
        }

        private Mino[,] ApplyPlayTetriMino(int rotate)
        {
            Mino[,] playTetriMino = new Mino[MinoSize, MinoSize];
            int minoNum = GetMinoNum();
            for (int i = 0; i < MinoSize; i++)
            {
                for (int j = 0; j < MinoSize; j++)
                {
                    playTetriMino[i, j] = new Mino();
                }
            }

            int count = 0;
            for (int n = 0; n < MinoSize * MinoSize; n++)
            {
                if ((MinoArray[(int) ThisTetriMinoType, rotate] & (1 << n)) != 0)
                {
                    playTetriMino[n % MinoSize, n / MinoSize].ThisMinoType = ThisTetriMinoType;
                    count++;
                    if (count == MinoSize)
                    {
                        break;
                    }
                }
            }
            return playTetriMino;
        }

        private Mino[,] applyPlayField = new Mino[FieldWidth, FieldHeight];

        private Mino[,] ApplyPlayField(Mino[,] playTetriMino, IntVector2 position)
        {
            int countApply = 0;
            for (int row = 0; row < FieldWidth; row++)
            {
                for (int col = 0; col < FieldHeight; col++)
                {
                    applyPlayField[row, col] = new();
                }
            }

            for (int row = 0; row < playTetriMino.GetLength(0); row++)
            {
                for (int col = 0; col < playTetriMino.GetLength(1); col++)
                {
                    if (playTetriMino[row, col].ThisMinoType != MinoType.None)
                    {
                        if (position.x + row >= 0 && position.x + row < FieldWidth && position.y + col >= 0 && position.y + col < FieldHeight)
                        {
                            applyPlayField[row + position.x, col + position.y].ThisMinoType = ThisTetriMinoType;
                            
                            if(ThisTetriMinoType != MinoType.None)
                            {
                                countApply++;
                            }

                            if(countApply == MaxApplyCount)
                            {
                                return applyPlayField;
                            }
                        }
                    }
                }
            }

            return applyPlayField;
        }

        private int tempMinoRotation;
        private Mino[,] tempPlayTetriMino = new Mino[MinoSize, MinoSize];
        private Mino[,] tempPlayField = new Mino[FieldWidth, FieldHeight];
        private IntVector2 tempPlayTetriMinoPosition = new();

        public void ApplyTetriMino(IntVector2 moveVector, int rotation, Mino[,] tetrisField)
        {
            tempMinoRotation = MinoRotation;
            tempPlayTetriMinoPosition = PlayTetriMinoPosition;

            tempPlayTetriMinoPosition.x += moveVector.x;
            tempPlayTetriMinoPosition.y += moveVector.y;
            tempMinoRotation += rotation;

            if(tempMinoRotation == MaxRotation)
            {
                tempMinoRotation = MinRotation;
            }
            else if(tempMinoRotation == -1)
            {
                tempMinoRotation = MaxRotation - 1;
            }

            tempPlayTetriMino = ApplyPlayTetriMino(tempMinoRotation);
            tempPlayField = ApplyPlayField(tempPlayTetriMino, tempPlayTetriMinoPosition);

            int count = 0;

            for(int row = 0; row < tempPlayField.GetLength(0); row++)
            {
                for(int col = 0; col < tempPlayField.GetLength(1); col++)
                {
                    if(tempPlayField[row, col].ThisMinoType != MinoType.None && tetrisField[row, col].ThisMinoType == MinoType.None)
                    {
                        if(row >= 0 && row < tempPlayField.GetLength(0) && col >= 0 && col < tempPlayField.GetLength(1))
                        {
                            count++;
                        }

                        if(count == MaxApplyCount)
                        {
                            PlayTetriMinoPosition = tempPlayTetriMinoPosition;
                            MinoRotation = tempMinoRotation;
                            PlayTetriMino = tempPlayTetriMino;
                            PlayTetriMinoAboveField = tempPlayField;
                            return;
                        }
                    }
                }
            }

            return;
        }
    }
}