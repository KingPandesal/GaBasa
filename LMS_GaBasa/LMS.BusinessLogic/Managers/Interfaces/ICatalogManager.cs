using LMS.Model.DTOs.Book;
using LMS.Model.DTOs.Catalog;
using LMS.Model.Models.Catalog;
using System.Collections.Generic;

namespace LMS.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// Manages catalog operations: categories, languages, publishers
    /// </summary>
    public interface ICatalogManager
    {
        List<Category> GetAllCategories();
        Category GetOrCreateCategory(string categoryName);
        List<string> GetAllLanguages();
        void AddLanguageIfNotExists(string language);

        /// <summary>
        /// Gets books added within the last 7 days (new arrivals).
        /// </summary>
        List<DTOCatalogBook> GetNewArrivals();

        /// <summary>
        /// Gets books with the most borrows (popular books).
        /// </summary>
        List<DTOCatalogBook> GetPopularBooks(int topCount = 10);
    }
}