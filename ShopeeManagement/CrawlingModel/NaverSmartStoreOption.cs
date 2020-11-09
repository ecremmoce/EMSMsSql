using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement.CrawlingModel
{
    public class NaverSmartStoreOption : IConvertible
    {
        List<NaverSmartStoreOptionData> Datas { get; set; }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }

    public class NaverSmartStoreOptionData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("optionName1")]
        public string OptionName1 { get; set; }

        [JsonProperty("optionName2")]
        public string OptionName2 { get; set; }

        [JsonProperty("optionName3")]
        public string OptionName3 { get; set; }

        [JsonProperty("optionName4")]
        public string OptionName4 { get; set; }

        [JsonProperty("optionName5")]
        public string OptionName5 { get; set; }

        [JsonProperty("stockQuantity")]
        public int StockQuantity { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("regOrder")]
        public int RegOrder { get; set; }

        [JsonProperty("registerDate")]
        public DateTime RegisterDate { get; set; }

        [JsonProperty("sellerManagerCode")]
        public string SellerManagerCode { get; set; }
    }
}
