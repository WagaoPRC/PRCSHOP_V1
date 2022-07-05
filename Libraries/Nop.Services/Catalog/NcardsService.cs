using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;


namespace Nop.Services.Catalog
{
    public partial class NcardsService : INcardsService
    {
        #region Constant
        private const string NCARDS_BY_ID_KEY = "Nop.Ncards.Id-{0}";
        #endregion


        #region Fields
        private IRepository<Ncards> _ncardsRepository;
        private readonly IRepository<Product> _productRepository;
        #endregion

        #region Ctor
        public NcardsService(
             IRepository<Ncards> ncardsRepository
            , IRepository<Product> productRepository)
        {
            _ncardsRepository = ncardsRepository;
            _productRepository = productRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Updates the Ncards
        /// </summary>
        /// <param name="product">Ncards</param>
        public virtual void UpdateProduct(Ncards ncards)
        {
            if (ncards == null)
                throw new ArgumentNullException("ncards");

            //update
            _ncardsRepository.Update(ncards);

        }

        /// <summary>
        /// Delete Ncard
        /// </summary>
        /// <param name="Ncard"></param>
        public virtual void DeleteNcard(Ncards ncard)
        {
            if (ncard == null)
                throw new ArgumentNullException("Ncards");

            //work here
            try
            {
                _ncardsRepository.Delete(ncard);
            }
            catch { };
        }

        /// <summary>
        /// Get All NCard
        /// </summary>
        /// <returns></returns>
        public virtual IList<Ncards> GetAllNcards()
        {
            List<Ncards> oNcards = new List<Ncards>();
            try
            {
                IQueryable<Ncards> query = from card in _ncardsRepository.Table
                                           select card;
                List<Ncards> listNcards = query.ToList();
                return listNcards;
            }
            catch (Exception Ex)
            {
                var log = Ex;
                return null;
            }
        }

        public virtual IList<Ncards> GetCardByNname(string Nname)
        {
            IQueryable<Ncards> query = from card in _ncardsRepository.Table
                                       where card.Nname.Contains(Nname)
                                       select card;
            return query.ToList();
        }

        /// <summary>
        /// Get Ncard by ID
        /// </summary>
        /// <param name="ncardsId"></param>
        /// <returns></returns>
        public virtual Ncards GetNcardsById(int ncardsId)
        {
            if (ncardsId <= 0)
                return null;
            IQueryable<Ncards> query = from card in _ncardsRepository.Table
                                       where card.Id == ncardsId
                                       select card;
            return query.ToList().First();
        }

        /// <summary>
        /// Return listNcard with diferent Product.SKU != ncard.Nid
        /// </summary>
        /// <returns></returns>
        public virtual IList<Ncards> GetDiferenceNcardsProduct()
        {
            var query = from card in _ncardsRepository.Table
                        where(
                        //card.Nset.Equals("UMA")||
                        //card.Nset.Equals("UMAP")
                        //card.Nset.Equals("GRN") ||
                        //card.Nset.Equals("C18") 
                        //card.Nset.Equals("M19")||
                        //card.Nset.Equals("BBD")||
                        //card.Nset.Equals("SS1") || card.Nset.Equals("MVG")
                        //card.Nset.Equals("DOM")||
                        //card.Nset.Equals("RIX")||
                        //card.Nset.Equals("XLN")
                        //||card.Nset.Equals("EVI") || 
                        //card.Nset.Equals("KLD") || card.Nset.Equals("AER") 
                        //card.Nset.Equals("AKH") || card.Nset.Equals("HOU")
                        //card.Nset.Equals("JOU") || card.Nset.Equals("BNG") || card.Nset.Equals("THS")
                        //card.Nset.Equals("IMA") || card.Nset.Equals("V17") || card.Nset.Equals("M25")
                        //||card.Nset.Equals("SOI")
                        //|| card.Nset.Equals("EMN") 
                        //card.Nset.Equals("BFZ") ||card.Nset.Equals("OGW") ||card.Nset.Equals("ORI") 
                        //||card.Nset.Equals("KTK") || card.Nset.Equals("FRF") || card.Nset.Equals("DTK") 
                        card.Nset.Equals("MMA") || card.Nset.Equals("MM2") || card.Nset.Equals("MM3")//|| card.Nset.Equals("M15")
                        )
                       // && (card.Nrarity.Contains("R") || card.Nrarity.Contains("M") || card.Nrarity.Contains("U"))
                        select card;
            var obj = query.ToList();
            return obj;
        }
        #endregion

    }
}
