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
			private SqlConnection connection;

			public LoginSystem(SqlConnection connection)
            {
				this.connection = connection;
            }

			public bool CheckLogin(string username, string pass)
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
			public User PrintLogin()
			{
				string tempname, temppass;
				User user = new User();
				do
				{
					Console.Write("Welcome! Please log in\nUsername: "); tempname = Console.ReadLine();
					Console.Write("Password: "); temppass = Console.ReadLine();
					Console.Clear();
				} while (!CheckLogin(tempname, temppass));
				user.LoadUser(connection,tempname);
				return user;
			}
			public void PrintAllUsers(User currentUser)
            {
				Console.Clear();
				SqlCommand cmd = new SqlCommand($"SELECT Id, Username,FullName,Roles,CreationDate,CreatedBy,LastChangeDate,LastChangedBy FROM vUsers",connection);			
				SqlDataReader reader = cmd.ExecuteReader();
				while(reader.Read())
                {
					string maybe = reader.GetInt32(3)==1 ? "Yes" : "No";
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nUsername: {reader.GetString(1)}\nFull name: {reader.GetString(2)}\nPermissions: {maybe}\nCreated on: {reader.GetDateTime(4)}\nCreated by: {reader.GetString(5)}\nLast edited on: {reader.GetDateTime(6)}\nLast edited by: {reader.GetString(7)}\n");
                }
				reader.Close();
				Console.ReadKey();
				UserManagementView(currentUser);
            }
			public void printOneUser(string id, User currentUser)
            {
				int numid;
				Console.Clear();
				SqlCommand cmd = new SqlCommand("SELECT Id, Username,FullName,Roles,CreationDate,CreatedBy,LastChangeDate,LastChangedBy FROM vUsers WHERE Id = @ID", connection);
                try
                {
					numid = int.Parse(id);
					cmd.Parameters.AddWithValue("@ID", numid);
				}
                catch (FormatException)
                {
					Console.Clear();
					Console.Write("Please, enter a number\nGoing back to User Management View"); Thread.Sleep(2000);
					UserManagementView(currentUser);
                }
				
				SqlDataReader reader;
				reader = cmd.ExecuteReader();
				Console.Clear();
				try
				{
					reader.Read();
					if (reader.FieldCount == -1)
					{
						System.InvalidOperationException exc = new System.InvalidOperationException("Invalid ID");
						throw exc;
					}
					string maybe = reader.GetInt32(3) == 1 ? "Yes" : "No";
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nUsername: {reader.GetString(1)}\nFull name: {reader.GetString(2)}\nPermissions: {maybe}\nCreated on: {reader.GetDateTime(4)}\nCreated by: {reader.GetString(5)}\nLast edited on: {reader.GetDateTime(6)}\nLast edited by: {reader.GetString(7)}\n");
					reader.Close();
					Console.ReadKey();
				}
				catch (System.InvalidOperationException exc)
				{
					Console.Write($"Error: {exc.Message}\nReturning to User Management View"); Console.ReadKey();
				}
				reader.Close();
				UserManagementView(currentUser);
			}
			public void UserManagementView(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: { currentUser.FullName}\n\n     User Management View\n");
				Console.Write("1. List Users\n2. Create new user\n3. Edit existing user\n4. Delete an existing user\n:");
				string temp;
				string tempStr;
				switch (Console.ReadKey().Key)
                {
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						Console.Write("Do you want to print one user?(Y/N) "); tempStr = Console.ReadKey().Key.ToString();
                        switch (tempStr)
                        {
							case "y":
							case "Y":
								Console.Clear();
								Console.Write("Enter ID of user: "); temp = Console.ReadLine();
								printOneUser(temp,currentUser);
								break;
							case "n":
							case "N":
								Console.Clear();
								Console.Write("Printing all users"); Thread.Sleep(1000);
								PrintAllUsers(currentUser);

								break;
                            default:
                                break;
                        }
                        break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						break;
					
					default:
						
                        break;
                }

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
							UserManagementView(currentUser);
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
