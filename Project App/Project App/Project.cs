using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Manager.Teams;
namespace Manager
{
	namespace Projects
	{
		class Project
		{
			private SqlConnection connection;
			/// <summary>
			/// Loads the project with a connection instance
			/// </summary>
			/// <param name="connection"><see cref="SqlConnection"/> with loaded connectionString</param>
			public Project(SqlConnection connection)
            {
				this.connection = connection;
            }
			/// <summary>
			/// Gets/sets the ID of the project
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the project</returns>
			public int Id { get { return id; } set { id = value; } }
			/// <summary>
			/// Gets/sets the title of the project
			/// </summary>
			/// <returns><see cref="string"/> representing the title of the project</returns>
			public string Title { get { return title; } set { title = value; } }
			/// <summary>
			/// Gets/sets the description of the project
			/// </summary>
			/// <returns><see cref="string"/> representing the description</returns>
			public string Description { get { return description; } set { description = value; } }
			/// <summary>
			/// Gets/sets the date of creation of the project
			/// </summary>
			/// <returns><see cref="DateTime"/> representing the time of creation</returns>
			public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
			/// <summary>
			/// Gets/sets the ID of the project creator
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the creator</returns>
			public int CreatorId { get { return creatorId; } set { creatorId = value; } }
			/// <summary>
			/// Gets/sets the date of the last change to the project
			/// </summary>
			/// <returns><see cref="DateTime"/> representing the time of creation</returns>
			public DateTime LastChangeDate { get { return lastChangeDate; } set { lastChangeDate = value; } }
			/// <summary>
			/// Gets/sets the id of the last editor
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the last editor</returns>
			public int LastChangeUserId { get { return lastChangeUserId; } set { lastChangeUserId = value; } }
			public Team Team { get { return team; } set { team = value; } }
			/// <summary>
			/// Saves a project to a database
			/// </summary>
			public void SaveProject()
            {
				SqlCommand cmd = new SqlCommand("INSERT INTO Projects(Title,Description,CreationDate,CreatorId,LastChangeDate, LastChangeUserId) VALUES(@title,@desc,@creationdate,@creatorid,@lastchangedate,@lastchangeuserid)", connection);
				cmd.Parameters.AddWithValue("@title", title);
				cmd.Parameters.AddWithValue("@desc", description);
				cmd.Parameters.AddWithValue("@creationdate", creationDate);
				cmd.Parameters.AddWithValue("@creatorid", creatorId);
				cmd.Parameters.AddWithValue("@lastchangedate", lastChangeDate);
				cmd.Parameters.AddWithValue("@lastchangeuserid", lastChangeUserId);
				cmd.ExecuteNonQuery();
			}
			/// <summary>
			/// Loads a team into the current instance
			/// </summary>
			/// <param name="id">ID of team you want to load</param>
			public void LoadProject(int id)
			{
				SqlCommand cmd = new SqlCommand("SELECT Id,Title, Description,CreationDate,CreatorId,LastChangeDate,LastChangeUserId FROM Projects WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					this.id = reader.GetInt32(0);
					title = reader.GetString(1);
					description = reader.GetString(2);
					CreationDate = reader.GetDateTime(3);
					creatorId = reader.GetInt32(4);
					lastChangeDate = reader.GetDateTime(5);
					lastChangeUserId = reader.GetInt32(6);
				}
				reader.Close();
			}
			public void AssignTeam(Team team)
            {
				SqlCommand cmd = new SqlCommand("UPDATE Projects SET TeamId = @teamid WHERE Id = @ID", connection);
				cmd.Parameters.AddWithValue("@ID",id);
				cmd.Parameters.AddWithValue("teamid", team.Id);
				cmd.ExecuteNonQuery();
            }


			private int id;
			private string title;
			private string description;
			private DateTime creationDate;
			private int creatorId;
			private DateTime lastChangeDate;
			private int lastChangeUserId;
			private Team team = null;
		}
	}
}
