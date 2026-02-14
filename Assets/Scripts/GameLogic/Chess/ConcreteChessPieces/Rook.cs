using UnityEngine;

public class Rook : ChessPiece
{
    protected override void Start() => Type = Type.Rook;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        // 检查是否为直线移动
        if ((gridPosition.x != CurrentPosition.x && gridPosition.y != CurrentPosition.y )|| gridPosition == CurrentPosition)
        {
            return false;
        }

        // 检查路径是否有障碍物
        if (gridPosition.x == CurrentPosition.x)
        {
            int minY = Mathf.Min(CurrentPosition.y, gridPosition.y);
            int maxY = Mathf.Max(CurrentPosition.y, gridPosition.y);
            for (int y = minY + 1; y < maxY; y++)
            {
                if (BoardManager.Instance.Board[CurrentPosition.x, y] != null)
                {
                    return false;
                }
            }
        }
        else if (gridPosition.y == CurrentPosition.y)
        {
            int minX = Mathf.Min(CurrentPosition.x, gridPosition.x);
            int maxX = Mathf.Max(CurrentPosition.x, gridPosition.x);
            for (int x = minX + 1; x < maxX; x++)
            {
                if (BoardManager.Instance.Board[x, CurrentPosition.y] != null)
                {
                    return false;
                }
            }
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