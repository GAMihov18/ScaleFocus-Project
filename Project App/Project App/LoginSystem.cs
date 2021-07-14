using System;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Users;
using Manager.Teams;
using Manager.Projects;


namespace Manager
{
	namespace Login
	{
		class LoginSystem
		{
			public SqlConnection connection;
			/// <summary>
			/// Builds the login system with a connection
			/// </summary>
			/// <param name="connection"></param>
			public LoginSystem(SqlConnection connection)
			{
				this.connection = connection;
			}
			/// <summary>
			/// Checks if your credentials for logging in are correct
			/// </summary>
			/// <param name="username">Your username</param>
			/// <param name="pass">Your password</param>
			/// <returns></returns>
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
				catch (System.InvalidOperationException ev)
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
			/// <summary>
			/// Prints the login menu and returns the logged in user
			/// </summary>
			/// <returns><see cref="User"/></returns>
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
				user.LoadUser(tempname, connection);
				return user;
			}
			/// <summary>
			/// Prints all the users
			/// </summary>
			/// <param name="currentUser">The user that currrently is logged in</param>
			public void PrintAllUsers(User currentUser)
			{
				Console.Clear();
				SqlCommand cmd = new SqlCommand($"SELECT Id, Username,FullName,Roles,CreationDate,CreatedBy,LastChangeDate,LastChangedBy FROM vUsers", connection);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string maybe = reader.GetInt32(3) == 1 ? "Yes" : "No";
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nUsername: {reader.GetString(1)}\nFull name: {reader.GetString(2)}\nPermissions: {maybe}\nCreated on: {reader.GetDateTime(4)}\nCreated by: {reader.GetString(5)}\nLast edited on: {reader.GetDateTime(6)}\nLast edited by: {reader.GetString(7)}\n");
				}
				reader.Close();
				Console.ReadKey();
				UserManagementView(currentUser);
			}
			/// <summary>
			/// Prints one user with given ID
			/// </summary>
			/// <param name="id">ID of user you want to print</param>
			/// <param name="currentUser">the currently logged in user</param>
			public void PrintOneUser(string id, User currentUser)
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
					if (reader.FieldCount <= 0)
					{
						System.InvalidOperationException exc = new System.InvalidOperationException("Invalid ID");
						throw exc;
					}
					string maybe = reader.GetInt32(3) == 1 ? "Yes" : "No";
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nUsername: {reader.GetString(1)}\nFull name: {reader.GetString(2)}\nPermissions: {maybe}\nCreated on: {reader.GetDateTime(4)}\nCreated by: {reader.GetString(5)}\nLast edited on: {reader.GetDateTime(6)}\nLast edited by: {reader.GetString(7)}\n");
					reader.Close();
					Console.ReadKey();
				}
				catch (System.InvalidOperationException)
				{
					Console.Write($"No User found with given ID\nReturning to User Management View"); Thread.Sleep(1500);
				}
				reader.Close();
				UserManagementView(currentUser);
			}
			/// <summary>
			/// Calls <see cref="User.SaveUser(SqlConnection)"/>
			/// </summary>
			/// <param name="user">The <see cref="User"/> you want to save</param>
			public void CreateNewUser(User user)
			{
				user.SaveUser(connection);
			}
			/// <summary>
			/// Prepares the user to be saved and calls <see cref="LoginSystem.CreateNewUser(User)"/>
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="username">The username of the new user you want to create</param>
			/// <param name="password">The password of the new user you want to create</param>
			/// <param name="firstName">The first name of the new user you want to create</param>
			/// <param name="lastName">The last name of the new user you want to create</param>
			/// <param name="roles">The roles of the new user you want to create</param>
			public void PrepareUser(User currentUser, string username, string password, string firstName, string lastName, int roles)
			{
				int creatorId, lastChangeUserId;
				DateTime creationDate, editDate;
				creationDate = DateTime.Now;
				creatorId = currentUser.Id;
				editDate = DateTime.Now;
				lastChangeUserId = currentUser.Id;
				User user = new User(username, firstName, lastName, password, roles, creationDate, creatorId, editDate, lastChangeUserId);
				CreateNewUser(user);
			}
			/// <summary>
			/// Edits an existing user
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="id">ID of user you want to edit</param>
			/// <param name="username">The new username given to the user you want to edit</param>
			/// <param name="password">The new password given to the user you want to edit</param>
			/// <param name="firstName">The new first name given to the user you want to edit</param>
			/// <param name="lastName">The new last name given to the user you want to edit</param>
			public void EditUser(User currentUser, int id, string username, string password, string firstName, string lastName)
			{
				DateTime editTime = DateTime.Now;

				SqlCommand cmd = new SqlCommand("UPDATE Users SET Username = @username, Password = @password,FirstName = @firstname,LastName = @lastname, LastChangeDate = @edittime , LastChangeUserId = @userId WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@password", password);
				cmd.Parameters.AddWithValue("@firstname", firstName);
				cmd.Parameters.AddWithValue("@lastname", lastName);
				cmd.Parameters.AddWithValue("edittime", editTime);
				cmd.Parameters.AddWithValue("userId", currentUser.Id);
				cmd.ExecuteNonQuery();
			}
			/// <summary>
			/// Deletes an existing user
			/// </summary>
			/// <param name="id">The ID of the user you want to delete</param>
			/// <param name="currentUser">The currently logged in user</param>
			public void DeleteUser(int id, User currentUser)
			{
				SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				if (cmd.ExecuteNonQuery() <= 0)
				{
					Console.Write("No user found with given ID.\nReturning to User Management View"); Thread.Sleep(1500);
					UserManagementView(currentUser);
				}
				Console.Clear();
			}
			/// <summary>
			/// Prints the user management view
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			public void UserManagementView(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: { currentUser.FullName}\n\n     User Management View\n");
				Console.Write("1. List Users\n2. Create new user\n3. Edit existing user\n4. Delete an existing user\n:");
				string temp;
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						Console.Write("1. Print all users\n2. Print one user");
						switch (Console.ReadKey().Key)
						{
							case ConsoleKey.D1:
								PrintAllUsers(currentUser);
								break;
							case ConsoleKey.D2:
								Console.Write("\nEnter ID: "); temp = Console.ReadLine();
								PrintOneUser(temp, currentUser);
								break;
							default:
								Console.Write("Invalid input.\nGoing back to User Management View"); Thread.Sleep(1500);
								UserManagementView(currentUser);
								break;
						}
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						int roles;

						string username, password, firstName, lastName;
						Console.Clear();
						try
						{
							Console.Write("Username: "); username = Console.ReadLine();
							Console.Write("Password: "); password = Console.ReadLine();
							Console.Write("First name: "); firstName = Console.ReadLine();
							Console.Write("Last name: "); lastName = Console.ReadLine();
							Console.Write("Roles:(1/0) "); roles = int.Parse(Console.ReadLine());
							PrepareUser(currentUser, username, password, firstName, lastName, roles);
						}
						catch (Exception ex)
						{
							Console.Write($"{ex.Message}\n Going back to User Management View."); Thread.Sleep(1500);
							UserManagementView(currentUser);
						}
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						string userName, Password, FirstName, LastName;
						string temps;

						Console.Write("\nEdit an existing user\nEnter ID: "); temp = Console.ReadLine();
						Console.Clear();
						SqlCommand countcmd = new SqlCommand("SELECT count(Username) FROM Users WHERE Id = @ID", connection);
						SqlCommand cmd = new SqlCommand("SELECT Username, Password,FirstName,LastName FROM Users WHERE Id = @ID", connection);
						try
						{
							countcmd.Parameters.AddWithValue("@ID", int.Parse(temp));
							cmd.Parameters.AddWithValue("@ID", int.Parse(temp));
						}
						catch (Exception)
						{
							Console.Write("Please enter a valid ID.\nReturning to User Management View..."); Thread.Sleep(1500);
							UserManagementView(currentUser);
						}
						if ((int)countcmd.ExecuteScalar() == 0)
						{
							Console.Write("No user found with given ID.\nReturning to User Management View"); Thread.Sleep(1500);
							UserManagementView(currentUser);
						}
						SqlDataReader reader = cmd.ExecuteReader();
						reader.Read();
						Console.Write("Leave a field blank if you don't wish to edit it\n"); Console.ReadLine();
						Console.Write("Username: "); temps = Console.ReadLine();
						if (temps == "")
						{
							userName = reader.GetString(0);
						}
						else
						{
							userName = temps;
						}
						Console.Write("Password: "); temps = Console.ReadLine();
						if (temps == "")
						{

							Password = reader.GetString(1);
						}
						else
						{

							Password = temps;
						}
						Console.Write("First name: "); temps = Console.ReadLine();
						if (temps == "")
						{

							FirstName = reader.GetString(2);
						}
						else
						{

							FirstName = temps;
						}
						Console.Write("Last name: "); temps = Console.ReadLine();
						if (temps == "")
						{
							LastName = reader.GetString(3);
						}
						else
						{
							LastName = temps;
						}
						reader.Close();
						EditUser(currentUser, int.Parse(temp), userName, Password, FirstName, LastName);
						UserManagementView(currentUser);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						Console.Clear();
						Console.Write("Delete a user\nEnter ID: "); temps = Console.ReadLine();
						try
						{
							DeleteUser(int.Parse(temps), currentUser);
						}
						catch (Exception)
						{
							Console.Write("Please enter a valid ID.\nReturning to User Management View..."); Thread.Sleep(1500);
							UserManagementView(currentUser);
						}


						UserManagementView(currentUser);
						break;

					default:

						break;
				}

			}

			/// <summary>
			/// Prints all teams
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			public void PrintAllTeams(User currentUser)
			{
				SqlCommand cmd = new SqlCommand("SELECT Id,Title,CreationDate,CreatedBy,LastChangeDate,LastEditedBy FROM vTeams", connection);
				SqlDataReader reader = cmd.ExecuteReader();
				Console.Clear();
				while (reader.Read())
				{
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nTitle: {reader.GetString(1)}\nCreated on: {reader.GetDateTime(2)}\nCreated by: {reader.GetString(3)}\nLast edited on: {reader.GetDateTime(4)}\nLast edited by: {reader.GetString(5)}\n");
				}
				reader.Close();
				Console.ReadKey();
			}
			/// <summary>
			/// Prints one team by given ID
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="id">The ID of the team you want to print</param>
			public void PrintOneTeam(User currentUser, int id)
			{
				Console.Clear();
				SqlCommand cmd = new SqlCommand("SELECT Id,Title,CreationDate,CreatedBy,LastChangeDate,LastEditedBy FROM vTeams WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader;
				reader = cmd.ExecuteReader();
				Console.Clear();
				try
				{
					reader.Read();
					if (reader.FieldCount <= 0)
					{
						System.InvalidOperationException exc = new System.InvalidOperationException("Invalid ID");
						throw exc;
					}

					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nTitle: {reader.GetString(1)}\nCreated on: {reader.GetDateTime(2)}\nCreated by: {reader.GetString(3)}\nLast edited on: {reader.GetDateTime(4)}\nLast edited by: {reader.GetString(5)}\n");
					reader.Close();
					Console.ReadKey();
				}
				catch (System.InvalidOperationException)
				{
					Console.Write($"No team found with given ID\nReturning to Team Management View"); Thread.Sleep(1500);
				}
				Console.Clear();
				reader.Close();
				TeamManagementView(currentUser);
			}
			/// <summary>
			/// Calls <see cref="Team.SaveTeam"/>
			/// </summary>
			/// <param name="team">The team to be saved</param>
			public void CreateNewTeam(Team team)
			{
				team.SaveTeam();
			}
			/// <summary>
			/// Prepares the newly created team for saving and calls <see cref="CreateNewTeam(Team)"/>
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="title">The given title of the team</param>
			public void prepareNewTeam(User currentUser,string title)
			{
				Team team = new Team(title, connection);
				team.CreationDate = DateTime.Now;
				team.LastChangeDate = DateTime.Now;
				team.CreatorId = currentUser.Id;
				team.LastChangeUserId = currentUser.Id;
				CreateNewTeam(team);
			}
			/// <summary>
			/// Edits an already existing team
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="id">The ID of the team you want to edit</param>
			/// <param name="title"></param>
			public void EditTeam(User currentUser,int id,string title)
			{
				DateTime editTime = DateTime.Now;

				SqlCommand cmd = new SqlCommand("UPDATE Teams SET Title = @title, LastChangeDate = @edittime , LastChangeUserId = @userId WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				cmd.Parameters.AddWithValue("@title", title);
				cmd.Parameters.AddWithValue("@edittime", editTime);
				cmd.Parameters.AddWithValue("@userId", currentUser.Id);
				cmd.ExecuteNonQuery();
			}
			/// <summary>
			/// Deletes an existing team
			/// </summary>
			/// <param name="id">The ID of the team you want to delete</param>
			/// <param name="currentUser">The currently logged in user</param>
			public void DeleteTeam(int id,User currentUser)
			{
				SqlCommand cmd = new SqlCommand("DELETE FROM Teams WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				if (cmd.ExecuteNonQuery() <= 0)
				{
					Console.Write("No team found with given ID.\nReturning to Team Management View"); Thread.Sleep(1500);
					TeamManagementView(currentUser);
				}
			}
			/// <summary>
			/// Prints all options for team management available
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			public void TeamManagementView(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: { currentUser.FullName}\n\n     Team Management View\n");
				Console.Write("1. List Teams\n2. Create new team\n3. Edit existing team\n4. Delete an existing team\n:");
				string temp,temps;
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						Console.Write("1. List all teams\n2. List one team");
						switch (Console.ReadKey().Key)
						{
							case ConsoleKey.D1:
							case ConsoleKey.NumPad1:
								Console.Clear();
								PrintAllTeams(currentUser);
								break;
							case ConsoleKey.D2:
							case ConsoleKey.NumPad2:
								Console.Clear();
								Console.Write("Enter ID: "); temp = Console.ReadLine();
								try
								{
									PrintOneTeam(currentUser,int.Parse(temp));

								}
								catch (FormatException)
								{
									Console.Clear();
									Console.Write("Please, enter a valid ID.\nReturning to Team Management View");
									TeamManagementView(currentUser);
								}
								
								break;

							default:
								Console.Clear();
								Console.Write("Incorrect input.\nReturning to Team Management View");
								TeamManagementView(currentUser);
								break;
						}
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						Console.Clear();
						Console.Write("Creating new team\nEnter team name: ");temp = Console.ReadLine();
						prepareNewTeam(currentUser, temp);
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						SqlCommand countcmd = new SqlCommand("SELECT count(Title) FROM Teams WHERE Id = @ID", connection);
						Console.Write("\nEdit an existing user\nEnter ID: "); temp = Console.ReadLine();
						Console.Clear();
						
						try
						{
							countcmd.Parameters.AddWithValue("@ID", int.Parse(temp));
						}
						catch (Exception)
						{
							Console.Write("Please enter a valid ID.\nReturning to Team Management View..."); Thread.Sleep(1500);
							TeamManagementView(currentUser);
						}

						if ((int)countcmd.ExecuteScalar() == 0)
						{
							Console.Write("No user found with given ID.\nReturning to Team Management View"); Thread.Sleep(1500);
							TeamManagementView(currentUser);
						}
						Console.Write("Title: "); temps = Console.ReadLine();
						EditTeam(currentUser,int.Parse(temp),temps);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						Console.Clear();
						Console.Write("Deleting a team\nEnter ID: ");temp = Console.ReadLine();
						try
						{
							DeleteTeam(int.Parse(temp),currentUser);
						}
						catch (Exception)
						{
							Console.Write("Please enter a valid ID\nReturning to Team Management View");
						}
						Console.Clear();
						TeamManagementView(currentUser);
						break;
					default:
						break;
				}
			}
			/// <summary>
			/// Prints all projects available
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			public void PrintAllProjects(User currentUser)
			{
				SqlCommand cmd = new SqlCommand("SELECT Id,Title,Description,CreationDate,CreatedBy,LastChangeDate,LastEditedBy, TeamName FROM vProjects", connection);
				SqlDataReader reader = cmd.ExecuteReader();
				Console.Clear();
				while (reader.Read())
				{
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nTitle: {reader.GetString(1)}\nDescription: {reader.GetString(2)}\nCreated on: {reader.GetDateTime(3)}\nCreated by: {reader.GetString(4)}\nLast edited on: {reader.GetDateTime(5)}\nLast edited by: {reader.GetString(6)}\n");
				}
				reader.Close();
				Console.ReadKey();
			}
			/// <summary>
			/// Prints a project with the given ID
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="id">The ID of the project you want to print</param>
			public void PrintOneProject(User currentUser, int id)
			{
				Console.Clear();
				SqlCommand cmd = new SqlCommand("SELECT Id,Title,Description,CreationDate,CreatedBy,LastChangeDate,LastEditedBy, TeamName FROM vProjects WHERE Id = @ID", connection);  
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader;
				reader = cmd.ExecuteReader();
				Console.Clear();
				try
				{
					reader.Read();
					if (reader.FieldCount <= 0)
					{
						System.InvalidOperationException exc = new System.InvalidOperationException("Invalid ID");
						throw exc;
					}

					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nTitle: {reader.GetString(1)}\nDescription: {reader.GetString(2)}\nCreated on: {reader.GetDateTime(3)}\nCreated by: {reader.GetString(4)}\nLast edited on: {reader.GetDateTime(5)}\nLast edited by: {reader.GetString(6)}\n");
					reader.Close();
					Console.ReadKey();
				}
				catch (System.InvalidOperationException)
				{
					Console.Write($"No project found with given ID\nReturning to Project Management View"); Thread.Sleep(1500);
				}
				Console.Clear();
				reader.Close();
				ProjectManagementView(currentUser);
			}
			/// <summary>
			/// Saves the given project by calling <see cref="User.SaveUser(SqlConnection)"/>
			/// </summary>
			/// <param name="project">The project to be saved</param>
			public void CreateNewProject(Project project)
			{
				project.SaveProject();
			}
			/// <summary>
			/// Prepares a new project for saving and calls <see cref="CreateNewProject(Project)"/>
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="project">The project to be prepared</param>
			public void PrepareNewProject(User currentUser, Project project)
			{
				project.CreationDate = DateTime.Now;
				project.LastChangeDate = DateTime.Now;
				project.CreatorId = currentUser.Id;
				project.LastChangeUserId = currentUser.Id;
				CreateNewProject(project);
			}
			/// <summary>
			/// Edits an existing project
			/// </summary>
			/// <param name="currentUser">The currently logged in user</param>
			/// <param name="id">The ID of the project you want to edit</param>
			/// <param name="project">The edited project</param>
			public void EditProject(User currentUser, int id, Project project)
			{
				DateTime editTime = DateTime.Now;

				SqlCommand cmd = new SqlCommand("UPDATE Projects SET Title = @title, Description = @desc, LastChangeDate = @edittime, LastChangeUserId = @userId WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				cmd.Parameters.AddWithValue("@title", project.Title);
				cmd.Parameters.AddWithValue("@desc", project.Description);
				cmd.Parameters.AddWithValue("@edittime", editTime);
				cmd.Parameters.AddWithValue("@userId", currentUser.Id);
				cmd.ExecuteNonQuery();
			}
			/// <summary>
			/// Deletes an existing project
			/// </summary>
			/// <param name="id">The ID of the project you want to delete</param>
			/// <param name="currentUser">The currently logged in user</param>
			public void DeleteProject(int id, User currentUser)
			{
				SqlCommand cmd = new SqlCommand("DELETE FROM Projects WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				if (cmd.ExecuteNonQuery() <= 0)
				{
					Console.Write("No project found with given ID.\nReturning to Project Management View"); Thread.Sleep(1500);
					TeamManagementView(currentUser);
				}
			}
			public void ProjectManagementView(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: { currentUser.FullName}\n\n     Project Management View\n");
				Console.Write("1. List Projects\n2. Create new project\n3. Edit existing project\n4. Delete an existing project\n5. Assign team to projects owned by me\n:");
				string temp;
				Project project;
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						Console.Write("1. List all projects\n2. List one project");
						switch (Console.ReadKey().Key)
						{
							case ConsoleKey.D1:
							case ConsoleKey.NumPad1:
								Console.Clear();
								PrintAllProjects(currentUser);
								break;
							case ConsoleKey.D2:
							case ConsoleKey.NumPad2:
								Console.Clear();
								Console.Write("Enter ID: "); temp = Console.ReadLine();
								try
								{
									PrintOneProject(currentUser, int.Parse(temp));

								}
								catch (FormatException)
								{
									Console.Clear();
									Console.Write("Please, enter a valid ID.\nReturning to Project Management View");
									ProjectManagementView(currentUser);
								}

								break;

							default:
								Console.Clear();
								Console.Write("Incorrect input.\nReturning to Project Management View");
								ProjectManagementView(currentUser);
								break;
						}
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						Console.Clear();
						project = new Project(connection);
						Console.Write("Creating new project\nEnter project name: "); project.Title = Console.ReadLine();
						Console.Write("Enter project description: "); project.Description = Console.ReadLine();
						PrepareNewProject(currentUser, project);
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						SqlCommand countcmd = new SqlCommand("SELECT count(Title) FROM Projects WHERE Id = @ID", connection);
						Console.Write("\nEdit an existing project\nEnter ID: "); temp = Console.ReadLine();
						Console.Clear();

						try
						{
							countcmd.Parameters.AddWithValue("@ID", int.Parse(temp));
						}
						catch (Exception)
						{
							Console.Write("Please enter a valid ID.\nReturning to Project Management View..."); Thread.Sleep(1500);
							ProjectManagementView(currentUser);
						}

						if ((int)countcmd.ExecuteScalar() == 0)
						{
							Console.Write("No project found with given ID.\nReturning to Project Management View"); Thread.Sleep(1500);
							ProjectManagementView(currentUser);
						}
						project = new Project(connection);
						SqlCommand cmd = new SqlCommand("SELECT Title,Description FROM Projects WHERE Id = @ID", connection);
						cmd.Parameters.AddWithValue("@ID", int.Parse(temp));
						SqlDataReader reader = cmd.ExecuteReader();
						reader.Read();
						Console.Write("Leave a field blank if you don't wish to edit it\n"); Console.ReadLine();
						Console.Write("Title: "); project.Title = Console.ReadLine();
						if (project.Title == "")
						{
							project.Title = reader.GetString(0);

						}
						Console.Write("Description: "); project.Description = Console.ReadLine();
						if (project.Description == "")
						{
							project.Description = reader.GetString(1);
						}
						reader.Close();
						EditProject(currentUser, int.Parse(temp), project);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						Console.Clear();
						Console.Write("Deleting a Project\nEnter ID: "); temp = Console.ReadLine();
						try
						{
							DeleteProject(int.Parse(temp), currentUser);
						}
						catch (Exception)
						{
							Console.Write("Please enter a valid ID\nReturning to Project Management View");
						}
						Console.Clear();
						ProjectManagementView(currentUser);
						break;
					case ConsoleKey.D5:
					case ConsoleKey.NumPad5:
						Console.Clear();
						Console.Write("Assign team to project I own\n");
						Console.Write("ID of project: "); temp = Console.ReadLine();

						project = new Project(connection);
						project.LoadProject(int.Parse(temp));
						try
						{
							if (project.CreatorId == currentUser.Id)
							{
								Console.Write("ID of team: "); temp = Console.ReadLine();
								Team team = new Team(connection);
								team.LoadTeam(int.Parse(temp));
								project.AssignTeam(team);
							}
							else
							{
								throw new InvalidOperationException("You don't own this project");
							}
						}
						catch (InvalidOperationException exc)
						{
							Console.Clear();
							Console.Write(exc.Message);
						}
						catch (FormatException)
						{
							Console.Clear();
							Console.Write("Enter valid ID");
						}
						finally
						{
							ProjectManagementView(currentUser);
						}
						break;
					default:
						break;
				}
			}

			public void PrintUserTeams(User currentUser, int id)
			{
				Console.Clear();
				SqlCommand cmd = new SqlCommand("SELECT * FROM vTeams WHERE CreatorId = @ID",connection);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nTitle: {reader.GetString(1)}\nCreated on: {reader.GetDateTime(2)}\nCreated by: {reader.GetString(4)}\nLast edited on: {reader.GetDateTime(5)}\nLast edited by: {reader.GetString(7)}\n");
				}
				reader.Close();
			}

			

			public void MyTeams(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: {currentUser.FullName}\n\n      My teams\n");
				Console.Write("1. List my teams\n2. Add people to my teams\n3. Edit my teams\n4.Delete my teams");
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						PrintUserTeams(currentUser, currentUser.Id);
						Console.ReadKey();
						MyTeams(currentUser);
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						Console.Clear();
						Console.Write("Add people to my teams"); Thread.Sleep(1000);
						MyTeams(currentUser);
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						Console.Clear();
						Console.Write("Edit my teams"); Thread.Sleep(1000);
						MyTeams(currentUser);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						Console.Clear();
						Console.Write("Delete my teams"); Thread.Sleep(1000);
						MyTeams(currentUser);
						break;
					default:
						MainMenu(currentUser);
						break;
				}
			}

			public void PrintUserProjects(User currentUser, int id)
			{
				Console.Clear();
				SqlCommand cmd = new SqlCommand("SELECT * FROM vProjects WHERE CreatorId = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					Console.Write($"--------------------\nID: {reader.GetInt32(0)}\nTitle: {reader.GetString(1)}\nDescription: {reader.GetString(2)}\nCreated on: {reader.GetDateTime(3)}\nCreated by: {reader.GetString(5)}\nLast edited on: {reader.GetDateTime(6)}\nLast edited by: {reader.GetString(8)}\n");
				}
				reader.Close();
			}

			public void MyProjects(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: {currentUser.FullName}\n\n      My Projects\n");
				Console.Write("1. List my projects\n2. Add people to my projects\n3. Edit my teams\n4.Delete my projects");
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						Console.Clear();
						PrintUserProjects(currentUser, currentUser.Id);
						Console.ReadKey();
						MyProjects(currentUser);
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						Console.Clear();
						Console.Write("Add people to my projects"); Thread.Sleep(1000);
						MyProjects(currentUser);
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						Console.Clear();
						Console.Write("Edit my projects"); Thread.Sleep(1000);
						MyProjects(currentUser);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						Console.Clear(); 
						Console.Write("Delete my projects"); Thread.Sleep(1000);
						MyProjects(currentUser);
						break;
					default:
						MainMenu(currentUser);
						break;
				}
			}

			/// <summary>
			/// Prints the main menu and provides with input
			/// </summary>
			/// <param name="currentUser">Represents the currently logged in user</param>
			public void MainMenu(User currentUser)
			{
				Console.Clear();
				Console.Write($"Currently logged in user: {currentUser.FullName}\n\n      Main Menu\n");
				Console.Write("1. My Teams\n2. My Projects\n3. My Tasks\n4. My worklog\n");
				
				if (currentUser.Roles == 1)
				{
					Console.Write("5. User Management View\n6. Teams Management View\n7. Project Management View\n");
				}
				Console.Write("Esc. Exit\n:");
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						MyTeams(currentUser);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						Console.Clear();
						MyProjects(currentUser);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						Console.Clear();
						Console.Write("My Tasks"); Thread.Sleep(1000);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						Console.Clear();
						Console.Write("My Worklog"); Thread.Sleep(1000);
						MainMenu(currentUser);
						break;
					case ConsoleKey.D5:
					case ConsoleKey.NumPad5:
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
					case ConsoleKey.D6:
					case ConsoleKey.NumPad6:
						if (currentUser.Roles == 1)
						{
							Console.Clear();
							TeamManagementView(currentUser);
						}
						else
						{
							Console.Clear();
							Console.Write("Please enter a valid option"); Thread.Sleep(2000);
						}
						MainMenu(currentUser);
						break;
					case ConsoleKey.D7:
					case ConsoleKey.NumPad7:
						if (currentUser.Roles == 1)
						{
							Console.Clear();
							ProjectManagementView(currentUser);
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
