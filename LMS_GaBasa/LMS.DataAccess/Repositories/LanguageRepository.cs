using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly DbConnection _db;

        public LanguageRepository() : this(new DbConnection()) { }

        public LanguageRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public List<string> GetAll()
        {
            var languages = new List<string>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT Name FROM [Language] ORDER BY Name";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            languages.Add(reader.GetString(0));
                    }
                }
            }

            return languages;
        }

        public void Add(string language)
        {
            if (string.IsNullOrWhiteSpace(language)) return;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "INSERT INTO [Language] (Name) VALUES (@Name)";

                var p = cmd.CreateParameter();
                p.ParameterName = "@Name";
                p.DbType = DbType.String;
                p.Size = 100;
                p.Value = language.Trim();
                cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }
        }

        public bool Exists(string language)
        {
            if (string.IsNullOrWhiteSpace(language)) return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(1) FROM [Language] WHERE Name = @Name";

                var p = cmd.CreateParameter();
                p.ParameterName = "@Name";
                p.DbType = DbType.String;
                p.Size = 100;
                p.Value = language.Trim();
                cmd.Parameters.Add(p);

                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}
