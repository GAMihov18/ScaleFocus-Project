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
			public User()
			{
				username = "sampleUser";
				firstName = "Bosh";
				lastName = "Joshian";
				password = "password";
				roles = 1;
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
			public User(int id, string username, string firstName, string lastName, string password, int roles, DateTime creationDate, int creatorId, DateTime lastChangeDate, int lastChangeUserId)
			{
				this.id = id;
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

			public int Id { get { return id; } set { id = value; } }
			public string Username { get { return username; } set { username = value; } }
			public string Password { get { return password; } set { password = value; } }
			public string FirstName { get { return firstName; } set { firstName = value; } }
			public string LastName { get { return lastName; } set { lastName = value; } }
			public string FullName { get { return $"{firstName} {lastName}"; } }
			public int Roles { get { return roles; } set { roles = value; } }
			public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
			public int CreatorId { get { return creatorId; } set { creatorId = value; } }
			public DateTime TimeOfEdit { get { return timeOfEdit; } set { timeOfEdit = value; } }
			public int EditorId { get { return editorId; } set { editorId = value; } }


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
			public void LoadUser(SqlConnection connection, string username)
			{
				SqlCommand cmd = new SqlCommand($"SELECT Id,Username,Password,FirstName,LastName,Roles,CreationDate,CreatorId,LastChangeDate,LastChangeUserId FROM Users WHERE Username  = '{username}'", connection);
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
			List<Team> teams;
			List<Project> projects;
			List<Manager.Tasks.Task> tasks;
		}
	}
}
