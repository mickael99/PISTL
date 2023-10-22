using MySql.Data.MySqlClient;
using System;

using Microsoft.AspNetCore.Mvc;

[Route("api/auth")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly string connectionString = "server=localhost;user=root;password=password;database=users;";

    [HttpPost]
    public IActionResult GetUsers([FromBody] User user)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                string selectUserSQL = "SELECT * FROM utilisateurs WHERE nom_utilisateur = @nom_utilisateur AND mot_de_passe = @mot_de_passe";
                MySqlCommand selectUserCmd = new MySqlCommand(selectUserSQL, connection);
                
                selectUserCmd.Parameters.AddWithValue("@nom_utilisateur", user.Email);
                selectUserCmd.Parameters.AddWithValue("@mot_de_passe", user.MotDePasse);
                
                using (MySqlDataReader reader = selectUserCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Ok(new { message = "Successful connection" });
                    }
                    else
                    {
                        return Ok(new { message = "Incorrect email or password" });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
