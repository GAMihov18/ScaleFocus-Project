using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DatabaseActions;
using Manager.Projects;
using Manager.Tasks;
using Manager.Teams;
namespace Manager
{
	namespace Users
	{
		class UserException
        {
			
        }
		class User
		{
			/// <summary>
			/// Creates a sample user
			/// </summary>
			public User()
			{
				username = "sampleUser";
				firstName = "Bosh";
				lastName = "Joshian";
				password = "password";
				roles = 0;
				creationDate = DateTime.Now;
				creatorId = 0;
				timeOfEdit = DateTime.Now;
				editorId = 0;
			}
			public User(bool isAdmin)
			{
				if (isAdmin)
				{
					username = "admin";
					firstName = "Example";
					lastName = "User";
					password = "adminpass";
					roles = 1;
					creationDate = DateTime.Now;
					creatorId = 0;
					timeOfEdit = DateTime.Now;
					editorId = 0;
				}
			}
			/// <summary>
			/// Creates a user with given values
			/// </summary>
			/// <param name="username">Username of new user</param>
			/// <param name="firstName">First name of new user</param>
			/// <param name="lastName">Last name of new user</param>
			/// <param name="password">Password of new user</param>
			/// <param name="roles">Roles of new user</param>
			/// <param name="creationDate">Date of creation for new user</param>
			/// <param name="creatorId">ID of creator of new user</param>
			/// <param name="lastChangeDate">Date of last change for new user</param>
			/// <param name="lastChangeUserId">ID of the user of the last edit</param>
			public User(string username, string firstName, string lastName, string password, int roles, DateTime creationDate, int creatorId, DateTime lastChangeDate, int lastChangeUserId)
			{
				this.username = username;
				this.firstName = firstName;
				this.lastName = lastName;
				this.password = password;
				this.roles = roles;
				this.creationDate = creationDate;
				this.creatorId = creatorId;
				timeOfEdit = lastChangeDate;
				editorId = lastChangeUserId;
			}


			//Getters/Setters

			/// <summary>
			/// Gets/sets the ID of the the user
			/// </summary>
			/// <returns><see cref="int"/> representing the user's ID number</returns>
			public int Id { get { return id; } set { id = value; } }
			/// <summary>
			/// Gets/sets the username of the user
			/// </summary>
			/// <returns><see cref="string"/> containing the username</returns>
			public string Username { get { return username; } set { username = value; } }
			/// <summary>
			/// Gets/sets the password of the user
			/// </summary>
			/// <returns><see cref="string"/> containing the password</returns>
			public string Password { get { return password; } set { password = value; } }
			/// <summary>
			/// Gets/sets the first name of the user
			/// </summary>
			/// <returns><see cref="string"/> containing the first name of the user</returns>
			public string FirstName { get { return firstName; } set { firstName = value; } }
			/// <summary>
			/// Gets/sets the last name of the user
			/// </summary>
			/// <returns><see cref="string"/> containing the last name of the user</returns>
			public string LastName { get { return lastName; } set { lastName = value; } }
			/// <summary>
			/// Returns the full name of the user
			/// </summary>
			/// <returns><see cref="FirstName"/> and <see cref="LastName"/> concatenated</returns>
			public string FullName { get { return $"{firstName} {lastName}"; } }
			/// <summary>
			/// Gets/sets the roles of a user
			/// </summary>
			/// <returns><see cref="int"/> representing the user's roles</returns>
			public int Roles { get { return roles; } set { roles = value; } }
			/// <summary>
			/// Gets/sets the date and time of when a user was created
			/// </summary>
			/// <returns><see cref="DateTime"/> representing when the user was created</returns>
			public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
			/// <summary>
			/// Gets/sets the ID of the person that created the current user
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the creator of this user</returns>
			public int CreatorId { get { return creatorId; } set { creatorId = value; } }
			/// <summary>
			/// Gets/sets the time of last edit
			/// </summary>
			/// <returns><see cref="DateTime"/> representing the time of the last edit of the user</returns>
			public DateTime TimeOfEdit { get { return timeOfEdit; } set { timeOfEdit = value; } }
			/// <summary>
			/// Gets/sets the ID of the last editor
			/// </summary>
			/// <returns><see cref="int"/> representing the id of the user that last edited this user</returns>
			public int EditorId { get { return editorId; } set { editorId = value; } }

			/// <summary>
			/// Pushes a user into the database of the given <see cref="SqlConnection"/>
			/// </summary>
			/// <param name="connection"><see cref="SqlConnection"/> with inserted connectionString</param>
			public void SaveUser(SqlConnection connection)
			{
				SqlCommand cmd = new SqlCommand($"INSERT INTO Users(Username,Password,FirstName,LastName,Roles,CreatorId,LastChangeUserId) VALUES (@username,@password,@firstName,@lastName,@roles,@creatorId,@editorId)", connection);
				cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
				cmd.Parameters["@username"].Value = username;
				cmd.Parameters.Add("@password", System.Data.SqlDbType.NVarChar);
				cmd.Parameters["@password"].Value = password;
				cmd.Parameters.Add("@firstName", System.Data.SqlDbType.NVarChar);
				cmd.Parameters["@firstName"].Value = firstName;
				cmd.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar);
				cmd.Parameters["@lastName"].Value = lastName;
				cmd.Parameters.Add("@roles", System.Data.SqlDbType.Int);
				cmd.Parameters["@roles"].Value = roles;
				cmd.Parameters.Add("@creatorId", System.Data.SqlDbType.Int);
				cmd.Parameters["@creatorId"].Value = creatorId;
				cmd.Parameters.Add("@editorId", System.Data.SqlDbType.Int);
				cmd.Parameters["@editorId"].Value = editorId;

				cmd.ExecuteNonQuery();
			}
			/// <summary>
			/// Loads a user with the given username 
			/// </summary>
			/// <param name="username">The username of the user you want to load</param>
			/// <param name="connection">The <see cref="SqlConnection">SqlConnection to a database</see></param>
			public void LoadUser(string username, SqlConnection connection)
			{
				SqlCommand cmd = new SqlCommand($"SELECT Id,Username,Password,FirstName,LastName,Roles,CreationDate,CreatorId,LastChangeDate,LastChangeUserId FROM Users WHERE Username  = @username", connection);
				cmd.Parameters.AddWithValue("@username", username);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					id = reader.GetInt32(0);
					this.username = reader.GetString(1);
					password = reader.GetString(2);
					firstName = reader.GetString(3);
					lastName = reader.GetString(4);
					roles = reader.GetInt32(5);
					creationDate = reader.GetDateTime(6);
					creatorId = reader.GetInt32(7);
					timeOfEdit = reader.GetDateTime(8);
					editorId = reader.GetInt32(9);
				}
				reader.Close();
			}
			
			//All fields in the user class
			private int id;
			private string username;
			private string firstName;
			private string lastName;
			private string password;
			private int roles;
			private DateTime creationDate = new DateTime();
			private int creatorId;
			private DateTime timeOfEdit = new DateTime();
			private int editorId;
			List<Teams.Team> teams;
			List<Projects.Project> projects;
			List<Manager.Tasks.Task> tasks;
		}
	}
}
