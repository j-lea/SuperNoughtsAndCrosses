using System.Collections.Generic;

namespace SuperNoughtsAndCrosses.models
{
    public class GameManager
    {
        private readonly Dictionary<int, SuperGameBoard> _gameBoards;
        private int _count;

        public GameManager()
        {
            _gameBoards = new Dictionary<int, SuperGameBoard>();
            _count = 0;
        }

        public int Add(SuperGameBoard superGameBoard)
        {
            _count++;
            _gameBoards.Add(_count, superGameBoard);
            return _count;
        }

        public SuperGameBoard GetGame(int id)
        {
            return _gameBoards[id];
        }
    }
}