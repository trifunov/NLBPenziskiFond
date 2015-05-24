using Newtonsoft.Json;
using NLBPenziskiFond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace NLBPenziskiFond.Controllers
{
    [Authorize]
    public class BazaController : Controller
    {
        private NLBEntities context = new NLBEntities();
        // GET: Baza
        public ActionResult Index()
        {
            var model = new List<RecordAssetsViewModel>();

            var listTypes = new List<string>();

            var listCurrencies = new List<string>();

            var listCountries = new List<string>();
            
            var listSectors = new List<string>();

            var dateFromDateTo = new List<YieldsViewModel>();

            var typesNames = new List<YieldsViewModel>();

            var countriesNames = new List<YieldsViewModel>();

            var currenciesNames = new List<YieldsViewModel>();

            var sectorsNames = new List<YieldsViewModel>();

            var listIsins = (
                from instrument in this.context.Instruments
                select new YieldsViewModel()
                {
                    Isin = instrument.Isin
                }
                ).Distinct().ToList();

            foreach(var isinItem in listIsins)
            {
                var isin = isinItem.Isin;
                
                var listDates = (
                    from instrument in this.context.Instruments
                    where instrument.Isin == isinItem.Isin
                    select new YieldsViewModel()
                    {
                        Datum = instrument.Date
                    }
                    ).OrderBy(x => x.Datum).ToList();

                var dateFrom = listDates.First();

                var dateTo = listDates.Last();

                dateFromDateTo.Add(new YieldsViewModel()
                    {
                        Isin = isinItem.Isin,
                        DatumOd = dateFrom.Datum.ToString(),
                        DatumDo = dateTo.Datum.ToString()
                    });
            }

            var listRecordAssets = (
                from recordAsset in this.context.Record_Assets
                select new RecordAssetsViewModel()
                {
                    Isin = recordAsset.Isin,
                    Type = recordAsset.Type,
                    Currency = recordAsset.Currency,
                    Entry_Date = recordAsset.Entry_Date,
                    Full_Name = recordAsset.Full_Name,
                    Sector = recordAsset.Sector,
                    Country = recordAsset.Country
                }
                ).Distinct().ToList();

            var IsinFullName = (
                from recordAsset in this.context.Instruments
                select new RecordAssetsViewModel()
                {
                    Isin = recordAsset.Isin,
                    Full_Name = recordAsset.Full_Name
                }
                ).Distinct().ToList();

            var Types = (
                from recordAsset in this.context.Instruments
                select new RecordAssetsViewModel()
                {
                    Type = recordAsset.Type
                }
                ).Distinct().ToList();

            var Currencies = (
                from recordAsset in this.context.Instruments
                select new RecordAssetsViewModel()
                {
                    Currency = recordAsset.Currency
                }
                ).Distinct().ToList();

            var Sectors = (
                from recordAsset in this.context.Instruments
                select new RecordAssetsViewModel()
                {
                    Sector = recordAsset.Sector
                }
                ).Distinct().ToList();

            var Countries = (
                from recordAsset in this.context.Instruments
                select new RecordAssetsViewModel()
                {
                    Country = recordAsset.Country
                }
                ).Distinct().ToList();

            foreach(var typeItem in Types)
            {
                var listFullNames = (
                    from instrument in this.context.Instruments
                    where instrument.Type == typeItem.Type
                    select new YieldsViewModel()
                    {
                        Ime = instrument.Full_Name
                    }
                    ).Distinct().ToList();

                typesNames.Add(new YieldsViewModel()
                    {
                        Tip = typeItem.Type,
                        listIminja = listFullNames
                    });
            }

            foreach (var countryItem in Countries)
            {
                var listFullNames = (
                    from instrument in this.context.Instruments
                    where instrument.Country == countryItem.Country
                    select new YieldsViewModel()
                    {
                        Ime = instrument.Full_Name
                    }
                    ).Distinct().ToList();

                countriesNames.Add(new YieldsViewModel()
                {
                    Drzhava = countryItem.Country,
                    listIminja = listFullNames
                });
            }

            foreach (var currencyItem in Currencies)
            {
                var listFullNames = (
                    from instrument in this.context.Instruments
                    where instrument.Currency == currencyItem.Currency
                    select new YieldsViewModel()
                    {
                        Ime = instrument.Full_Name
                    }
                    ).Distinct().ToList();

                currenciesNames.Add(new YieldsViewModel()
                {
                    Valuta = currencyItem.Currency,
                    listIminja = listFullNames
                });
            }

            foreach (var sectorItem in Sectors)
            {
                var listFullNames = (
                    from instrument in this.context.Instruments
                    where instrument.Sector == sectorItem.Sector
                    select new YieldsViewModel()
                    {
                        Ime = instrument.Full_Name
                    }
                    ).Distinct().ToList();

                sectorsNames.Add(new YieldsViewModel()
                {
                    Sektor = sectorItem.Sector,
                    listIminja = listFullNames
                });
            }

            ViewBag.listTypes = Types;
            ViewBag.listCurrencies = Currencies;
            ViewBag.listSectors = Sectors;
            ViewBag.listCountries = Countries;
            ViewBag.listIsinFullName = IsinFullName;
            ViewBag.listDateFromDateTo = dateFromDateTo;
            ViewBag.listTypesName = typesNames;
            ViewBag.listCountriesName = countriesNames;
            ViewBag.listCurrenciesName = currenciesNames;
            ViewBag.listSectorsName = sectorsNames;

            foreach(var recordAssetsItem in listRecordAssets)
            {
                model.Add(new RecordAssetsViewModel()
                {
                    Isin = recordAssetsItem.Isin,
                    Type = recordAssetsItem.Type,
                    Currency = recordAssetsItem.Currency,
                    Entry_Date = recordAssetsItem.Entry_Date,
                    Full_Name = recordAssetsItem.Full_Name,
                    Sector = recordAssetsItem.Sector,
                    Country = recordAssetsItem.Country
                });
            }

            //foreach (var modelItemHartija in modelListHartijaIme)
            //{
            //    foreach (var modelItemBaza in modelListBaza)
            //    {
            //        if (modelItemBaza.Isin == modelItemHartija.ISIN && modelItemHartija.Izdavatelj != null)
            //        {
            //            if (!listSektor.Contains(modelItemHartija.Polje_30))
            //            {
            //                model.Add(new DnevniPrinosiViewModel()
            //                {
            //                    Id_Evid = modelItemBaza.Id_Evid,
            //                    Izdavatelj = modelItemHartija.Izdavatelj,
            //                    Sektor = modelItemHartija.Polje_30
            //                });
            //                listSektor.Add(modelItemHartija.Polje_30);
            //            }
            //            else
            //            {
            //                model.Add(new DnevniPrinosiViewModel()
            //                {
            //                    Id_Evid = modelItemBaza.Id_Evid,
            //                    Izdavatelj = modelItemHartija.Izdavatelj
            //                });
            //            }
            //        }
            //    }
            //}
            return View(model.ToList());
        }

        [GridAction]
        public ActionResult GetBaza(string dateFrom, string dateTo, string isin, string nacinNaPrinos, string type, string country, string currency, string sector)
        {
            if (dateTo != null && dateFrom != null)
            {
                if (nacinNaPrinos == null)
                {
                    if (type != "-----" && isin == "-----")
                    {
                        var dateToDate = Convert.ToDateTime(dateTo);
                        var dateFromDate = Convert.ToDateTime(dateFrom);
                        var modelListType = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin,
                                Datum = instrument.Date,
                                Valuta = instrument.Currency,
                                Uchestvo = instrument.Share / 100,
                                Tip = instrument.Type,
                                Vkupen = instrument.Total_Yield / 100,
                                VkupenOrg = instrument.Total_Yield_Currency / 100,
                                Cenoven = instrument.Price_Yield / 100,
                                CenovenOrg = instrument.Price_Yield_Currency / 100,
                                Dividen = instrument.Dividend_Yield / 100,
                                DividenOrg = instrument.Dividend_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                            select new YieldsViewModel()
                            {
                                Datum = instrument.Date
                            }
                            ).Distinct().ToList();

                        var modelListIsins = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin
                            }
                            ).Distinct().ToList();

                        ViewBag.modelListIsins = modelListIsins;

                        var modelProduction = new List<YieldsViewModel>();
                        decimal? productionTotal = 0;
                        decimal? productionPrice = 0;
                        decimal? productionDividend = 0;
                        decimal? productionTotalCurrency = 0;
                        decimal? productionPriceCurrency = 0;
                        decimal? productionDividendCurrency = 0;
                        decimal? sumShares = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            productionTotal = 0;
                            productionPrice = 0;
                            productionDividend = 0;
                            productionTotalCurrency = 0;
                            productionPriceCurrency = 0;
                            productionDividendCurrency = 0;
                            sumShares = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if(modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                {
                                    productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                    productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                    productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                    productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                    productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                    productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                    sumShares += modelItemType.Uchestvo;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Datum = modelItemDate.Datum,
                                ProductionTotal = productionTotal,
                                ProductionPrice = productionPrice,
                                ProductionDividend = productionDividend,
                                ProductionTotalCurrency = productionTotalCurrency,
                                ProductionPriceCurrency = productionPriceCurrency,
                                ProductionDividendCurrency = productionDividendCurrency,
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal productTotal = 1;
                        decimal productPrice = 1;
                        decimal productDividend = 1;
                        decimal productTotalCurrency = 1;
                        decimal productPriceCurrency = 1;
                        decimal productDividendCurrency = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yieldTotal = modelItem.ProductionTotal.Value;
                            var yieldPrice = modelItem.ProductionPrice.Value;
                            var yieldDividend = modelItem.ProductionDividend.Value;
                            var yieldTotalCurrency = modelItem.ProductionTotalCurrency.Value;
                            var yieldPriceCurrency = modelItem.ProductionPriceCurrency.Value;
                            var yieldDividendCurrency = modelItem.ProductionDividendCurrency.Value;
                            productTotal *= (yieldTotal + 1);
                            productPrice *= (yieldPrice + 1);
                            productDividend *= (yieldDividend + 1);
                            productTotalCurrency *= (yieldTotalCurrency + 1);
                            productPriceCurrency *= (yieldPriceCurrency + 1);
                            productDividendCurrency *= (yieldDividendCurrency + 1);
                        }
                        productTotal = productTotal - 1;
                        productPrice = productPrice - 1;
                        productDividend = productDividend - 1;
                        productTotalCurrency = productTotalCurrency - 1;
                        productPriceCurrency = productPriceCurrency - 1;
                        productDividendCurrency = productDividendCurrency - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DatumOd = dateFromDate.ToString(),
                            DatumDo = dateToDate.ToString(),
                            Vkupen = Math.Round(productTotal * 100, 2),
                            VkupenOrg = Math.Round(productTotalCurrency * 100, 2),
                            Cenoven = Math.Round(productPrice * 100, 2),
                            CenovenOrg = Math.Round(productPriceCurrency * 100, 2),
                            Dividen = Math.Round(productDividend * 100, 2),
                            DividenOrg = Math.Round(productDividendCurrency * 100, 2),
                            Uchestvo = Math.Round(sumShares.Value * 100, 2)
                        });
                        return View(new GridModel(model));
                    }

                    else if (country != "-----" && isin == "-----")
                    {
                        var dateToDate = Convert.ToDateTime(dateTo);
                        var dateFromDate = Convert.ToDateTime(dateFrom);
                        var modelListType = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin,
                                Datum = instrument.Date,
                                Valuta = instrument.Currency,
                                Uchestvo = instrument.Share / 100,
                                Tip = instrument.Type,
                                Vkupen = instrument.Total_Yield / 100,
                                VkupenOrg = instrument.Total_Yield_Currency / 100,
                                Cenoven = instrument.Price_Yield / 100,
                                CenovenOrg = instrument.Price_Yield_Currency / 100,
                                Dividen = instrument.Dividend_Yield / 100,
                                DividenOrg = instrument.Dividend_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                            select new YieldsViewModel()
                            {
                                Datum = instrument.Date
                            }
                            ).Distinct().ToList();

                        var modelListIsins = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin
                            }
                            ).Distinct().ToList();

                        ViewBag.modelListIsins = modelListIsins;

                        var modelProduction = new List<YieldsViewModel>();
                        decimal? productionTotal = 0;
                        decimal? productionPrice = 0;
                        decimal? productionDividend = 0;
                        decimal? productionTotalCurrency = 0;
                        decimal? productionPriceCurrency = 0;
                        decimal? productionDividendCurrency = 0;
                        decimal? sumShares = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            productionTotal = 0;
                            productionPrice = 0;
                            productionDividend = 0;
                            productionTotalCurrency = 0;
                            productionPriceCurrency = 0;
                            productionDividendCurrency = 0;
                            sumShares = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                {
                                    productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                    productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                    productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                    productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                    productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                    productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                    sumShares += modelItemType.Uchestvo;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Datum = modelItemDate.Datum,
                                ProductionTotal = productionTotal,
                                ProductionPrice = productionPrice,
                                ProductionDividend = productionDividend,
                                ProductionTotalCurrency = productionTotalCurrency,
                                ProductionPriceCurrency = productionPriceCurrency,
                                ProductionDividendCurrency = productionDividendCurrency,
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal productTotal = 1;
                        decimal productPrice = 1;
                        decimal productDividend = 1;
                        decimal productTotalCurrency = 1;
                        decimal productPriceCurrency = 1;
                        decimal productDividendCurrency = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yieldTotal = modelItem.ProductionTotal.Value;
                            var yieldPrice = modelItem.ProductionPrice.Value;
                            var yieldDividend = modelItem.ProductionDividend.Value;
                            var yieldTotalCurrency = modelItem.ProductionTotalCurrency.Value;
                            var yieldPriceCurrency = modelItem.ProductionPriceCurrency.Value;
                            var yieldDividendCurrency = modelItem.ProductionDividendCurrency.Value;
                            productTotal *= (yieldTotal + 1);
                            productPrice *= (yieldPrice + 1);
                            productDividend *= (yieldDividend + 1);
                            productTotalCurrency *= (yieldTotalCurrency + 1);
                            productPriceCurrency *= (yieldPriceCurrency + 1);
                            productDividendCurrency *= (yieldDividendCurrency + 1);
                        }
                        productTotal = productTotal - 1;
                        productPrice = productPrice - 1;
                        productDividend = productDividend - 1;
                        productTotalCurrency = productTotalCurrency - 1;
                        productPriceCurrency = productPriceCurrency - 1;
                        productDividendCurrency = productDividendCurrency - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DatumOd = dateFromDate.ToString(),
                            DatumDo = dateToDate.ToString(),
                            Vkupen = Math.Round(productTotal * 100, 2),
                            VkupenOrg = Math.Round(productTotalCurrency * 100, 2),
                            Cenoven = Math.Round(productPrice * 100, 2),
                            CenovenOrg = Math.Round(productPriceCurrency * 100, 2),
                            Dividen = Math.Round(productDividend * 100, 2),
                            DividenOrg = Math.Round(productDividendCurrency * 100, 2),
                            Uchestvo = Math.Round(sumShares.Value * 100, 2)
                        });
                        return View(new GridModel(model));
                    }

                    else if (currency != "-----" && isin == "-----")
                    {
                        var dateToDate = Convert.ToDateTime(dateTo);
                        var dateFromDate = Convert.ToDateTime(dateFrom);
                        var modelListType = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin,
                                Datum = instrument.Date,
                                Valuta = instrument.Currency,
                                Uchestvo = instrument.Share / 100,
                                Tip = instrument.Type,
                                Vkupen = instrument.Total_Yield / 100,
                                VkupenOrg = instrument.Total_Yield_Currency / 100,
                                Cenoven = instrument.Price_Yield / 100,
                                CenovenOrg = instrument.Price_Yield_Currency / 100,
                                Dividen = instrument.Dividend_Yield / 100,
                                DividenOrg = instrument.Dividend_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                            select new YieldsViewModel()
                            {
                                Datum = instrument.Date
                            }
                            ).Distinct().ToList();

                        var modelListIsins = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin
                            }
                            ).Distinct().ToList();

                        ViewBag.modelListIsins = modelListIsins;

                        var modelProduction = new List<YieldsViewModel>();
                        decimal? productionTotal = 0;
                        decimal? productionPrice = 0;
                        decimal? productionDividend = 0;
                        decimal? productionTotalCurrency = 0;
                        decimal? productionPriceCurrency = 0;
                        decimal? productionDividendCurrency = 0;
                        decimal? sumShares = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            productionTotal = 0;
                            productionPrice = 0;
                            productionDividend = 0;
                            productionTotalCurrency = 0;
                            productionPriceCurrency = 0;
                            productionDividendCurrency = 0;
                            sumShares = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                {
                                    productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                    productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                    productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                    productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                    productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                    productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                    sumShares += modelItemType.Uchestvo;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Datum = modelItemDate.Datum,
                                ProductionTotal = productionTotal,
                                ProductionPrice = productionPrice,
                                ProductionDividend = productionDividend,
                                ProductionTotalCurrency = productionTotalCurrency,
                                ProductionPriceCurrency = productionPriceCurrency,
                                ProductionDividendCurrency = productionDividendCurrency,
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal productTotal = 1;
                        decimal productPrice = 1;
                        decimal productDividend = 1;
                        decimal productTotalCurrency = 1;
                        decimal productPriceCurrency = 1;
                        decimal productDividendCurrency = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yieldTotal = modelItem.ProductionTotal.Value;
                            var yieldPrice = modelItem.ProductionPrice.Value;
                            var yieldDividend = modelItem.ProductionDividend.Value;
                            var yieldTotalCurrency = modelItem.ProductionTotalCurrency.Value;
                            var yieldPriceCurrency = modelItem.ProductionPriceCurrency.Value;
                            var yieldDividendCurrency = modelItem.ProductionDividendCurrency.Value;
                            productTotal *= (yieldTotal + 1);
                            productPrice *= (yieldPrice + 1);
                            productDividend *= (yieldDividend + 1);
                            productTotalCurrency *= (yieldTotalCurrency + 1);
                            productPriceCurrency *= (yieldPriceCurrency + 1);
                            productDividendCurrency *= (yieldDividendCurrency + 1);
                        }
                        productTotal = productTotal - 1;
                        productPrice = productPrice - 1;
                        productDividend = productDividend - 1;
                        productTotalCurrency = productTotalCurrency - 1;
                        productPriceCurrency = productPriceCurrency - 1;
                        productDividendCurrency = productDividendCurrency - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DatumOd = dateFromDate.ToString(),
                            DatumDo = dateToDate.ToString(),
                            Vkupen = Math.Round(productTotal * 100, 2),
                            VkupenOrg = Math.Round(productTotalCurrency * 100, 2),
                            Cenoven = Math.Round(productPrice * 100, 2),
                            CenovenOrg = Math.Round(productPriceCurrency * 100, 2),
                            Dividen = Math.Round(productDividend * 100, 2),
                            DividenOrg = Math.Round(productDividendCurrency * 100, 2),
                            Uchestvo = Math.Round(sumShares.Value * 100, 2)
                        });
                        return View(new GridModel(model));
                    }

                    else if (sector != "-----" && isin == "-----")
                    {
                        var dateToDate = Convert.ToDateTime(dateTo);
                        var dateFromDate = Convert.ToDateTime(dateFrom);
                        var modelListType = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin,
                                Datum = instrument.Date,
                                Valuta = instrument.Currency,
                                Uchestvo = instrument.Share / 100,
                                Tip = instrument.Type,
                                Vkupen = instrument.Total_Yield / 100,
                                VkupenOrg = instrument.Total_Yield_Currency / 100,
                                Cenoven = instrument.Price_Yield / 100,
                                CenovenOrg = instrument.Price_Yield_Currency / 100,
                                Dividen = instrument.Dividend_Yield / 100,
                                DividenOrg = instrument.Dividend_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                            select new YieldsViewModel()
                            {
                                Datum = instrument.Date
                            }
                            ).Distinct().ToList();

                        var modelListIsins = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin
                            }
                            ).Distinct().ToList();

                        ViewBag.modelListIsins = modelListIsins;

                        var modelProduction = new List<YieldsViewModel>();
                        decimal? productionTotal = 0;
                        decimal? productionPrice = 0;
                        decimal? productionDividend = 0;
                        decimal? productionTotalCurrency = 0;
                        decimal? productionPriceCurrency = 0;
                        decimal? productionDividendCurrency = 0;
                        decimal? sumShares = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            productionTotal = 0;
                            productionPrice = 0;
                            productionDividend = 0;
                            productionTotalCurrency = 0;
                            productionPriceCurrency = 0;
                            productionDividendCurrency = 0;
                            sumShares = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                {
                                    productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                    productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                    productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                    productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                    productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                    productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                    sumShares += modelItemType.Uchestvo;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Datum = modelItemDate.Datum,
                                ProductionTotal = productionTotal,
                                ProductionPrice = productionPrice,
                                ProductionDividend = productionDividend,
                                ProductionTotalCurrency = productionTotalCurrency,
                                ProductionPriceCurrency = productionPriceCurrency,
                                ProductionDividendCurrency = productionDividendCurrency,
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal productTotal = 1;
                        decimal productPrice = 1;
                        decimal productDividend = 1;
                        decimal productTotalCurrency = 1;
                        decimal productPriceCurrency = 1;
                        decimal productDividendCurrency = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yieldTotal = modelItem.ProductionTotal.Value;
                            var yieldPrice = modelItem.ProductionPrice.Value;
                            var yieldDividend = modelItem.ProductionDividend.Value;
                            var yieldTotalCurrency = modelItem.ProductionTotalCurrency.Value;
                            var yieldPriceCurrency = modelItem.ProductionPriceCurrency.Value;
                            var yieldDividendCurrency = modelItem.ProductionDividendCurrency.Value;
                            productTotal *= (yieldTotal + 1);
                            productPrice *= (yieldPrice + 1);
                            productDividend *= (yieldDividend + 1);
                            productTotalCurrency *= (yieldTotalCurrency + 1);
                            productPriceCurrency *= (yieldPriceCurrency + 1);
                            productDividendCurrency *= (yieldDividendCurrency + 1);
                        }
                        productTotal = productTotal - 1;
                        productPrice = productPrice - 1;
                        productDividend = productDividend - 1;
                        productTotalCurrency = productTotalCurrency - 1;
                        productPriceCurrency = productPriceCurrency - 1;
                        productDividendCurrency = productDividendCurrency - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DatumOd = dateFromDate.ToString(),
                            DatumDo = dateToDate.ToString(),
                            Vkupen = Math.Round(productTotal * 100, 2),
                            VkupenOrg = Math.Round(productTotalCurrency * 100, 2),
                            Cenoven = Math.Round(productPrice * 100, 2),
                            CenovenOrg = Math.Round(productPriceCurrency * 100, 2),
                            Dividen = Math.Round(productDividend * 100, 2),
                            DividenOrg = Math.Round(productDividendCurrency * 100, 2),
                            Uchestvo = Math.Round(sumShares.Value * 100, 2)
                        });
                        return View(new GridModel(model));
                    }

                    else
                    {
                        var dateToDate = Convert.ToDateTime(dateTo);
                        var dateFromDate = Convert.ToDateTime(dateFrom);
                        var modelList = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Isin == isin
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin,
                                Datum = instrument.Date,
                                Valuta = instrument.Currency,
                                Uchestvo = instrument.Share / 100,
                                Tip = instrument.Type,
                                Vkupen = instrument.Total_Yield / 100,
                                VkupenOrg = instrument.Total_Yield_Currency / 100,
                                Cenoven = instrument.Price_Yield / 100,
                                CenovenOrg = instrument.Price_Yield_Currency / 100,
                                Dividen = instrument.Dividend_Yield / 100,
                                DividenOrg = instrument.Dividend_Yield_Currency / 100
                            }
                            ).ToList();
                        var model = new List<YieldsViewModel>();
                        decimal productTotal = 1;
                        decimal productPrice = 1;
                        decimal productDividend = 1;
                        decimal productTotalCurrency = 1;
                        decimal productPriceCurrency = 1;
                        decimal productDividendCurrency = 1;
                        decimal sumShares = 0;
                        foreach (var modelItem in modelList)
                        {
                            var yieldTotal = modelItem.Vkupen.Value;
                            var yieldPrice = modelItem.Cenoven.Value;
                            var yieldDividend = modelItem.Dividen.Value;
                            var yieldTotalCurrency = modelItem.VkupenOrg.Value;
                            var yieldPriceCurrency = modelItem.CenovenOrg.Value;
                            var yieldDividendCurrency = modelItem.DividenOrg.Value;
                            productTotal *= (yieldTotal + 1);
                            productPrice *= (yieldPrice + 1);
                            productDividend *= (yieldDividend + 1);
                            productTotalCurrency *= (yieldTotalCurrency + 1);
                            productPriceCurrency *= (yieldPriceCurrency + 1);
                            productDividendCurrency *= (yieldDividendCurrency + 1);
                            sumShares += modelItem.Uchestvo.Value;
                        }
                        productTotal = productTotal - 1;
                        productPrice = productPrice - 1;
                        productDividend = productDividend - 1;
                        productTotalCurrency = productTotalCurrency - 1;
                        productPriceCurrency = productPriceCurrency - 1;
                        productDividendCurrency = productDividendCurrency - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DatumOd = dateFromDate.ToString(),
                            DatumDo = dateToDate.ToString(),
                            Vkupen = Math.Round(productTotal * 100, 2),
                            VkupenOrg = Math.Round(productTotalCurrency * 100, 2),
                            Cenoven = Math.Round(productPrice * 100, 2),
                            CenovenOrg = Math.Round(productPriceCurrency * 100, 2),
                            Dividen = Math.Round(productDividend * 100, 2),
                            DividenOrg = Math.Round(productDividendCurrency * 100, 2)
                        });
                        return View(new GridModel(model));
                    }
                }
                else
                {
                    var dateToDate = Convert.ToDateTime(dateTo);
                    var dateFromDate = Convert.ToDateTime(dateFrom);
                    if(nacinNaPrinos == "dneven")
                    {
                        if (type != "-----" && isin == "-----")
                        {
                            var modelListType = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin,
                                    Datum = instrument.Date,
                                    Valuta = instrument.Currency,
                                    Uchestvo = instrument.Share / 100,
                                    Tip = instrument.Type,
                                    Vkupen = instrument.Total_Yield / 100,
                                    VkupenOrg = instrument.Total_Yield_Currency / 100,
                                    Cenoven = instrument.Price_Yield / 100,
                                    CenovenOrg = instrument.Price_Yield_Currency / 100,
                                    Dividen = instrument.Dividend_Yield / 100,
                                    DividenOrg = instrument.Dividend_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                                select new YieldsViewModel()
                                {
                                    Datum = instrument.Date
                                }
                                ).Distinct().ToList();

                            var modelListIsins = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin
                                }
                                ).Distinct().ToList();

                            ViewBag.modelListIsins = modelListIsins;

                            var modelProduction = new List<YieldsViewModel>();
                            decimal? productionTotal = 0;
                            decimal? productionPrice = 0;
                            decimal? productionDividend = 0;
                            decimal? productionTotalCurrency = 0;
                            decimal? productionPriceCurrency = 0;
                            decimal? productionDividendCurrency = 0;
                            decimal? sumShares = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                productionTotal = 0;
                                productionPrice = 0;
                                productionDividend = 0;
                                productionTotalCurrency = 0;
                                productionPriceCurrency = 0;
                                productionDividendCurrency = 0;
                                sumShares = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                    {
                                        productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                        productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                        productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                        productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                        productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                        productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                        sumShares += modelItemType.Uchestvo;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Datum = modelItemDate.Datum.Value,
                                    Vkupen = Math.Round(productionTotal.Value * 100, 2),
                                    VkupenOrg = Math.Round(productionTotalCurrency.Value * 100, 2),
                                    Cenoven = Math.Round(productionPrice.Value * 100, 2),
                                    CenovenOrg = Math.Round(productionPriceCurrency.Value * 100, 2),
                                    Dividen = Math.Round(productionDividend.Value * 100, 2),
                                    DividenOrg = Math.Round(productionDividendCurrency.Value * 100, 2),
                                    Uchestvo = Math.Round(sumShares.Value * 100, 2)
                                });
                            }
                            //var model = new List<YieldsViewModel>();
                            //decimal product = 1;
                            //foreach (var modelItem in modelProduction)
                            //{
                            //    var yield = modelItem.Production.Value;
                            //    product *= (yield + 1);
                            //}
                            //product = product - 1;
                            //model.Add(new YieldsViewModel()
                            //{
                            //    Isin = isin,
                            //    DateFrom = dateFromDate.ToString(),
                            //    DateTo = dateToDate.ToString(),
                            //    Yield = product * 100
                            //});
                            return View(new GridModel(modelProduction));
                        }

                        else if (country != "-----" && isin == "-----")
                        {
                            var modelListType = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin,
                                    Datum = instrument.Date,
                                    Valuta = instrument.Currency,
                                    Uchestvo = instrument.Share / 100,
                                    Tip = instrument.Type,
                                    Vkupen = instrument.Total_Yield / 100,
                                    VkupenOrg = instrument.Total_Yield_Currency / 100,
                                    Cenoven = instrument.Price_Yield / 100,
                                    CenovenOrg = instrument.Price_Yield_Currency / 100,
                                    Dividen = instrument.Dividend_Yield / 100,
                                    DividenOrg = instrument.Dividend_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                                select new YieldsViewModel()
                                {
                                    Datum = instrument.Date
                                }
                                ).Distinct().ToList();

                            var modelListIsins = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin
                                }
                                ).Distinct().ToList();

                            ViewBag.modelListIsins = modelListIsins;

                            var modelProduction = new List<YieldsViewModel>();
                            decimal? productionTotal = 0;
                            decimal? productionPrice = 0;
                            decimal? productionDividend = 0;
                            decimal? productionTotalCurrency = 0;
                            decimal? productionPriceCurrency = 0;
                            decimal? productionDividendCurrency = 0;
                            decimal? sumShares = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                productionTotal = 0;
                                productionPrice = 0;
                                productionDividend = 0;
                                productionTotalCurrency = 0;
                                productionPriceCurrency = 0;
                                productionDividendCurrency = 0;
                                sumShares = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                    {
                                        productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                        productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                        productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                        productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                        productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                        productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                        sumShares += modelItemType.Uchestvo;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Datum = modelItemDate.Datum.Value,
                                    Vkupen = Math.Round(productionTotal.Value * 100, 2),
                                    VkupenOrg = Math.Round(productionTotalCurrency.Value * 100, 2),
                                    Cenoven = Math.Round(productionPrice.Value * 100, 2),
                                    CenovenOrg = Math.Round(productionPriceCurrency.Value * 100, 2),
                                    Dividen = Math.Round(productionDividend.Value * 100, 2),
                                    DividenOrg = Math.Round(productionDividendCurrency.Value * 100, 2),
                                    Uchestvo = Math.Round(sumShares.Value * 100, 2)
                                });
                            }
                            //var model = new List<YieldsViewModel>();
                            //decimal product = 1;
                            //foreach (var modelItem in modelProduction)
                            //{
                            //    var yield = modelItem.Production.Value;
                            //    product *= (yield + 1);
                            //}
                            //product = product - 1;
                            //model.Add(new YieldsViewModel()
                            //{
                            //    Isin = isin,
                            //    DateFrom = dateFromDate.ToString(),
                            //    DateTo = dateToDate.ToString(),
                            //    Yield = product * 100
                            //});
                            return View(new GridModel(modelProduction));
                        }

                        else if (currency != "-----" && isin == "-----")
                        {
                            var modelListType = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin,
                                    Datum = instrument.Date,
                                    Valuta = instrument.Currency,
                                    Uchestvo = instrument.Share / 100,
                                    Tip = instrument.Type,
                                    Vkupen = instrument.Total_Yield / 100,
                                    VkupenOrg = instrument.Total_Yield_Currency / 100,
                                    Cenoven = instrument.Price_Yield / 100,
                                    CenovenOrg = instrument.Price_Yield_Currency / 100,
                                    Dividen = instrument.Dividend_Yield / 100,
                                    DividenOrg = instrument.Dividend_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                                select new YieldsViewModel()
                                {
                                    Datum = instrument.Date
                                }
                                ).Distinct().ToList();

                            var modelListIsins = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin
                                }
                                ).Distinct().ToList();

                            ViewBag.modelListIsins = modelListIsins;

                            var modelProduction = new List<YieldsViewModel>();
                            decimal? productionTotal = 0;
                            decimal? productionPrice = 0;
                            decimal? productionDividend = 0;
                            decimal? productionTotalCurrency = 0;
                            decimal? productionPriceCurrency = 0;
                            decimal? productionDividendCurrency = 0;
                            decimal? sumShares = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                productionTotal = 0;
                                productionPrice = 0;
                                productionDividend = 0;
                                productionTotalCurrency = 0;
                                productionPriceCurrency = 0;
                                productionDividendCurrency = 0;
                                sumShares = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                    {
                                        productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                        productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                        productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                        productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                        productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                        productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                        sumShares += modelItemType.Uchestvo;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Datum = modelItemDate.Datum.Value,
                                    Vkupen = Math.Round(productionTotal.Value * 100, 2),
                                    VkupenOrg = Math.Round(productionTotalCurrency.Value * 100, 2),
                                    Cenoven = Math.Round(productionPrice.Value * 100, 2),
                                    CenovenOrg = Math.Round(productionPriceCurrency.Value * 100, 2),
                                    Dividen = Math.Round(productionDividend.Value * 100, 2),
                                    DividenOrg = Math.Round(productionDividendCurrency.Value * 100, 2),
                                    Uchestvo = Math.Round(sumShares.Value * 100, 2)
                                });
                            }
                            //var model = new List<YieldsViewModel>();
                            //decimal product = 1;
                            //foreach (var modelItem in modelProduction)
                            //{
                            //    var yield = modelItem.Production.Value;
                            //    product *= (yield + 1);
                            //}
                            //product = product - 1;
                            //model.Add(new YieldsViewModel()
                            //{
                            //    Isin = isin,
                            //    DateFrom = dateFromDate.ToString(),
                            //    DateTo = dateToDate.ToString(),
                            //    Yield = product * 100
                            //});
                            return View(new GridModel(modelProduction));
                        }

                        else if (sector != "-----" && isin == "-----")
                        {
                            var modelListType = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin,
                                    Datum = instrument.Date,
                                    Valuta = instrument.Currency,
                                    Uchestvo = instrument.Share / 100,
                                    Tip = instrument.Type,
                                    Vkupen = instrument.Total_Yield / 100,
                                    VkupenOrg = instrument.Total_Yield_Currency / 100,
                                    Cenoven = instrument.Price_Yield / 100,
                                    CenovenOrg = instrument.Price_Yield_Currency / 100,
                                    Dividen = instrument.Dividend_Yield / 100,
                                    DividenOrg = instrument.Dividend_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                                select new YieldsViewModel()
                                {
                                    Datum = instrument.Date
                                }
                                ).Distinct().ToList();

                            var modelListIsins = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                                select new YieldsViewModel()
                                {
                                    Isin = instrument.Isin
                                }
                                ).Distinct().ToList();

                            ViewBag.modelListIsins = modelListIsins;

                            var modelProduction = new List<YieldsViewModel>();
                            decimal? productionTotal = 0;
                            decimal? productionPrice = 0;
                            decimal? productionDividend = 0;
                            decimal? productionTotalCurrency = 0;
                            decimal? productionPriceCurrency = 0;
                            decimal? productionDividendCurrency = 0;
                            decimal? sumShares = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                productionTotal = 0;
                                productionPrice = 0;
                                productionDividend = 0;
                                productionTotalCurrency = 0;
                                productionPriceCurrency = 0;
                                productionDividendCurrency = 0;
                                sumShares = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Datum.Value == modelItemType.Datum.Value)
                                    {
                                        productionTotal += modelItemType.Uchestvo * modelItemType.Vkupen;
                                        productionPrice += modelItemType.Uchestvo * modelItemType.Cenoven;
                                        productionDividend += modelItemType.Uchestvo * modelItemType.Dividen;
                                        productionTotalCurrency += modelItemType.Uchestvo * modelItemType.VkupenOrg;
                                        productionPriceCurrency += modelItemType.Uchestvo * modelItemType.CenovenOrg;
                                        productionDividendCurrency += modelItemType.Uchestvo * modelItemType.DividenOrg;
                                        sumShares += modelItemType.Uchestvo;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Datum = modelItemDate.Datum.Value,
                                    Vkupen = Math.Round(productionTotal.Value * 100, 2),
                                    VkupenOrg = Math.Round(productionTotalCurrency.Value * 100, 2),
                                    Cenoven = Math.Round(productionPrice.Value * 100, 2),
                                    CenovenOrg = Math.Round(productionPriceCurrency.Value * 100, 2),
                                    Dividen = Math.Round(productionDividend.Value * 100, 2),
                                    DividenOrg = Math.Round(productionDividendCurrency.Value * 100, 2),
                                    Uchestvo = Math.Round(sumShares.Value * 100, 2)
                                });
                            }
                            //var model = new List<YieldsViewModel>();
                            //decimal product = 1;
                            //foreach (var modelItem in modelProduction)
                            //{
                            //    var yield = modelItem.Production.Value;
                            //    product *= (yield + 1);
                            //}
                            //product = product - 1;
                            //model.Add(new YieldsViewModel()
                            //{
                            //    Isin = isin,
                            //    DateFrom = dateFromDate.ToString(),
                            //    DateTo = dateToDate.ToString(),
                            //    Yield = product * 100
                            //});
                            return View(new GridModel(modelProduction));
                        }

                        else
                        {
                            var modelList = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Isin == isin
                            select new YieldsViewModel()
                            {
                                Isin = instrument.Isin,
                                Datum = instrument.Date,
                                Valuta = instrument.Currency,
                                Uchestvo = Math.Round(instrument.Share.Value / 100, 2),
                                Tip = instrument.Type,
                                Vkupen = Math.Round(instrument.Total_Yield.Value / 100, 2),
                                VkupenOrg = Math.Round(instrument.Total_Yield_Currency.Value / 100, 2),
                                Cenoven = Math.Round(instrument.Price_Yield.Value / 100, 2),
                                CenovenOrg = Math.Round(instrument.Price_Yield_Currency.Value / 100, 2),
                                Dividen = Math.Round(instrument.Dividend_Yield.Value / 100, 2),
                                DividenOrg = Math.Round(instrument.Dividend_Yield_Currency.Value / 100, 2)
                            }
                            ).ToList();
                            return View(new GridModel(modelList));
                        }
                    }
                    else if (nacinNaPrinos == "nedelen")
                    {
                        var newDate = new DateTime();
                        var secondNewDate = new DateTime();
                        var model = new List<YieldsViewModel>();
                        var flag = 0;
                        while (newDate < dateToDate)
                        {
                            decimal product = 1;
                            decimal productCurrency = 1;
                            var setCurrency = "";
                            if (flag == 0)
                            {
                                newDate = dateFromDate;
                                secondNewDate = newDate.AddDays(7);
                                var modelList = (
                                    from yield in this.context.Total_Yields
                                    where yield.Isin == isin && yield.Date > newDate && yield.Date <= secondNewDate
                                    select new YieldsViewModel()
                                    {
                                        Isin = yield.Isin,
                                        Datum = yield.Date,
                                        Valuta = yield.Currency,
                                        Vkupen = yield.Total_Yield,
                                        VkupenOrg = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Valuta;
                                    var yield = modelItem.Vkupen.HasValue ? modelItem.Vkupen.Value : 0;
                                    var yieldCurrency = modelItem.VkupenOrg.HasValue ? modelItem.VkupenOrg.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Valuta = setCurrency,
                                    Vkupen = product*100,
                                    VkupenOrg = productCurrency*100,
                                    DatumOd = newDate.ToString(),
                                    DatumDo = secondNewDate.ToString()
                                });
                                newDate = newDate.AddDays(7);
                                flag = 1;
                            }
                            else
                            {
                                secondNewDate = newDate.AddDays(7);
                                var modelList = (
                                    from yield in this.context.Total_Yields
                                    where yield.Isin == isin && yield.Date > newDate && yield.Date <= secondNewDate
                                    select new YieldsViewModel()
                                    {
                                        Isin = yield.Isin,
                                        Datum = yield.Date,
                                        Valuta = yield.Currency,
                                        Vkupen = yield.Total_Yield,
                                        VkupenOrg = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Valuta;
                                    var yield = modelItem.Vkupen.HasValue ? modelItem.Vkupen.Value : 0;
                                    var yieldCurrency = modelItem.VkupenOrg.HasValue ? modelItem.VkupenOrg.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Vkupen = product*100,
                                    VkupenOrg = productCurrency*100,
                                    Valuta = setCurrency,
                                    DatumOd = newDate.ToString(),
                                    DatumDo = secondNewDate.ToString()
                                });
                                newDate = newDate.AddDays(7);
                            }
                        }
                        return View(new GridModel(model));
                    }
                    else if(nacinNaPrinos == "mesecen")
                    {
                        var newDate = new DateTime();
                        var secondNewDate = new DateTime();
                        var model = new List<YieldsViewModel>();
                        var flag = 0;
                        while (newDate < dateToDate)
                        {
                            decimal product = 1;
                            decimal productCurrency = 1;
                            var setCurrency = "";
                            if (flag == 0)
                            {
                                newDate = dateFromDate;
                                if (newDate.Day == 30)
                                {
                                    secondNewDate = newDate.AddMonths(1);
                                    secondNewDate = secondNewDate.AddDays(1);
                                }
                                else if(newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(1);
                                    }
                                    else if (dateFromDate.Day == 30)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                    }
                                    else if(dateFromDate.Day == 30)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(1);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    secondNewDate = newDate.AddMonths(1);
                                }
                                var modelList = (
                                    from yield in this.context.Total_Yields
                                    where yield.Isin == isin && yield.Date > newDate && yield.Date <= secondNewDate
                                    select new YieldsViewModel()
                                    {
                                        Isin = yield.Isin,
                                        Datum = yield.Date,
                                        Valuta = yield.Currency,
                                        Vkupen = yield.Total_Yield,
                                        VkupenOrg = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Valuta;
                                    var yield = modelItem.Vkupen.HasValue ? modelItem.Vkupen.Value : 0;
                                    var yieldCurrency = modelItem.VkupenOrg.HasValue ? modelItem.VkupenOrg.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Vkupen = product*100,
                                    VkupenOrg = productCurrency*100,
                                    Valuta = setCurrency,
                                    DatumOd = newDate.ToString(),
                                    DatumDo = secondNewDate.ToString()
                                });
                                if (newDate.Day == 30)
                                {
                                    newDate = newDate.AddMonths(1);
                                    newDate = newDate.AddDays(1);
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(1);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(1);
                                    }
                                    else if (dateFromDate.Day == 30)
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(2);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(1);
                                    }
                                    else if (dateFromDate.Day == 30)
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(1);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    newDate = newDate.AddMonths(1);
                                }
                                flag = 1;
                            }
                            else
                            {
                                if (newDate.Day == 30)
                                {
                                    secondNewDate = newDate.AddMonths(1);
                                    secondNewDate = secondNewDate.AddDays(1);
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(1);
                                    }
                                    else if (dateFromDate.Day == 30)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                    }
                                    else if(dateFromDate.Day == 30)
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(1);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(1);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    secondNewDate = newDate.AddMonths(1);
                                }
                                var modelList = (
                                    from yield in this.context.Total_Yields
                                    where yield.Isin == isin && yield.Date > newDate && yield.Date <= secondNewDate
                                    select new YieldsViewModel()
                                    {
                                        Isin = yield.Isin,
                                        Datum = yield.Date,
                                        Valuta = yield.Currency,
                                        Vkupen = yield.Total_Yield,
                                        VkupenOrg = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Valuta;
                                    var yield = modelItem.Vkupen.HasValue ? modelItem.Vkupen.Value : 0;
                                    var yieldCurrency = modelItem.VkupenOrg.HasValue ? modelItem.VkupenOrg.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Vkupen = product*100,
                                    VkupenOrg = productCurrency*100,
                                    Valuta = setCurrency,
                                    DatumOd = newDate.ToString(),
                                    DatumDo = secondNewDate.ToString()
                                });
                                if (newDate.Day == 30)
                                {
                                    newDate = newDate.AddMonths(1);
                                    newDate = newDate.AddDays(1);
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(1);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(1);
                                    }
                                    else if (dateFromDate.Day == 30)
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(2);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(1);
                                    }
                                    else if(dateFromDate.Day == 30)
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(1);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(1);
                                        newDate = newDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    newDate = newDate.AddMonths(1);
                                }
                            }
                        }
                        return View(new GridModel(model));
                    }
                    else
                    {
                        var newDate = new DateTime();
                        var secondNewDate = new DateTime();
                        var model = new List<YieldsViewModel>();
                        var flag = 0;
                        while (newDate < dateToDate)
                        {
                            decimal product = 1;
                            decimal productCurrency = 1;
                            var setCurrency = "";
                            if (flag == 0)
                            {
                                newDate = dateFromDate;
                                if (newDate.Day == 30)
                                {
                                    if (newDate.Month == 11 || newDate.Month == 6)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(1);
                                    }
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                    }
                                    else if(dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(3);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if(dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    secondNewDate = newDate.AddMonths(3);
                                }
                                var modelList = (
                                    from yield in this.context.Total_Yields
                                    where yield.Isin == isin && yield.Date > newDate && yield.Date <= secondNewDate
                                    select new YieldsViewModel()
                                    {
                                        Isin = yield.Isin,
                                        Datum = yield.Date,
                                        Valuta = yield.Currency,
                                        Vkupen = yield.Total_Yield,
                                        VkupenOrg = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Valuta;
                                    var yield = modelItem.Vkupen.HasValue ? modelItem.Vkupen.Value : 0;
                                    var yieldCurrency = modelItem.VkupenOrg.HasValue ? modelItem.VkupenOrg.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Vkupen = product*100,
                                    VkupenOrg = productCurrency*100,
                                    Valuta = setCurrency,
                                    DatumOd = newDate.ToString(),
                                    DatumDo = secondNewDate.ToString()
                                });
                                if (newDate.Day == 30)
                                {
                                    if (newDate.Month == 11 || newDate.Month == 6)
                                    {
                                        newDate = newDate.AddMonths(3);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(1);
                                    }
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(2);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(3);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(2);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    newDate = newDate.AddMonths(3);
                                }
                                flag = 1;
                            }
                            else
                            {
                                if (newDate.Day == 30)
                                {
                                    if (newDate.Month == 11 || newDate.Month == 6)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(1);
                                    }
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(3);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                    else
                                    {
                                        secondNewDate = newDate.AddMonths(3);
                                        secondNewDate = secondNewDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    secondNewDate = newDate.AddMonths(3);
                                }
                                var modelList = (
                                    from yield in this.context.Total_Yields
                                    where yield.Isin == isin && yield.Date > newDate && yield.Date <= secondNewDate
                                    select new YieldsViewModel()
                                    {
                                        Isin = yield.Isin,
                                        Datum = yield.Date,
                                        Valuta = yield.Currency,
                                        Vkupen = yield.Total_Yield,
                                        VkupenOrg = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Valuta;
                                    var yield = modelItem.Vkupen.HasValue ? modelItem.Vkupen.Value : 0;
                                    var yieldCurrency = modelItem.VkupenOrg.HasValue ? modelItem.VkupenOrg.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Vkupen = product*100,
                                    VkupenOrg = productCurrency*100,
                                    Valuta = setCurrency,
                                    DatumOd = newDate.ToString(),
                                    DatumDo = secondNewDate.ToString()
                                });
                                if (newDate.Day == 30)
                                {
                                    if (newDate.Month == 11 || newDate.Month == 6)
                                    {
                                        newDate = newDate.AddMonths(3);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(1);
                                    }
                                }
                                else if (newDate.Day == 28 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 28 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(2);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(3);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(3);
                                    }
                                }
                                else if (newDate.Day == 29 && newDate.Month == 2)
                                {
                                    if (dateFromDate.Day == 29 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                    }
                                    else if (dateFromDate.Day == 30 && dateFromDate.Month != 2)
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(2);
                                    }
                                    else
                                    {
                                        newDate = newDate.AddMonths(3);
                                        newDate = newDate.AddDays(2);
                                    }
                                }
                                else
                                {
                                    newDate = newDate.AddMonths(3);
                                }
                            }
                        }
                        return View(new GridModel(model));
                    }
                }
            }
            else
            {
                var model = (
                    from yield in this.context.Total_Yields
                    where yield.Id < 100
                    select new YieldsViewModel()
                    {
                        Isin = yield.Isin,
                        Datum = yield.Date,
                        Valuta = yield.Currency,
                        Vkupen = yield.Total_Yield,
                        VkupenOrg = yield.Total_Yield_Currency
                    }
                    ).ToList();
                return View(new GridModel(model));
            }
            
        }
    }
}