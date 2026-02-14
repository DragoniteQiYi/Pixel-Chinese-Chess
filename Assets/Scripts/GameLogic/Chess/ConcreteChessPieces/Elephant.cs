using UnityEngine;

public class Elephant : ChessPiece
{
    protected override void Start() => Type = Type.Elephant;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        int deltaX = gridPosition.x - CurrentPosition.x;
        int deltaY = gridPosition.y - CurrentPosition.y;

        // 检查是否以斜对角移动两格
        if (Mathf.Abs(deltaX) != 2 || Mathf.Abs(deltaY) != 2)
        {
            return false;
        }

        // 不能过河
        if (Label == Label.Red)
        {
            if (gridPosition.y > 4)
            {
                return false;
            }
        }
        else if (Label == Label.Black)
        {
            if (gridPosition.y < 5)
            {
                return false;
            }
        }

        // 检查中间是否有障碍物
        int midX = CurrentPosition.x + deltaX / 2;
        int midY = CurrentPosition.y + deltaY / 2;

        if (BoardManager.Instance.Board[midX, midY] != null)
        {
            return false;
        }

        // 检查目标位置
        ChessPiece targetPiece = BoardManager.Instance.Board[gridPosition.x, gridPosition.y];
        if (targetPiece != null)
        {
            return targetPiece.Label != Label;
        }

        return true;
    }
}