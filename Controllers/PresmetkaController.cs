using NLBPenziskiFond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NLBPenziskiFond.Controllers
{
    public class PresmetkaController : Controller
    {
        private NLBEntities context = new NLBEntities();
        // GET: Presmetka
        public ActionResult Index()
        {
            var date = new DateTime();
            var dateRegister = new DateTime();
            var listContoNumerator = new List<string>();
            //var listNumerators = new List<FractionsViewModel>();
            var dictionaryIsin = new Dictionary<string, decimal?>();
            var dictionaryIsinCurrency = new Dictionary<string, decimal?>();
            var listShares = new List<SharesViewModel>();
            var listYields = new List<YieldsViewModel>();
            decimal? share = 0;
            decimal? yield = 0;
            decimal? yield_currency = 0;
            //var listKontoBroitel = new List<string>();
            //listKontoBroitel.Add("3");
            //listKontoBroitel.Add("4");
            //listKontoBroitel.Add("5");
            listContoNumerator.Add("700");
            listContoNumerator.Add("710");
            listContoNumerator.Add("723");
            listContoNumerator.Add("727");
            listContoNumerator.Add("733");
            listContoNumerator.Add("737");
            listContoNumerator.Add("623");
            listContoNumerator.Add("627");
            listContoNumerator.Add("633");
            listContoNumerator.Add("637");
            listContoNumerator.Add("650");
            decimal? sumDenominator = 0;
            decimal? sumNumerator = 0;
            decimal? sumDenominatorCurrency = 0;
            string currency = null;
            DateTime? containsDate = new DateTime(2010, 10, 15);

            var listNumerators = (
                from register in this.context.Registers_Total_Yields_Numerators
                select new FractionsViewModel()
                {
                    Isin = register.Isin,
                    Date = register.Date,
                    Currency = register.Currency,
                    Type = register.Type,
                    Sum = register.Daily_Sum,
                    Sum_Currency = register.Daily_Sum_Currency
                }
                ).OrderBy(x => x.Date).ToList();

            //var listDates = (
            //    from register in this.context.Registers_Yields_Denominators
            //    select new FractionsViewModel()
            //    {
            //        Date = register.Date
            //    }
            //    ).Distinct().OrderBy(x => x.Date).ToList();

            //var listRegistersNumerators = (
            //    from register in this.context.Numerators
            //    select new FractionsViewModel()
            //    {
            //        Isin = register.Isin,
            //        Date = register.Date,
            //        Sum = register.Sum_Numerator
            //    }
            //    ).OrderBy(x => x.Date).ToList();

                //foreach (var registerItem in listRegistersTotalNumerators)
                //{
                    
                //        if (registerItem.Id_Record != null && registerItem.Currency != "MKD")
                //        {
                //            if (!dictionaryIsin.ContainsKey(registerItem.Id_Record))
                //            {
                //                dictionaryIsin.Add(registerItem.Id_Record, registerItem.Daily_Sum);
                //                dictionaryIsinCurrency.Add(registerItem.Id_Record + registerItem.Currency, registerItem.Daily_Sum_Currency);
                //            }
                //            else
                //            {
                //                dictionaryIsin[registerItem.Id_Record] = dictionaryIsin[registerItem.Id_Record] + registerItem.Daily_Sum;
                //                dictionaryIsinCurrency[registerItem.Id_Record + registerItem.Currency] = dictionaryIsinCurrency[registerItem.Id_Record + registerItem.Currency] + registerItem.Daily_Sum_Currency;
                //            }
                //            var fractionViewModel = new FractionsViewModel();
                //            fractionViewModel.Isin = registerItem.Id_Record;
                //            fractionViewModel.Date = registerItem.Date;
                //            fractionViewModel.Currency = registerItem.Currency;
                //            fractionViewModel.Type = registerItem.Type;
                //            fractionViewModel.Sum = dictionaryIsin[registerItem.Id_Record];
                //            fractionViewModel.Sum_Currency = dictionaryIsinCurrency[registerItem.Id_Record + registerItem.Currency];
                //            listNumerators.Add(fractionViewModel);
                //        }

                //    if(registerItem.Id_Record != null && registerItem.Currency == "MKD")
                //    {
                //        if (!dictionaryIsin.ContainsKey(registerItem.Id_Record))
                //            {
                //                dictionaryIsin.Add(registerItem.Id_Record, registerItem.Daily_Sum);
                //            }
                //            else
                //            {
                //                dictionaryIsin[registerItem.Id_Record] = dictionaryIsin[registerItem.Id_Record] + registerItem.Daily_Sum;
                //            }
                //            var fractionViewModel = new FractionsViewModel();
                //            fractionViewModel.Isin = registerItem.Id_Record;
                //            fractionViewModel.Date = registerItem.Date;
                //            fractionViewModel.Currency = registerItem.Currency;
                //            fractionViewModel.Type = registerItem.Type;
                //            fractionViewModel.Sum = dictionaryIsin[registerItem.Id_Record];
                //            listNumerators.Add(fractionViewModel);
                //    }
                    
                //}

            var listDenominators = (
                from register in this.context.Yields_Denominators
                select new FractionsViewModel()
                {
                    Isin = register.Isin,
                    Date = register.Date,
                    Currency = register.Currency,
                    Sum = register.Sum,
                    Sum_Currency = register.Sum_Currency
                }
                ).OrderBy(x => x.Date).ToList();
            
            foreach(var denominator in listDenominators)
            {
                if (containsDate.Value.Month == denominator.Date.Value.Month || (containsDate.Value.Month + 1) == denominator.Date.Value.Month || (containsDate.Value.Month + 2) == denominator.Date.Value.Month)
                {
                    var newDate = denominator.Date.Value.AddDays(1);
                    foreach (var numerator in listNumerators)
                    {
                        if (numerator.Isin == denominator.Isin && numerator.Date == newDate)
                        {
                            if (numerator.Currency != "MKD")
                            {
                                yield = (numerator.Sum / denominator.Sum) * 100;
                                if (denominator.Sum_Currency != 0)
                                {
                                    yield_currency = (numerator.Sum_Currency / denominator.Sum_Currency) * 100;
                                }
                                else
                                {
                                    yield_currency = 0;
                                }
                                var newRow = this.context.Total_Yields.Add(new Total_Yields()
                                {
                                    Isin = numerator.Isin,
                                    Date = numerator.Date,
                                    Currency = numerator.Currency,
                                    Total_Yield = yield,
                                    Total_Yield_Currency = yield_currency
                                });
                                this.context.SaveChanges();
                            }
                            else
                            {
                                yield = (numerator.Sum / denominator.Sum) * 100;
                                var newRow = this.context.Total_Yields.Add(new Total_Yields()
                                {
                                    Isin = numerator.Isin,
                                    Date = numerator.Date,
                                    Currency = numerator.Currency,
                                    Total_Yield = yield
                                });
                                this.context.SaveChanges();
                            }
                        }
                    }
                }
            }

            //foreach(var yieldItem in listYields)
            //{
            //    if (containsDate.Value.Month == yieldItem.Date.Value.Month)
            //    {
            //        var newRow = this.context.Total_Yields.Add(new Total_Yields()
            //            {
            //                Isin = yieldItem.Isin,
            //                Date = yieldItem.Date,
            //                Currency = yieldItem.Currency,
            //                Total_Yield = yieldItem.Yield,
            //                Total_Yield_Currency = yieldItem.Yield_Currency
            //            });
            //        this.context.SaveChanges();
            //    }
            //}

            //foreach (var yieldDenominatorItem in listNumerators)
            //{
            //    if (containsDate.Value.Month == yieldDenominatorItem.Date.Value.Month)
            //    {
            //        var updateRow = this.context.Yields_Denominators.Where(x => x.Isin == yieldDenominatorItem.Isin && x.Date == yieldDenominatorItem.Date && x.Currency == yieldDenominatorItem.Currency);
            //        foreach (var item in updateRow)
            //        {
            //            item.Sum_Currency = yieldDenominatorItem.Sum;
            //        }
            //        //var newRow = this.context.Yields_Denominators.Add(new Yields_Denominators()
            //        //{
            //        //    Isin = yieldDenominatorItem.Isin,
            //        //    Date = yieldDenominatorItem.Date,
            //        //    Currency = yieldDenominatorItem.Currency,
            //        //    Sum = yieldDenominatorItem.Sum
            //        //});
            //        this.context.SaveChanges();
            //    }
            //}

            //var listNumerators = (
            //    from numerator in this.context.Numerators
            //    select new FractionsViewModel()
            //    {
            //        Isin = numerator.Isin,
            //        Date = numerator.Date,
            //        Sum = numerator.Sum_Numerator
            //    }
            //    ).OrderBy(x => x.Date).ToList();

            //var listDenominators = (
            //    from denominator in this.context.Denominators
            //    select new FractionsViewModel()
            //    {
            //        Date = denominator.Date,
            //        Sum = denominator.Sum_Denominator
            //    }
            //    ).OrderBy(x => x.Date).ToList();



            //foreach (var denominator in listDenominators)
            //{
            //    foreach (var numerator in listNumerators)
            //    {
            //        if (denominator.Date == numerator.Date)
            //        {
            //            share = (numerator.Sum / denominator.Sum) * 100;
            //            var newShare = new SharesViewModel()
            //            {
            //                Isin = numerator.Isin,
            //                Date = numerator.Date,
            //                Share = share
            //            };
            //            listShares.Add(newShare);
            //        }
            //    }
            //}

            //foreach (var shareItem in listShares)
            //{
            //    if (containsDate.Value.Month == shareItem.Date.Value.Month)
            //    {
            //        var newRowShare = this.context.Shares.Add(new Shares()
            //        {
            //            Isin = shareItem.Isin,
            //            Date = shareItem.Date,
            //            Share = shareItem.Share
            //        });
            //        this.context.SaveChanges();
            //    }
            //}

            

            //var listNumerators = (
            //    from numerator in this.context.Registers_Yields
            //    select new FractionsViewModel()
            //    {
            //        Isin = numerator.Isin,
            //        Date = numerator.Date,
            //        Conto = numerator.Conto,
            //        Currency = numerator.Currency,
            //        Sum = numerator.Sum_Registers,
            //        Sum_Currency = numerator.Sum_Registers_Currency
            //    }
            //    ).OrderBy(x => x.Date).ToList();

            //var listDenominatorsFromNumerators = (
            //    from denominator in this.context.Numerators
            //    select new FractionsViewModel()
            //    {
            //        Isin = denominator.Isin,
            //        Date = denominator.Date,
            //        Sum = denominator.Sum_Numerator
            //    }
            //    ).OrderBy(x => x.Date).ToList();

            //foreach (var denominator in listDenominatorsFromNumerators)
            //{
            //    foreach (var numerator in listNumerators)
            //    {
            //        if (numerator.Currency == "MKD")
            //        {
            //            if (listContoNumerator.Contains(numerator.Conto.Substring(0, 3)))
            //            {
            //                if (denominator.Date == numerator.Date && denominator.Isin == numerator.Isin)
            //                {
            //                    if (denominator.Sum != 0)
            //                    {
            //                        yield = (numerator.Sum / denominator.Sum) * 100;
            //                        //yield_currency = (numerator.Sum_Currency / denominator.Sum) * 100;
            //                    }
            //                    else
            //                    {
            //                        yield = 0;
            //                        //yield_currency = 0;
            //                    }
            //                    var newYield = new YieldsViewModel()
            //                    {
            //                        Isin = numerator.Isin,
            //                        Date = numerator.Date,
            //                        Currency = numerator.Currency,
            //                        Yield = yield
            //                        //Yield_Currency = yield_currency
            //                    };
            //                    listYields.Add(newYield);
            //                }
            //            }
            //        }
            //    }
            //}

            //foreach (var yieldItem in listYields)
            //{
            //    if (containsDate.Value.Month == yieldItem.Date.Value.Month)
            //    {
            //        var newRowYield = this.context.Total_Yields.Add(new Total_Yields()
            //        {
            //            Isin = yieldItem.Isin,
            //            Date = yieldItem.Date,
            //            Currency = yieldItem.Currency,
            //            Total_Yield = yieldItem.Yield,
            //            //Total_Yield_Currency = yieldItem.Yield_Currency
            //        });
            //        this.context.SaveChanges();
            //    }
            //}


            //foreach (var dateItem in listDates)
            //{
            //    date = dateItem.Date.HasValue ? dateItem.Date.Value : new DateTime();
            //    foreach (var registerItem in listRegisters)
            //    {
            //        dateRegister = registerItem.Date.HasValue ? registerItem.Date.Value : new DateTime();
            //        if (date.Date == dateRegister.Date)
            //        {
            //            if (listContoDenominator.Contains(registerItem.Conto.Substring(0, 1)))
            //            {
            //                sumDenominator += (registerItem.Owe - registerItem.Claim);
            //                currency = registerItem.Currency;
            //            }
            //        }
            //    }
            //    var newRowDenominator = this.context.Denominators.Add(
            //        new Denominators()
            //        {
            //            Date = date,
            //            Sum_Denominator = sumDenominator
            //        }
            //        );
            //    this.context.SaveChanges();
            //}

            return View();
        }
    }
}