using UnityEngine;

public class Knight : ChessPiece
{
    protected override void Start() => Type = Type.Knight;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        int deltaX = gridPosition.x - CurrentPosition.x;
        int deltaY = gridPosition.y - CurrentPosition.y;

        // 检查是否走“日”字格
        if (!(Mathf.Abs(deltaX) == 2 && Mathf.Abs(deltaY) == 1) && !(Mathf.Abs(deltaX) == 1 && Mathf.Abs(deltaY) == 2))
        {
            return false;
        }

        // 检查是否有绊马脚
        if (Mathf.Abs(deltaX) == 2)
        {
            int blockingX = CurrentPosition.x + deltaX / 2;
            if (BoardManager.Instance.Board[blockingX, CurrentPosition.y] != null)
            {
                return false;
            }
        }
        else if (Mathf.Abs(deltaY) == 2)
        {
            int blockingY = CurrentPosition.y + deltaY / 2;
            if (BoardManager.Instance.Board[CurrentPosition.x, blockingY] != null)
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

        return true;
    }
}
