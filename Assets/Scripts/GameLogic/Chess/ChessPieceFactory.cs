//public static class ChessPieceFactory
//{
//    public static ChessPiece CreateChessPiece(Type type, Coordinate position, Label label)
//    {
//        switch (type)
//        {
//            case Type.General: return new General(type, position, label);
//            case Type.Mandarin: return new Mandarin(type, position, label);
//            case Type.Elephant: return new Elephant(type, position, label);
//            case Type.Knight: return new Knight(type, position, label);
//            case Type.Rook: return new Rook(type, position, label);
//            case Type.Cannon: return new Cannon(type, position, label);
//            case Type.Pawn: return new Pawn(type, position, label);
//        }
//        throw new System.Exception("无法确定棋子的类型！");
//    }
//}