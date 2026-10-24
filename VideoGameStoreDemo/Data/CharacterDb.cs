using AdoDotNetTutorial.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace AdoDotNetTutorial.Data
{
    public class CharacterDb
    {
        public string connectionString = "";

        public CharacterDb()
        {
            var builder = new ConfigurationBuilder().SetBasePath(
                Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            connectionString = builder.GetSection("ConnectionStrings:DefaultConnection").Value!;
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            List<Character> characters = new List<Character>();

            using(SqlConnection connection= new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetCharacters", connection))
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();

                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        characters.Add(
                            new Character()
                            {
                                Id = int.Parse(reader["Id"].ToString() ?? ""),
                                Name = reader["Name"].ToString() ?? "",
                                Game = reader["Game"].ToString() ?? "",
                                Role = reader["Role"].ToString() ?? ""
                            });
                    }

                    return characters;
                }
            }
        }

        public async Task<List<Character>> GetCharacterById(int id)
        {
            List<Character> characters = new List<Character>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetCharacterByID", connection))
                {
                    command.Parameters.Add("@CharacterID", SqlDbType.Int);
                    command.Parameters["@CharacterID"].Value = id;
                    command.CommandType = CommandType.StoredProcedure;
                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();
                    //dataset
                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        characters.Add(
                            new Character()
                            {
                                Id = int.Parse(reader["Id"].ToString() ?? ""),
                                Name = reader["Name"].ToString() ?? "",
                                Game = reader["Game"].ToString() ?? "",
                                Role = reader["Role"].ToString() ?? ""
                            });
                    }

                    return characters;
                }
            }
        }

        
    }
}
