using System.Collections.Generic;

public class Player
{
    public Label Label { get; private set; }
    public List<ChessPiece> RemainingPieces { get; private set; }

    public Player(Label label)
    {
        Label = label;
        RemainingPieces = new List<ChessPiece>();
    }    

    public Player(Player player)
    {
        Label = player.Label;
        RemainingPieces = player.RemainingPieces;
    }
}