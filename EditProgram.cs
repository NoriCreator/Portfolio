using UnityEngine;
using Commons;
using static MinoManagement.Mino;

namespace EditProgram
{
    class EditProgram
    {
        // 問題点: Reflectという名前の変数や関数が多く、意味が分かりづらい。
        // 修正内容: Reflectという名前をApplyに変更し、意味を明確にしました。

        // 修正前のコード (一部抜粋):
        // private void ReflectTetriMinoOnField()
        // {
        //     DestroyObjects(CurrentMinoObjects);
        //     int posX = currentTetriMino.PlayTetriMinoPosition.x;
        //     int posY = currentTetriMino.PlayTetriMinoPosition.y;
        //     // ...existing code...
        // }

        // 修正後のコード:
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

        // 問題点: 変数名が分かりづらい。
        // 修正内容: 変数名をわかりやすく変更しました。

        // 修正前のコード (一部抜粋):
        // private int tempMinoRotation;
        // private Mino[,] tempPlayTetriMino = new Mino[MinoSize, MinoSize];
        // private Mino[,] tempPlayField = new Mino[FieldWidth, FieldHeight];
        // private IntVector2 tempPlayTetriMinoPosition = new();

        // 修正後のコード:
        private int temporaryMinoRotation;
        private Mino[,] temporaryPlayTetriMino = new Mino[MinoSize, MinoSize];
        private Mino[,] temporaryPlayField = new Mino[FieldWidth, FieldHeight];
        private IntVector2 temporaryPlayTetriMinoPosition = new();

        // 問題点: マジックナンバーが多く、意味が分かりづらい。
        // 修正内容: マジックナンバーを定数に置き換えました。

        // 修正前のコード (一部抜粋):
        // private const float DefaultDownSpeed = 1.0f;
        // private const float FastDownSpeed = 0.2f;
        // private const float StopTimeThreshold = 2.0f;

        // 修正後のコード:
        private const float DefaultDownSpeed = 1.0f;
        private const float FastDownSpeed = 0.2f;
        private const float StopTimeThreshold = 2.0f;

        // 問題点: C#の機能を使っていない部分がある。
        // 修正内容: C#の機能を使って簡潔にしました。

        // 修正前のコード (一部抜粋):
        // public Vector3 SpawnOrigin { get; private set; } = new Vector3(3.0f, 16.0f, 0.0f);

        // 修正後のコード:
        public Vector3 SpawnOrigin { get; private set; } = new(3.0f, 16.0f, 0.0f);
    }
}
