using UnityEngine;

public class General : ChessPiece
{
    protected override void Start() => Type = Type.General;

    public override bool MovingLogic(Vector2Int gridPosition)
    {
        if (gridPosition.x > 8 || gridPosition.x < 0 || gridPosition.y > 9 || gridPosition.y < 0)
        {
            return false;
        }

        // 判断是否可以飞将
        if (gridPosition.x == CurrentPosition.x)
        {

            int minY = Mathf.Min(CurrentPosition.y, gridPosition.y);
            int maxY = Mathf.Max(CurrentPosition.y, gridPosition.y);
            bool noObstacles = true;
            for (int y = minY + 1; y < maxY; y++)
            {
                if (BoardManager.Instance.Board[CurrentPosition.x, y] != null)
                {
                    noObstacles = false;
                    break;
                }
            }

            ChessPiece targetPiece = BoardManager.Instance.Board[gridPosition.x, gridPosition.y];
            if (noObstacles && targetPiece != null && targetPiece.Type == Type.General && targetPiece.Label != Label)
            {
                return true;
            }
        }

        // 判断是否在营内
        bool inCamp = Label == Label.Red
            ? (gridPosition.x >= 3 && gridPosition.x <= 5 && gridPosition.y >= 0 && gridPosition.y <= 2)
            : (gridPosition.x >= 3 && gridPosition.x <= 5 && gridPosition.y >= 7 && gridPosition.y <= 9);

        if (!inCamp)
        {
            return false;
        }

        // 判断是否在相邻格
        bool isAdjacent = Mathf.Abs(gridPosition.x - CurrentPosition.x) + Mathf.Abs(gridPosition.y - CurrentPosition.y) == 1;

        if (isAdjacent)
        {
            ChessPiece targetPiece = BoardManager.Instance.Board[gridPosition.x, gridPosition.y];
            if (targetPiece != null && targetPiece.Label == Label)
            {
                return false;
            }
            return true;
        }

        return false;
    }
}
