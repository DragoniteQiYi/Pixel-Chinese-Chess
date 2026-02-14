using UnityEngine;

public class Pawn : ChessPiece
{
    protected override void Start() => Type = Type.Pawn;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        // 判断是否前进方向
        bool isForward = Label == Label.Red ? gridPosition.y > CurrentPosition.y : gridPosition.y < CurrentPosition.y;
        bool isHorizontal = Mathf.Abs(gridPosition.x - CurrentPosition.x) == 1 && gridPosition.y == CurrentPosition.y;
        bool isVertical = Mathf.Abs(gridPosition.y - CurrentPosition.y) == 1 && gridPosition.x == CurrentPosition.x;

        // 判断是否过河
        bool crossedRiver = Label == Label.Red ? CurrentPosition.y >= 5 : CurrentPosition.y <= 4;

        if (!crossedRiver)
        {
            if (isForward && isVertical)
            {
                return true;
            }
        }
        else
        {
            if ((isForward && isVertical) || isHorizontal)
            {
                ChessPiece targetPiece = BoardManager.Instance.Board[gridPosition.x, gridPosition.y];
                if (targetPiece != null && targetPiece.Label == Label)
                {
                    return false;
                }
                return true;
            }
        }

        return false;
    }
}