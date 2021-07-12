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

			public int Id { get { return id; } set { id = value; } }
			public string Title { get { return title; } set { title = value; } }
			public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
			public int CreatorId { get { return creatorId; } set { creatorId = value; } }
			public DateTime LastChangeDate { get { return lastChangeDate; } set { lastChangeDate = value; } }
			public int LastChangeUserId { get { return lastChangeUserId; } set { lastChangeUserId = value; } }

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
			public void LoadTeam(int id)
            {
				SqlCommand cmd = new SqlCommand("SELECT Id,Title,CreationDate,CreatorId,LastChangeDate,LastChangeUserId FROM Teams WHERE Id = @ID",connection);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					id = reader.GetInt32(0);
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