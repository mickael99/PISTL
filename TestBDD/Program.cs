using MySql.Data.MySqlClient;
using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

class Program
{
    static void Main(string[] args)
    {
        

        string connectionString = "server=localhost;user=root;password=password;database=users;";
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Connexion a la base de donnees reussie.");

            string createTableSQL = "CREATE TABLE IF NOT EXISTS utilisateurs (id INT AUTO_INCREMENT PRIMARY KEY, nom_utilisateur VARCHAR(255), mot_de_passe VARCHAR(255))";
            MySqlCommand createTableCmd = new MySqlCommand(createTableSQL, connection);
            createTableCmd.ExecuteNonQuery();

            string insertUserSQL = "INSERT INTO utilisateurs (nom_utilisateur, mot_de_passe) VALUES ('test@test.com', 'motdepasse')";
            MySqlCommand insertUserCmd = new MySqlCommand(insertUserSQL, connection);
            int rowsAffected = insertUserCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("L'utilisateur a été inséré avec succès.");
            }
            else
            {
                Console.WriteLine("L'insertion de l'utilisateur a échoué.");
            }

            string selectUsersSQL = "SELECT * FROM utilisateurs";
            MySqlCommand selectUsersCmd = new MySqlCommand(selectUsersSQL, connection);

            using (MySqlDataReader reader = selectUsersCmd.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    string nom_utilisateur = reader.GetString("nom_utilisateur");
                    string mot_de_passe = reader.GetString("mot_de_passe");

                    Console.WriteLine($"nom_utilisateur : {nom_utilisateur}, mot_de_passe : {mot_de_passe}");
                }
            }

            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur : " + ex.Message);
        }

        CreateHostBuilder(args).Build().Run();
    }

   public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseUrls("http://localhost:5050"); 
            webBuilder.UseStartup<Startup>(); 
        });
}
