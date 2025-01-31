namespace Bootcamp.GameManager;

public interface IGameRepository
{
    Task GetStudioByName();

    Task DeleteGame();
    Task GetAllGames();
    Task GetStudiosDisconnected();
    Task GetAveragePrice();
    Task ExecuteQuery(string query);
}
