using PersonsViewer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.DataLayer.SQL
{
    public class DepartamentRepository : IRepository<Departament>
    {
        private List<Departament> departaments;
        private string connectionString;

        public DepartamentRepository(string connectionString)
        {
            departaments = null;
            this.connectionString = connectionString;
        }

        public IEnumerable<Departament> GetAllEntitys()
        {
            if (departaments == null)
            {
                InitDepartments();
            }

            return departaments;
        }

        public Departament GetEntityByID(int id)
        {
            if(departaments == null)
            {
                InitDepartments();
            }

            return departaments[id-1];
        }

        private void InitDepartments()
        {
            departaments = new List<Departament>();

            using (var sql = new SqlConnection(connectionString))
            {
                sql.Open();
                using (var command = sql.CreateCommand())
                {
                    command.CommandText = "get_deps";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Departament departament = ParseDepartament(reader);
                            departaments.Add(departament);
                        }
                    }
                }
            }
        }

        private Departament ParseDepartament(SqlDataReader reader)
        {
            Departament departament = new Departament()
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name"))
            };

            return departament;
        }
    }
}
