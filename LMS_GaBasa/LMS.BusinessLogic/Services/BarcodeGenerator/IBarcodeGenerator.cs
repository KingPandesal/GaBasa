using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.BarcodeGenerator
{
    public interface IBarcodeGenerator
    {
        /// <summary>
        /// Generate barcode images for the given texts and return mapping text -> saved path.
        /// Implementation decides where to store images (configured in ctor).
        /// </summary>
        IDictionary<string, string> GenerateMany(IEnumerable<string> texts);
    }
}
