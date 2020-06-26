using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class PriceCalculator
    {
        public decimal SourcePrice { get; set; }
        public decimal Margin { get; set; }
        public double Weight { get; set; }
        public decimal PgFeeRate { get; set; }
        public decimal ShopeeRate { get; set; }
        public decimal PayoneerRate { get; set; }
        public decimal CurrencyRate { get; set; }
        public decimal RetailPriceRate { get; set; }
        public decimal SellPrice { get; set; }
        public string CountryCode { get; set; }

        public Dictionary<string, decimal> calculatePrice()
        {
            using (AppDbContext context = new AppDbContext())
            {
                Dictionary<string, decimal> result = new Dictionary<string, decimal>();

                //배송비
                decimal shippingFee = 0;

                //리베이트
                decimal rebate = 0;

                //히든피
                decimal hiddenFee = 0;

                //현지가격으로 바꾼 공급가
                decimal supplyPrice = 0;

                //쇼피수수료
                decimal shopeeFee = 0;

                //PG수수료
                decimal pgFee = 0;

                //판매가
                decimal targetSellPrice = 0;

                //소비자가
                decimal targetRetailPrice = 0;

                //판매가 원화
                decimal targetSellPriceKRW = 0;

                //소비자가 원화
                decimal targetRetailPriceKRW = 0;

                //구매자 배송 결제액
                decimal buyerPurchace = 0;


                if (CountryCode == "SG")
                {   

                    List<ShippingRateSlsSg> rateList = context.ShippingRateSlsSgs
                            .Where(a => a.Weight >= Weight)
                            .OrderBy(x => x.Weight).Take(1).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = rateList[0].ShippingFeeAvg;
                        hiddenFee = rateList[0].HiddenFee;

                        supplyPrice = (SourcePrice + Margin) / CurrencyRate;
                        decimal customerShippingFee = 0;

                        if (supplyPrice > 0 && supplyPrice <= 9.99m)
                        {
                            rebate = 1.99m;
                            buyerPurchace = 0;
                            customerShippingFee = 1.99m - 0m;
                        }
                        else if (supplyPrice >= 10 && supplyPrice <= 24.99m)
                        {
                            rebate = 1m;
                            buyerPurchace = 0.99m;
                            customerShippingFee = 1.99m - 1m;
                        }
                        else
                        {
                            rebate = 0m;
                            buyerPurchace = 1.99m;
                            customerShippingFee = 1.99m - 1.99m;

                        }

                        
                        shippingFee = shippingFee - 1.99m;

                        pgFee = (supplyPrice + customerShippingFee) * (PgFeeRate / 100) * 100 / 100;
                        shopeeFee = (supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100 / 100;
                        targetSellPrice = (supplyPrice + shippingFee + pgFee + shopeeFee) * 100 / 100;
                        targetRetailPrice = Math.Round(targetSellPrice * (RetailPriceRate / 100) * 10) / 10;
                        targetSellPriceKRW = Math.Round(targetSellPrice * CurrencyRate);
                        targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                           "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                           "배송요율 누락", System.Windows.Forms.MessageBoxButtons.OK,
                           System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (CountryCode == "ID")
                {   
                    if(Weight < 10)
                    {
                        Weight = 10;
                    }
                    else if(Weight > 30000)
                    {
                        Weight = 30000;
                    }

                    List<ShippingRateSlsId> rateList = context.ShippingRateSlsIds
                            .Where(a => a.Weight >= Weight)
                            .OrderBy(x => x.Weight).Take(1).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = rateList[0].ZoneA;
                        hiddenFee = rateList[0].HiddenFee;

                        supplyPrice = ((SourcePrice + Margin) / CurrencyRate) * 100;

                        if (supplyPrice >= 90000)
                        {
                            rebate = 0;
                            shippingFee = shippingFee - 10000;
                        }
                        else
                        {
                            rebate = 10000;
                            shippingFee = shippingFee - 10000;
                        }

                        pgFee = Math.Round((supplyPrice + rebate) * (PgFeeRate / 100) * 100) / 100;
                        shopeeFee = ((supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100) / 100;

                        targetSellPrice = supplyPrice + shippingFee + pgFee + shopeeFee;
                        targetRetailPrice = targetSellPrice * (RetailPriceRate / 100);
                        targetSellPriceKRW = Math.Round(targetSellPrice * CurrencyRate / 100);
                        targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate / 100);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                           "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                           "배송요율 누락", System.Windows.Forms.MessageBoxButtons.OK,
                           System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (CountryCode == "MY")
                {
                    //SLS로 변경되어 계산식 변경 2020-02-04
                    //SLS배송비 요율은 그램으로 되어 있으므로 킬로그램을 그램으로 변경 후 계산해야 함.
                    //Weight = (int)(Weight * 1000);                    
                    var rateList = context.ShippingRateSlsMys
                            .Where(a => a.Weight >= Weight)
                            .OrderBy(x => x.Weight).Take(1).ToList();

                    if (rateList.Count > 0)
                    {
                        //SLS로 변경되었으므로 요율표는 현지 말레이시아 금액으로 되어 있다. 환율 계산 안해도 됨
                        //배송비가 원화로 되어 있으므로 환산한다.
                        //실제 배송비에서 7이 무조건 지원된다. 쇼피가 다내주던 반반내던, 구매자가 다 내던 7링깃은 무조건 지원됨
                        //따라서 실제 배송비에서 7을 빼고 계산하면 된다.
                        
                        shippingFee = rateList[0].ZoneA - 4m;
                        supplyPrice = (SourcePrice + Margin) / CurrencyRate;
                        
                        if(supplyPrice >= 40)
                        {
                            rebate = 0;
                        }
                        else if(supplyPrice < 40 && supplyPrice >= 30)
                        {
                            rebate = 2;
                        }
                        else
                        {
                            rebate = 4;
                        }


                        pgFee = Math.Round((supplyPrice + rebate) * (PgFeeRate / 100) * 100) / 100;
                        shopeeFee = ((supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100) / 100;
                        targetSellPrice = Math.Round((supplyPrice + shippingFee + pgFee + shopeeFee) * 100) / 100;
                        targetRetailPrice = Math.Round(targetSellPrice * (RetailPriceRate / 100) * 100) / 100;
                        targetSellPriceKRW = Math.Round(targetSellPrice * CurrencyRate);
                        targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                            "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                            "배송요율 누락",System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (CountryCode == "TH")
                {
                    //원화로 되어 있으므로 환산한다.
                    List<ShippingRateDRTh> rateList = context.ShippingRateDRThs
                            .Where(a => a.Weight >= Weight)
                            .OrderBy(x => x.Weight).Take(1).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = rateList[0].ShippingFeeAvg / CurrencyRate;
                        supplyPrice = (SourcePrice + Margin) / CurrencyRate;

                        pgFee = (supplyPrice + shippingFee) * (PgFeeRate / 100) * 100 / 100;
                        shopeeFee = (supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100 / 100;
                        targetSellPrice = supplyPrice + shippingFee + pgFee + shopeeFee;
                        targetRetailPrice = targetSellPrice * (RetailPriceRate / 100);
                        targetSellPriceKRW = Math.Round(targetSellPrice * CurrencyRate);
                        targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                           "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                           "배송요율 누락", System.Windows.Forms.MessageBoxButtons.OK,
                           System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (CountryCode == "TW")
                {

                    //대만의 경우 배송비 55 보조
                    //따라서 배송비에서 55빼고 계산한다.
                    List<ShippingRateYTOTw> rateList = context.ShippingRateYTOTws
                            .Where(a => a.Weight > Weight)
                            .OrderBy(x => x.Weight).Take(1).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = (rateList[0].ShippingFeeAvg / CurrencyRate) - 55;
                        supplyPrice = (SourcePrice + Margin) / CurrencyRate;

                        pgFee = (supplyPrice + shippingFee) * (PgFeeRate / 100) * 100 / 100;
                        shopeeFee = (supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100 / 100;
                        targetSellPrice = supplyPrice + shippingFee + pgFee + shopeeFee;
                        targetRetailPrice = targetSellPrice * (RetailPriceRate / 100);
                        targetSellPriceKRW = Math.Round(targetSellPrice * CurrencyRate);
                        targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                           "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                           "배송요율 누락", System.Windows.Forms.MessageBoxButtons.OK,
                           System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (CountryCode == "VN")
                {
                    //원화로 되어 있으므로 환산한다.
                    List<ShippingRateDRVn> rateList = context.ShippingRateDRVns
                            .Where(a => a.Weight >= Weight)
                            .OrderBy(x => x.Weight).Take(1).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = (rateList[0].ShippingFeeAvg / CurrencyRate) * 100;
                        supplyPrice = ((SourcePrice + Margin) / CurrencyRate) * 100;

                        pgFee = (supplyPrice + shippingFee) * (PgFeeRate / 100) * 100 / 100;
                        shopeeFee = (supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100 / 100;
                        targetSellPrice = supplyPrice + shippingFee + pgFee + shopeeFee;
                        targetRetailPrice = targetSellPrice * (RetailPriceRate / 100);
                        targetSellPriceKRW = Math.Round(targetSellPrice * CurrencyRate);
                        targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                           "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                           "배송요율 누락", System.Windows.Forms.MessageBoxButtons.OK,
                           System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (CountryCode == "PH")
                {
                    List<ShippingRateSlsPh> rateList = context.ShippingRateSlsPhs
                            .Where(a => a.Weight <= Weight)
                            .OrderByDescending(x => x.Weight).Take(1).ToList();

                    decimal customerShippingFee = 0;

                    
                    if (rateList.Count > 0)
                    {
                        shippingFee = rateList[0].ZoneA - 50;
                        hiddenFee = rateList[0].HiddenFee;

                        supplyPrice = (SourcePrice + Margin) / CurrencyRate;

                        if(supplyPrice >= 500)
                        {
                            customerShippingFee = 0;
                        }
                        else
                        {
                            customerShippingFee = 50;
                        }

                        pgFee = (supplyPrice + customerShippingFee) * (PgFeeRate / 100) * 100 / 100;
                        shopeeFee = (supplyPrice + shippingFee + pgFee) * (ShopeeRate / 100) * 100 / 100;
                        targetSellPrice = supplyPrice + shippingFee + pgFee + shopeeFee;
                        targetRetailPrice = targetSellPrice * (RetailPriceRate/100);
                        targetSellPriceKRW = targetSellPrice * CurrencyRate;
                        targetRetailPriceKRW = targetRetailPrice * CurrencyRate;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("배송비 요율이 없습니다." +
                           "\r\n상단메뉴의 배송비 설정에서 배송요율을 업로드 하여 주세요.",
                           "배송요율 누락", System.Windows.Forms.MessageBoxButtons.OK,
                           System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }

                result.Add("pgFee", pgFee);
                result.Add("targetSellPrice", targetSellPrice);
                result.Add("targetRetailPrice", targetRetailPrice);
                result.Add("targetSellPriceKRW", targetSellPriceKRW);
                result.Add("targetRetailPriceKRW", targetRetailPriceKRW);
                result.Add("currencyRate", CurrencyRate);

                return result;
            }   
        }

        public Dictionary<string, decimal> calculatePriceFromSellPrice()
        {
            using (AppDbContext context = new AppDbContext())
            {
                Dictionary<string, decimal> result = new Dictionary<string, decimal>();

                //배송비
                decimal shippingFee = 0;

                //리베이트
                decimal rebate = 0;

                //현지가격으로 바꾼 공급가
                decimal supplyPrice = 0;

                //PG수수료
                decimal pgFee = 0;

                //판매가
                decimal targetSellPrice = 0;

                //소비자가
                decimal targetRetailPrice = 0;

                //판매가 원화
                decimal targetSellPriceKRW = 0;

                //소비자가 원화
                decimal targetRetailPriceKRW = 0;

                //오더인컴
                decimal orderIncome = 0;

                decimal targetMarginKRW = 0;

                if (CountryCode == "SG")
                {
                    Weight = (Weight + 9) / 10 * 10;
                    List<ShippingRateSlsSg> rateList = context.ShippingRateSlsSgs
                            .Where(a => a.Weight == Weight)
                            .OrderByDescending(x => x.Weight).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = rateList[0].ShippingFeeAvg;
                        rebate = rateList[0].HiddenFee;
                    }

                    if (SellPrice > 20)
                    {
                        //소수 2째까지
                        pgFee = Math.Round((SellPrice) * (PgFeeRate / 100) * 100) / 100;
                        orderIncome = SellPrice - pgFee;
                        rebate = 1.99m;
                    }
                    else if (SellPrice > 10 && SellPrice <= 20)
                    {
                        pgFee = Math.Round((SellPrice + 0.99m) * (PgFeeRate / 100) * 100) / 100;
                        orderIncome = SellPrice - pgFee;
                        rebate = 1m;
                    }
                    else
                    {
                        pgFee = Math.Round((SellPrice + 1.99m) * (PgFeeRate / 100) * 100) / 100;
                        orderIncome = SellPrice - pgFee;
                        rebate = 0m;
                    }

                    //최종금액 산출
                    orderIncome = orderIncome - shippingFee + rebate;

                    //원화로 환산
                    decimal tempKRW = orderIncome * CurrencyRate;

                    //공급가를 빼준다. 마진 계산
                    tempKRW = Math.Round(tempKRW - SourcePrice);
                    targetMarginKRW = tempKRW;

                    //판매가의 한국 원화
                    targetSellPriceKRW = Math.Round(SellPrice * CurrencyRate);

                    //소비자가
                    targetRetailPrice = Math.Round(SellPrice * (RetailPriceRate / 100) * 10) / 10;

                    //소비자가 원화
                    targetRetailPriceKRW = Math.Round(targetRetailPrice * CurrencyRate);

                    targetSellPrice = SellPrice;
                }
                else if (CountryCode == "ID")
                {
                    Weight = (Weight + 9) / 10 * 10;
                    List<ShippingRateSlsId> rateList = context.ShippingRateSlsIds
                            .Where(a => a.Weight == Weight)
                            .OrderByDescending(x => x.Weight).ToList();

                    if (rateList.Count > 0)
                    {
                        shippingFee = rateList[0].ZoneA;
                        rebate = rateList[0].HiddenFee;
                    }
                }
                else if (CountryCode == "MY")
                {

                }
                else if (CountryCode == "TH")
                {

                }
                else if (CountryCode == "TW")
                {

                }
                else if (CountryCode == "VN")
                {

                }
                else if (CountryCode == "PH")
                {

                }

                result.Add("pgFee", pgFee);
                result.Add("targetSellPrice", targetSellPrice);
                result.Add("targetRetailPrice", targetRetailPrice);
                result.Add("targetSellPriceKRW", targetSellPriceKRW);
                result.Add("targetRetailPriceKRW", targetRetailPriceKRW);
                result.Add("targetMarginKRW", targetMarginKRW);
                result.Add("currencyRate", CurrencyRate);

                return result;
            }
        }
    }
}
