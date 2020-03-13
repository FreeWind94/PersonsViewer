using PersonsViewer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.DataLayer.SQL
{
    public class StatusRepository : IRepository<Status>
    {
        private List <Status> statuses;
        private string connectionString;

        public StatusRepository(string connectionString)
        {
            statuses = null;
            this.connectionString = connectionString;
        }

        public IEnumerable<Status> GetAllEntitys()
        {
            if (statuses == null)
            {
                InitStatuses();
            }

            return statuses;
        }

        public Status GetEntityByID(int id)
        {
            if(statuses == null)
            {
                InitStatuses();
            }

            return statuses[id-1];
        }

        private void InitStatuses()
        {
            statuses = new List<Status>();

            using (var sql = new SqlConnection(connectionString))
            {
                sql.Open();
                using (var command = sql.CreateCommand())
                {
                    command.CommandText = "get_status";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Status status = ParseStatus(reader);
                            statuses.Add(status);
                        }
                    }
                }
            }
        }

        private Status ParseStatus(SqlDataReader reader)
        {
            Status status = new Status()
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name"))
            };

            return status;
        }
    }
}
