using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly DbConnection _db;

        public BookAuthorRepository() : this(new DbConnection()) { }

        public BookAuthorRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Add(BookAuthor bookAuthor)
        {
            if (bookAuthor == null)
                throw new ArgumentNullException(nameof(bookAuthor));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"INSERT INTO [BookAuthor] (BookID, AuthorID, Role, IsPrimaryAuthor) 
                                    VALUES (@BookID, @AuthorID, @Role, @IsPrimaryAuthor)";

                AddParameter(cmd, "@BookID", DbType.Int32, bookAuthor.BookID, 0);
                AddParameter(cmd, "@AuthorID", DbType.Int32, bookAuthor.AuthorID, 0);
                AddParameter(cmd, "@Role", DbType.String, bookAuthor.Role, 50);
                AddParameter(cmd, "@IsPrimaryAuthor", DbType.Boolean, bookAuthor.IsPrimaryAuthor, 0);

                cmd.ExecuteNonQuery();
            }
        }

        public List<BookAuthor> GetByBookId(int bookId)
        {
            var bookAuthors = new List<BookAuthor>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT BookID, AuthorID, Role, IsPrimaryAuthor 
                                    FROM [BookAuthor] WHERE BookID = @BookID";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookAuthors.Add(new BookAuthor
                        {
                            BookID = reader.GetInt32(0),
                            AuthorID = reader.GetInt32(1),
                            Role = reader.IsDBNull(2) ? "Author" : reader.GetString(2),
                            IsPrimaryAuthor = !reader.IsDBNull(3) && reader.GetBoolean(3)
                        });
                    }
                }
            }

            return bookAuthors;
        }

        // New: return distinct AuthorIDs which have the specified role across the BookAuthor table.
        public List<int> GetDistinctAuthorIdsByRole(string role)
        {
            var ids = new List<int>();

            if (string.IsNullOrWhiteSpace(role)) return ids;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT DISTINCT AuthorID FROM [BookAuthor] WHERE Role = @Role";
                AddParameter(cmd, "@Role", DbType.String, role, 50);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            ids.Add(reader.GetInt32(0));
                    }
                }
            }

            return ids;
        }

        // New: delete all BookAuthor rows for a given BookID (used by Edit flow to replace associations)
        public void DeleteByBookId(int bookId)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM [BookAuthor] WHERE BookID = @BookID";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);
                cmd.ExecuteNonQuery();
            }
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
