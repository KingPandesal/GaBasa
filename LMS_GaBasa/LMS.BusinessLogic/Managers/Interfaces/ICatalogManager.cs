using System.Collections.Generic;
using LMS.Model.DTOs.Catalog;
using LMS.Model.Models.Catalog;

namespace LMS.BusinessLogic.Managers.Interfaces
{
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

        /// <summary>
        /// Search the catalog using a query and optional filters.
        /// </summary>
        List<DTOCatalogBook> SearchCatalog(string query,
            string category = null,
            string publisher = null,
            int? year = null,
            string callNumber = null,
            string accessionNumber = null);
    }
}