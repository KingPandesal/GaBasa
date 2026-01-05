using System.Collections.Generic;

namespace LMS.DataAccess.Interfaces
{
    public interface ILanguageRepository
    {
        List<string> GetAll();
        void Add(string language);
        bool Exists(string language);
    }
}
