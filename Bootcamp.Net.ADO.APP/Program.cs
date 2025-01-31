using Bootcamp.GameManager;
using Bootcamp.Net.ADO.APP;

GameCenter gameCenter = new();

string query = "SELECT * FROM Studio";
gameCenter.ExecuteQuery(query);









//gameCenter.GetAllGames();
//gameCenter.GetAveragePrice();
//gameCenter.GetStudiosDisconnected();

//bool continuer = true;
//while (continuer)
//{
//    Console.WriteLine("1. Afficher les Details studio par nom\n" +
//                      "2. Supprimer un jeu\n" +
//                      "0. Quitter");
//    string choix = Console.ReadLine();
//    Console.Clear();


//    switch(choix)
//    {
//        case "0":
//            continuer = false;
//            break;
//        case "1":
//            gameCenter.GetStudioByName();
//            break;
//        case "2":
//            gameCenter.DeleteGame();
//            break;

//        default:
//            Console.WriteLine("choix invalide");
//            break;
//    }

//}