using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameOver = false;

    public Player CurrentPlayer { get; private set; }
    public Player OpponentPlayer { get; private set; }

    [SerializeField] public bool IsAIEnable = false;
    private MinimaxAI minimaxAI;

    [SerializeField] private GameObject[] Buttons;

    [SerializeField] public ChessPiece generalRed;
    [SerializeField] public ChessPiece generalBlack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(bool isAIEnable)
    {
        IsGameOver = false;
        IsAIEnable = isAIEnable;
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.BUTTON_CLICK);
        BoardManager boardManager = BoardManager.Instance;
        if (boardManager != null)
        {
            BoardManager.Instance.ResetBoard();
            InitializePlayers();
            foreach (GameObject button in Buttons)
            {
                button.GetComponent<Button>().interactable = false;
            }
            if (IsAIEnable)
            {
                minimaxAI = new MinimaxAI();
            }

            Debug.Log("棋盘初始化成功了，当前为红方回合");
        }
        else
        {
            Debug.LogError("BoardManager的实例是空的！");
        }
    }

    /// <summary>
    /// 切换回合
    /// </summary>
    public void EndTurn()
    {
        BoardManager.Instance.IsOperating = false;
        if (CurrentPlayer == null)
        {
            Debug.LogError("当前玩家为空！");
            return;
        }

        Player temp = new Player(CurrentPlayer);
        CurrentPlayer = OpponentPlayer;
        OpponentPlayer = temp;

        if (Checkmate())
        {
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.GAME_ALARM);
        }

        if (IsAIEnable && CurrentPlayer.Label == Label.Black && !IsGameOver)
        {
            StartCoroutine(MakeAIMove());
        }
    }

    private IEnumerator MakeAIMove()
    {
        Vector2Int[] move = minimaxAI.GetBestMove(BoardManager.Instance.Board, BoardManager.Instance.playerBlack.Label);
        yield return null;

        if (move != null)
        {
            Debug.Log("现在是AI回合");
            Debug.Log($"AI：我想尝试把({move[0].x},{move[0].y})的棋子{BoardManager.Instance.Board[move[0].x, move[0].y].Type}移动到({move[1].x},{move[1].y})");
            ChessPiece piece = BoardManager.Instance.Board[move[0].x, move[0].y];
            BoardManager.Instance.IsOperating = false;
            StartCoroutine(BoardManager.Instance.MoveChess(piece, move[1]));
            Debug.Log("AI移动成功了");
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        StartCoroutine("PlayGameOverSound", 1.5f);
        foreach (Transform child in BoardManager.Instance.transform)
        {
            child.GetComponent<ChessPiece>().enabled = false;
        }
        foreach (GameObject button in Buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        Debug.Log("Game Over!");
    }

    private IEnumerator PlayGameOverSound()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.GAME_OVER);
        return null;
    }

    /// <summary>
    /// 检查任意一方在该步之后是否将军
    /// </summary>
    /// <returns></returns>
    private bool Checkmate()
    {
        List<ChessPiece> redPlayerPieces = BoardManager.Instance.playerRed.RemainingPieces;
        List<ChessPiece> blackPlayerPieces = BoardManager.Instance.playerBlack.RemainingPieces;

        if (generalBlack == null || generalRed == null)
        {
            return false;
        }

        Vector2Int redGeneralPosition = generalRed.CurrentPosition;
        Vector2Int blackGeneralPosition = generalBlack.CurrentPosition;

        foreach (ChessPiece piece in redPlayerPieces)
        {
            if (BoardManager.IsValidMove(piece, blackGeneralPosition))
            {
                return true;
            }
        }
        foreach (ChessPiece piece in blackPlayerPieces)
        {
            if (BoardManager.IsValidMove(piece, redGeneralPosition))
            {
                return true;
            }
        }

        return false;
    }

    private void InitializePlayers()
    {
        CurrentPlayer = BoardManager.Instance.playerRed;
        OpponentPlayer = BoardManager.Instance.playerBlack;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}