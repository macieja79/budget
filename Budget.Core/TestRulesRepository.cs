using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Budget.Core.UI;

namespace Budget.Core
{
    public class TestRulesRepository : IRulesRepository
    {
        public List<Rule> GetRules()
        {
            List<Rule> rules = new List<Rule>();

            object[] parameters = new object[] {rules};

            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MethodInfo m in methods)
            {
                if (m.IsPublic) continue;

                ParameterInfo[] iParams = m.GetParameters();
                if (iParams.Length != 1) continue;


                m.Invoke(this, parameters);
            }





            return rules;
        }

        List<Category> _categories = new List<Category>();

        public void SaveRules(List<Rule> rules)
        {

        }

        /*
        void Auto(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Category = "AUTO",
                SubCategory = "PALIWO",
                Value = "*ORLEN;LOTOS;ARTUS;STATOIL",
                Color = "#FF8080"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Category = "AUTO",
                SubCategory = "SERWIS",
                Value = "^SERWIS OPON"
            });

        }


        void Jedzenie(List<Rule> rules)
        {

            rules.Add(new Rule
            {
                Category = "JEDZENIE",
                SubCategory = "MARKET",
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value =
                    "^LIDL;ZABKA;STOKROTKA;LEWIATAN;RENETA;SKLEP MIESNY;CARREFOUR;TESCO;FOCZKA;KAUFLAND;MEGANA;MARKET PUNK;PIOTR I PAWEL;*BIEDRONKA"

            });

            rules.Add(new Rule
            {
                Category = "JEDZENIE",
                SubCategory = "RESTAURACJA",
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value = "^RESTAURACJA"
            });

        }

        void Dom(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "26249010287000003700003859",
                Category = "DOM",
                SubCategory = "INTERNET"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "80103019447059070000634961",
                Category = "DOM",
                SubCategory = "WODA"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "95106001359150006800009100",
                Category = "DOM",
                SubCategory = "CZYNSZ"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "06103019447141000000588983",
                Category = "DOM",
                SubCategory = "GAZ"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "29105000996027010016587938",
                Category = "DOM",
                SubCategory = "PRAD"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "45160015052222010011544595",
                Category = "DOM",
                SubCategory = "TELEFON"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "36109000047777010016486986",
                Category = "DOM",
                SubCategory = "TELEFON"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value = "^IKEA;CASTORAMA",
                Category = "DOM",
                SubCategory = "SPRZET"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "74105000441000002351944471",
                Category = "DOM",
                SubCategory = "KREDYT"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value = "^MEDIA MARKT",
                Category = "DOM",
                SubCategory = "ELEKTRONIKA"
            });

        }

        void Finanse(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                Category = "FINANSE",
                SubCategory = "SOCJAL",
                TypeOfRule = Rule.RuleType.Account,
                Value = "92102028920000500205901055"

            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Title,
                Value = "^WYPŁATA GOTÓWKI",
                Category = "BANKOMAT",
                SubCategory = ""
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "53147000022406909350000009",
                Category = "FINANSE",
                SubCategory = "OSZCZĘDNOŚCI"
            });


            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Title,
                Value = "^PROWIZJA WYPŁATA GOTÓWKI",
                Category = "FINANSE",
                SubCategory = "PROWIZJE"
            });

        }

        void Zus(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "78101010230000261395200000",
                Category = "ZUS",
                SubCategory = "ZDROWOTNE"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "83101010230000261395100000",
                Category = "ZUS",
                SubCategory = "SPOLECZNE"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "73101010230000261395300000",
                Category = "ZUS",
                SubCategory = "FUNDUSZ"
            });
        }

        void Kultura(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                Category = "KULTURA",
                SubCategory = "KSIĄŻKI",
                TypeOfRule = Rule.RuleType.Title,
                Value = "*KSIĘGARNIA, EMPIK",
            });

        }

        void Edukacja(List<Rule> rules)
        {

        }

        void Ciuchy(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value = "^LPP",
                Category = "CIUCHY",
                SubCategory = "RESERVED"
            });
        }

        void Przychod(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "78116022020000000098401147",
                Category = "FV",
                SubCategory = "ANTAL"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "58203000451110000000345060",
                Category = "FV",
                SubCategory = "GSBK"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "77150011261211200816260000",
                Category = "PRZYCHOD",
                SubCategory = "LANG"
            });


            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "84124050801111000051942944",
                Category = "FV",
                SubCategory = "DPK"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "19114020040000300230760499",
                Category = "FV",
                SubCategory = "ANGLOKOM"
            });
        }

        void ToRemove(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Category = BudgetConst.CategoryInternal,
                Value = "93105013601000002210158438;25105014451000002270574334;27105013601000002282752001",
            });
        }

        void Dzieci(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "21102028920000580205902814;68106000760000320001434014",
                Category = "DZIECI",
                SubCategory = "PRZEDSZKOLE"
            });
        }

        void Tax(List<Rule> rules)
        {

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "96101012700004462223000000",
                Category = "TAX",
                SubCategory = "PIT"
            });

            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.Account,
                Value = "49101012700004462222000000",
                Category = "TAX",
                SubCategory = "VAT"
            });
        }

        void Zdrowie(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value = "APTEKA",
                Category = "ZDROWIE",
                SubCategory = "LEKI"
            });

            rules.Add(new Rule
            {
                Category = "ZDROWIE",
                SubCategory = "WELLNESS",
                TypeOfRule = Rule.RuleType.CounterpartData,
                Value = "ROSSMAN;DROGERIA NATURA;SALON URODY",

            });








        }

        void Sport(List<Rule> rules)
        {


            rules.Add(new Rule
            {
                Category = "SPORT",
                SubCategory = "SPRZET",
                TypeOfRule = Rule.RuleType.CounterpartData,

                Value = "DECATHLON;MEGASKLEP SPORTOWY"
            });

            rules.Add(new Rule
            {
                Category = "SPORT",
                SubCategory = "BASEN",
                TypeOfRule = Rule.RuleType.CounterpartData,

                Value = "AQUA KOREKT"
            });

        }

        void Rozrywka(List<Rule> rules)
        {
            rules.Add(new Rule
            {
                TypeOfRule = Rule.RuleType.CounterpartData,
                Category = "ROZRYWKA",
                SubCategory = "KNAJPA",
                Value = "^KTO WYPUSCIL SKOWRONKA;KLUB NA KOTLOWEM"
            });

        }
        */

      



    public List<Category> GetCategoriesFromFile(string file)
        {



            string[] lines = file.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            List<Category> categories = new List<Category>();

            foreach (var line in lines)
            {
                string[] items = line.SplitAndTrim(':');
                string categoryName = items[0];
                string[] subCategories = items[1].SplitAndTrim(',');

                Category category = Category.Create(categoryName, subCategories);

                categories.Add(category);

            }

            return categories;







        }


    






    }



    }
