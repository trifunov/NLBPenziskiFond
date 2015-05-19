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

            ViewBag.listTypes = Types;
            ViewBag.listCurrencies = Currencies;
            ViewBag.listSectors = Sectors;
            ViewBag.listCountries = Countries;
            ViewBag.listIsinFullName = IsinFullName;

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
                                Date = instrument.Date,
                                Currency = instrument.Currency,
                                Share = instrument.Share / 100,
                                Type = instrument.Type,
                                Yield = instrument.Total_Yield / 100,
                                Yield_Currency = instrument.Total_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                            select new YieldsViewModel()
                            {
                                Date = instrument.Date
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
                        decimal? production = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            production = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if(modelItemDate.Date.Value == modelItemType.Date.Value)
                                {
                                    production += modelItemType.Share * modelItemType.Yield;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Date = modelItemDate.Date,
                                Production = production
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal product = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yield = modelItem.Production.Value;
                            product *= (yield + 1);
                        }
                        product = product - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DateFrom = dateFromDate.ToString(),
                            DateTo = dateToDate.ToString(),
                            Yield = product * 100
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
                                Date = instrument.Date,
                                Currency = instrument.Currency,
                                Share = instrument.Share / 100,
                                Type = instrument.Type,
                                Yield = instrument.Total_Yield / 100,
                                Yield_Currency = instrument.Total_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                            select new YieldsViewModel()
                            {
                                Date = instrument.Date
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
                        decimal? production = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            production = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                {
                                    production += modelItemType.Share * modelItemType.Yield;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Date = modelItemDate.Date,
                                Production = production
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal product = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yield = modelItem.Production.Value;
                            product *= (yield + 1);
                        }
                        product = product - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DateFrom = dateFromDate.ToString(),
                            DateTo = dateToDate.ToString(),
                            Yield = product * 100
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
                                Date = instrument.Date,
                                Currency = instrument.Currency,
                                Share = instrument.Share / 100,
                                Type = instrument.Type,
                                Yield = instrument.Total_Yield / 100,
                                Yield_Currency = instrument.Total_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                            select new YieldsViewModel()
                            {
                                Date = instrument.Date
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
                        decimal? production = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            production = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                {
                                    production += modelItemType.Share * modelItemType.Yield;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Date = modelItemDate.Date,
                                Production = production
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal product = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yield = modelItem.Production.Value;
                            product *= (yield + 1);
                        }
                        product = product - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DateFrom = dateFromDate.ToString(),
                            DateTo = dateToDate.ToString(),
                            Yield = product * 100
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
                                Date = instrument.Date,
                                Currency = instrument.Currency,
                                Share = instrument.Share / 100,
                                Type = instrument.Type,
                                Yield = instrument.Total_Yield / 100,
                                Yield_Currency = instrument.Total_Yield_Currency / 100
                            }
                            ).ToList();

                        var modelListDate = (
                            from instrument in this.context.Instruments
                            where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                            select new YieldsViewModel()
                            {
                                Date = instrument.Date
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
                        decimal? production = 0;
                        foreach (var modelItemDate in modelListDate)
                        {
                            production = 0;
                            foreach (var modelItemType in modelListType)
                            {
                                if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                {
                                    production += modelItemType.Share * modelItemType.Yield;
                                }
                            }
                            modelProduction.Add(new YieldsViewModel()
                            {
                                Date = modelItemDate.Date,
                                Production = production
                            });
                        }
                        var model = new List<YieldsViewModel>();
                        decimal product = 1;
                        foreach (var modelItem in modelProduction)
                        {
                            var yield = modelItem.Production.Value;
                            product *= (yield + 1);
                        }
                        product = product - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DateFrom = dateFromDate.ToString(),
                            DateTo = dateToDate.ToString(),
                            Yield = product * 100
                        });
                        return View(new GridModel(model));
                    }

                    else
                    {
                        var dateToDate = Convert.ToDateTime(dateTo);
                        var dateFromDate = Convert.ToDateTime(dateFrom);
                        var modelList = (
                            from yield in this.context.Instruments
                            where yield.Date > dateFromDate && yield.Date <= dateToDate && yield.Isin == isin
                            select new YieldsViewModel()
                            {
                                Isin = yield.Isin,
                                Date = yield.Date,
                                Currency = yield.Currency,
                                Yield = yield.Total_Yield / 100,
                                Yield_Currency = yield.Total_Yield_Currency /100
                            }
                            ).ToList();
                        var model = new List<YieldsViewModel>();
                        decimal product = 1;
                        decimal productCurrency = 1;
                        var setCurrency = "";
                        foreach (var modelItem in modelList)
                        {
                            setCurrency = modelItem.Currency;
                            var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                            var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                            product *= (yield + 1);
                            productCurrency *= (yieldCurrency + 1);
                        }
                        product = product - 1;
                        productCurrency = productCurrency - 1;
                        model.Add(new YieldsViewModel()
                        {
                            Isin = isin,
                            DateFrom = dateFromDate.ToString(),
                            DateTo = dateToDate.ToString(),
                            Currency = setCurrency,
                            Yield = product * 100,
                            Yield_Currency = productCurrency * 100
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
                                    Date = instrument.Date,
                                    Currency = instrument.Currency,
                                    Share = instrument.Share / 100,
                                    Type = instrument.Type,
                                    Yield = instrument.Total_Yield / 100,
                                    Yield_Currency = instrument.Total_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Type == type
                                select new YieldsViewModel()
                                {
                                    Date = instrument.Date
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
                            decimal? production = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                production = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                    {
                                        production += modelItemType.Share * modelItemType.Yield;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Date = modelItemDate.Date,
                                    Yield = production * 100
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
                                    Date = instrument.Date,
                                    Currency = instrument.Currency,
                                    Share = instrument.Share / 100,
                                    Type = instrument.Type,
                                    Yield = instrument.Total_Yield / 100,
                                    Yield_Currency = instrument.Total_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Country == country
                                select new YieldsViewModel()
                                {
                                    Date = instrument.Date
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
                            decimal? production = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                production = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                    {
                                        production += modelItemType.Share * modelItemType.Yield;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Date = modelItemDate.Date,
                                    Yield = production * 100
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
                                    Date = instrument.Date,
                                    Currency = instrument.Currency,
                                    Share = instrument.Share / 100,
                                    Type = instrument.Type,
                                    Yield = instrument.Total_Yield / 100,
                                    Yield_Currency = instrument.Total_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Currency == currency
                                select new YieldsViewModel()
                                {
                                    Date = instrument.Date
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
                            decimal? production = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                production = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                    {
                                        production += modelItemType.Share * modelItemType.Yield;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Date = modelItemDate.Date,
                                    Yield = production * 100
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
                                    Date = instrument.Date,
                                    Currency = instrument.Currency,
                                    Share = instrument.Share / 100,
                                    Type = instrument.Type,
                                    Yield = instrument.Total_Yield / 100,
                                    Yield_Currency = instrument.Total_Yield_Currency / 100
                                }
                                ).ToList();

                            var modelListDate = (
                                from instrument in this.context.Instruments
                                where instrument.Date > dateFromDate && instrument.Date <= dateToDate && instrument.Sector == sector
                                select new YieldsViewModel()
                                {
                                    Date = instrument.Date
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
                            decimal? production = 0;
                            foreach (var modelItemDate in modelListDate)
                            {
                                production = 0;
                                foreach (var modelItemType in modelListType)
                                {
                                    if (modelItemDate.Date.Value == modelItemType.Date.Value)
                                    {
                                        production += modelItemType.Share * modelItemType.Yield;
                                    }
                                }
                                modelProduction.Add(new YieldsViewModel()
                                {
                                    Date = modelItemDate.Date,
                                    Yield = production * 100
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
                            from yield in this.context.Instruments
                            where yield.Date > dateFromDate && yield.Date <= dateToDate && yield.Isin == isin
                            select new YieldsViewModel()
                            {
                                Isin = yield.Isin,
                                Date = yield.Date,
                                Currency = yield.Currency,
                                Yield = yield.Total_Yield / 100,
                                Yield_Currency = yield.Total_Yield_Currency / 100,
                                Share = yield.Share / 100
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
                                        Date = yield.Date,
                                        Currency = yield.Currency,
                                        Yield = yield.Total_Yield,
                                        Yield_Currency = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Currency;
                                    var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                                    var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Currency = setCurrency,
                                    Yield = product*100,
                                    Yield_Currency = productCurrency*100,
                                    DateFrom = newDate.ToString(),
                                    DateTo = secondNewDate.ToString()
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
                                        Date = yield.Date,
                                        Currency = yield.Currency,
                                        Yield = yield.Total_Yield,
                                        Yield_Currency = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Currency;
                                    var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                                    var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Yield = product*100,
                                    Yield_Currency = productCurrency*100,
                                    Currency = setCurrency,
                                    DateFrom = newDate.ToString(),
                                    DateTo = secondNewDate.ToString()
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
                                        Date = yield.Date,
                                        Currency = yield.Currency,
                                        Yield = yield.Total_Yield,
                                        Yield_Currency = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Currency;
                                    var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                                    var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Yield = product*100,
                                    Yield_Currency = productCurrency*100,
                                    Currency = setCurrency,
                                    DateFrom = newDate.ToString(),
                                    DateTo = secondNewDate.ToString()
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
                                        Date = yield.Date,
                                        Currency = yield.Currency,
                                        Yield = yield.Total_Yield,
                                        Yield_Currency = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Currency;
                                    var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                                    var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Yield = product*100,
                                    Yield_Currency = productCurrency*100,
                                    Currency = setCurrency,
                                    DateFrom = newDate.ToString(),
                                    DateTo = secondNewDate.ToString()
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
                                        Date = yield.Date,
                                        Currency = yield.Currency,
                                        Yield = yield.Total_Yield,
                                        Yield_Currency = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Currency;
                                    var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                                    var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Yield = product*100,
                                    Yield_Currency = productCurrency*100,
                                    Currency = setCurrency,
                                    DateFrom = newDate.ToString(),
                                    DateTo = secondNewDate.ToString()
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
                                        Date = yield.Date,
                                        Currency = yield.Currency,
                                        Yield = yield.Total_Yield,
                                        Yield_Currency = yield.Total_Yield_Currency
                                    }
                                    ).ToList();
                                foreach (var modelItem in modelList)
                                {
                                    setCurrency = modelItem.Currency;
                                    var yield = modelItem.Yield.HasValue ? modelItem.Yield.Value : 0;
                                    var yieldCurrency = modelItem.Yield_Currency.HasValue ? modelItem.Yield_Currency.Value : 0;
                                    product *= (yield/100 + 1);
                                    productCurrency *= (yieldCurrency/100 + 1);
                                }
                                product = product - 1;
                                productCurrency = productCurrency - 1;
                                model.Add(new YieldsViewModel()
                                {
                                    Isin = isin,
                                    Yield = product*100,
                                    Yield_Currency = productCurrency*100,
                                    Currency = setCurrency,
                                    DateFrom = newDate.ToString(),
                                    DateTo = secondNewDate.ToString()
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
                        Date = yield.Date,
                        Currency = yield.Currency,
                        Yield = yield.Total_Yield,
                        Yield_Currency = yield.Total_Yield_Currency
                    }
                    ).ToList();
                return View(new GridModel(model));
            }
            
        }
    }
}