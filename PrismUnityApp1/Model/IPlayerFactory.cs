namespace TicTacToe.Model
{
    public interface IPlayerFactory
    {
        IPlayer GetPlayer(PlayerType playerType);
    }
}