using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Selenium.Domain
{
    public class Ligamagic
    {
        public string sNameStore;
        public string sNameSetPT;
        public decimal dPrice { get; set; }
        public string sCardsQuality;
        public int iStockQuantity;

        readonly static public string URLSearch = "https://www.ligamagic.com.br/?view=cards/card&card=";
        static public string URLAuction
        {
            get
            {
                URLAuction = "https://www.ligamagic.com.br/?view=leilao%2Flistar&btSubmit=btSubmit&vbuscar=";
                return URLAuction;
            }
            set { }
        }
         
        static public string[] StoreRef = new string[] {
            "Elder Dragon",
            "Kinoene Cards",
            "Cards Of Paradise",
            "Nerdz",
            "Bazar de Bagdá",
            "POWERIX",
            "Epic Game",
            "Lets Collect",
            "TCGeeK",
            "City Class Games"
        };
    }
}
