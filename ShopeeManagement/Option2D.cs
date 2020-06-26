using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class Option2D
    {
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public decimal Price { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Option2D o && Option1 == o.Option1 && Option2 == o.Option2;
        }

        public override int GetHashCode()
        {
            var hashCode = -2070893015;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Option1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Option2);
            return hashCode;
        }

        protected Option2D ToOption2Ds(string optionName1, string optionName2, string optionName3, string optionName4, string optionName5, decimal optionPrice)
        {
            var option1 = string.IsNullOrWhiteSpace(optionName1) ? "NA" : optionName1;

            var option2 = "";

            if (string.IsNullOrWhiteSpace(optionName2))
            {
                option2 = "NA";
            }
            else
            {
                option2 += optionName2;

                if (!string.IsNullOrWhiteSpace(optionName3))
                    option2 += $"_{optionName3}";

                if (!string.IsNullOrWhiteSpace(optionName4))
                    option2 += $"_{optionName4}";

                if (!string.IsNullOrWhiteSpace(optionName5))
                    option2 += $"_{optionName5}";
            }

            return new Option2D
            {
                Option1 = option1.Trim(),
                Option2 = option2.Trim(),
                Price = optionPrice
            };
        }
    }
}
