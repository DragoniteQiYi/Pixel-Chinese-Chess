using UnityEngine;

public class Cannon : ChessPiece
{
    protected override void Start() => Type = Type.Cannon;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        // 检查是否为直线移动
        if ((gridPosition.x != CurrentPosition.x && gridPosition.y != CurrentPosition.y) || gridPosition == CurrentPosition)
        {
            return false;
        }

        // 检查路径上的障碍物
        int obstacleCount = 0;

        if (gridPosition.x == CurrentPosition.x)
        {
            int minY = Mathf.Min(CurrentPosition.y, gridPosition.y);
            int maxY = Mathf.Max(CurrentPosition.y, gridPosition.y);
            for (int y = minY + 1; y < maxY; y++)
            {
                if (BoardManager.Instance.Board[CurrentPosition.x, y] != null)
                {
                    obstacleCount += 1;
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
                    obstacleCount += 1;
                }
            }
        }

        // 看看能不能吃
        ChessPiece targetPiece = BoardManager.Instance.Board[gridPosition.x, gridPosition.y];
        if (obstacleCount == 0 && targetPiece == null)
        {
            return true;
        }
        else if (obstacleCount == 1 && targetPiece != null && targetPiece.Label != this.Label)
        {
            return true;
        }

        return false;
    }
}