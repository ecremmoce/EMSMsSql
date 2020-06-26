using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MoreLinq;

namespace ShopeeManagement
{
    class AtiveCategory
    {
        public List<Tuple<string, string>> Categories { get; set; } = new List<Tuple<string, string>>();



        public AtiveCategory(string categoryHtml)
        {
            var myDoc = new HtmlDocument();
            myDoc.LoadHtml(categoryHtml);
            Categories.Add(Tuple.Create(myDoc.DocumentNode.SelectSingleNode("//div[@class='categoryName1']").InnerText.Trim(),
                myDoc.DocumentNode.SelectSingleNode("//div[@class='categoryCode1']").InnerText.Trim()));
            Categories.Add(Tuple.Create(myDoc.DocumentNode.SelectSingleNode("//div[@class='categoryName2']").InnerText.Trim(),
                myDoc.DocumentNode.SelectSingleNode("//div[@class='categoryCode2']").InnerText.Trim()));
            Categories.Add(Tuple.Create(myDoc.DocumentNode.SelectSingleNode("//div[@class='categoryName3']").InnerText.Trim(),
                myDoc.DocumentNode.SelectSingleNode("//div[@class='categoryCode3']").InnerText.Trim()));

            //categoryDoc.QuerySelectorAll("li a").ForEach(a =>
            //{
            //    if (a.GetAttribute("href") != "/" && a.TextContent != "쿠팡 홈")
            //    {
            //        Categories.Add(Tuple.Create(a.GetAttribute("href").Split('/').Last(), a.TextContent.Trim()));
            //    }
            //    else
            //    {
            //        Categories.Add(Tuple.Create("1234","의류" ));
            //    }
            //});
        }
    }
}
