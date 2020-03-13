using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonsViewer.DataLayer;
using PersonsViewer.Model;
using System.Data.SqlClient;

namespace PersonsViewer.DataLayer.SQL
{
    public class TsqlDataManage : IDataManage
    {
        private string connectionString;

        private StatusRepository statusRepository;
        private DepartamentRepository departamentRepository;
        private PostRepository postRepository;

        public TsqlDataManage(string connectionString)
        {
            this.connectionString = connectionString;
            statusRepository = new StatusRepository(connectionString);
            departamentRepository = new DepartamentRepository(connectionString);
            postRepository = new PostRepository(connectionString);
        }

        public IEnumerable<Person> GetPeople(Filter filterOptions)
        {
            List<Person> people = new List<Person>();

            using (var sql = new SqlConnection(connectionString))
            {
                sql.Open();
                using (var command = sql.CreateCommand())
                {
                    command.CommandText = "get_people";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@status",
                        Value = (filterOptions.Status != null) ? (object)filterOptions.Status.Id : DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@dep",
                        Value = (filterOptions.Departament != null) ? (object)filterOptions.Departament.Id : DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@post",
                        Value = (filterOptions.Post != null) ? (object)filterOptions.Post.Id : DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@last_name",
                        Value = (object)filterOptions.LastName ?? DBNull.Value
                    });

                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            people.Add(ParsePerson(reader));
                        }
                    }
                }
            }

            return people;
        }

        public IDictionary<DateTime, int> GetStatistic(Status status, bool isDateEmploy, DateTime startDate, DateTime endDate)
        {
            Dictionary<DateTime, int> statistic = new Dictionary<DateTime, int>();

            using (var sql = new SqlConnection(connectionString))
            {
                sql.Open();
                using (var command = sql.CreateCommand())
                {
                    command.CommandText = "get_statistic";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@status",
                        Value = (object)status.Id ?? DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@is_date_employ",
                        Value = isDateEmploy
                    });
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@start_date",
                        Value = startDate
                    });
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@end_date",
                        Value = endDate
                    });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime dateTime = reader.GetDateTime(0);
                            int count = reader.GetInt32(1);

                            statistic.Add(dateTime, count);
                        }
                    }
                }
            }


            return statistic;
        }

        private Person ParsePerson(SqlDataReader reader)
        {
            Person person = new Person()
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                SecondName = reader.GetString(reader.GetOrdinal("second_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),

                Status = statusRepository.GetEntityByID(reader.GetInt32(reader.GetOrdinal("id_status"))),
                Departament = departamentRepository.GetEntityByID(reader.GetInt32(reader.GetOrdinal("id_dep"))),
                Post = postRepository.GetEntityByID(reader.GetInt32(reader.GetOrdinal("id_post")))
            };

            if(!reader.IsDBNull(reader.GetOrdinal("date_employ")))
            {
                person.DateEmploy = reader.GetDateTime(reader.GetOrdinal("date_employ"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("date_unemploy")))
            {
                person.DateUnemploy = reader.GetDateTime(reader.GetOrdinal("date_unemploy"));
            }

            return person;
        }
    }
}
