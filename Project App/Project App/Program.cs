using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Encryption;
using Manager;
using Manager.Users;
using Manager.Teams;
using Manager.Tasks;
using Manager.Projects;
using Manager.Login;
using DatabaseActions;

namespace Project_App
{
	class Program
	{
		static void Main(string[] args)
		{
			User admin = new User(true);
			string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ProjectDB;Integrated Security=True";
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			LoginSystem printer = new LoginSystem(connection);
			printer.MainMenu(printer.PrintLogin());
			connection.Close();
		}
	}
}