using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using OpenQA.Selenium;
using Nop.Plugin.Widgets.Selenium.Domain;
using Nop.Core.Data;
using OpenQA.Selenium.Edge;
using Nop.Plugin.Widgets.Selenium.Service;
using System.Text.RegularExpressions;
using Nop.Services.Catalog;

namespace Nop.Plugin.Widgets.Selenium.Service
{
    public class SeleniumService : ISeleniumService
    {
        #region Constants

        #endregion

        #region Fields
        private IRepository<SeleniumRecord> _seleniumRepository;
        private INsetsService _nsetsRepository;
        private INcardsService _ncardsRepository;
        private IProductService _productService;
        #endregion

        #region Ctor
        public SeleniumService(IRepository<SeleniumRecord> seleniumRepository, INsetsService nsetsRepository, IProductService productService, INcardsService ncardsRepository)
        {
            _seleniumRepository = seleniumRepository;
            _nsetsRepository = nsetsRepository;
            _productService = productService;
            _ncardsRepository = ncardsRepository;
        }
        #endregion

        #region Methods


        public Ncards UpdatePrice(Ncards oProduct)
        {
            throw new NotImplementedException();
        }

        public void UpdatePrice(Product oProduct)
        {
            throw new NotImplementedException();
        }

        public bool VerifyChangePrice(Product oProduct)
        {
            throw new NotImplementedException();
        }

        public decimal GetBetterPrice(List<Ligamagic> lstLigamagic, Ncards oNcard)
        {
            //Decimal to Return
            decimal dReturn = 0;

            //List of Decimal
            List<Decimal> lstDecimal = new List<decimal>();


            foreach (var item in lstLigamagic)
            {
                if (item.iStockQuantity > 3  && !item.sNameSetPT.Contains("Foil") 
                    && !item.sNameSetPT.Contains("Pre Release") && item.sNameSetPT.Contains(oNcard.Nset.ToLower()))
                    lstDecimal.Add(item.dPrice);
            }

            //Set Price
            if (lstDecimal.Count() < 1)
            {
                dReturn = lstLigamagic[lstLigamagic.Count() / 2].dPrice;
            }
            else
                dReturn = lstDecimal.Min();

            dReturn = RoundDownToNearest(dReturn, 0.5m);
            if (dReturn < 1)
                dReturn = 0.5m;

            return dReturn;
        }
        public static Double RoundUpToNearest(Double passednumber, Double roundto)
        {
            // 105.5 up to nearest 1 = 106
            // 105.5 up to nearest 10 = 110
            // 105.5 up to nearest 7 = 112
            // 105.5 up to nearest 100 = 200
            // 105.5 up to nearest 0.2 = 105.6
            // 105.5 up to nearest 0.3 = 105.6

            //if no rounto then just pass original number back
            if (roundto == 0)
            {
                return passednumber;
            }
            else
            {
                return Math.Ceiling(passednumber / roundto) * roundto;
            }
        }

        public static Decimal RoundDownToNearest(Decimal passednumber, Decimal roundto)
        {
            // 105.5 down to nearest 1 = 105
            // 105.5 down to nearest 10 = 100
            // 105.5 down to nearest 7 = 105
            // 105.5 down to nearest 100 = 100
            // 105.5 down to nearest 0.2 = 105.4
            // 105.5 down to nearest 0.3 = 105.3

            //if no rounto then just pass original number back
            if (roundto == 0)
            {
                return passednumber;
            }
            else
            {
                return Math.Floor(passednumber / roundto) * roundto;
            }
        }

        public List<Ligamagic> ReadyTable(IReadOnlyCollection<IWebElement> TableElement)
        {
            List<Ligamagic> lstLigamagic = new List<Ligamagic>();

            foreach (IWebElement row in TableElement)
            {
                Ligamagic oLigamagic = new Ligamagic();
                oLigamagic.sNameStore = row.FindElement(By.TagName("img")).GetAttribute("title");
                oLigamagic.sNameSetPT = row.FindElement(By.ClassName("ed-simb")).GetAttribute("href");
                oLigamagic.sCardsQuality = row.FindElement(By.ClassName("e-col4 e-col4-offmktplace")).Text.Trim();
                try
                {
                    oLigamagic.iStockQuantity = 4;//Convert.ToInt32(row.FindElement(By.ClassName("e-col5 e-col5-offmktplace")).Text.Replace(",", "").Split()[0]);
                }
                catch { continue; }
                try
                {
                    oLigamagic.dPrice = Convert.ToDecimal(row.FindElement(By.ClassName("mob-preco-desconto")).Text.Split()[1]);
                }
                catch { continue; }

                lstLigamagic.Add(oLigamagic);
            }
            return lstLigamagic;
        }


        public void GetLigaMagicData(IWebDriver oEdgeDriver, IList<Ncards> lstNcards)
        {
            try
            {

                foreach (var Ncard in lstNcards)
                {
                    if (Ncard.Nname.Contains("Token"))
                        continue;

                    List<Ligamagic> lstLigamagic = new List<Ligamagic>(); // List Ligamagic record
                    try
                    {
                        oEdgeDriver.Navigate().GoToUrl(Ligamagic.URLSearch + Ncard.Nname);
                    }
                    catch
                    {
                        try
                        {
                            oEdgeDriver.Navigate().GoToUrl(Ligamagic.URLSearch + Ncard.Nname_PT);
                        }
                        catch { continue; }
                    }

                    IReadOnlyCollection<IWebElement> PriceRows;
                    try
                    {
                        PriceRows = oEdgeDriver.FindElements(By.XPath("//div[contains(@id,\"line_e\")]"));
                    }
                    catch
                    {
                        Console.WriteLine(Ncard.Nid + Ncard.Nname + "/" + Ncard.Nname_PT);
                        continue;
                    }
                    List<Ligamagic> lstSeleniumRecord = ReadyTable(PriceRows);

                    //get better price of Ligamagic
                    if (lstSeleniumRecord.Count() > 0)
                    {
                        decimal dBetterPrice = GetBetterPrice(lstSeleniumRecord, Ncard);

                        Product oProduct = _productService.GetProductBySku(Ncard.Nid.ToString());
                        if (oProduct.Price != dBetterPrice)
                        {
                            Ncard.Npricing_low = Ncard.Npricing_mid = Ncard.Npricing_high = dBetterPrice.ToString();
                            oProduct.Price = dBetterPrice;
                            _ncardsRepository.UpdateProduct(Ncard);
                            _productService.UpdateProduct(oProduct);
                        }
                    }
                }
            }
            catch (System.UnauthorizedAccessException erro)
            {

            }

        }


        #endregion
    }

}

