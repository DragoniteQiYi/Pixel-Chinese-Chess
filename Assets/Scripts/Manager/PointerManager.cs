using UnityEngine;

public class PointerManager : MonoBehaviour
{
    // PointerManager类的共享实例
    public static PointerManager Instance { get; private set; }

    [SerializeField] private Texture2D mouseCursor;

    [SerializeField] public ChessPiece SelectedPiece;

    [SerializeField] private GameObject[] transparentRedPiecesPrefabs;

    [SerializeField] private GameObject[] transparentBlackPiecesPrefabs;

    [SerializeField] private GameObject transparentChessObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);        
    }

    private void Update()
    {
        if (SelectedPiece != null)
        {
            CreateTransparentChess();
        }
        else
        {
            Destroy(transparentChessObject);
        }
    }

    /// <summary>
    /// 检测鼠标左键单击，如果选中了棋子，则走棋
    /// </summary>
    public void OnMove()
    {
        Debug.Log("哔！你点了一下左键");
        if (SelectedPiece != null) 
        {
            TryMoveSelectedPiece();
        }
    }

    /// <summary>
    /// 检测鼠标右键单击，取消选择
    /// </summary>
    public void OnCancel()
    {
        Debug.Log("咻！你点了一下右键");
        if (SelectedPiece != null)
        {
            Instance.Deselect(SelectedPiece);
        }
    }

    public void Select(ChessPiece newPiece)
    {
        ChessPiece currentChess = Instance.SelectedPiece;
        if (currentChess != null)
        {
            Instance.Deselect(currentChess);
        }
        if (currentChess == newPiece)
        {
            Instance.Deselect(newPiece);
            return;
        }

        newPiece.IsSelected = true;
        Instance.SelectedPiece = newPiece;
        Debug.Log($"已选中：{newPiece.Label} {newPiece.Type}");
        Debug.Log($"坐标：{newPiece.CurrentPosition.x},{newPiece.CurrentPosition.y}");
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.CHESS_FLOAT);
        StartCoroutine(newPiece.FloatUp());
    }

    public void Deselect(ChessPiece piece)
    {
        piece.IsSelected = false;
        Instance.SelectedPiece = null;
        StartCoroutine(piece.SinkDown());
    }

    private void TryMoveSelectedPiece()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPosition = BoardManager.WorldToGridPosition(worldPosition);

        if (SelectedPiece.MovingLogic(gridPosition))
        {
            StartCoroutine(BoardManager.Instance.MoveChess(SelectedPiece, gridPosition));
            BoardManager.Instance.IsOperating = true;
            Instance.Deselect(SelectedPiece);
            SelectedPiece = null;
            if (transparentChessObject != null)
            {
                Destroy(transparentChessObject);
            }
        }
    }

    /// <summary>
    /// 如果选中棋子，且落棋点合法，实例化一个棋子的透明预制件跟随鼠标在方格间移动
    /// </summary>
    private void CreateTransparentChess()
    {
        if (transparentChessObject != null)
        {
            Destroy(transparentChessObject);
        }

        int index = BoardManager.TypeIndexMap[SelectedPiece.Type];
        if (SelectedPiece.Label == Label.Red)
        {
            transparentChessObject = Instantiate(transparentRedPiecesPrefabs[index]);
        }
        else if (SelectedPiece.Label == Label.Black)
        {

            transparentChessObject = Instantiate(transparentBlackPiecesPrefabs[index]);
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPosition = BoardManager.WorldToGridPosition(worldPosition);

        if (BoardManager.IsValidMove(SelectedPiece, gridPosition))
        {
            transparentChessObject.transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
        }
        else if (transparentChessObject != null)
        {
            Destroy(transparentChessObject);
        }
    }
}