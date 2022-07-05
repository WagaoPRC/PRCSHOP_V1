using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
    public partial class NsetsService: INsetsService
    {
        #region Constant
        private const string NSETS_BY_ID_KEY = "Nop.Nsets.Id-{0}";
        #endregion


        #region Fields
        private readonly IRepository<Nsets> _nsetsRepository;
        private readonly IRepository<Category> _categoryRepository;
        #endregion

        #region Ctor
        public NsetsService(
             IRepository<Nsets> nsetsRepository
            , IRepository<Category> categoryRepository
            )
        {
            _nsetsRepository = nsetsRepository;
            _categoryRepository = categoryRepository;
        }
        #endregion

        #region Methods
        public virtual IList<Nsets> GetAllNsets()
        {
            IQueryable<Nsets> query = from nset in _nsetsRepository.Table
                                      orderby nset.Ndate descending
                        select nset;
            List<Nsets> obj = query.ToList();
            return query.ToList();
        }
        public virtual void DeleteNsets(Nsets oNsets)
        {
            if (oNsets == null)
                throw new ArgumentNullException("Nsets");

            //work here
            try
            {
                _nsetsRepository.Delete(oNsets);
            }
            catch { };
        }

        public virtual Nsets GetNsetsByName( string Name)
        {
            Nsets oNsets = new Nsets();
            IQueryable<Nsets> query = from nset in _nsetsRepository.Table
                                      where nset.Nname.Contains(Name) || nset.Ncode.Contains(Name)
                                      select nset;
            List<Nsets> obj = query.ToList();
            return query.ToList().FirstOrDefault();
        } 
        #endregion
    }
}
