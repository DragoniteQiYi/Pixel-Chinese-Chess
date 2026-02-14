using System.Collections.Generic;
using UnityEngine;

public class MinimaxAI
{
    private int maxDepth = 3;

    public Vector2Int[] GetBestMove(ChessPiece[,] board, Label playerLabel)
    {
        int bestScore = int.MinValue;
        Vector2Int[] bestMove = null;

        List<Vector2Int[]> possibleMoves = GetAllPossibleMoves(board, playerLabel);
        foreach (var move in possibleMoves)
        {
            ChessPiece capturedPiece = board[move[1].x, move[1].y];
            ChessPiece movingPiece = board[move[0].x, move[0].y];

            // 模拟一次操作
            board[move[1].x, move[1].y] = movingPiece;
            board[move[0].x, move[0].y] = null;
            movingPiece.CurrentPosition = new Vector2Int(move[1].x, move[1].y);

            int score = Minimax(board, maxDepth, int.MinValue, int.MaxValue, false, playerLabel);

            // 撤销操作
            board[move[0].x, move[0].y] = movingPiece;
            board[move[1].x, move[1].y] = capturedPiece;
            movingPiece.CurrentPosition = new Vector2Int(move[0].x, move[0].y);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private List<Vector2Int[]> GetAllPossibleMoves(ChessPiece[,] board, Label playerLabel)
    {
        List<Vector2Int[]> possibleMoves = new List<Vector2Int[]>();

        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.Label == playerLabel)
            {
                List<Vector2Int> validMoves = BoardManager.Instance.GetValidMoves(piece);
                foreach (Vector2Int move in validMoves)
                {
                    possibleMoves.Add(new Vector2Int[] { piece.CurrentPosition, move });
                }
            }
        }

        return possibleMoves;
    }

    private int Minimax(ChessPiece[,] board, int depth, int alpha, int beta, bool isMaximizing, Label playerLabel)
    {
        if (depth == 0 || IsGameOver(board))
        {
            return EvaluateBoard(board, playerLabel);
        }

        List<Vector2Int[]> possibleMoves = GetAllPossibleMoves(board, playerLabel);

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (var move in possibleMoves)
            {
                ChessPiece capturedPiece = board[move[1].x, move[1].y];
                ChessPiece movingPiece = board[move[0].x, move[0].y];

                // 模拟一次操作
                board[move[1].x, move[1].y] = movingPiece;
                board[move[0].x, move[0].y] = null;
                movingPiece.CurrentPosition = new Vector2Int(move[1].x, move[1].y);

                int eval = Minimax(board, depth - 1, alpha, beta, false, playerLabel);

                // 撤销一次操作
                board[move[0].x, move[0].y] = movingPiece;
                board[move[1].x, move[1].y] = capturedPiece;
                movingPiece.CurrentPosition = new Vector2Int(move[0].x, move[0].y);

                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var move in possibleMoves)
            {
                ChessPiece capturedPiece = board[move[1].x, move[1].y];
                ChessPiece movingPiece = board[move[0].x, move[0].y];

                // 模拟一次操作
                board[move[1].x, move[1].y] = movingPiece;
                board[move[0].x, move[0].y] = null;
                movingPiece.CurrentPosition = new Vector2Int(move[1].x, move[1].y);

                int eval = Minimax(board, depth - 1, alpha, beta, true, playerLabel);

                // 撤销操作
                board[move[0].x, move[0].y] = movingPiece;
                board[move[1].x, move[1].y] = capturedPiece;
                movingPiece.CurrentPosition = new Vector2Int(move[0].x, move[0].y);

                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return minEval;
        }
    }

    private bool IsGameOver(ChessPiece[,] board)
    {
        return false;
    }

    private int EvaluateBoard(ChessPiece[,] board, Label playerLabel)
    {
        int score = 0;

        foreach (ChessPiece piece in board)
        {
            if (piece != null)
            {
                if (piece.Label == playerLabel)
                {
                    score += GetPieceValue(piece.Type);
                }
                else
                {
                    score -= GetPieceValue(piece.Type);
                }
            }
        }

        return score;
    }

    private int GetPieceValue(Type pieceType)
    {
        switch (pieceType)
        {
            case Type.General:
                return 1000;
            case Type.Rook:
                return 500;
            case Type.Cannon:
                return 400;
            case Type.Knight:
                return 300;
            case Type.Elephant:
                return 200;
            case Type.Mandarin:
                return 100;
            case Type.Pawn:
                return 50;
            default:
                return 0;
        }
    }
}