using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DbConnection _db;

        public AuthorRepository() : this(new DbConnection()) { }

        public AuthorRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Add(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"INSERT INTO [Author] ([Name]) VALUES (@Name);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@Name", DbType.String, author.FullName, 200);
                return (int)cmd.ExecuteScalar();
            }
        }

        public Author GetById(int authorId)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT AuthorID, [Name] FROM [Author] WHERE AuthorID = @AuthorID";
                AddParameter(cmd, "@AuthorID", DbType.Int32, authorId, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new Author
                    {
                        AuthorID = reader.GetInt32(0),
                        FullName = reader.IsDBNull(1) ? null : reader.GetString(1)
                    };
                }
            }
        }

        public Author GetByName(string fullName)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT AuthorID, [Name] FROM [Author] WHERE [Name] = @Name";
                AddParameter(cmd, "@Name", DbType.String, fullName, 200);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new Author
                    {
                        AuthorID = reader.GetInt32(0),
                        FullName = reader.IsDBNull(1) ? null : reader.GetString(1)
                    };
                }
            }
        }

        public List<Author> GetAll()
        {
            var authors = new List<Author>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT AuthorID, [Name] FROM [Author] ORDER BY [Name]";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        authors.Add(new Author
                        {
                            AuthorID = reader.GetInt32(0),
                            FullName = reader.IsDBNull(1) ? null : reader.GetString(1)
                        });
                    }
                }
            }

            return authors;
        }

        private void AddParameter(IDbCommand cmd, string name, DbType type, object value, int size)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.DbType = type;
            if (size > 0) p.Size = size;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}
