using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class cls_currency
    {
        public void clear_currency()
        {
            using (AppDbContext context = new AppDbContext())
            {
                context.Database.ExecuteSqlCommand("delete from CurrencyRates where UserId='" + global_var.userId + "'");
            }
        }
        public string currency_date()
        {
            string rtn = string.Empty;
            using (AppDbContext context = new AppDbContext())
            {
                var currency = context.CurrencyRates.Where(x => x.UserId == global_var.userId).Take(1);
               

            }
                
            return rtn;
        }
        public Dictionary<string, decimal> get_currency()
        {
            Dictionary<string, decimal> dic_currency = new Dictionary<string, decimal>();
            using (AppDbContext context = new AppDbContext())
            {
                List<CurrencyRate> currencyList = context.CurrencyRates
                    .Where(x => (x.cr_name.Contains("IDR")
                    || x.cr_name.Contains("SGD")
                    || x.cr_name.Contains("MYR")
                    || x.cr_name.Contains("VND")
                    || x.cr_name.Contains("THB")
                    || x.cr_name.Contains("TWD")
                    || x.cr_name.Contains("PHP"))
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.cr_name).ToList();
                for (int i = 0; i < currencyList.Count; i++)
                {
                    dic_currency.Add(currencyList[i].cr_name.ToString(),
                        currencyList[i].cr_exchange);
                }
            }
            
            return dic_currency;
        }
        public void update_currency(ArrayList ar_currency_data)
        {
            ArrayList ar_price_type = new ArrayList();
            clear_currency();

            for (int i = 0; i < ar_currency_data.Count; i++)
            {
                string[] data = (string[])ar_currency_data[i];

                using (AppDbContext context = new AppDbContext())
                {
                    CurrencyRate newCurrency = new CurrencyRate
                    {
                        cr_code = data[0].ToString(),
                        cr_name = data[1].ToString(),
                        cr_exchange = Convert.ToDecimal(data[2].ToString()),
                        cr_save_date = Convert.ToDateTime(data[3].ToString()),
                        UserId = global_var.userId
                    };
                    context.CurrencyRates.Add(newCurrency);
                    context.SaveChanges();
                }
            }
        }
    }
}
