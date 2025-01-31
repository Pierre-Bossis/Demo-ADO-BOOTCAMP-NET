using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Bootcamp.GameManager;
//pas de patern singleton sur repo en situation réelles !!
public class GameRepository : IGameRepository
{
    private readonly SqlConnection _connection;
    private static GameRepository _instance;

    private GameRepository()
    {
        _connection = new SqlConnection("Data Source=E6K-VDI21702\\TFTIC;Initial Catalog=BootcampADO;Integrated Security=True;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public static GameRepository Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameRepository();
            }
            return _instance;
        }
    }
    public async Task DeleteGame()
    {
        Console.WriteLine("Veuillez entrer l'id du jeu à supprimer : ");
        int id = int.Parse(Console.ReadLine());

        using (SqlCommand cmd = _connection.CreateCommand())
        {
            cmd.CommandText = "DeleteGame";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            await _connection.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            if (rows == 1)
            {
                Console.WriteLine("le jeu à été supprimé !");
            }
            else if (rows > 1)
            {
                Console.WriteLine("Oups !");
                // peut être un rollback ici ?
            }
            else
            {
                Console.WriteLine("le jeu n'existe pas ou n'a pas pu être supprimé !");
            }
        }
    }

    public async Task GetAllGames()
    {

        using (DbCommand cmd = _connection.CreateCommand())
        {
            cmd.CommandText = "Select Id, Title, Description from Game";
            cmd.CommandType = CommandType.Text;

            await _connection.OpenAsync();
            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"{reader["Id"]} {reader["Title"]} - {reader["Description"]}");
                }
            }
            await _connection.CloseAsync();
        }
    }

    public async Task GetAveragePrice()
    {
        decimal res = 0;
        int entries = 0;
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            cmd.CommandText = "Select Price from Game";

            await _connection.OpenAsync();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    res += (decimal)reader["Price"];
                    entries++;
                }
            }
            await _connection.CloseAsync();
        }
        res = res / entries;
        Console.WriteLine("Le prix moyen est :" + res + "euros");
    }

    public async Task GetStudioByName()
    {

        List<string> list = new List<string>();
        Console.WriteLine("Veuillez entrer le nom du studio que vous cherchez : ");
        string studioName = Console.ReadLine();

        using (SqlCommand cmd = _connection.CreateCommand())
        {
            cmd.CommandText = $"SELECT * FROM [dbo].[Studio] WHERE Name = @name";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@name", studioName);


            await _connection.OpenAsync();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string name = Convert.ToString(reader["Name"]);
                    DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);
                    string city = Convert.ToString(reader["City"]);
                    int employeNbr = Convert.ToInt32(reader["EmployeNbr"]);
                    bool IsIndependant = Convert.ToBoolean(reader["IsIndependant"]);
                    bool IsActive = Convert.ToBoolean(reader["IsActive"]);

                    list.Add($"{id} | {name} | {creationDate} | {city} | {employeNbr} | {IsIndependant} | {IsActive}");
                }

                await _connection.CloseAsync();
            }

            foreach (string studio in list)
            {
                Console.WriteLine(studio);
            }
            Console.ReadLine();
            Console.Clear();
        }
    }

    public async Task GetStudiosDisconnected()
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            await _connection.OpenAsync();
            cmd.CommandText = "SELECT Id, Name FROM Studio";

            using (SqlDataAdapter da = new(cmd))
            {
                //da.SelectCommand = cmd;
                DataTable dt = new();
                da.Fill(dt);
                await _connection.CloseAsync();

                foreach (DataRow dr in dt.Rows)
                {
                    Console.WriteLine($"{dr["Id"]} {dr["Name"]}");
                }
            }
        }
    }
    public async Task ExecuteQuery(string query)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            cmd.CommandText = query;

            _connection.Open();
            using (SqlDataAdapter da = new(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                _connection.Close();

                //titres de colonnes
                int length = MaxLengthColumn(ds.Tables[0]);
                Console.WriteLine($"╔╦════╦═════╦═══╗"); //boucle
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (length % 2 == 0) length++;
                    Console.Write($"║{new string(' ', length/2)}{dc.ColumnName}{new string(' ', length / 2)}");
                }
                Console.WriteLine();
                Console.WriteLine("╠═══╬════╬═════╬═══╣");

                //contenu
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        if (dr[dc] is DateTime)
                        {
                            //Console.WriteLine("║   ║    ║     ║   ║");
                            DateTime dateValue = (DateTime)dr[dc];
                            Console.Write(dateValue.ToString("yyyy-MM-dd") + " ");
                        }
                        else
                            Console.Write(dr[dc] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("╚═══╩════╩═════╩═══╝");
            }
        }
    }

    private int MaxLengthColumn(DataTable table)
    {
        int maxLength = 0;

        // Parcourir chaque colonne de la table
        foreach (DataColumn column in table.Columns)
        {
            // Parcourir chaque ligne de la table
            foreach (DataRow row in table.Rows)
            {
                // Vérifier que la cellule n'est pas vide et qu'elle contient une chaîne de caractères
                if (row[column] != DBNull.Value && row[column] is string currentValue)
                {
                    // Si la longueur de la valeur actuelle est plus grande, mettre à jour la longueur maximale
                    int currentLength = currentValue.Length;
                    if (currentLength > maxLength)
                    {
                        maxLength = currentLength;
                    }
                }
            }
        }

        return maxLength;
    }

}
