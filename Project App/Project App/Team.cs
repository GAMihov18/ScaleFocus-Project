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

            private int id;
            private string title;
            private DateTime creationDate;
            private int creatorId;
            private DateTime lastChangeDate;
            private int lastChangeUserId;

            public int Id { get { return id; } set { id = value; } }
            public string Title { get { return title; }set { title = value; } }


        }
    }
}