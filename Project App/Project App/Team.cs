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
            public int Id { get { return id; } set { id = value; } }
            public string Title { get { return title; } set { title = value; } }
            public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
            public int CreatorId { get { return creatorId; } set { creatorId = value; } }
            public DateTime LastChangeDate { get { return lastChangeDate; } set { lastChangeDate = value; } }
            public int LastChangeUserId { get { return lastChangeUserId; } }

            private int id;
            private string title;
            private DateTime creationDate;
            private int creatorId;
            private DateTime lastChangeDate;
            private int lastChangeUserId;

        }
    }
}