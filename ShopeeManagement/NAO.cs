using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class NAO
    {
        public string mid { get; set; }
        public string mtyp { get; set; }
        public string vtyp { get; set; }
        public string catid { get; set; }
        public string catnm { get; set; }

        public string pid { get; set; }
        public string pnm { get; set; }
        public string lcatid { get; set; }
        public string lcatnm { get; set; }
        public string mcatid { get; set; }
        public string mcatnm { get; set; }
        public string scatid { get; set; }
        public string scatnm { get; set; }
        public string dcatid { get; set; }
        public string dcatnm { get; set; }



        public NAO(string html)
        {
            var pFrom = html.IndexOf("var _nao = {};") + "var _nao = {};".Length;
            var pTo = html.LastIndexOf("aCategoryList");
            var naoPart = html.Substring(pFrom, pTo - pFrom);


            mid = Parse_nao(naoPart, "mid");
            mtyp = Parse_nao(naoPart, "mtyp");
            vtyp = Parse_nao(naoPart, "vtyp");
            catid = Parse_nao(naoPart, "catid");
            catnm = Parse_nao(naoPart, "catnm");

            pid = Parse_nao(naoPart, "pid");
            pnm = Parse_nao(naoPart, "pnm");
            lcatid = Parse_nao(naoPart, "lcatid");
            lcatnm = Parse_nao(naoPart, "lcatnm");
            mcatid = Parse_nao(naoPart, "mcatid");
            mcatnm = Parse_nao(naoPart, "mcatnm");
            scatid = Parse_nao(naoPart, "scatid");
            scatnm = Parse_nao(naoPart, "scatnm");
            dcatid = Parse_nao(naoPart, "dcatid");
            dcatnm = Parse_nao(naoPart, "dcatnm");
        }



        private string Parse_nao(string html, string propertyName)
        {
            var regex = new Regex($@"(?<=_nao\['{propertyName}'\]\s*=\s*"")(.*)(?="";)");
            var match = regex.Match(html);
            if (!match.Success) return "";

            return match.Value;
        }
    }
}
