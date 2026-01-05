using LMS.Model.Models.Catalog;
using System.Collections.Generic;

namespace LMS.DataAccess.Interfaces
{
    public interface IPublisherRepository
    {
        List<Publisher> GetAll();
        Publisher GetById(int publisherId);
        int Add(Publisher publisher); // new: returns inserted PublisherID
    }
}
