using MetroFramework.Forms;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormProductMapper : MetroForm
    {
        public FormProductMapper(string lang)
        {
            InitializeComponent();
        }
        private void set_double_buffer()
        {
            System.Windows.Forms.Control[] controls = GetAllControlsUsingRecursive(this);
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i].GetType() == typeof(DataGridView))
                {
                    ((DataGridView)controls[i]).DoubleBuffered(true);
                }
            }
        }

        static System.Windows.Forms.Control[] GetAllControlsUsingRecursive(System.Windows.Forms.Control containerControl)
        {
            List<System.Windows.Forms.Control> allControls = new List<System.Windows.Forms.Control>();
            Queue<System.Windows.Forms.Control.ControlCollection> queue = new Queue<System.Windows.Forms.Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);
            while (queue.Count > 0)
            {
                System.Windows.Forms.Control.ControlCollection controls = queue.Dequeue();
                if (controls == null || controls.Count == 0) continue;
                foreach (System.Windows.Forms.Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }
            return allControls.ToArray();
        }
        
        
        private void FormProductMapper_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            set_double_buffer();

            CboSrcSearchArea.SelectedIndex = 0;
            CboTarSearchArea.SelectedIndex = 0;
            getShopeeAccount();
            Cursor.Current = Cursors.Default;
        }
        
        
        
        
        private void getShopeeAccount()
        {
            dg_site_id.Rows.Clear();
            try
            {
                ClsShopee cc = new ClsShopee();
                IList<ShopeeAccount> ShopeeAccounts = cc.GetShopeeAccountList();
                for (int i = 0; i < ShopeeAccounts.Count; i++)
                {
                    dg_site_id.Rows.Add(i + 1, ShopeeAccounts[i].ShopeeId,
                        ShopeeAccounts[i].ShopeeCountry,
                        ShopeeAccounts[i].PartnerId,
                        ShopeeAccounts[i].ShopId,
                        ShopeeAccounts[i].SecretKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getShopeeAccount: " + ex.Message);
            }

        }
        private void BtnSetSource_Click(object sender, EventArgs e)
        {
            TxtSourceCountry.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            TxtSourceId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
        }

        private void BtnSetTarget_Click(object sender, EventArgs e)
        {
            TxtTargetCountry.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            TxtTargetId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();



            if (TxtSourceId.Text.Trim() != string.Empty && TxtTargetId.Text.Trim() != string.Empty)
            {
                btnGetSrcItemList_Click(null, null);
            }
            
            Cursor.Current = Cursors.Default;
        }
        
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }        
        public static string ByteToString(byte[] buff)
        {
            string sbinary = BitConverter.ToString(buff).Replace("-", "");
            return (sbinary);
        }
        
        
        
        public delegate void InvokeDelegate();
        private void btnGetSrcItemList_Click(object sender, EventArgs e)
        {
            if(TxtSourceId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("원본을 지정해 주세요.", "원본 미지정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (TxtTargetId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("대상을 지정해 주세요.", "대상 미지정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (TxtSourceId.Text.Trim() != string.Empty && TxtTargetId.Text.Trim() != string.Empty)
            {
                if (TxtSourceId.Text.Trim().ToUpper() == TxtTargetId.Text.Trim().ToUpper())
                {
                    MessageBox.Show("같은 아이디를 설정할 수 없습니다.", "동일 아이디", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DataGridView dv = dgSrcItemList;
            getProductList("", "", dv, TxtSourceId.Text.Trim());

            DataGridView dv2 = dgTarItemList;
            getProductList("", "", dv2, TxtTargetId.Text.Trim());

            displayLinked();


            MessageBox.Show("상품 목록을 로드하였습니다.", "상품목록 로드", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void displayLinked()
        {
            //상품 목록을 가지고 올때 연결된 상품 정보도 가지고 온다.
            using (AppDbContext context = new AppDbContext())
            {
                List<ProductLink> accountList = context.ProductLinks.Where(
                    a => a.SourceCountry == TxtSourceCountry.Text &&
                    a.TargetCountry == TxtTargetCountry.Text
                    && a.UserId == global_var.userId)
                    .OrderBy(x => x.SourceProductId).ToList();

                for (int i = 0; i < accountList.Count; i++)
                {
                    for (int j = 0; j < dgSrcItemList.Rows.Count; j++)
                    {
                        long ItmeId = Convert.ToInt64(dgSrcItemList.Rows[j].Cells["dgItemList_item_id"].Value.ToString());
                        if (accountList[i].SourceProductId == ItmeId)
                        {
                            dgSrcItemList.Rows[j].DefaultCellStyle.BackColor = Color.GreenYellow;
                            break;
                        }
                    }
                }

                for (int i = 0; i < accountList.Count; i++)
                {
                    for (int j = 0; j < dgTarItemList.Rows.Count; j++)
                    {
                        long ItmeIdTar = Convert.ToInt64(dgTarItemList.Rows[j].Cells["dgItemList_item_id_tar"].Value.ToString());
                        if (accountList[i].TargetProductId == ItmeIdTar)
                        {
                            dgTarItemList.Rows[j].DefaultCellStyle.BackColor = Color.GreenYellow;
                            break;
                        }
                    }
                }
            }
        }
        private void getProductList(string area, string keyword, DataGridView dv, string shopeeId)
        {
            dv.Rows.Clear();
            if (dg_site_id.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                decimal rateSrc = 0;
                string currencyDigit = "";
                string countryCode = "";

                if (shopeeId.ToUpper().Contains(".ID"))
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "IDR";
                }
                else if (shopeeId.ToUpper().Contains(".MY"))
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "MYR";
                }
                else if (shopeeId.ToUpper().Contains(".SG"))
                {
                    currencyDigit = "{0:n2}";
                    countryCode = "SGD";
                }
                else if (shopeeId.ToUpper().Contains(".TW"))
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "TWD";
                }
                else if (shopeeId.ToUpper().Contains(".TH"))
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "THB";
                }
                else if (shopeeId.ToUpper().Contains(".PH"))
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "PHP";
                }

                //환율을 가지고 온다.
                

                using (AppDbContext context = new AppDbContext())
                {
                    List<ItemInfo> productList = null;
                    if (area == "상품ID")
                    {
                        long ItemId = 0;
                        if (keyword != string.Empty)
                        {
                            if (long.TryParse(keyword, out ItemId))
                            {
                                productList = context.ItemInfoes
                                .Where(b => b.shopeeAccount == shopeeId &&
                                b.item_id == ItemId
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                            }
                        }
                    }
                    else if (area == "상품SKU")
                    {
                        if (keyword != string.Empty)
                        {
                            productList = context.ItemInfoes
                                .Where(b => b.shopeeAccount == shopeeId &&
                                b.item_sku.Contains(keyword)
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                        }
                    }
                    else if (area == "상품명")
                    {
                        if (keyword != string.Empty)
                        {
                            productList = context.ItemInfoes
                                .Where(b => b.shopeeAccount == shopeeId &&
                                b.name.Contains(keyword)
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                        }
                    }
                    else if (area == "")
                    {
                        productList = context.ItemInfoes
                        .Where(b => b.shopeeAccount == shopeeId
                        && b.UserId == global_var.userId)
                        .OrderBy(x => x.update_time).ToList();
                    }

                    if (productList != null)
                    {
                        progressBar.Value = 0;
                        progressBar.Maximum = productList.Count;
                        for (int i = 0; i < productList.Count; i++)
                        {
                            string strImage = productList[i].images;

                            if (strImage.Length > 0)
                            {
                                var arImages = strImage.Split('^');
                                if (arImages.Length > 0)
                                {
                                    strImage = arImages[0].ToString().Trim();
                                }
                            }

                            decimal SrcPrice = productList[i].price;
                            //SrcPrice = 275310m;
                            //원화로 변경한다.
                            decimal transKRW = 0;

                            decimal pg_fee = 0;

                            decimal PuchasePrice = 0;

                            if (countryCode == "SGD")
                            {
                                //PG수수료 계산
                                if (SrcPrice > 20)
                                {
                                    pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m) * 100) / 100;
                                    PuchasePrice = SrcPrice;
                                }
                                else if (SrcPrice > 10 && SrcPrice <= 20)
                                {
                                    pg_fee = Math.Round(decimal.Multiply((SrcPrice + 1.99m - 1m), 0.02m) * 100) / 100;
                                    PuchasePrice = SrcPrice + 0.99m;
                                }
                                else
                                {
                                    pg_fee = Math.Round(decimal.Multiply((SrcPrice + 1.99m), 0.02m) * 100) / 100;
                                    PuchasePrice = SrcPrice + 1.99m;
                                }
                            }
                            else if (countryCode == "IDR")
                            {
                                //PG수수료 계산
                                //무조건 A Zone으로 간주하여 계산한다.
                                if (SrcPrice >= 90000)
                                {
                                    PuchasePrice = SrcPrice;
                                    pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m));
                                }
                                else
                                {
                                    PuchasePrice = SrcPrice + 20000m;
                                    pg_fee = Math.Round(decimal.Multiply(PuchasePrice, 0.02m));
                                }
                            }
                            else if (countryCode == "MYR")
                            {
                                PuchasePrice = SrcPrice;
                                pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m));
                            }
                            else if (countryCode == "THB")
                            {
                                PuchasePrice = SrcPrice;
                                pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m));
                            }
                            else if (countryCode == "TWD")
                            {
                                PuchasePrice = SrcPrice;
                                pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m));
                            }
                            else if (countryCode == "VND")
                            {
                                PuchasePrice = SrcPrice;
                                pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m));
                            }
                            else if (countryCode == "PHP")
                            {
                                PuchasePrice = SrcPrice;
                                pg_fee = Math.Round(decimal.Multiply(SrcPrice, 0.02m));
                            }


                            //무게에 따른 배송비를 구한다.
                            decimal shippingFee = 0;
                            decimal rebate = 0;
                            int iWeight = Convert.ToInt32(productList[i].weight);
                            if (countryCode == "SGD")
                            {
                                iWeight = (iWeight + 9) / 10 * 10;
                                List<ShippingRateSlsSg> rateList = context.ShippingRateSlsSgs
                                        .Where(a => a.Weight == iWeight)
                                        .OrderByDescending(x => x.Weight).ToList();

                                if (rateList.Count > 0)
                                {
                                    shippingFee = rateList[0].ShippingFeeAvg;
                                    rebate = rateList[0].HiddenFee;
                                }
                            }
                            else if (countryCode == "IDR")
                            {
                                iWeight = (iWeight + 9) / 10 * 10;
                                List<ShippingRateSlsId> rateList = context.ShippingRateSlsIds
                                        .Where(a => a.Weight == iWeight)
                                        .OrderByDescending(x => x.Weight).ToList();

                                if (rateList.Count > 0)
                                {
                                    shippingFee = rateList[0].ZoneA;
                                    rebate = rateList[0].HiddenFee;
                                }
                            }
                            else if (countryCode == "MYR")
                            {

                            }
                            else if (countryCode == "TWD")
                            {

                            }
                            else if (countryCode == "THB")
                            {

                            }
                            else if (countryCode == "VND")
                            {

                            }
                            else if (countryCode == "PHP")
                            {

                            }

                            //공급가가 존재한다면 마진을 계산해 준다.

                            decimal margin = 0;

                            if (countryCode.Contains("JPN") ||
                            countryCode.Contains("IDR") ||
                            countryCode.Contains("VND"))
                            {
                                transKRW = SrcPrice * rateSrc / 100;

                                //판가 - 배송비 - pg수수료  + 리베이트 - 공급가 = 마진
                                margin = (((PuchasePrice - shippingFee - pg_fee + rebate) * rateSrc) / 100) - productList[i].supply_price;
                                margin = Math.Round(margin);
                            }
                            else
                            {
                                transKRW = SrcPrice * rateSrc;

                                //판가 - 배송비 - pg수수료  + 리베이트 - 공급가 = 마진
                                margin = (PuchasePrice - shippingFee - pg_fee + rebate) * rateSrc - productList[i].supply_price;
                                margin = Math.Round(margin);
                            }

                            System.Drawing.Image img = null;
                            dv.Rows.Add(
                                i + 1,
                                false,
                                img,
                                productList[i].item_id,
                                productList[i].shopid,
                                productList[i].item_sku,
                                productList[i].status,
                                productList[i].name,
                                productList[i].currency,
                                (bool)productList[i].has_variation,
                                string.Format("{0:n0}", productList[i].supply_price),
                                string.Format("{0:n0}", margin),
                                shippingFee,
                                string.Format(currencyDigit, pg_fee),
                                string.Format("{0:n0}", transKRW),
                                string.Format(currencyDigit, productList[i].price),
                                string.Format(currencyDigit, productList[i].original_price),
                                string.Format("{0:n0}", productList[i].stock),
                                productList[i].create_time,
                                productList[i].update_time,
                                string.Format("{0:n0}", productList[i].weight),
                                productList[i].category_id,
                                Math.Truncate(productList[i].rating_star * 100) / 100,
                                string.Format("{0:n0}", productList[i].cmt_count),
                                string.Format("{0:n0}", productList[i].sales),
                                string.Format("{0:n0}", productList[i].views),
                                string.Format("{0:n0}", productList[i].likes),
                                productList[i].package_length,
                                productList[i].package_width,
                                productList[i].package_height,
                                productList[i].days_to_ship,
                                productList[i].size_chart,
                                productList[i].condition,
                                productList[i].discount_id,
                                productList[i].is_2tier_item,
                                strImage);

                            if (productList[i].isChanged)
                            {
                                dv.Rows[dv.Rows.Count - 1].Cells[3].Style.BackColor = Color.Orange;
                            }
                        }

                        CancellationTokenSource cts = new CancellationTokenSource();

                        Task.Run(() =>
                        {
                            try
                            {
                                Parallel.ForEach(dv.Rows.Cast<DataGridViewRow>(),
                                    new ParallelOptions { MaxDegreeOfParallelism = 200 }
                                    , dgRow =>
                                    {
                                        var retryCount = 0;
                                        Retry:
                                        try
                                        {
                                            if (cts.IsCancellationRequested) return;

                                            var iUrl = dgRow.Cells[35].Value.ToString() as string;
                                            var ic = dgRow.Cells[2] as DataGridViewImageCell;

                                            var request = WebRequest.Create(iUrl);
                                            var response = (HttpWebResponse)request.GetResponse();
                                            var dataStream = response.GetResponseStream();

                                            ic.Value = new Bitmap(dataStream);

                                            progressBar.BeginInvoke(new InvokeDelegate(() => Progress(cts)));
                                        }
                                        catch
                                        {
                                            if (++retryCount < 3)
                                            {
                                                Thread.Sleep(retryCount * 1000);
                                                goto Retry;
                                            }

                                        }
                                    });
                            }
                            catch
                            {
                                // ignore
                            }
                        });
                    }
                }
                if(dv.Name == "dgSrcItemList")
                {
                    grp_src.Text = "원본 상품 목록 : [ " + string.Format("{0:n0}", dv.Rows.Count) + " ]";
                }
                else
                {
                    grp_tar.Text = "대상 상품 목록 : [ " + string.Format("{0:n0}", dv.Rows.Count) + " ]";
                }
                
                dv.ClearSelection();
                Cursor.Current = Cursors.Default;
            }
        }
        public void Progress(CancellationTokenSource cts)
        {
            try
            {
                if (!cts.IsCancellationRequested) progressBar.Value++;
            }
            catch
            {
                // ignored
            }
        }

        private void btnGetTarItemList_Click(object sender, EventArgs e)
        {
            
        }

        private void arrangeNo(DataGridView dv)
        {
            for (int i = 0; i < dv.Rows.Count; i++)
            {
                dv.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void btn_link_product_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.SelectedRows.Count > 0 && dgTarItemList.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    long srcProductId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());
                    long tarProductId = Convert.ToInt64(dgTarItemList.SelectedRows[0].Cells["dgItemList_item_id_tar"].Value.ToString());
                    ProductLink result = context.ProductLinks.SingleOrDefault(
                        b => b.SourceCountry == TxtSourceCountry.Text.Trim() &&
                        b.TargetCountry == TxtTargetCountry.Text.Trim() &&
                        b.SourceProductId == srcProductId
                        && b.UserId == global_var.userId);

                    //만약 있는 경우라면 옮기는 것이므로 기존꺼는 흰색으로 새로 연결한 놈은 녹색으로
                    if (result == null)
                    {
                        ProductLink newProductLink = new ProductLink
                        {
                            SourceCountry = TxtSourceCountry.Text.Trim(),
                            TargetCountry = TxtTargetCountry.Text.Trim(),
                            SourceAccount = TxtSourceId.Text.Trim(),
                            SourceProductId = srcProductId,
                            TargetProductId = tarProductId,
                            TargetAccount = TxtTargetId.Text.Trim(),
                            UserId = global_var.userId
                        };

                        context.ProductLinks.Add(newProductLink);
                        context.SaveChanges();

                        dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                        dgTarItemList.Rows[dgTarItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                        dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[0].Index);
                        dgTarItemList.Rows.RemoveAt(dgTarItemList.SelectedRows[0].Index);

                        arrangeNo(dgSrcItemList);
                        arrangeNo(dgTarItemList);
                    }
                    else
                    {
                        for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                        {
                            if(tarProductId == result.TargetProductId)
                            {
                                dgTarItemList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                        //이 경우 이전에 다른놈과 연결되어 있으므로 이전 연결 정보의 색칠을 바꿔준다.
                        result.SourceCountry = TxtSourceCountry.Text.Trim();
                        result.TargetCountry = TxtTargetCountry.Text.Trim();
                        result.SourceAccount = TxtSourceId.Text.Trim();
                        result.SourceProductId = srcProductId;
                        result.TargetProductId = tarProductId;
                        result.TargetAccount = TxtTargetId.Text.Trim();
                        context.SaveChanges();

                        
                        dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                        dgTarItemList.Rows[dgTarItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                        dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[0].Index);
                        dgTarItemList.Rows.RemoveAt(dgTarItemList.SelectedRows[0].Index);

                        arrangeNo(dgSrcItemList);
                        arrangeNo(dgTarItemList);
                    }
                }


                grp_src.Text = "원본 상품 목록 : [ " + string.Format("{0:n0}", dgSrcItemList.Rows.Count) + " ]";
                grp_tar.Text = "대상 상품 목록 : [ " + string.Format("{0:n0}", dgTarItemList.Rows.Count) + " ]";
                
            }
        }

        private void BtnClearLinkedData_Click(object sender, EventArgs e)
        {
            string srcAccount = TxtSourceId.Text.Trim();
            string tarAccount = TxtTargetId.Text.Trim();
            if (srcAccount != string.Empty &&
                    tarAccount != string.Empty)
            {
                DialogResult dlg_Result = MessageBox.Show("선택한 계정의 상품 연결정보를 모두 삭제하시겠습니까?", "상품 연결정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {

                    //계정별 데이터는 모두 삭제한다.

                    using (AppDbContext context = new AppDbContext())
                    {
                        List<ProductLink> linkedList = context.ProductLinks
                        .Where(x => x.SourceAccount == srcAccount &&
                        x.TargetAccount == tarAccount
                        && x.UserId == global_var.userId)
                        .ToList();

                        context.ProductLinks.RemoveRange(linkedList);
                        context.SaveChanges();
                        MessageBox.Show("상품 연결 정보를 삭제하였습니다.", "연결정보 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnGetSrcItemList_Click(null, null);
                    }
                }
            }   
        }

        private void dgTarItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_link_product_Click(null, null);
            //if (dgSrcItemList.Rows.Count > 0)
            //{
            //    if(dgSrcItemList.SelectedRows.Count > 0)
            //    {
            //        btn_unlink_product_Click(null, null);
            //    }
            //    else
            //    {
                    
            //    }
            //}
        }

        private void dgSrcItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7
                    || e.ColumnIndex == 8)
                {
                    string cellValue = dgSrcItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Trim();
                    if (cellValue != string.Empty)
                    {
                        Clipboard.SetText(cellValue);
                    }
                }
                using (AppDbContext context = new AppDbContext())
                {
                    long srcProductId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());

                    ProductLink result = context.ProductLinks.SingleOrDefault(
                        b => b.SourceCountry == TxtSourceCountry.Text &&
                        b.TargetCountry == TxtTargetCountry.Text &&
                        b.SourceProductId == srcProductId 
                        && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        long tarProductId = result.TargetProductId;
                        for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                        {
                            long temTarProductId = Convert.ToInt64(dgTarItemList.Rows[i].Cells["dgItemList_item_id_tar"].Value.ToString());
                            if (temTarProductId == tarProductId)
                            {
                                dgTarItemList.Rows[i].Selected = true;
                                dgTarItemList.Rows[i].Visible = true;
                                dgTarItemList.FirstDisplayedScrollingRowIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        dgTarItemList.ClearSelection();
                    }
                }
            }
            else if (e.RowIndex == -1 && e.ColumnIndex == 1)
            {
                if (dgSrcItemList.Rows.Count > 0)
                {
                    bool chk = (bool)dgSrcItemList.Rows[0].Cells["dgItemList_Chk"].Value;
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = !chk;
                    }

                    grp_src.Select();
                }
            }

            if (e.RowIndex > -1)
            {
                string val = dgSrcItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Trim();
                if (val != string.Empty)
                {
                    Clipboard.SetText(val);
                }
            }
        }

        private void btn_unlink_product_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("상품 연결 정보를 삭제 하시겠습니까?", "상품 연결 정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());

                        ProductLink result = context.ProductLinks.SingleOrDefault(
                            b => b.SourceCountry == TxtSourceCountry.Text &&
                            b.TargetCountry == TxtTargetCountry.Text &&
                            b.SourceAccount == TxtSourceId.Text.Trim() &&
                            b.TargetAccount == TxtTargetId.Text.Trim() &&
                            b.SourceProductId == srcProductId
                            && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            context.ProductLinks.Remove(result);
                            context.SaveChanges();

                            dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.White;
                            dgTarItemList.Rows[dgTarItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.White;
                            dgTarItemList.ClearSelection();
                        }
                    }
                }
            }
        }

        private void dgTarItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string val = dgTarItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Trim();
                if (val != string.Empty)
                {
                    Clipboard.SetText(val);
                }
            }
        }

        private void Menu_Target_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Menu_Tar_link_Click(object sender, EventArgs e)
        {
            btn_link_product_Click(null, null);
        }

        private void Menu_Src_link_Click(object sender, EventArgs e)
        {
            btn_link_product_Click(null, null);
        }

        private void Menu_Src_unlink_Click(object sender, EventArgs e)
        {
            btn_unlink_product_Click(null, null);
        }

        private void Menu_Tar_unlink_Click(object sender, EventArgs e)
        {
            btn_unlink_product_Click(null, null);
        }

        private void btn_remove_link_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                for (int i = dgSrcItemList.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgSrcItemList.Rows[i].DefaultCellStyle.BackColor == Color.GreenYellow)
                    {
                        dgSrcItemList.Rows.RemoveAt(i);
                    }
                }

                arrangeNo(dgSrcItemList);
                grp_src.Text = "원본 상품 목록 : [ " + string.Format("{0:n0}", dgSrcItemList.Rows.Count) + " ]";

                for (int i = dgTarItemList.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgTarItemList.Rows[i].DefaultCellStyle.BackColor == Color.GreenYellow)
                    {
                        dgTarItemList.Rows.RemoveAt(i);
                    }
                }

                grp_tar.Text = "대상 상품 목록 : [ " + string.Format("{0:n0}", dgTarItemList.Rows.Count) + " ]";
                arrangeNo(dgTarItemList);
            }
        }

        private void Menu_Tar_view_product_Click(object sender, EventArgs e)
        {
            if (dgTarItemList.Rows.Count > 0 && dgTarItemList.SelectedRows.Count > 0)
            {
                string siteUrl = "";
                string currency = dgTarItemList.SelectedRows[0].Cells["dgItemList_currency_tar"].Value.ToString();
                string shop_id = dgTarItemList.SelectedRows[0].Cells["dgItemList_shopid_tar"].Value.ToString();
                string goods_no = dgTarItemList.SelectedRows[0].Cells["dgItemList_item_id_tar"].Value.ToString();

                if (currency == "SGD")
                {
                    siteUrl = "https://shopee.sg/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "MYR")
                {
                    siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "IDR")
                {
                    siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "THB")
                {
                    siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "TWD")
                {
                    siteUrl = "https://shopee.tw/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "PHP")
                {
                    siteUrl = "https://shopee.ph/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "VND")
                {
                    siteUrl = "https://shopee.vn/product/" + shop_id + "/" + goods_no;
                }
                System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            }
        }

        private void Menu_Src_view_product_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                string siteUrl = "";
                string currency = dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString();
                string shop_id = dgSrcItemList.SelectedRows[0].Cells["dgItemList_shopid"].Value.ToString();
                string goods_no = dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();

                if (currency == "SGD")
                {
                    siteUrl = "https://shopee.sg/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "MYR")
                {
                    siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "IDR")
                {
                    siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "THB")
                {
                    siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "TWD")
                {
                    siteUrl = "https://shopee.tw/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "PHP")
                {
                    siteUrl = "https://shopee.ph/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "VND")
                {
                    siteUrl = "https://shopee.vn/product/" + shop_id + "/" + goods_no;
                }
                System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            }
        }

        private void dg_site_id_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(TxtSourceId.Text.Trim() == string.Empty)
            {
                BtnSetSource_Click(null, null);
            }
            else
            {
                BtnSetTarget_Click(null, null);
            }
        }

        private void BtnSrcSearchProduct_Click(object sender, EventArgs e)
        {
            string keyword = TxtSrcSearchProduct.Text.ToUpper().Trim();
            if(keyword != string.Empty)
            {
                string colName = "";
                if(CboSrcSearchArea.Text == "상품ID")
                {
                    colName = "dgItemList_item_id";

                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        if (dgSrcItemList.Rows[i].Cells[colName].Value.ToString() == keyword)
                        {
                            dgSrcItemList.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgSrcItemList.Rows[i].Visible = false;
                        }
                    }
                }
                else if (CboSrcSearchArea.Text == "상품SKU")
                {
                    colName = "dgItemList_item_sku";

                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        if (dgSrcItemList.Rows[i].Cells[colName].Value.ToString().ToUpper().Contains(keyword))
                        {
                            dgSrcItemList.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgSrcItemList.Rows[i].Visible = false;
                        }
                    }
                }
                else if (CboSrcSearchArea.Text == "상품명")
                {
                    colName = "dgItemList_name";
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        if (dgSrcItemList.Rows[i].Cells[colName].Value.ToString().ToUpper().Contains(keyword))
                        {
                            dgSrcItemList.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgSrcItemList.Rows[i].Visible = false;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    dgSrcItemList.Rows[i].Visible = true;
                }
            }
        }

        private void TxtSrcSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnSrcSearchProduct_Click(null, null);
            }
        }

        private void BtnTarSearchProduct_Click(object sender, EventArgs e)
        {
            string keyword = TxtTarSearchProduct.Text.ToUpper().Trim();
            if (keyword != string.Empty)
            {
                string colName = "";
                if (CboTarSearchArea.Text == "상품ID")
                {
                    colName = "dgItemList_item_id_tar";

                    for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                    {
                        if (dgTarItemList.Rows[i].Cells[colName].Value.ToString() == keyword)
                        {
                            dgTarItemList.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgTarItemList.Rows[i].Visible = false;
                        }
                    }
                }
                else if (CboTarSearchArea.Text == "상품SKU")
                {
                    colName = "dgItemList_item_sku_tar";

                    for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                    {
                        if (dgTarItemList.Rows[i].Cells[colName].Value.ToString().ToUpper().Contains(keyword))
                        {
                            dgTarItemList.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgTarItemList.Rows[i].Visible = false;
                        }
                    }
                }
                else if (CboTarSearchArea.Text == "상품명")
                {
                    colName = "dgItemList_name_tar";
                    for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                    {
                        if (dgTarItemList.Rows[i].Cells[colName].Value.ToString().ToUpper().Contains(keyword))
                        {
                            dgTarItemList.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgTarItemList.Rows[i].Visible = false;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                {
                    dgTarItemList.Rows[i].Visible = true;
                }
            }
        }

        private void TxtTarSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnTarSearchProduct_Click(null, null);
            }
        }

        private void BtnCopyRegister_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("체크한 원본 상품의 정보를 상품등록기로 복사하시겠습니까?\r\n기존자료는 그대로 보존되며 업데이트 필요시 등록기에서 삭제 후 다시 복사하세요.", "상품 데이터 복사", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                progressCopy.Value = 0;
                progressCopy.Maximum = dgSrcItemList.Rows.Count;
                string srcShopeeId = TxtSourceId.Text.Trim();
                string tarShopeeId = TxtTargetId.Text.Trim();

                int copyCount = 0;
                int updateCount = 0;
                int totalCount = 0;
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        totalCount++;
                        //원본을 상품관리에서 수정하고 여기에 왔을 수 있으므로 DB에서 읽어 와서 저장한다.
                        using (AppDbContext context = new AppDbContext())
                        {
                            
                            ItemInfo result_src_data = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == srcProductId &&
                                b.shopeeAccount == srcShopeeId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if(result_src_data != null)
                            {
                                ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == srcProductId &&
                                b.src_shopeeAccount == srcShopeeId &&
                                b.tar_shopeeAccount == tarShopeeId
                                && b.UserId == global_var.userId);

                                //없는 경우 신규 생성해준다.
                                if (result == null)
                                {
                                    //속성값 추가
                                    List<ItemAttribute> attList = context.ItemAttributes
                                    .Where(b => b.item_id == srcProductId
                                    && b.UserId == global_var.userId)
                                    .OrderBy(x => x.attribute_id).ToList();

                                    if(attList != null)
                                    {
                                        for (int att = 0; att < attList.Count; att++)
                                        {
                                            ItemAttributeDraft newItemAttributeDraft = new ItemAttributeDraft
                                            {
                                                attribute_id = attList[att].attribute_id,
                                                attribute_name = attList[att].attribute_name,
                                                is_mandatory = attList[att].is_mandatory,
                                                attribute_type = attList[att].attribute_type,
                                                attribute_value = attList[att].attribute_value,
                                                src_item_id = srcProductId,
                                                src_shopeeAccount = srcShopeeId,
                                                tar_shopeeAccount = tarShopeeId,
                                                UserId = global_var.userId
                                            };

                                            context.ItemAttributeDrafts.Add(newItemAttributeDraft);
                                        }
                                    }

                                    //옵션값 추가
                                    //옵션에서 가격은 0으로 설정하여 후에 계산하여 넣는다.
                                    List<ItemVariation> variList = context.ItemVariations
                                    .Where(b => b.item_id == srcProductId
                                    && b.UserId == global_var.userId)
                                    .OrderBy(x => x.variation_id).ToList();

                                    if (variList != null)
                                    {
                                        for (int vari = 0; vari < variList.Count; vari++)
                                        {
                                            ItemVariationDraft newItemVariationDraft = new ItemVariationDraft
                                            {
                                                variation_id = variList[vari].variation_id,
                                                variation_sku = variList[vari].variation_sku,
                                                name = variList[vari].name,
                                                price = 0,
                                                stock = variList[vari].stock,
                                                status = variList[vari].status,
                                                create_time = variList[vari].create_time,
                                                update_time = variList[vari].update_time,
                                                original_price = 0,
                                                discount_id = 0,
                                                src_item_id = srcProductId,
                                                src_shopeeAccount = srcShopeeId,
                                                tar_shopeeAccount = tarShopeeId,
                                                targetSellPriceKRW = 0,
                                                targetRetailPriceKRW = 0,
                                                currencyRate = 0,
                                                currencyDate = DateTime.Now,       
                                                UserId = global_var.userId
                                            };
                                            context.ItemVariationDrafts.Add(newItemVariationDraft);
                                        }
                                    }

                                    //도매값 추가
                                    List<ItemWholesale> wholesaleList = context.ItemWholesales
                                    .Where(b => b.item_id == srcProductId
                                    && b.UserId == global_var.userId)
                                    .OrderBy(x => x.Idx).ToList();

                                    if (wholesaleList != null)
                                    {
                                        for (int wsale = 0; wsale < wholesaleList.Count; wsale++)
                                        {
                                            ItemWholesaleDraft newItemWholesaleDraft = new ItemWholesaleDraft
                                            {
                                                min = wholesaleList[wsale].min,
                                                max = wholesaleList[wsale].max,
                                                unit_price = wholesaleList[wsale].unit_price,
                                                src_item_id = srcProductId,
                                                src_shopeeAccount = srcShopeeId,
                                                tar_shopeeAccount = tarShopeeId,
                                                UserId = global_var.userId
                                            };
                                            context.ItemWholesaleDrafts.Add(newItemWholesaleDraft);
                                        }
                                    }

                                    ItemInfoDraft newItemInfoDraft = new ItemInfoDraft
                                    {
                                        src_item_id = result_src_data.item_id,
                                        src_shopeeAccount = srcShopeeId,
                                        tar_shopeeAccount = tarShopeeId,
                                        src_shopid = result_src_data.shopid,
                                        tar_shopid = 0,
                                        item_sku = result_src_data.item_sku,
                                        status = result_src_data.status,
                                        name = result_src_data.name,
                                        description = result_src_data.description,
                                        currency = result_src_data.currency,
                                        has_variation = result_src_data.has_variation,
                                        price = 0,
                                        stock = result_src_data.stock,
                                        create_time = result_src_data.create_time,
                                        update_time = result_src_data.update_time,
                                        copy_time = DateTime.Now,
                                        weight = result_src_data.weight,
                                        category_id = result_src_data.category_id,
                                        original_price = 0,
                                        rating_star = 0,
                                        cmt_count = 0,
                                        sales = 0,
                                        views = 0,
                                        likes = 0,
                                        package_length = 0,
                                        package_width = 0,
                                        package_height = 0,
                                        days_to_ship = result_src_data.days_to_ship,
                                        size_chart = result_src_data.size_chart,
                                        condition = result_src_data.condition,
                                        discount_id = 0,
                                        is_2tier_item = result_src_data.is_2tier_item,
                                        images = result_src_data.images,
                                        supply_price = result_src_data.supply_price,
                                        isChanged = false,
                                        targetSellPriceKRW = 0,
                                        targetRetailPriceKRW = 0,
                                        currencyRate = 0,
                                        currencyDate = DateTime.Now,
                                        UserId = global_var.userId
                                    };

                                    context.ItemInfoDrafts.Add(newItemInfoDraft);
                                    context.SaveChanges();
                                    copyCount++;
                                }
                                else
                                {
                                    //고민을 해보니 그냥 업데이트는 문제가 있다.
                                    //기존데이터는 보존해 주는것이 좋겠고,
                                    //정말 업데이트가 필요하다면 삭제후에 다시 가지고 오는 걸로
                                    //if(ChkUpdateMode.Checked)
                                    //{
                                    //    //존재하는 경우는 업데이트 해준다.
                                    //    result.src_item_id = result_src_data.item_id;
                                    //    result.src_shopeeAccount = srcShopeeId;
                                    //    result.tar_shopeeAccount = tarShopeeId;
                                    //    result.src_shopid = result_src_data.shopid;
                                    //    result.tar_shopid = 0;
                                    //    result.item_sku = result_src_data.item_sku;
                                    //    result.status = result_src_data.status;
                                    //    result.name = result_src_data.name;
                                    //    result.description = result_src_data.description;
                                    //    result.currency = result_src_data.currency;
                                    //    result.has_variation = result_src_data.has_variation;
                                    //    result.price = result_src_data.price;
                                    //    result.stock = result_src_data.stock;
                                    //    result.create_time = result_src_data.create_time;
                                    //    result.update_time = result_src_data.update_time;
                                    //    result.copy_time = DateTime.Now;
                                    //    result.weight = result_src_data.weight;
                                    //    result.category_id = result_src_data.category_id;
                                    //    result.original_price = result_src_data.original_price;
                                    //    result.rating_star = 0;
                                    //    result.cmt_count = 0;
                                    //    result.sales = 0;
                                    //    result.views = 0;
                                    //    result.likes = 0;
                                    //    result.package_length = 0;
                                    //    result.package_width = 0;
                                    //    result.package_height = 0;
                                    //    result.days_to_ship = result_src_data.days_to_ship;
                                    //    result.size_chart = result_src_data.size_chart;
                                    //    result.condition = result_src_data.condition;
                                    //    result.discount_id = result_src_data.discount_id;
                                    //    result.is_2tier_item = result_src_data.is_2tier_item;
                                    //    result.images = result_src_data.images;
                                    //    result.supply_price = result_src_data.supply_price;
                                    //    result.isChanged = false;
                                    //    context.SaveChanges();
                                        updateCount++;
                                    //}
                                }
                            }
                        }   
                    }

                    progressCopy.Value = i + 1;
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("총 " + totalCount + "건의 데이터 중 신규:" + copyCount + "건 스킵: " + updateCount + "건을 처리하였습니다.","상품 데이터 복사",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void btn_validate_category_Click(object sender, EventArgs e)
        {

        }

        private void btn_set_category_Click(object sender, EventArgs e)
        {

        }

        private void TxtSrcSearchProduct_KeyDown_1(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnSrcSearchProduct_Click(null, null);
            }
        }

        private void BtnAutoMapper_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("체크한 원본상품의 선택한 연결 기준으로 자동연결을 실행 하시겠습니까?", "상품 자동 연결", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if(rdLinkSku.Checked)
                {
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        if((bool)dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value == true)
                        {
                            string srcSku = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value.ToString().Trim().ToUpper();
                            for (int j = 0; j < dgTarItemList.Rows.Count; j++)
                            {
                                string tarSku = dgTarItemList.Rows[j].Cells["dgItemList_item_sku_Tar"].Value.ToString().Trim().ToUpper();

                                if (srcSku == tarSku)
                                {
                                    using (AppDbContext context = new AppDbContext())
                                    {
                                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                                        long tarProductId = Convert.ToInt64(dgTarItemList.Rows[j].Cells["dgItemList_item_id_tar"].Value.ToString());
                                        ProductLink result = context.ProductLinks.SingleOrDefault(
                                            b => b.SourceCountry == TxtSourceCountry.Text.Trim() &&
                                            b.TargetCountry == TxtTargetCountry.Text.Trim() &&
                                            b.SourceProductId == srcProductId
                                            && b.UserId == global_var.userId);

                                        //만약 있는 경우라면 옮기는 것이므로 기존꺼는 흰색으로 새로 연결한 놈은 녹색으로
                                        if (result == null)
                                        {
                                            ProductLink newProductLink = new ProductLink
                                            {
                                                SourceCountry = TxtSourceCountry.Text.Trim(),
                                                TargetCountry = TxtTargetCountry.Text.Trim(),
                                                SourceAccount = TxtSourceId.Text.Trim(),
                                                SourceProductId = srcProductId,
                                                TargetProductId = tarProductId,
                                                TargetAccount = TxtTargetId.Text.Trim(),
                                                UserId = global_var.userId
                                            };

                                            context.ProductLinks.Add(newProductLink);
                                            context.SaveChanges();

                                            dgSrcItemList.Rows[dgSrcItemList.Rows[i].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                            dgTarItemList.Rows[dgTarItemList.Rows[j].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                                            //dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[i].Index);
                                            //dgTarItemList.Rows.RemoveAt(dgTarItemList.Rows[j].Index);

                                            //arrangeNo(dgSrcItemList);
                                            //arrangeNo(dgTarItemList);
                                        }
                                        else
                                        {
                                            for (int k = 0; k < dgTarItemList.Rows.Count; k++)
                                            {
                                                if (tarProductId == result.TargetProductId)
                                                {
                                                    dgTarItemList.Rows[k].DefaultCellStyle.BackColor = Color.White;
                                                }
                                            }
                                            //이 경우 이전에 다른놈과 연결되어 있으므로 이전 연결 정보의 색칠을 바꿔준다.
                                            result.SourceCountry = TxtSourceCountry.Text.Trim();
                                            result.TargetCountry = TxtTargetCountry.Text.Trim();
                                            result.SourceAccount = TxtSourceId.Text.Trim();
                                            result.SourceProductId = srcProductId;
                                            result.TargetProductId = tarProductId;
                                            result.TargetAccount = TxtTargetId.Text.Trim();
                                            context.SaveChanges();


                                            dgSrcItemList.Rows[dgSrcItemList.Rows[i].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                            dgTarItemList.Rows[dgTarItemList.Rows[j].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                                            //dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[i].Index);
                                            //dgTarItemList.Rows.RemoveAt(dgTarItemList.Rows[j].Index);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    btn_remove_link_Click(null, null);
                    arrangeNo(dgSrcItemList);
                    arrangeNo(dgTarItemList);                    
                }
                else
                {
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        if ((bool)dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value == true)
                        {
                            string srcTitle = dgSrcItemList.Rows[i].Cells["dgItemList_name"].Value.ToString().Trim().ToUpper();
                            for (int j = 0; j < dgTarItemList.Rows.Count; j++)
                            {
                                string tarTitle = dgTarItemList.Rows[j].Cells["dgItemList_name_Tar"].Value.ToString().Trim().ToUpper();

                                if (srcTitle == tarTitle)
                                {
                                    using (AppDbContext context = new AppDbContext())
                                    {
                                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                                        long tarProductId = Convert.ToInt64(dgTarItemList.Rows[j].Cells["dgItemList_item_id_tar"].Value.ToString());
                                        ProductLink result = context.ProductLinks.SingleOrDefault(
                                            b => b.SourceCountry == TxtSourceCountry.Text.Trim() &&
                                            b.TargetCountry == TxtTargetCountry.Text.Trim() &&
                                            b.SourceProductId == srcProductId
                                            && b.UserId == global_var.userId);

                                        //만약 있는 경우라면 옮기는 것이므로 기존꺼는 흰색으로 새로 연결한 놈은 녹색으로
                                        if (result == null)
                                        {
                                            ProductLink newProductLink = new ProductLink
                                            {
                                                SourceCountry = TxtSourceCountry.Text.Trim(),
                                                TargetCountry = TxtTargetCountry.Text.Trim(),
                                                SourceAccount = TxtSourceId.Text.Trim(),
                                                SourceProductId = srcProductId,
                                                TargetProductId = tarProductId,
                                                TargetAccount = TxtTargetId.Text.Trim(),
                                                UserId = global_var.userId
                                            };

                                            context.ProductLinks.Add(newProductLink);
                                            context.SaveChanges();

                                            dgSrcItemList.Rows[dgSrcItemList.Rows[i].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                            dgTarItemList.Rows[dgTarItemList.Rows[j].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                                            //dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[i].Index);
                                            //dgTarItemList.Rows.RemoveAt(dgTarItemList.Rows[j].Index);

                                            //arrangeNo(dgSrcItemList);
                                            //arrangeNo(dgTarItemList);
                                        }
                                        else
                                        {
                                            for (int k = 0; k < dgTarItemList.Rows.Count; k++)
                                            {
                                                if (tarProductId == result.TargetProductId)
                                                {
                                                    dgTarItemList.Rows[k].DefaultCellStyle.BackColor = Color.White;
                                                }
                                            }
                                            //이 경우 이전에 다른놈과 연결되어 있으므로 이전 연결 정보의 색칠을 바꿔준다.
                                            result.SourceCountry = TxtSourceCountry.Text.Trim();
                                            result.TargetCountry = TxtTargetCountry.Text.Trim();
                                            result.SourceAccount = TxtSourceId.Text.Trim();
                                            result.SourceProductId = srcProductId;
                                            result.TargetProductId = tarProductId;
                                            result.TargetAccount = TxtTargetId.Text.Trim();
                                            context.SaveChanges();


                                            dgSrcItemList.Rows[dgSrcItemList.Rows[i].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                            dgTarItemList.Rows[dgTarItemList.Rows[j].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                                            //dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[i].Index);
                                            //dgTarItemList.Rows.RemoveAt(dgTarItemList.Rows[j].Index);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    btn_remove_link_Click(null, null);
                    arrangeNo(dgSrcItemList);
                    arrangeNo(dgTarItemList);
                }
                
            }
        }
    }
}
