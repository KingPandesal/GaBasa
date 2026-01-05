using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Enums;

namespace LMS.BusinessLogic.Factories
{
    public class BookFactory : IBookFactory
    {
        public Book Create(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.PhysicalBook:
                    return new PhysicalBook();
                case ResourceType.EBook:
                    return new EBook();
                case ResourceType.Periodical:
                    return new Periodical();
                case ResourceType.Thesis:
                    return new Thesis();
                case ResourceType.AV:
                    return new AV();
                default:
                    throw new ArgumentException($"Unknown resource type: {resourceType}");
            }
        }
    }
}
