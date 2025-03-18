using System;
using MySql.Data.MySqlClient;

public class Database
{
    private string connectionString = "server=localhost;database=BookHeaven;user=root;password=;";

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}
