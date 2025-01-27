using static MinoManagement.Mino;

namespace MinoManagement
{
    public class TetrisField
    {
        private const int FieldWidth = 10;
        private const int FieldHeight = 20;

        public Mino[,] FieldArray { get; private set; } = new Mino[FieldWidth, FieldHeight];

        public TetrisField()
        {
            InitializeField();
        }

        private void InitializeField()
        {
            for (int row = 0; row < FieldWidth; row++)
            {
                for (int col = 0; col < FieldHeight; col++)
                {
                    FieldArray[row, col] = new Mino();
                }
            }
        }

        public void ClearFullLines()
        {
            for (int col = 0; col < FieldHeight; col++)
            {
                if (IsLineFull(col))
                {
                    ClearLine(col);
                    MoveDownLinesAbove(col);
                    col--;
                }
            }
        }

        private bool IsLineFull(int col)
        {
            for (int row = 0; row < FieldWidth; row++)
            {
                if (FieldArray[row, col].ThisMinoType == MinoType.None)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearLine(int col)
        {
            for (int row = 0; row < FieldWidth; row++)
            {
                FieldArray[row, col].ThisMinoType = MinoType.None;
            }
        }

        private void MoveDownLinesAbove(int clearedLine)
        {
            for (int col = clearedLine; col < FieldHeight - 1; col++)
            {
                for (int row = 0; row < FieldWidth; row++)
                {
                    FieldArray[row, col].ThisMinoType = FieldArray[row, col + 1].ThisMinoType;
                }
            }

            for (int row = 0; row < FieldWidth; row++)
            {
                FieldArray[row, FieldHeight - 1].ThisMinoType = MinoType.None;
            }
        }
    }
}