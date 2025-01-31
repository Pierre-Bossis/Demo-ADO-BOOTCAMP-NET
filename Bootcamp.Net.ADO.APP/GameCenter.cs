using Bootcamp.GameManager;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Bootcamp.Net.ADO.APP
{
    public class GameCenter
    {
        private readonly IGameRepository _repository;

        public GameCenter()
        {
            _repository = GameRepository.Instance;
        }

        public void GetStudioByName()
        {
            _repository.GetStudioByName();
        }

        public void DeleteGame()
        {
            _repository.DeleteGame();
        }
        public void GetAllGames()
        {
            _repository.GetAllGames();
        }
        public void GetStudiosDisconnected()
        {
            _repository.GetStudiosDisconnected();
        }
        public void GetAveragePrice()
        {
            _repository.GetAveragePrice();
        }
        public void ExecuteQuery(string query)
        {
            _repository.ExecuteQuery(query);
        }
    }
}
