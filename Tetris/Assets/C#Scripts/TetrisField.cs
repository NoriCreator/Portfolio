using static MinoManagement.Mino;
using Commons;

namespace MinoManagement
{
    public class TetrisField
    {
        public Mino[,] TetrisFieldArray { get; private set; } = new Mino[10, 20];

        public TetrisField()
        {
            InitializeField();
        }

        private void InitializeField()
        {
            for (int row = 0; row < TetrisFieldArray.GetLength(0); row++)
            {
                for (int col = 0; col < TetrisFieldArray.GetLength(1); col++)
                {
                    TetrisFieldArray[row, col] = new Mino();
                }
            }
        }

        public void ReflectPlayTetriMinoOnField(Mino[,] playTetriMino, IntVector2 position)
        {
            int rows = playTetriMino.GetLength(0);
            int cols = playTetriMino.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (playTetriMino[row, col].ThisMinoType != MinoType.None)
                    {
                        TetrisFieldArray[position.x + row, position.y + col].ThisMinoType = playTetriMino[row, col].ThisMinoType;
                    }
                }
            }
            ClearFullLines();
        }

        private void ClearFullLines()
        {
            for (int col = 0; col < TetrisFieldArray.GetLength(1); col++)
            {
                if (IsLineFull(col))
                {
                    ClearLine(col);
                    MoveDownLinesAbove(col);
                    col--; // Check the same line again after moving down
                }
            }
        }

        private bool IsLineFull(int col)
        {
            for (int row = 0; row < TetrisFieldArray.GetLength(0); row++)
            {
                if (TetrisFieldArray[row, col].ThisMinoType == MinoType.None)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearLine(int col)
        {
            for (int row = 0; row < TetrisFieldArray.GetLength(0); row++)
            {
                TetrisFieldArray[row, col].ThisMinoType = MinoType.None;
            }
        }

        private void MoveDownLinesAbove(int clearedLine)
        {
            for (int col = clearedLine; col < TetrisFieldArray.GetLength(1) - 1; col++)
            {
                for (int row = 0; row < TetrisFieldArray.GetLength(0); row++)
                {
                    TetrisFieldArray[row, col].ThisMinoType = TetrisFieldArray[row, col + 1].ThisMinoType;
                }
            }

            // Clear the top line
            for (int row = 0; row < TetrisFieldArray.GetLength(0); row++)
            {
                TetrisFieldArray[row, TetrisFieldArray.GetLength(1) - 1].ThisMinoType = MinoType.None;
            }
        }
    }
}