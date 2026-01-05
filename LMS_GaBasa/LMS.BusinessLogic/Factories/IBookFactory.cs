using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Enums;

namespace LMS.BusinessLogic.Factories
{
    public interface IBookFactory
    {
        Book Create(ResourceType resourceType);
    }
}
