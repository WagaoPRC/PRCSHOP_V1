using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;

namespace Nop.Services.Catalog
{
    public partial interface INcardsService
    {
        /// <summary>
        /// Updates the Ncards
        /// </summary>
        /// <param name="product">Ncards</param>
        void UpdateProduct(Ncards ncards);
        /// <summary>
        /// Delete a Ncard
        /// </summary>
        /// <param name="Ncard"></param>
        void DeleteNcard(Ncards Ncard);

        /// <summary>
        /// Return All Cards
        /// </summary>
        /// <returns></returns>
        IList<Ncards> GetAllNcards();

        IList<Ncards> GetCardByNname(string Nname);
        Ncards GetNcardsById(int Nid);

        /// <summary>
        /// Get cards miss.
        /// </summary>
        /// <returns></returns>
        IList<Ncards> GetDiferenceNcardsProduct();
    }
}