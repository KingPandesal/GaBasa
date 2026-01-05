using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;

namespace LMS.BusinessLogic.Managers
{
    public class CatalogManager : ICatalogManager
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILanguageRepository _languageRepo;

        // Predefined languages for the dropdown
        private static readonly List<string> PredefinedLanguages = new List<string>
        {
            "English", "Spanish", "French", "German", "Italian", "Portuguese",
            "Chinese", "Japanese", "Korean", "Russian", "Arabic", "Hindi",
            "Tagalog", "Cebuano", "Ilocano", "Hiligaynon", "Waray", "Bikol",
            "Dutch", "Swedish", "Norwegian", "Danish", "Finnish", "Polish",
            "Greek", "Turkish", "Vietnamese", "Thai", "Indonesian", "Malay",
            "Hebrew", "Persian", "Bengali", "Urdu", "Tamil", "Telugu"
        };

        public CatalogManager(ICategoryRepository categoryRepo, ILanguageRepository languageRepo)
        {
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            _languageRepo = languageRepo ?? throw new ArgumentNullException(nameof(languageRepo));
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepo.GetAll();
        }

        public Category GetOrCreateCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return null;

            var existing = _categoryRepo.GetByName(categoryName.Trim());
            if (existing != null)
                return existing;

            var newCategory = new Category { Name = categoryName.Trim() };
            int id = _categoryRepo.Add(newCategory);
            newCategory.CategoryID = id;
            return newCategory;
        }

        public List<string> GetAllLanguages()
        {
            return PredefinedLanguages;
        }

        public void AddLanguageIfNotExists(string language)
        {
            if (string.IsNullOrWhiteSpace(language)) return;

            if (!_languageRepo.Exists(language))
            {
                _languageRepo.Add(language);
            }
        }
    }
}
