using System;
using Npgsql;
using System.Text;

namespace Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost; Port=5432; User Id=postgres; Password=123; Database=FlashCard;";


            InsertData(connectionString, 50);

            PrintAllUsers(connectionString);
            Console.WriteLine("\n------------------------------------------\n");
            PrintAllDecks(connectionString);
            Console.WriteLine("\n------------------------------------------\n");
            PrintAllCards(connectionString);

            Console.ReadLine();
        }
        public static void PrintAllUsers(string connectionString){
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM users";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string email = reader.GetString(1);
                        string password = reader.GetString(2);
                        User user = new User { Id = id, Email = email, Password = password };

                        Console.WriteLine(user);
                    }
                }
            }
        }
        public static void PrintAllDecks(string connectionString){
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM decks";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string deck_name = reader.GetString(1);
                        int user_id = reader.GetInt32(2);
                        Deck deck = new Deck { Id = id, DeckName = deck_name, UserId = user_id };

                        Console.WriteLine(deck);
                    }
                }
            }
        }
        public static void PrintAllCards(string connectionString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM cards";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string front = reader.GetString(1);
                        string back = reader.GetString(2);
                        int status = reader.GetInt32(3);
                        int deck_id = reader.GetInt32(4);
                        Card card = new Card { Id = id, Front = front, Back = back, Status = status, DeckId = deck_id };

                        Console.WriteLine(card);
                    }
                }
            }
        }

        public static void Insert(string connectionString, string insertDataQuery)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("USER", "localhost");
                    command.Parameters.AddWithValue("PW", "123");
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void InsertData(string connectionString, int count)
        {
            var insertDataQuery = new StringBuilder("INSERT INTO users (email, password) VALUES");
            for (int i = 0; i < count; i++){
                insertDataQuery.Append($"('email{i + 1}@gmail.com', 'password')");
                insertDataQuery.Append(i + 1 != count ? ", " : ";");
            }

            Insert(connectionString, insertDataQuery.ToString());

            insertDataQuery = new StringBuilder("INSERT INTO decks (deckname, userid) VALUES ");
            for (int i = 0; i < count; i++){
                insertDataQuery.Append($"('Deck {i + 1}', 1)");
                insertDataQuery.Append(i + 1 != count ? ", " : ";");
            }

            Insert(connectionString, insertDataQuery.ToString());

            insertDataQuery = new StringBuilder("INSERT INTO cards (front, back, status, deckid) VALUES ");
            for (int i = 0; i < count; i++){
                insertDataQuery.Append($"('front {i+1}', 'front {i + 1}', 0, 1)");
                insertDataQuery.Append(i + 1 != count ? ", " : ";");
            }
            Insert(connectionString, insertDataQuery.ToString());
        }
    }
    public class User{
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public override string ToString(){
            return  $"User: Id: {Id}\t " +
                    $"Email: {Email}\t" +
                    $"Password: {Password}\t";
        }
    }
    public class Deck{
        public int Id { get; set; }
        public string DeckName { get; set; }
        public int UserId { get; set; }
        public override string ToString(){
            return  $"Deck: Id: {Id}\t  " +
                    $"Deck name: {DeckName}\t" +
                    $"User Id: {UserId}\t";
        }
    }
    public class Card{
        public int Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public int Status { get; set; }
        public int DeckId { get; set; }
        public override string ToString(){
            return $"Card: Id: {Id}\t  " +
                   $"Front: {Front}\t" +
                   $"Back: {Back}\t" +
                   $"Status: {Status}\t" +
                   $"Deck Id: {DeckId}\t";
        }
    }
}
