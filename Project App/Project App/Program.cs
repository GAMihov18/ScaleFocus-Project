using System;
using System.Data.SqlClient;
using System.Collections.Generic;

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
            List<User> users;
			User admin = new User(true);
			string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ProjectDB;Integrated Security=True";
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			LoginSystem printer = new LoginSystem();
			printer.MainMenu(printer.PrintLogin(connection));
			connection.Close();
		}
	}
}
