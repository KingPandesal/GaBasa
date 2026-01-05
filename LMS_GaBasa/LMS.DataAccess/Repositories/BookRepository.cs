using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LMS.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DbConnection _db;

        public BookRepository() : this(new DbConnection()) { }

        public BookRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Add(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"INSERT INTO [Book] 
                    (ISBN, CallNumber, Title, Subtitle, PublisherID, CategoryID, Language, 
                     Pages, Edition, PublicationYear, PhysicalDescription, ResourceType, 
                     CoverImage, LoanType, DownloadURL)
                    VALUES (@ISBN, @CallNumber, @Title, @Subtitle, @PublisherID, @CategoryID, 
                            @Language, @Pages, @Edition, @PublicationYear, @PhysicalDescription, 
                            @ResourceType, @CoverImage, @LoanType, @DownloadURL);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@ISBN", DbType.String, book.ISBN, 20);
                AddParameter(cmd, "@CallNumber", DbType.String, book.CallNumber, 50);
                AddParameter(cmd, "@Title", DbType.String, book.Title, 500);
                AddParameter(cmd, "@Subtitle", DbType.String, book.Subtitle, 500);
                AddParameter(cmd, "@PublisherID", DbType.Int32, book.PublisherID, 0);
                AddParameter(cmd, "@CategoryID", DbType.Int32, book.CategoryID, 0);
                AddParameter(cmd, "@Language", DbType.String, book.Language, 50);
                AddParameter(cmd, "@Pages", DbType.Int32, book.Pages, 0);
                AddParameter(cmd, "@Edition", DbType.String, book.Edition, 50);
                AddParameter(cmd, "@PublicationYear", DbType.Int32, book.PublicationYear, 0);
                AddParameter(cmd, "@PhysicalDescription", DbType.String, book.PhysicalDescription, 500);

                // Map enum -> DB value, adjust mapping to match your CHECK constraint
                string resourceTypeDbValue = MapResourceTypeToDbValue(book.ResourceType);
                AddParameter(cmd, "@ResourceType", DbType.String, resourceTypeDbValue, 50);

                AddParameter(cmd, "@CoverImage", DbType.String, book.CoverImage, 500);
                AddParameter(cmd, "@LoanType", DbType.String, book.LoanType, 50);
                AddParameter(cmd, "@DownloadURL", DbType.String, book.DownloadURL, 500);

                try
                {
                    return (int)cmd.ExecuteScalar();
                }
                catch (System.Data.SqlClient.SqlException sqlEx)
                {
                    // Provide actionable debugging info if constraint still fails
                    throw new Exception($"Failed inserting Book. ResourceType sent: '{resourceTypeDbValue}'. DB error: {sqlEx.Message}", sqlEx);
                }
            }
        }

        public bool Update(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"UPDATE [Book]
                                    SET ISBN = @ISBN,
                                        CallNumber = @CallNumber,
                                        Title = @Title,
                                        Subtitle = @Subtitle,
                                        PublisherID = @PublisherID,
                                        CategoryID = @CategoryID,
                                        Language = @Language,
                                        Pages = @Pages,
                                        Edition = @Edition,
                                        PublicationYear = @PublicationYear,
                                        PhysicalDescription = @PhysicalDescription,
                                        ResourceType = @ResourceType,
                                        CoverImage = @CoverImage,
                                        LoanType = @LoanType,
                                        DownloadURL = @DownloadURL
                                    WHERE BookID = @BookID";

                AddParameter(cmd, "@ISBN", DbType.String, book.ISBN, 20);
                AddParameter(cmd, "@CallNumber", DbType.String, book.CallNumber, 50);
                AddParameter(cmd, "@Title", DbType.String, book.Title, 500);
                AddParameter(cmd, "@Subtitle", DbType.String, book.Subtitle, 500);
                AddParameter(cmd, "@PublisherID", DbType.Int32, book.PublisherID, 0);
                AddParameter(cmd, "@CategoryID", DbType.Int32, book.CategoryID, 0);
                AddParameter(cmd, "@Language", DbType.String, book.Language, 50);
                AddParameter(cmd, "@Pages", DbType.Int32, book.Pages, 0);
                AddParameter(cmd, "@Edition", DbType.String, book.Edition, 50);
                AddParameter(cmd, "@PublicationYear", DbType.Int32, book.PublicationYear, 0);
                AddParameter(cmd, "@PhysicalDescription", DbType.String, book.PhysicalDescription, 500);

                string resourceTypeDbValue = MapResourceTypeToDbValue(book.ResourceType);
                AddParameter(cmd, "@ResourceType", DbType.String, resourceTypeDbValue, 50);

                AddParameter(cmd, "@CoverImage", DbType.String, book.CoverImage, 500);
                AddParameter(cmd, "@LoanType", DbType.String, book.LoanType, 50);
                AddParameter(cmd, "@DownloadURL", DbType.String, book.DownloadURL, 500);

                AddParameter(cmd, "@BookID", DbType.Int32, book.BookID, 0);

                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (System.Data.SqlClient.SqlException sqlEx)
                {
                    throw new Exception($"Failed updating Book. ResourceType sent: '{resourceTypeDbValue}'. DB error: {sqlEx.Message}", sqlEx);
                }
            }
        }

        public Book GetById(int bookId)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT BookID, ISBN, CallNumber, Title, Subtitle, PublisherID, 
                                           CategoryID, Language, Pages, Edition, PublicationYear, 
                                           PhysicalDescription, ResourceType, CoverImage, LoanType, DownloadURL
                                    FROM [Book] WHERE BookID = @BookID";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return MapReaderToBook(reader);
                }
            }
        }

        public bool ISBNExists(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(1) FROM [Book] WHERE ISBN = @ISBN";
                AddParameter(cmd, "@ISBN", DbType.String, isbn, 20);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool CallNumberExists(string callNumber)
        {
            if (string.IsNullOrWhiteSpace(callNumber))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(1) FROM [Book] WHERE CallNumber = @CallNumber";
                AddParameter(cmd, "@CallNumber", DbType.String, callNumber, 50);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public List<Book> GetAll()
        {
            var books = new List<Book>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT BookID, ISBN, CallNumber, Title, Subtitle, PublisherID, 
                                           CategoryID, Language, Pages, Edition, PublicationYear, 
                                           PhysicalDescription, ResourceType, CoverImage, LoanType, DownloadURL
                                    FROM [Book] ORDER BY BookID DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(MapReaderToBook(reader));
                    }
                }
            }

            return books;
        }

        private Book MapReaderToBook(IDataReader reader)
        {
            string resourceTypeStr = reader.IsDBNull(12) ? "Book" : reader.GetString(12);

            // Map DB string values to enum explicitly (DB uses user-friendly literals like 'E-Book', 'Book', 'Periodical', etc.)
            ResourceType resourceType = MapDbValueToResourceType(resourceTypeStr);

            Book book = CreateBookByResourceType(resourceType);

            book.BookID = reader.GetInt32(0);
            book.ISBN = reader.IsDBNull(1) ? null : reader.GetString(1);
            book.CallNumber = reader.IsDBNull(2) ? null : reader.GetString(2);
            book.Title = reader.IsDBNull(3) ? null : reader.GetString(3);
            book.Subtitle = reader.IsDBNull(4) ? null : reader.GetString(4);
            book.PublisherID = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
            book.CategoryID = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
            book.Language = reader.IsDBNull(7) ? null : reader.GetString(7);
            book.Pages = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
            book.Edition = reader.IsDBNull(9) ? null : reader.GetString(9);
            book.PublicationYear = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
            book.PhysicalDescription = reader.IsDBNull(11) ? null : reader.GetString(11);
            book.ResourceType = resourceType;
            book.CoverImage = reader.IsDBNull(13) ? null : reader.GetString(13);
            book.LoanType = reader.IsDBNull(14) ? null : reader.GetString(14);
            book.DownloadURL = reader.IsDBNull(15) ? null : reader.GetString(15);

            return book;
        }

        private ResourceType MapDbValueToResourceType(string dbValue)
        {
            if (string.IsNullOrWhiteSpace(dbValue)) return ResourceType.PhysicalBook;

            switch (dbValue.Trim())
            {
                case "E-Book":
                case "EBook":
                case "EB":
                    return ResourceType.EBook;

                case "Thesis":
                case "Theses":
                case "TH":
                    return ResourceType.Thesis;

                case "AV":
                case "Audio-Visual":
                    return ResourceType.AV;

                case "Periodical":
                case "Periodicals":
                case "PR":
                    return ResourceType.Periodical;

                case "Book":
                case "PhysicalBook":
                case "Physical Book":
                default:
                    return ResourceType.PhysicalBook;
            }
        }

        private Book CreateBookByResourceType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.EBook:
                    return new EBook();
                case ResourceType.Periodical:
                    return new Periodical();
                case ResourceType.Thesis:
                    return new Thesis();
                case ResourceType.AV:
                    return new AV();
                case ResourceType.PhysicalBook:
                default:
                    return new PhysicalBook();
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

        // Add this helper in the same class (BookRepository)
        private string MapResourceTypeToDbValue(ResourceType type)
        {
            // Adjust these return values to match the exact literals in the DB CHECK constraint.
            switch (type)
            {
                case ResourceType.EBook:
                    return "E-Book";
                case ResourceType.Thesis:
                    return "Thesis";
                case ResourceType.AV:
                    return "AV";
                case ResourceType.Periodical:
                    return "Periodical";
                case ResourceType.PhysicalBook:
                default:
                    return "Book";
            }
        }
    }
}
