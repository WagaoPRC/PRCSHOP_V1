using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using System.Collections.Generic;

namespace Nop.Services.Catalog
{
    public partial interface INsetsService
    {
        /// <summary>
        /// Get All editon Sets
        /// </summary>
        /// <returns></returns>
        IList<Nsets> GetAllNsets();

        void DeleteNsets(Nsets Nsets);
        Nsets GetNsetsByName(string Name);
    }
}
