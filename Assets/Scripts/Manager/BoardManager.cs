using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // BoardManager类的共享实例
    public static BoardManager Instance { get; private set; }

    // 存放棋子的预制件
    [SerializeField] private List<GameObject> blackPiecesPrefabs;
    [SerializeField] private List<GameObject> redPiecesPrefabs;

    // 棋子类型与预制件列表索引下标的映射关系
    public static readonly Dictionary<Type, int> TypeIndexMap = new Dictionary<Type, int>()
    {
        {Type.General, 0},
        {Type.Mandarin, 1},
        {Type.Elephant, 2},
        {Type.Knight, 3},
        {Type.Rook, 4 },
        {Type.Cannon, 5},
        {Type.Pawn, 6}
    };

    // 二位玩家的信息
    public Player playerRed;
    public Player playerBlack;

    /// <summary>
    /// 这是棋盘本体
    /// </summary>
    public ChessPiece[,] Board { get; private set; }

    public bool IsOperating = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<BoardManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeBoard()
    {
        Board = new ChessPiece[9, 10];
        playerRed = new Player(Label.Red);
        playerBlack = new Player(Label.Black);
    }

    /// <summary>
    /// 初始化所有棋子并进行必要处理
    /// </summary>
    private void InitializePieces()
    {
        InitializeRedPieces();
        InitializeBlackPieces();
    }

    /// <summary>
    /// 布置红方棋子
    /// </summary>
    private void InitializeRedPieces()
    {
        Board[0, 0] = InstantiateChessGameObject(redPiecesPrefabs[4], new Vector2Int(0, 0));
        Board[1, 0] = InstantiateChessGameObject(redPiecesPrefabs[3], new Vector2Int(1, 0));
        Board[2, 0] = InstantiateChessGameObject(redPiecesPrefabs[2], new Vector2Int(2, 0));
        Board[3, 0] = InstantiateChessGameObject(redPiecesPrefabs[1], new Vector2Int(3, 0));
        Board[4, 0] = InstantiateChessGameObject(redPiecesPrefabs[0], new Vector2Int(4, 0));
        Board[5, 0] = InstantiateChessGameObject(redPiecesPrefabs[1], new Vector2Int(5, 0));
        Board[6, 0] = InstantiateChessGameObject(redPiecesPrefabs[2], new Vector2Int(6, 0));
        Board[7, 0] = InstantiateChessGameObject(redPiecesPrefabs[3], new Vector2Int(7, 0));
        Board[8, 0] = InstantiateChessGameObject(redPiecesPrefabs[4], new Vector2Int(8, 0));
        for (int i = 1; i <= 7; i += 6)
        {
            Board[i, 2] = InstantiateChessGameObject(redPiecesPrefabs[5], new Vector2Int(i, 2));
        }
        for (int i = 0; i <= 8; i += 2)
        {
            Board[i, 3] = InstantiateChessGameObject(redPiecesPrefabs[6], new Vector2Int(i, 3));
        }
        GameManager.Instance.generalRed = Board[4, 0];
    }

    /// <summary>
    /// 布置黑方棋子
    /// </summary>
    private void InitializeBlackPieces()
    {
        Board[0, 9] = InstantiateChessGameObject(blackPiecesPrefabs[4], new Vector2Int(0, 9));
        Board[1, 9] = InstantiateChessGameObject(blackPiecesPrefabs[3], new Vector2Int(1, 9));
        Board[2, 9] = InstantiateChessGameObject(blackPiecesPrefabs[2], new Vector2Int(2, 9));
        Board[3, 9] = InstantiateChessGameObject(blackPiecesPrefabs[1], new Vector2Int(3, 9));
        Board[4, 9] = InstantiateChessGameObject(blackPiecesPrefabs[0], new Vector2Int(4, 9));
        Board[5, 9] = InstantiateChessGameObject(blackPiecesPrefabs[1], new Vector2Int(5, 9));
        Board[6, 9] = InstantiateChessGameObject(blackPiecesPrefabs[2], new Vector2Int(6, 9));
        Board[7, 9] = InstantiateChessGameObject(blackPiecesPrefabs[3], new Vector2Int(7, 9));
        Board[8, 9] = InstantiateChessGameObject(blackPiecesPrefabs[4], new Vector2Int(8, 9));
        for (int i = 1; i <= 7; i += 6)
        {
            Board[i, 7] = InstantiateChessGameObject(blackPiecesPrefabs[5], new Vector2Int(i, 7));
        }
        for (int i = 0; i <= 8; i += 2)
        {
            Board[i, 6] = InstantiateChessGameObject(blackPiecesPrefabs[6], new Vector2Int(i, 6));
        }
        GameManager.Instance.generalBlack = Board[4, 9];
    }

    /// <summary>
    /// 在棋盘指定位置实例化一个棋子游戏对象，并设置它的属性，将其设为棋盘的子对象
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="piece"></param>
    private ChessPiece InstantiateChessGameObject(GameObject chessPrefab, Vector2Int coordinate)
    {
        Vector2 position = new Vector2(coordinate.x, coordinate.y);
        GameObject chessObject = Instantiate(chessPrefab, position, Quaternion.identity);
        ChessPiece chessPiece = chessObject.GetComponent<ChessPiece>();
        chessPiece.CurrentPosition = coordinate;
        chessObject.transform.SetParent(gameObject.transform);

        if (chessPiece.Label == Label.Red)
        {
            playerRed.RemainingPieces.Add(chessPiece);
        }
        else if (chessPiece.Label == Label.Black)
        {
            playerBlack.RemainingPieces.Add(chessPiece);
        }

        return chessPiece;
    }

    /// <summary>
    /// 将世界坐标转换为棋盘的网格坐标
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public static Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        // 棋盘的左下角是(0, 0)，每个格子的大小是1
        int x = Mathf.FloorToInt(Mathf.Round(worldPosition.x));
        int y = Mathf.FloorToInt(Mathf.Round(worldPosition.y));
        return new Vector2Int(x, y);
    }

    public static bool IsValidMove(ChessPiece piece, Vector2Int targetPosition)
    {
        return piece.MovingLogic(targetPosition);
    }

    public List<Vector2Int> GetValidMoves(ChessPiece piece)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (piece.MovingLogic(gridPosition))
                {
                    validMoves.Add(gridPosition);
                }
            }
        }

        return validMoves;
    }

    /// <summary>
    /// 处理棋子在棋盘上的移动
    /// </summary>
    /// <param name="selectedPiece"></param>
    /// <param name="newPosition"></param>
    public IEnumerator MoveChess(ChessPiece selectedPiece, Vector2Int newPosition)
    {
        Vector3 targetWorldPosition = new Vector3(newPosition.x, newPosition.y, 0);
        ChessPiece targetPiece = Board[newPosition.x, newPosition.y];

        float moveDuration = 0.5f;
        Vector3 startPosition = selectedPiece.transform.position;
        float elapsedTime = 0f;

        // 将棋子在固定时间内平滑移动至目标坐标
        while (elapsedTime < moveDuration)
        {
            selectedPiece.transform.position = Vector3.Lerp(startPosition, targetWorldPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保棋子的世界坐标准确无误
        selectedPiece.transform.position = targetWorldPosition;

        // 进行吃子判定
        if (targetPiece != null)
        {
            Capture(newPosition);
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.CHESS_CAPTURE);
            if (targetPiece.Type == Type.General)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.CHESS_MOVE);
        }

        // 更改棋盘上相应棋子的逻辑坐标
        Board[selectedPiece.CurrentPosition.x, selectedPiece.CurrentPosition.y] = null;
        Board[newPosition.x, newPosition.y] = selectedPiece;
        selectedPiece.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedPiece.MoveTo(newPosition.x, newPosition.y);
        yield return new WaitForSeconds(0.1f);
        
        // 结束当前回合
        GameManager.Instance.EndTurn();
    }

    /// <summary>
    /// 成功执行后，吃掉指定坐标的棋子
    /// </summary>
    /// <param name="targetPosition"></param>
    private void Capture(Vector2Int targetPosition)
    {
        ChessPiece capturedPiece = Board[targetPosition.x, targetPosition.y];

        // 将被销毁的棋子从对应玩家的剩余棋子列表中移除
        if (capturedPiece.Label == Label.Red)
        {
            playerRed.RemainingPieces.Remove(capturedPiece);
        }
        else if (capturedPiece.Label == Label.Black)
        {
            playerBlack.RemainingPieces.Remove(capturedPiece);
        }

        // 延迟0.5秒销毁棋子
        StartCoroutine(DestroyPieceAfterDelay(capturedPiece, 0f));
    }

    private IEnumerator DestroyPieceAfterDelay(ChessPiece piece, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(piece.gameObject);
    }

    /// <summary>
    /// 游戏结束后，重置棋盘
    /// </summary>
    public void ResetBoard()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        InitializeBoard();
        InitializePieces();
    }
}