using UnityEngine;

public class Mandarin : ChessPiece
{
    protected override void Start() => Type = Type.Mandarin;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        int deltaX = gridPosition.x - CurrentPosition.x;
        int deltaY = gridPosition.y - CurrentPosition.y;

        // 检查是否以斜对角移动一格
        if (Mathf.Abs(deltaX) != 1 || Mathf.Abs(deltaY) != 1)
        {
            return false;
        }

        // 不能出界
        if (Label == Label.Red)
        {
            if (gridPosition.x < 3 || gridPosition.x > 5 || gridPosition.y > 2)
            {
                return false;
            }
        }
        else if (Label == Label.Black)
        {
            if (gridPosition.x < 3 || gridPosition.x > 5 || gridPosition.y < 7)
            {
                return false;
            }
        }

        // 检查目标位置
        ChessPiece targetPiece = BoardManager.Instance.Board[gridPosition.x, gridPosition.y];
        if (targetPiece != null)
        {
            return targetPiece.Label != Label;
        }
        else
        {
            return true;
        }
    }
}