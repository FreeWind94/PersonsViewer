using PersonsViewer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.DataLayer.SQL
{
    public class PostRepository : IRepository<Post>
    {
        private List<Post> posts;
        private string connectionString;

        public PostRepository(string connectionString)
        {
            posts = null;
            this.connectionString = connectionString;
        }

        public IEnumerable<Post> GetAllEntitys()
        {
            if (posts == null)
            {
                InitPosts();
            }

            return posts;
        }

        public Post GetEntityByID(int id)
        {
            if(posts == null)
            {
                InitPosts();
            }

            return posts[id-1];
        }

        private void InitPosts()
        {
            posts = new List<Post>();

            using (var sql = new SqlConnection(connectionString))
            {
                sql.Open();
                using (var command = sql.CreateCommand())
                {
                    command.CommandText = "get_posts";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Post post = ParsePost(reader);
                            posts.Add(post);
                        }
                    }
                }
            }
        }

        private Post ParsePost(SqlDataReader reader)
        {
            Post post = new Post()
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name"))
            };

            return post;
        }
    }
}
