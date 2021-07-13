using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Manager
{
	namespace Teams
	{
		class Team
		{

			SqlConnection connection;
			public Team(SqlConnection connection)
			{
				this.connection = connection;
			}

			public Team(string title, SqlConnection connection)
            {
				this.title = title;
				this.connection = connection;
            }

			/// <summary>
			/// Gets/sets the ID of the team
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the team</returns>
			public int Id { get { return id; } set { id = value; } }
			/// <summary>
			/// Gets/sets the title of the project
			/// </summary>
			/// <returns><see cref="string"/> representing the title of the team</returns>
			public string Title { get { return title; } set { title = value; } }
			/// <summary>
			/// Gets/sets the date of creation of the team
			/// </summary>
			/// <returns><see cref="DateTime"/> representing the time of creation</returns>
			public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
			/// <summary>
			/// Gets/sets the ID of the team creator
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the creator</returns>
			public int CreatorId { get { return creatorId; } set { creatorId = value; } }
			/// <summary>
			/// Gets/sets the date of the last change to the team
			/// </summary>
			/// <returns><see cref="DateTime"/> representing the time of creation</returns>
			public DateTime LastChangeDate { get { return lastChangeDate; } set { lastChangeDate = value; } }
			/// <summary>
			/// Gets/sets the id of the last editor
			/// </summary>
			/// <returns><see cref="int"/> representing the ID of the last editor</returns>
			public int LastChangeUserId { get { return lastChangeUserId; } set { lastChangeUserId = value; } }
			/// <summary>
			/// Saves the team to the database
			/// </summary>
			public void SaveTeam()
			{
				SqlCommand cmd = new SqlCommand("INSERT INTO Teams(Title,CreationDate,CreatorId,LastChangeDate, LastChangeUserId) VALUES(@title,@creationdate,@creatorid,@lastchangedate,@lastchangeuserid)", connection);
				cmd.Parameters.AddWithValue("@title",title);
				cmd.Parameters.AddWithValue("@creationdate", creationDate);
				cmd.Parameters.AddWithValue("@creatorid",creatorId);
				cmd.Parameters.AddWithValue("@lastchangedate",lastChangeDate);
				cmd.Parameters.AddWithValue("@lastchangeuserid",lastChangeUserId);
				cmd.ExecuteNonQuery();
			}
			/// <summary>
			/// Loads a team by given <see cref="Id"/> as <see cref="int"/>
			/// </summary>
			/// <param name="id">ID to search for in the database</param>
			public void LoadTeam(int id)
            {
				SqlCommand cmd = new SqlCommand("SELECT Id,Title,CreationDate,CreatorId,LastChangeDate,LastChangeUserId FROM Teams WHERE Id = @ID",connection);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					this.id = reader.GetInt32(0);
					title = reader.GetString(1);
					CreationDate = reader.GetDateTime(2);
					creatorId = reader.GetInt32(3);
					lastChangeDate = reader.GetDateTime(4);
					lastChangeUserId = reader.GetInt32(5);
				}
				reader.Close();
			}
			private int id;
			private string title;
			private DateTime creationDate;
			private int creatorId;
			private DateTime lastChangeDate;
			private int lastChangeUserId;

		}
	}
}