using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Manager
{
    namespace Tasks
    {
        /// <summary>
        /// Enumeration representing the available statuses for tasks
        /// </summary>
        enum TaskStatus
        {
            Pending,InProgress,Completed
        }
        /// <summary>
        /// A task
        /// </summary>
        class Task
        {
            int id;
            int projectId;
            int assigneeId;
            string title;
            string description;
            TaskStatus status;
            private DateTime creationDate;
            private int creatorId;
            private DateTime timeOfEdit;
            private int editorId;
        }
    }
}
