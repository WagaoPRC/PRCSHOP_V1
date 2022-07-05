using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
//NopCommerce
using Nop.Core.Data;
using Nop.Plugin.Widgets.Selenium.Domain;
using Nop.Web.Framework.Controllers;
using Nop.Services.Catalog;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Plugin;

//Selenium
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using Nop.Plugin.Widgets.Selenium.Service;

namespace Nop.Plugin.Widgets.Selenium.Controllers
{
    public class SeleniumController : BasePluginController
    {
        #region Fields
        private readonly ISeleniumService _seleniumRepository;
        private readonly IProductService _productService;
        private readonly INcardsService _ncardsService;
        private readonly INsetsService _nsetsService;
        #endregion

        #region Ctor
        public SeleniumController(IProductService productService, INcardsService ncardsService
            , INsetsService nsetsService, ISeleniumService seleniumRepository)
        {
            _productService = productService;
            _ncardsService = ncardsService;
            _nsetsService = nsetsService;
            _seleniumRepository = seleniumRepository;
        }
        #endregion

        public ActionResult Manage()
        {
            IList<Nsets> AllNsets = _nsetsService.GetAllNsets();
            ViewBag.ListNsets = AllNsets;
            return View();
        }

        #region Methods
        [HttpPost]
        public ActionResult Manage(Nsets oNsets, FormCollection oForm)
        {
            EdgeDriver oEdgeDriver = new EdgeDriver(@"C:\Windows\SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe");

            IList<Ncards> lstNcards = _ncardsService.GetDiferenceNcardsProduct();
            _seleniumRepository.GetLigaMagicData(oEdgeDriver, lstNcards);
            var chk = oForm["chkSet"];
            return null;
        }
        #endregion
    }
}
