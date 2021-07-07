using System;
using System.Threading;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Users;



namespace Manager
{
	namespace Login
	{
		class LoginSystem
		{
			public bool CheckLogin(string username, string pass, SqlConnection connection)
			{
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand($"SELECT Username, Password FROM Users WHERE Username = '{username}'", connection);
				reader = cmd.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						if (reader.GetString(1) == pass)
						{
							reader.Close();
							return true;

						}
						else
						{
							reader.Close();
							return false;
						}
					}
				}
				catch (SqlException ev)
				{

					Console.Clear();
					Console.Write(ev.Message);
					Thread.Sleep(1000);
					reader.Close();
					return false;
				}
				catch(System.InvalidOperationException ev)
                {
					Console.Clear();
					Console.Write(ev.Message);
					Thread.Sleep(1000);
					reader.Close();
					return false;
                }
				reader.Close();
				return true;
			}
			public User PrintLogin(SqlConnection connection)
			{
				string tempname, temppass;
				User user = new User();
				Console.Write("Welcome! Please log in\nUsername: ");tempname = Console.ReadLine();
				Console.Write("Password: "); temppass = Console.ReadLine();
				if (CheckLogin(tempname,temppass,connection))
				{
					user.LoadUser(connection,tempname);
					return user;
				}
				else
				{
					Console.Clear();
					PrintLogin(connection); return null;
				}
				
			}
			public void MainMenu(User currentUser)
            {
				Console.Clear();
				Console.Write($"Currently logged in user: {currentUser.FullName}\n\n      Main Menu\n");
                if (currentUser.Roles == 1)
                {
					Console.Write("1. User Management View\n2. Teams Management View\n3. Project Management View\n");
                }
            }  
			
		}
	}
	
}
