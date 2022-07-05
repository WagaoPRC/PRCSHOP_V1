
using System.IO;
using Nop.Core.Domain.Catalog;
using System.Collections.Generic;

namespace Nop.Services.ExportImport
{
    /// <summary>
    /// Import manager interface
    /// </summary>
    public partial interface IImportManager
    {
        /// <summary>
        /// Import products from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        void ImportProductsFromXlsx(Stream stream);

        /// <summary>
        /// Import newsletter subscribers from TXT file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Number of imported subscribers</returns>
        int ImportNewsletterSubscribersFromTxt(Stream stream);

        /// <summary>
        /// Import states from TXT file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Number of imported states</returns>
        int ImportStatesFromTxt(Stream stream);

        /// <summary>
        /// Make match in Product and Ncards then Update 
        /// </summary>
        void ImportProductsFromNcardsTable();

        void ImportNsetsFromTable();
    }
}
