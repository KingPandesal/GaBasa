using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbConnection _db;

        public CategoryRepository() : this(new DbConnection()) { }

        public CategoryRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Add(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"INSERT INTO [Category] (Name, Description)
                                    VALUES (@Name, @Description);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@Name", DbType.String, category.Name, 200);
                AddParameter(cmd, "@Description", DbType.String, category.Description, 1000);

                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Category> GetAll()
        {
            var categories = new List<Category>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT CategoryID, Name, Description FROM [Category] ORDER BY Name";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryID = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        });
                    }
                }
            }

            return categories;
        }

        public Category GetById(int categoryId)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT CategoryID, Name, Description FROM [Category] WHERE CategoryID = @CategoryID";
                AddParameter(cmd, "@CategoryID", DbType.Int32, categoryId, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new Category
                    {
                        CategoryID = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                    };
                }
            }
        }

        public Category GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT CategoryID, Name, Description FROM [Category] WHERE Name = @Name";
                AddParameter(cmd, "@Name", DbType.String, name.Trim(), 200);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new Category
                    {
                        CategoryID = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                    };
                }
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
