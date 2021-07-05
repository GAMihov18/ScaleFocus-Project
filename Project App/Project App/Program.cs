using System;
using System.Data.SqlClient;
using Manager;
using Manager.Users;
using Manager.Teams;
using Manager.Tasks;

namespace Project_App
{
	class Program
	{
		static void Main(string[] args)
		{
			string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ProjectDB;Integrated Security=True";
			SqlConnection connection = new SqlConnection(connectionString);
		}
	}
}
