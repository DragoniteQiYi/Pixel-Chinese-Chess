using System.Collections.Generic;

public static class Evaluator
{
    private static readonly Dictionary<Type, int> pieceValues = new Dictionary<Type, int>()
    {
        {Type.General, 10000},
        {Type.Rook, 500},
        {Type.Cannon, 400},
        {Type.Knight, 300},
        {Type.Elephant, 250},
        {Type.Mandarin, 250},
        {Type.Pawn, 100}
    };

    public static int EvaluateBoard(ChessPiece[,] board)
    {
        int score = 0;

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                ChessPiece piece = board[x, y];
                if (piece != null)
                {
                    int value = pieceValues[piece.Type];
                    if (piece.Label == Label.Red)
                    {
                        score += value;
                    }
                    else
                    {
                        score -= value;
                    }
                }
            }
        }

        return score;
    }
}