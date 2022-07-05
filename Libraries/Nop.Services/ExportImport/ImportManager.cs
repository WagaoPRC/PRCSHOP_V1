using System;
using System.IO;
using System.Linq;
using System.Web;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Messages;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Seo;
using OfficeOpenXml;
using System.Collections.Generic;
using Nop.Services.Localization;
using Nop.Core.Domain.Localization;
using Nop.Core.Data;
using System.Text;
using System.Globalization;

namespace Nop.Services.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager : IImportManager
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreContext _storeContext;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly INcardsService _ncardsService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly INsetsService _nsetsService;

        #endregion

        #region Ctor

        public ImportManager(IProductService productService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IPictureService pictureService,
            IUrlRecordService urlRecordService,
            IStoreContext storeContext,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService
            , INcardsService ncardsService
            , ILocalizedEntityService localizedEntityService
            , INsetsService nsetsService)
        {
            _nsetsService = nsetsService;
            _ncardsService = ncardsService;
            _localizedEntityService = localizedEntityService;
            this._productService = productService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._pictureService = pictureService;
            this._urlRecordService = urlRecordService;
            this._storeContext = storeContext;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
        }

        #endregion

        #region Utilities
        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }

        protected virtual string ConvertColumnToString(object columnValue)
        {
            if (columnValue == null)
                return null;

            return Convert.ToString(columnValue);
        }

        protected virtual string GetMimeTypeFromFilePath(string filePath)
        {
            var mimeType = MimeMapping.GetMimeMapping(filePath);

            //little hack here because MimeMapping does not contain all mappings (e.g. PNG)
            if (mimeType == "application/octet-stream")
                mimeType = "image/jpeg";

            return mimeType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Import products from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        public virtual void ImportProductsFromXlsx(Stream stream)
        {
            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // get the first worksheet in the workbook
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new NopException("No worksheet found");

                //the columns
                var properties = new[]
                {
                    "ProductTypeId",
                    "ParentGroupedProductId",
                    "VisibleIndividually",
                    "Name",
                    "ShortDescription",
                    "FullDescription",
                    "VendorId",
                    "ProductTemplateId",
                    "ShowOnHomePage",
                    "MetaKeywords",
                    "MetaDescription",
                    "MetaTitle",
                    "SeName",
                    "AllowCustomerReviews",
                    "Published",
                    "SKU",
                    "ManufacturerPartNumber",
                    "Gtin",
                    "IsGiftCard",
                    "GiftCardTypeId",
                    "OverriddenGiftCardAmount",
                    "RequireOtherProducts",
                    "RequiredProductIds",
                    "AutomaticallyAddRequiredProducts",
                    "IsDownload",
                    "DownloadId",
                    "UnlimitedDownloads",
                    "MaxNumberOfDownloads",
                    "DownloadActivationTypeId",
                    "HasSampleDownload",
                    "SampleDownloadId",
                    "HasUserAgreement",
                    "UserAgreementText",
                    "IsRecurring",
                    "RecurringCycleLength",
                    "RecurringCyclePeriodId",
                    "RecurringTotalCycles",
                    "IsRental",
                    "RentalPriceLength",
                    "RentalPricePeriodId",
                    "IsShipEnabled",
                    "IsFreeShipping",
                    "ShipSeparately",
                    "AdditionalShippingCharge",
                    "DeliveryDateId",
                    "IsTaxExempt",
                    "TaxCategoryId",
                    "IsTelecommunicationsOrBroadcastingOrElectronicServices",
                    "ManageInventoryMethodId",
                    "UseMultipleWarehouses",
                    "WarehouseId",
                    "StockQuantity",
                    "DisplayStockAvailability",
                    "DisplayStockQuantity",
                    "MinStockQuantity",
                    "LowStockActivityId",
                    "NotifyAdminForQuantityBelow",
                    "BackorderModeId",
                    "AllowBackInStockSubscriptions",
                    "OrderMinimumQuantity",
                    "OrderMaximumQuantity",
                    "AllowedQuantities",
                    "AllowAddingOnlyExistingAttributeCombinations",
                    "DisableBuyButton",
                    "DisableWishlistButton",
                    "AvailableForPreOrder",
                    "PreOrderAvailabilityStartDateTimeUtc",
                    "CallForPrice",
                    "Price",
                    "OldPrice",
                    "ProductCost",
                    "SpecialPrice",
                    "SpecialPriceStartDateTimeUtc",
                    "SpecialPriceEndDateTimeUtc",
                    "CustomerEntersPrice",
                    "MinimumCustomerEnteredPrice",
                    "MaximumCustomerEnteredPrice",
                    "BasepriceEnabled",
                    "BasepriceAmount",
                    "BasepriceUnitId",
                    "BasepriceBaseAmount",
                    "BasepriceBaseUnitId",
                    "MarkAsNew",
                    "MarkAsNewStartDateTimeUtc",
                    "MarkAsNewEndDateTimeUtc",
                    "Weight",
                    "Length",
                    "Width",
                    "Height",
                    "CreatedOnUtc",
                    "CategoryIds",
                    "ManufacturerIds",
                    "Picture1",
                    "Picture2",
                    "Picture3"
                };


                int iRow = 2;
                while (true)
                {
                    bool allColumnsAreEmpty = true;
                    for (var i = 1; i <= properties.Length; i++)
                        if (worksheet.Cells[iRow, i].Value != null && !String.IsNullOrEmpty(worksheet.Cells[iRow, i].Value.ToString()))
                        {
                            allColumnsAreEmpty = false;
                            break;
                        }
                    if (allColumnsAreEmpty)
                        break;

                    int productTypeId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "ProductTypeId")].Value);
                    int parentGroupedProductId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "ParentGroupedProductId")].Value);
                    bool visibleIndividually = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "VisibleIndividually")].Value);
                    string name = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "Name")].Value);
                    string shortDescription = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "ShortDescription")].Value);
                    string fullDescription = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "FullDescription")].Value);
                    int vendorId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "VendorId")].Value);
                    int productTemplateId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "ProductTemplateId")].Value);
                    bool showOnHomePage = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "ShowOnHomePage")].Value);
                    string metaKeywords = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "MetaKeywords")].Value);
                    string metaDescription = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "MetaDescription")].Value);
                    string metaTitle = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "MetaTitle")].Value);
                    string seName = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "SeName")].Value);
                    bool allowCustomerReviews = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "AllowCustomerReviews")].Value);
                    bool published = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "Published")].Value);
                    string sku = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "SKU")].Value);
                    string manufacturerPartNumber = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "ManufacturerPartNumber")].Value);
                    string gtin = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "Gtin")].Value);
                    bool isGiftCard = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsGiftCard")].Value);
                    int giftCardTypeId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "GiftCardTypeId")].Value);
                    decimal? overriddenGiftCardAmount = null;
                    var overriddenGiftCardAmountExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "OverriddenGiftCardAmount")].Value;
                    if (overriddenGiftCardAmountExcel != null)
                        overriddenGiftCardAmount = Convert.ToDecimal(overriddenGiftCardAmountExcel);
                    bool requireOtherProducts = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "RequireOtherProducts")].Value);
                    string requiredProductIds = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "RequiredProductIds")].Value);
                    bool automaticallyAddRequiredProducts = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "AutomaticallyAddRequiredProducts")].Value);
                    bool isDownload = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsDownload")].Value);
                    int downloadId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "DownloadId")].Value);
                    bool unlimitedDownloads = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "UnlimitedDownloads")].Value);
                    int maxNumberOfDownloads = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "MaxNumberOfDownloads")].Value);
                    int downloadActivationTypeId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "DownloadActivationTypeId")].Value);
                    bool hasSampleDownload = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "HasSampleDownload")].Value);
                    int sampleDownloadId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "SampleDownloadId")].Value);
                    bool hasUserAgreement = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "HasUserAgreement")].Value);
                    string userAgreementText = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "UserAgreementText")].Value);
                    bool isRecurring = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsRecurring")].Value);
                    int recurringCycleLength = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "RecurringCycleLength")].Value);
                    int recurringCyclePeriodId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "RecurringCyclePeriodId")].Value);
                    int recurringTotalCycles = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "RecurringTotalCycles")].Value);
                    bool isRental = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsRental")].Value);
                    int rentalPriceLength = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "RentalPriceLength")].Value);
                    int rentalPricePeriodId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "RentalPricePeriodId")].Value);
                    bool isShipEnabled = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsShipEnabled")].Value);
                    bool isFreeShipping = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsFreeShipping")].Value);
                    bool shipSeparately = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "ShipSeparately")].Value);
                    decimal additionalShippingCharge = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "AdditionalShippingCharge")].Value);
                    int deliveryDateId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "DeliveryDateId")].Value);
                    bool isTaxExempt = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsTaxExempt")].Value);
                    int taxCategoryId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "TaxCategoryId")].Value);
                    bool isTelecommunicationsOrBroadcastingOrElectronicServices = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "IsTelecommunicationsOrBroadcastingOrElectronicServices")].Value);
                    int manageInventoryMethodId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "ManageInventoryMethodId")].Value);
                    bool useMultipleWarehouses = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "UseMultipleWarehouses")].Value);
                    int warehouseId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "WarehouseId")].Value);
                    int stockQuantity = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "StockQuantity")].Value);
                    bool displayStockAvailability = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "DisplayStockAvailability")].Value);
                    bool displayStockQuantity = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "DisplayStockQuantity")].Value);
                    int minStockQuantity = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "MinStockQuantity")].Value);
                    int lowStockActivityId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "LowStockActivityId")].Value);
                    int notifyAdminForQuantityBelow = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "NotifyAdminForQuantityBelow")].Value);
                    int backorderModeId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "BackorderModeId")].Value);
                    bool allowBackInStockSubscriptions = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "AllowBackInStockSubscriptions")].Value);
                    int orderMinimumQuantity = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "OrderMinimumQuantity")].Value);
                    int orderMaximumQuantity = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "OrderMaximumQuantity")].Value);
                    string allowedQuantities = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "AllowedQuantities")].Value);
                    bool allowAddingOnlyExistingAttributeCombinations = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "AllowAddingOnlyExistingAttributeCombinations")].Value);
                    bool disableBuyButton = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "DisableBuyButton")].Value);
                    bool disableWishlistButton = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "DisableWishlistButton")].Value);
                    bool availableForPreOrder = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "AvailableForPreOrder")].Value);
                    DateTime? preOrderAvailabilityStartDateTimeUtc = null;
                    var preOrderAvailabilityStartDateTimeUtcExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "PreOrderAvailabilityStartDateTimeUtc")].Value;
                    if (preOrderAvailabilityStartDateTimeUtcExcel != null)
                        preOrderAvailabilityStartDateTimeUtc = DateTime.FromOADate(Convert.ToDouble(preOrderAvailabilityStartDateTimeUtcExcel));
                    bool callForPrice = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "CallForPrice")].Value);
                    decimal price = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "Price")].Value);
                    decimal oldPrice = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "OldPrice")].Value);
                    decimal productCost = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "ProductCost")].Value);
                    decimal? specialPrice = null;
                    var specialPriceExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "SpecialPrice")].Value;
                    if (specialPriceExcel != null)
                        specialPrice = Convert.ToDecimal(specialPriceExcel);
                    DateTime? specialPriceStartDateTimeUtc = null;
                    var specialPriceStartDateTimeUtcExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "SpecialPriceStartDateTimeUtc")].Value;
                    if (specialPriceStartDateTimeUtcExcel != null)
                        specialPriceStartDateTimeUtc = DateTime.FromOADate(Convert.ToDouble(specialPriceStartDateTimeUtcExcel));
                    DateTime? specialPriceEndDateTimeUtc = null;
                    var specialPriceEndDateTimeUtcExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "SpecialPriceEndDateTimeUtc")].Value;
                    if (specialPriceEndDateTimeUtcExcel != null)
                        specialPriceEndDateTimeUtc = DateTime.FromOADate(Convert.ToDouble(specialPriceEndDateTimeUtcExcel));

                    bool customerEntersPrice = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "CustomerEntersPrice")].Value);
                    decimal minimumCustomerEnteredPrice = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "MinimumCustomerEnteredPrice")].Value);
                    decimal maximumCustomerEnteredPrice = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "MaximumCustomerEnteredPrice")].Value);
                    bool basepriceEnabled = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "BasepriceEnabled")].Value);
                    decimal basepriceAmount = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "BasepriceAmount")].Value);
                    int basepriceUnitId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "BasepriceUnitId")].Value);
                    decimal basepriceBaseAmount = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "BasepriceBaseAmount")].Value);
                    int basepriceBaseUnitId = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "BasepriceBaseUnitId")].Value);
                    bool markAsNew = Convert.ToBoolean(worksheet.Cells[iRow, GetColumnIndex(properties, "MarkAsNew")].Value);
                    DateTime? markAsNewStartDateTimeUtc = null;
                    var markAsNewStartDateTimeUtcExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "MarkAsNewStartDateTimeUtc")].Value;
                    if (markAsNewStartDateTimeUtcExcel != null)
                        markAsNewStartDateTimeUtc = DateTime.FromOADate(Convert.ToDouble(markAsNewStartDateTimeUtcExcel));
                    DateTime? markAsNewEndDateTimeUtc = null;
                    var markAsNewEndDateTimeUtcExcel = worksheet.Cells[iRow, GetColumnIndex(properties, "MarkAsNewEndDateTimeUtc")].Value;
                    if (markAsNewEndDateTimeUtcExcel != null)
                        markAsNewEndDateTimeUtc = DateTime.FromOADate(Convert.ToDouble(markAsNewEndDateTimeUtcExcel));
                    decimal weight = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "Weight")].Value);
                    decimal length = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "Length")].Value);
                    decimal width = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "Width")].Value);
                    decimal height = Convert.ToDecimal(worksheet.Cells[iRow, GetColumnIndex(properties, "Height")].Value);
                    DateTime createdOnUtc = DateTime.FromOADate(Convert.ToDouble(worksheet.Cells[iRow, GetColumnIndex(properties, "CreatedOnUtc")].Value));
                    string categoryIds = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "CategoryIds")].Value);
                    string manufacturerIds = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "ManufacturerIds")].Value);
                    string picture1 = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "Picture1")].Value);
                    string picture2 = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "Picture2")].Value);
                    string picture3 = ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(properties, "Picture3")].Value);



                    var product = _productService.GetProductBySku(sku);
                    bool newProduct = false;
                    if (product == null)
                    {
                        product = new Product();
                        newProduct = true;
                    }
                    product.ProductTypeId = productTypeId;
                    product.ParentGroupedProductId = parentGroupedProductId;
                    product.VisibleIndividually = visibleIndividually;
                    product.Name = name;
                    product.ShortDescription = shortDescription;
                    product.FullDescription = fullDescription;
                    product.VendorId = vendorId;
                    product.ProductTemplateId = productTemplateId;
                    product.ShowOnHomePage = showOnHomePage;
                    product.MetaKeywords = metaKeywords;
                    product.MetaDescription = metaDescription;
                    product.MetaTitle = metaTitle;
                    product.AllowCustomerReviews = allowCustomerReviews;
                    product.Sku = sku;
                    product.ManufacturerPartNumber = manufacturerPartNumber;
                    product.Gtin = gtin;
                    product.IsGiftCard = isGiftCard;
                    product.GiftCardTypeId = giftCardTypeId;
                    product.OverriddenGiftCardAmount = overriddenGiftCardAmount;
                    product.RequireOtherProducts = requireOtherProducts;
                    product.RequiredProductIds = requiredProductIds;
                    product.AutomaticallyAddRequiredProducts = automaticallyAddRequiredProducts;
                    product.IsDownload = isDownload;
                    product.DownloadId = downloadId;
                    product.UnlimitedDownloads = unlimitedDownloads;
                    product.MaxNumberOfDownloads = maxNumberOfDownloads;
                    product.DownloadActivationTypeId = downloadActivationTypeId;
                    product.HasSampleDownload = hasSampleDownload;
                    product.SampleDownloadId = sampleDownloadId;
                    product.HasUserAgreement = hasUserAgreement;
                    product.UserAgreementText = userAgreementText;
                    product.IsRecurring = isRecurring;
                    product.RecurringCycleLength = recurringCycleLength;
                    product.RecurringCyclePeriodId = recurringCyclePeriodId;
                    product.RecurringTotalCycles = recurringTotalCycles;
                    product.IsRental = isRental;
                    product.RentalPriceLength = rentalPriceLength;
                    product.RentalPricePeriodId = rentalPricePeriodId;
                    product.IsShipEnabled = isShipEnabled;
                    product.IsFreeShipping = isFreeShipping;
                    product.ShipSeparately = shipSeparately;
                    product.AdditionalShippingCharge = additionalShippingCharge;
                    product.DeliveryDateId = deliveryDateId;
                    product.IsTaxExempt = isTaxExempt;
                    product.TaxCategoryId = taxCategoryId;
                    product.IsTelecommunicationsOrBroadcastingOrElectronicServices = isTelecommunicationsOrBroadcastingOrElectronicServices;
                    product.ManageInventoryMethodId = manageInventoryMethodId;
                    product.UseMultipleWarehouses = useMultipleWarehouses;
                    product.WarehouseId = warehouseId;
                    product.StockQuantity = stockQuantity;
                    product.DisplayStockAvailability = displayStockAvailability;
                    product.DisplayStockQuantity = displayStockQuantity;
                    product.MinStockQuantity = minStockQuantity;
                    product.LowStockActivityId = lowStockActivityId;
                    product.NotifyAdminForQuantityBelow = notifyAdminForQuantityBelow;
                    product.BackorderModeId = backorderModeId;
                    product.AllowBackInStockSubscriptions = allowBackInStockSubscriptions;
                    product.OrderMinimumQuantity = orderMinimumQuantity;
                    product.OrderMaximumQuantity = orderMaximumQuantity;
                    product.AllowedQuantities = allowedQuantities;
                    product.AllowAddingOnlyExistingAttributeCombinations = allowAddingOnlyExistingAttributeCombinations;
                    product.DisableBuyButton = disableBuyButton;
                    product.DisableWishlistButton = disableWishlistButton;
                    product.AvailableForPreOrder = availableForPreOrder;
                    product.PreOrderAvailabilityStartDateTimeUtc = preOrderAvailabilityStartDateTimeUtc;
                    product.CallForPrice = callForPrice;
                    product.Price = price;
                    product.OldPrice = oldPrice;
                    product.ProductCost = productCost;
                    product.SpecialPrice = specialPrice;
                    product.SpecialPriceStartDateTimeUtc = specialPriceStartDateTimeUtc;
                    product.SpecialPriceEndDateTimeUtc = specialPriceEndDateTimeUtc;
                    product.CustomerEntersPrice = customerEntersPrice;
                    product.MinimumCustomerEnteredPrice = minimumCustomerEnteredPrice;
                    product.MaximumCustomerEnteredPrice = maximumCustomerEnteredPrice;
                    product.BasepriceEnabled = basepriceEnabled;
                    product.BasepriceAmount = basepriceAmount;
                    product.BasepriceUnitId = basepriceUnitId;
                    product.BasepriceBaseAmount = basepriceBaseAmount;
                    product.BasepriceBaseUnitId = basepriceBaseUnitId;
                    product.MarkAsNew = markAsNew;
                    product.MarkAsNewStartDateTimeUtc = markAsNewStartDateTimeUtc;
                    product.MarkAsNewEndDateTimeUtc = markAsNewEndDateTimeUtc;
                    product.Weight = weight;
                    product.Length = length;
                    product.Width = width;
                    product.Height = height;
                    product.Published = published;
                    product.CreatedOnUtc = createdOnUtc;
                    product.UpdatedOnUtc = DateTime.UtcNow;
                    if (newProduct)
                    {
                        _productService.InsertProduct(product);
                    }
                    else
                    {
                        _productService.UpdateProduct(product);
                    }

                    //search engine name
                    _urlRecordService.SaveSlug(product, product.ValidateSeName(seName, product.Name, true), 0);

                    //category mappings
                    if (!String.IsNullOrEmpty(categoryIds))
                    {
                        foreach (var id in categoryIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())))
                        {
                            if (product.ProductCategories.FirstOrDefault(x => x.CategoryId == id) == null)
                            {
                                //ensure that category exists
                                var category = _categoryService.GetCategoryById(id);
                                if (category != null)
                                {
                                    var productCategory = new ProductCategory
                                    {
                                        ProductId = product.Id,
                                        CategoryId = category.Id,
                                        IsFeaturedProduct = false,
                                        DisplayOrder = 1
                                    };
                                    _categoryService.InsertProductCategory(productCategory);
                                }
                            }
                        }
                    }

                    //manufacturer mappings
                    if (!String.IsNullOrEmpty(manufacturerIds))
                    {
                        foreach (var id in manufacturerIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())))
                        {
                            if (product.ProductManufacturers.FirstOrDefault(x => x.ManufacturerId == id) == null)
                            {
                                //ensure that manufacturer exists
                                var manufacturer = _manufacturerService.GetManufacturerById(id);
                                if (manufacturer != null)
                                {
                                    var productManufacturer = new ProductManufacturer
                                    {
                                        ProductId = product.Id,
                                        ManufacturerId = manufacturer.Id,
                                        IsFeaturedProduct = false,
                                        DisplayOrder = 1
                                    };
                                    _manufacturerService.InsertProductManufacturer(productManufacturer);
                                }
                            }
                        }
                    }

                    //pictures
                    foreach (var picturePath in new[] { picture1, picture2, picture3 })
                    {
                        if (String.IsNullOrEmpty(picturePath))
                            continue;

                        var mimeType = GetMimeTypeFromFilePath(picturePath);
                        var newPictureBinary = File.ReadAllBytes(picturePath);
                        var pictureAlreadyExists = false;
                        if (!newProduct)
                        {
                            //compare with existing product pictures
                            var existingPictures = _pictureService.GetPicturesByProductId(product.Id);
                            foreach (var existingPicture in existingPictures)
                            {
                                var existingBinary = _pictureService.LoadPictureBinary(existingPicture);
                                //picture binary after validation (like in database)
                                var validatedPictureBinary = _pictureService.ValidatePicture(newPictureBinary, mimeType);
                                if (existingBinary.SequenceEqual(validatedPictureBinary) || existingBinary.SequenceEqual(newPictureBinary))
                                {
                                    //the same picture content
                                    pictureAlreadyExists = true;
                                    break;
                                }
                            }
                        }

                        if (!pictureAlreadyExists)
                        {
                            var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(name));
                            product.ProductPictures.Add(new ProductPicture
                            {
                                //EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
                                //pictures are duplicated
                                //maybe because entity size is too large
                                PictureId = newPicture.Id,
                                DisplayOrder = 1,
                            });
                            _productService.UpdateProduct(product);
                        }
                    }

                    //update "HasTierPrices" and "HasDiscountsApplied" properties
                    _productService.UpdateHasTierPricesProperty(product);
                    _productService.UpdateHasDiscountsApplied(product);



                    //next product
                    iRow++;
                }
            }
        }


        /// <summary>
        /// Import newsletter subscribers from TXT file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Number of imported subscribers</returns>
        public virtual int ImportNewsletterSubscribersFromTxt(Stream stream)
        {
            int count = 0;
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (String.IsNullOrWhiteSpace(line))
                        continue;
                    string[] tmp = line.Split(',');

                    string email;
                    bool isActive = true;
                    int storeId = _storeContext.CurrentStore.Id;
                    //parse
                    if (tmp.Length == 1)
                    {
                        //"email" only
                        email = tmp[0].Trim();
                    }
                    else if (tmp.Length == 2)
                    {
                        //"email" and "active" fields specified
                        email = tmp[0].Trim();
                        isActive = Boolean.Parse(tmp[1].Trim());
                    }
                    else if (tmp.Length == 3)
                    {
                        //"email" and "active" and "storeId" fields specified
                        email = tmp[0].Trim();
                        isActive = Boolean.Parse(tmp[1].Trim());
                        storeId = Int32.Parse(tmp[2].Trim());
                    }
                    else
                        throw new NopException("Wrong file format");

                    //import
                    var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(email, storeId);
                    if (subscription != null)
                    {
                        subscription.Email = email;
                        subscription.Active = isActive;
                        _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscription);
                    }
                    else
                    {
                        subscription = new NewsLetterSubscription
                        {
                            Active = isActive,
                            CreatedOnUtc = DateTime.UtcNow,
                            Email = email,
                            StoreId = storeId,
                            NewsLetterSubscriptionGuid = Guid.NewGuid()
                        };
                        _newsLetterSubscriptionService.InsertNewsLetterSubscription(subscription);
                    }
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Import states from TXT file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Number of imported states</returns>
        public virtual int ImportStatesFromTxt(Stream stream)
        {
            int count = 0;
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (String.IsNullOrWhiteSpace(line))
                        continue;
                    string[] tmp = line.Split(',');

                    if (tmp.Length != 5)
                        throw new NopException("Wrong file format");

                    //parse
                    var countryTwoLetterIsoCode = tmp[0].Trim();
                    var name = tmp[1].Trim();
                    var abbreviation = tmp[2].Trim();
                    bool published = Boolean.Parse(tmp[3].Trim());
                    int displayOrder = Int32.Parse(tmp[4].Trim());

                    var country = _countryService.GetCountryByTwoLetterIsoCode(countryTwoLetterIsoCode);
                    if (country == null)
                    {
                        //country cannot be loaded. skip
                        continue;
                    }

                    //import
                    var states = _stateProvinceService.GetStateProvincesByCountryId(country.Id, showHidden: true);
                    var state = states.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                    if (state != null)
                    {
                        state.Abbreviation = abbreviation;
                        state.Published = published;
                        state.DisplayOrder = displayOrder;
                        _stateProvinceService.UpdateStateProvince(state);
                    }
                    else
                    {
                        state = new StateProvince
                        {
                            CountryId = country.Id,
                            Name = name,
                            Abbreviation = abbreviation,
                            Published = published,
                            DisplayOrder = displayOrder,
                        };
                        _stateProvinceService.InsertStateProvince(state);
                    }
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// import productos from table Ncards
        /// </summary>
        public void ImportProductsFromNcardsTable()
        {
            IList<Ncards> listNcards = _ncardsService.GetDiferenceNcardsProduct();

            if (listNcards != null)
            {
                foreach (var item in listNcards)
                {
                    var oProduct = _productService.GetProductBySku(Convert.ToString(item.Nid));

                    bool newProduct = false;
                    if (oProduct == null)
                    {
                        oProduct = new Product();
                        newProduct = true;
                    }
                    
                    oProduct.ProductTypeId = 5;
                    oProduct.ParentGroupedProductId = 0;
                    oProduct.VisibleIndividually = true;
                    oProduct.Name = RemoveDiacritics(item.Nname) + " (" + item.Nset + ")";

                    //If exist name in portuguese put PT-EN, else only EN
                    oProduct.Name = !string.IsNullOrEmpty(item.Nname_PT) ? item.Nname + " - " + item.Nname_PT+" ("+ item.Nset+")"  : oProduct.Name;

                    oProduct.ShortDescription = (item.Nmanacost + " (" +
                        item.Nset + ") " + item.Ntype + ".<p> "
                      + " " + item.Nability + "</p>").Replace("£", " — ");
                    oProduct.FullDescription = "<p><strong>Artist:</strong>" + item.Nartist + "</p>";

                    //If exist Nruling
                    if (!string.IsNullOrEmpty(item.Nruling))
                        foreach (var text in item.Nruling.Split('£'))
                        {
                            if (!string.IsNullOrEmpty(text))
                                oProduct.FullDescription = oProduct.FullDescription + "<p>" + text + "</p>";
                        }

                    oProduct.VendorId = 0;
                    oProduct.ProductTemplateId = 1;
                    oProduct.ShowOnHomePage = false;
                    oProduct.MetaKeywords = null;
                    oProduct.MetaDescription = null;
                    oProduct.MetaTitle = null;
                    oProduct.AllowCustomerReviews = true;
                    oProduct.Sku = Convert.ToString(item.Nid);
                    oProduct.ManufacturerPartNumber = null;
                    oProduct.Gtin = null;
                    oProduct.IsGiftCard = false;
                    oProduct.GiftCardTypeId = 0;
                    oProduct.OverriddenGiftCardAmount = null;
                    oProduct.RequireOtherProducts = false;
                    oProduct.RequiredProductIds = null;
                    oProduct.AutomaticallyAddRequiredProducts = false;
                    oProduct.IsDownload = false;
                    oProduct.DownloadId = 0;
                    oProduct.UnlimitedDownloads = false;
                    oProduct.MaxNumberOfDownloads = 0;
                    oProduct.DownloadActivationTypeId = 0;
                    oProduct.HasSampleDownload = false;
                    oProduct.SampleDownloadId = 0;
                    oProduct.HasUserAgreement = false;
                    oProduct.UserAgreementText = null;
                    oProduct.IsRecurring = false;
                    oProduct.RecurringCycleLength = 0;
                    oProduct.RecurringCyclePeriodId = 0;
                    oProduct.RecurringTotalCycles = 0;
                    oProduct.IsRental = false;
                    oProduct.RentalPriceLength = 0;
                    oProduct.RentalPricePeriodId = 0;
                    oProduct.IsShipEnabled = true;
                    oProduct.IsFreeShipping = false;
                    oProduct.ShipSeparately = false;
                    oProduct.AdditionalShippingCharge = 0;
                    oProduct.DeliveryDateId = 0;
                    oProduct.IsTaxExempt = false;
                    oProduct.TaxCategoryId = 0;
                    oProduct.IsTelecommunicationsOrBroadcastingOrElectronicServices = false;
                    oProduct.ManageInventoryMethodId = 1;
                    oProduct.UseMultipleWarehouses = false;
                    oProduct.WarehouseId = 0;
                    oProduct.DisplayStockAvailability = true;
                    oProduct.DisplayStockQuantity = true;
                    oProduct.MinStockQuantity = 0;
                    oProduct.LowStockActivityId = 0;
                    oProduct.NotifyAdminForQuantityBelow = 0;
                    oProduct.BackorderModeId = 0;
                    oProduct.AllowBackInStockSubscriptions = true;
                    oProduct.OrderMinimumQuantity = 1;
                    oProduct.OrderMaximumQuantity = 4;
                    oProduct.AllowedQuantities = null;
                    oProduct.AllowAddingOnlyExistingAttributeCombinations = false;
                    oProduct.DisableBuyButton = false;
                    oProduct.DisableWishlistButton = false;
                    oProduct.AvailableForPreOrder = false;
                    oProduct.PreOrderAvailabilityStartDateTimeUtc = null;
                    oProduct.CallForPrice = false;
                    oProduct.SpecialPrice = null;
                    oProduct.SpecialPriceStartDateTimeUtc = null;
                    oProduct.SpecialPriceEndDateTimeUtc = null;
                    oProduct.CustomerEntersPrice = false;
                    oProduct.MinimumCustomerEnteredPrice = 0;
                    oProduct.MaximumCustomerEnteredPrice = 0;
                    oProduct.BasepriceEnabled = false;
                    oProduct.BasepriceAmount = 0;
                    oProduct.BasepriceUnitId = 1;
                    oProduct.BasepriceBaseAmount = 0;
                    oProduct.BasepriceBaseUnitId = 1;
                    oProduct.MarkAsNew = false;
                    oProduct.MarkAsNewStartDateTimeUtc = null;
                    oProduct.MarkAsNewEndDateTimeUtc = null;
                    oProduct.Weight = 6;
                    oProduct.Length = 6;
                    oProduct.Width = 4;
                    oProduct.Height = 6;
                    oProduct.Published = true;
                    oProduct.CreatedOnUtc = DateTime.UtcNow;
                    oProduct.UpdatedOnUtc = DateTime.UtcNow;

                    if (newProduct)
                    {
                        _productService.InsertProduct(oProduct);

                        //search engine name
                        _urlRecordService.SaveSlug(oProduct, oProduct.ValidateSeName(oProduct.GetSeName(), oProduct.Name, true), 0);

                        //Categoria MTG
                        int iCategory = 17;

                        //ensure that category exists
                        Category oCategory = _categoryService.GetCategoryById(iCategory);
                        if (oProduct.ProductCategories.FirstOrDefault() == null)
                            if (oCategory != null)
                            {
                                var productCategory = new ProductCategory
                                {
                                    ProductId = oProduct.Id,
                                    CategoryId = oCategory.Id,
                                    IsFeaturedProduct = false,
                                    DisplayOrder = 1
                                };
                                _categoryService.InsertProductCategory(productCategory);
                            }
                    }
                    else
                    {
                        //UPDATE product
                        _productService.UpdateProduct(oProduct);
                    }
                    //Picture
                    string root = @"E:\MTG\pics";
                    if (System.IO.Directory.Exists(root))
                    {
                        string sNname = item.Nname + ".full.jpg";
                        string sNset = item.Nset;
                        string picturePath = Path.Combine(root, sNset, sNname);
                        //pictures

                        if (!File.Exists(picturePath))
                        {
                            picturePath = Path.Combine(root, sNset, item.Nid+ ".jpg");
                        }
                        
                        var mimeType = GetMimeTypeFromFilePath(picturePath);
                        var newPictureBinary = File.ReadAllBytes(picturePath);
                        var pictureAlreadyExists = false;

                        //compare with existing product pictures
                        var existingPictures = _pictureService.GetPicturesByProductId(oProduct.Id);
                        foreach (var existingPicture in existingPictures)
                        {
                            var existingBinary = _pictureService.LoadPictureBinary(existingPicture);
                            //picture binary after validation (like in database)
                            var validatedPictureBinary = _pictureService.ValidatePicture(newPictureBinary, mimeType);
                            if (existingBinary.SequenceEqual(validatedPictureBinary) || existingBinary.SequenceEqual(newPictureBinary))
                            {
                                //the same picture content
                                pictureAlreadyExists = true;
                                break;
                            }
                        }

                        if (!pictureAlreadyExists)
                        {
                            var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(oProduct.Name));
                            newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(item.Nname));
                            oProduct.ProductPictures.Add(new ProductPicture
                            {
                                //EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
                                //pictures are duplicated
                                //maybe because entity size is too large
                                PictureId = newPicture.Id,
                                DisplayOrder = 1,
                            });
                            _productService.UpdateProduct(oProduct);
                        }

                    }
                    //update "HasTierPrices" and "HasDiscountsApplied" properties
                    _productService.UpdateHasTierPricesProperty(oProduct);
                    _productService.UpdateHasDiscountsApplied(oProduct);

                    if (!string.IsNullOrEmpty(item.Nability_PT))
                    {
                        oProduct = _productService.GetProductBySku(oProduct.Sku);
                        if (!string.IsNullOrEmpty(item.Nname_PT) && oProduct != null)
                        {
                            _localizedEntityService.SaveLocalizedValue(oProduct,
                                                                   x => x.ShortDescription,
                                                                   (item.Nmanacost + " (" + item.Nset + ") " + item.Ntype_PT + ".<p> " + " " + item.Nability_PT + "</p>").Replace("£", " — "),
                                                                   2);

                        }
                    }
                }
            }
        }

        public void ImportNsetsFromTable()
        {
            IList<Nsets> listNsets = _nsetsService.GetAllNsets();
            int i = 10;
            foreach (var item in listNsets)
            {
                Category oCategory = _categoryService.GetCategoryByName(item.Nname) != null ?
                    _categoryService.GetCategoryByName(item.Nname) : new Category();
                oCategory.Name = item.Nname;
                oCategory.Description = oCategory.Description;
                oCategory.CategoryTemplateId = 1;
                oCategory.MetaKeywords = null;
                oCategory.MetaDescription = null;
                oCategory.MetaTitle = null;
                oCategory.ParentCategoryId = 17;
                oCategory.PictureId = oCategory.PictureId;
                oCategory.PageSize = 6;
                oCategory.AllowCustomersToSelectPageSize = true;
                oCategory.PageSizeOptions = "18, 54, 9, 6";
                oCategory.PriceRanges = null;
                oCategory.ShowOnHomePage = false;
                oCategory.IncludeInTopMenu = false;
                oCategory.SubjectToAcl = false;
                oCategory.LimitedToStores = false;
                oCategory.Published = oCategory.Published;
                oCategory.Deleted = oCategory.Deleted;
                oCategory.DisplayOrder = i++;
                oCategory.CreatedOnUtc = item.Ndate;
                oCategory.UpdatedOnUtc = item.Ndate;

                if (_categoryService.GetCategoryByName(item.Nname) != null ? true : false)
                {
                    //_categoryService.UpdateCategory(oCategory);
                }
                else
                {
                    _categoryService.InsertCategory(oCategory);
                    oCategory.Published = true;
                }
            }
        }
        static string RemoveDiacritics(string text)
        {
            if (!text.Contains("/"))
            {
                var normalizedString = text.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();

                foreach (var c in normalizedString)
                {
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            }
            else
            {
                text = text.Replace("//", "").Trim();
                text = text.Replace(" ", string.Empty);
                return text;
            }

        }
    }

    #endregion
}


