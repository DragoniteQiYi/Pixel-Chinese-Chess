using UnityEngine;
using System.Collections;

public abstract class ChessPiece : MonoBehaviour
{
    [Tooltip("决定这个棋子是红还是黑")][SerializeField] public Label Label;

    public Type Type;
    public bool IsSelected;
    public Vector2Int CurrentPosition;

    private Vector3 _originalPosition;

    private SpriteRenderer _sprite;

    public void MoveTo(int x, int y) 
    {
        _originalPosition = new Vector3(x, y, 0);
        CurrentPosition = new Vector2Int(x, y);
    }

    /// <summary>
    /// 具体类型棋子的移动逻辑，如果允许落棋，返回true
    /// </summary>
    /// <returns></returns>
    public abstract bool MovingLogic(Vector2Int gridPosition);

    protected virtual void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _originalPosition = transform.position;
    }

    protected virtual void Start()
    {
        Type = Type.None;
    }

    protected void OnMouseEnter()
    {
        if (CanNotContract())
        {
            return;
        }
        _sprite.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
    }

    protected void OnMouseExit()
    {
        _sprite.color = new Vector4(1, 1, 1, 1);
    }

    protected void OnMouseDown()
    {
        if (CanNotContract())
        {
            return;
        }
        if (PointerManager.Instance != null && GameManager.Instance.CurrentPlayer.Label == Label)
        {
            PointerManager.Instance.Select(this);
        }
    }

    public IEnumerator FloatUp()
    {
        Vector3 targetPosition = _originalPosition + new Vector3(0, 0.25f, 0); // 上浮0.25个单位
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        float speed = 2.0f; // 上浮速度
        while (IsSelected && Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }
    }

    public IEnumerator SinkDown()
    {
        float speed = 2.0f; // 下沉速度
        while (!IsSelected && Vector3.Distance(transform.position, _originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _originalPosition, Time.deltaTime * speed);
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    private bool IsValidPiece()
    {
        return Label == GameManager.Instance.CurrentPlayer.Label;
    }

    private bool CanNotContract()
    {
        return !IsValidPiece() ||
            GameManager.Instance.IsGameOver ||
            BoardManager.Instance.IsOperating ||
            (GameManager.Instance.IsAIEnable && Label == Label.Black); 
    }
}