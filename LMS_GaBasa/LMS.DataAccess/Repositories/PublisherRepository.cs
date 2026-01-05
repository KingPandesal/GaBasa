using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly DbConnection _db;

        public PublisherRepository() : this(new DbConnection()) { }

        public PublisherRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public List<Publisher> GetAll()
        {
            var publishers = new List<Publisher>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT PublisherID, Name, Address, ContactNumber FROM [Publisher] ORDER BY Name";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        publishers.Add(new Publisher
                        {
                            PublisherID = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                            ContactNumber = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }

            return publishers;
        }

        public Publisher GetById(int publisherId)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT PublisherID, Name, Address, ContactNumber FROM [Publisher] WHERE PublisherID = @PublisherID";

                var p = cmd.CreateParameter();
                p.ParameterName = "@PublisherID";
                p.DbType = DbType.Int32;
                p.Value = publisherId;
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new Publisher
                    {
                        PublisherID = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                        ContactNumber = reader.IsDBNull(3) ? null : reader.GetString(3)
                    };
                }
            }
        }

        // New method: add publisher and return new ID
        public int Add(Publisher publisher)
        {
            if (publisher == null)
                throw new ArgumentNullException(nameof(publisher));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"INSERT INTO [Publisher] (Name, Address, ContactNumber)
                                    VALUES (@Name, @Address, @ContactNumber);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@Name";
                p1.DbType = DbType.String;
                p1.Size = 200;
                p1.Value = publisher.Name ?? (object)DBNull.Value;
                cmd.Parameters.Add(p1);

                var p2 = cmd.CreateParameter();
                p2.ParameterName = "@Address";
                p2.DbType = DbType.String;
                p2.Size = 500;
                p2.Value = publisher.Address ?? (object)DBNull.Value;
                cmd.Parameters.Add(p2);

                var p3 = cmd.CreateParameter();
                p3.ParameterName = "@ContactNumber";
                p3.DbType = DbType.String;
                p3.Size = 50;
                p3.Value = publisher.ContactNumber ?? (object)DBNull.Value;
                cmd.Parameters.Add(p3);

                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
