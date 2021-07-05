using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Manager
{
    namespace Users
    {
        class User
        {
            private int Id;
            private string username;
            private string firstName;
            private string lastName;
            private string password;
            private DateTime creationDate;
            private int creatorId;
            private DateTime timeOfEdit;
            private int editorId;
        }
    }
}
