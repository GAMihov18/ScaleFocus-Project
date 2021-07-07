using System;
using System.IO;
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
				return false;
			}
			public User PrintLogin(SqlConnection connection)
			{
				string tempname, temppass;
				User user = new User();
				do
				{
					Console.Write("Welcome! Please log in\nUsername: "); tempname = Console.ReadLine();
					Console.Write("Password: "); temppass = Console.ReadLine();
					Console.Clear();
				} while (!CheckLogin(tempname, temppass, connection));
				user.LoadUser(connection,tempname);
				return user;
				
			}
			public void MainMenu(User currentUser)
            {
				Console.Clear();
				Console.Write($"Currently logged in user: {currentUser.FullName}\n\n      Main Menu\n");
				Console.Write("1. My Projects\n2. My Tasks\n3. My worklog\n");
				
				if (currentUser.Roles == 1)
                {
					Console.Write("4. User Management View\n5. Teams Management View\n6. Project Management View\n");
                }
				Console.Write("Esc. Exit\n:");
                switch (Console.ReadKey().Key)
                {
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						Console.Write("My projects"); Thread.Sleep(1000);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						Console.Clear();
						Console.Write("My Tasks"); Thread.Sleep(1000);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						Console.Clear();
						Console.Write("My Worklog"); Thread.Sleep(1000);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
                        if (currentUser.Roles == 1)
                        {
							Console.Clear();
							Console.Write("User Management View"); Thread.Sleep(1000);
						}
                        else
                        {
							Console.Clear();
							Console.Write("Please enter a valid option"); Thread.Sleep(2000);
						}
						MainMenu(currentUser);
						break;
					case ConsoleKey.D5:
					case ConsoleKey.NumPad5:
						if (currentUser.Roles == 1)
						{
							Console.Clear();
							Console.Write("Team Management View"); Thread.Sleep(1000);
						}
                        else
                        {
							Console.Clear();
							Console.Write("Please enter a valid option"); Thread.Sleep(2000);
						}
						MainMenu(currentUser);
						break;
					case ConsoleKey.D6:
					case ConsoleKey.NumPad6:
						if (currentUser.Roles == 1)
						{
							Console.Clear();
							Console.Write("Project Management View"); Thread.Sleep(1000);
                        }
                        else
                        {
							Console.Clear();
							Console.Write("Please enter a valid option"); Thread.Sleep(2000);
						}
						MainMenu(currentUser);
						break;
					case ConsoleKey.Escape:
						break;
                    default:
						Console.Clear();
						Console.Write("Please enter a valid option"); Thread.Sleep(1500);
						MainMenu(currentUser);
						break;
                }
            }  
		}
	}
	
}
