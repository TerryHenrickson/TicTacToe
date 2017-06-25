using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class PlayerFactory : IPlayerFactory
    {
        private int numberOfHumans = 0;
        private int numberOfComputerPlayers = 0;

        private ComputerIntelligence computerIntelligence;

        public PlayerFactory()
        {
            this.computerIntelligence = new ComputerIntelligence();
        }

        public IPlayer GetPlayer(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Human:
                    return new HumanPlayer() { Name = "Human " + ++this.numberOfHumans };
                case PlayerType.Computer:
                    return new ComputerPlayer(this.computerIntelligence.BoardToUniqueEquivalent, this.computerIntelligence.GetPositionScores()) { Name = "Computer " + ++this.numberOfComputerPlayers };
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}
