using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormSetVariationClone : MetroForm
    {
        public FormSetVariationClone()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool rtn = false;
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                if (keyData.Equals(Keys.Escape))
                {
                    this.Close();
                    this.Dispose();
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
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
        private void FormSetVariation_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            if (TxtStrVariation.Text.Trim() != string.Empty)
            {
                Cursor.Current = Cursors.WaitCursor;

                dynamic dynVariation = JsonConvert.DeserializeObject(TxtStrVariation.Text.Trim());

                if (dynVariation != null && dynVariation.Count > 0)
                {
                    for (int tier = 0; tier < dynVariation.Count; tier++)
                    {
                        string createTime = ConvertFromUnixTimestamp(Convert.ToDouble(dynVariation[tier].create_time)).ToString("yyyy-MM-dd HH:mm:ss");
                        string updateTime = ConvertFromUnixTimestamp(Convert.ToDouble(dynVariation[tier].update_time)).ToString("yyyy-MM-dd HH:mm:ss");
                        string variation_id = dynVariation[tier].variation_id;

                        decimal tarPrice = 0;
                        decimal tarOriginalPrice = 0;

                        using (AppDbContext context = new AppDbContext())
                        {
                            ShopeeVariationPrice result = context.ShopeeVariationPrices.SingleOrDefault(
                                    x => x.SrcShopeeCountry == TxtSrcCountry.Text &&
                                    x.SrcShopeeId == TxtSrcID.Text &&
                                    x.TarShopeeCountry == TxtTarCountry.Text &&
                                    x.TarShopeeId == TxtTarId.Text &&
                                    x.variation_id == variation_id
                                    && x.UserId == global_var.userId);

                            if (result != null)
                            {
                                if (result.TarNetPrice != 0)
                                {
                                    //정확한 판매가가 있는 경우임
                                    tarPrice = result.TarNetPrice;
                                    tarOriginalPrice = result.TarRetail_price;
                                }
                            }
                        }

                        DgSrcVariation.Rows.Add(tier + 1,
                            false,
                            dynVariation[tier].status,
                            dynVariation[tier].variation_id,
                            dynVariation[tier].variation_sku,
                            dynVariation[tier].name,
                            string.Format("{0:n0}", dynVariation[tier].price),
                            string.Format("{0:n0}", dynVariation[tier].original_price),
                            tarPrice,
                            tarOriginalPrice,
                            string.Format("{0:n0}", dynVariation[tier].stock),
                            createTime,
                            updateTime,
                            dynVariation[tier].discount_id                            
                            );
                    }
                }

                Cursor.Current = Cursors.Default;
            }
        }

        private void btn_View_Product_Click(object sender, EventArgs e)
        {

            string siteUrl = "";
            string shop_id = TxtShopId.Text;
            string goods_no = TxtProductId.Text;

            if (TxtSrcCountry.Text == "SG")
            {
                siteUrl = "https://shopee.sg/product/" + shop_id + "/" + goods_no;
            }
            else if (TxtSrcCountry.Text == "MY")
            {
                siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + goods_no;
            }
            else if (TxtSrcCountry.Text == "ID")
            {
                siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + goods_no;
            }
            else if (TxtSrcCountry.Text == "TH")
            {
                siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + goods_no;
            }
            else if (TxtSrcCountry.Text == "TW")
            {
                siteUrl = "https://shopee.tw/product/" + shop_id + "/" + goods_no;
            }
            else if (TxtSrcCountry.Text == "PH")
            {
                siteUrl = "https://shopee.ph/product/" + shop_id + "/" + goods_no;
            }
            else if (TxtSrcCountry.Text == "VN")
            {
                siteUrl = "https://shopee.vn/product/" + shop_id + "/" + goods_no;
            }
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
        }

        private void btn_save_data_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("설정값을 저장 하시겠습니까?", "설정값 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
                {
                    string variation_id = DgSrcVariation.Rows[i].Cells["DgSrcVariation_variation_id"].Value.ToString();
                    decimal tarPrice = Convert.ToDecimal(DgSrcVariation.Rows[i].Cells["DgSrcVariation_tar_price"].Value.ToString().Replace(",",""));
                    decimal tarOriginalPrice = Convert.ToDecimal(DgSrcVariation.Rows[i].Cells["DgSrcVariation_tar_original_price"].Value.ToString().Replace(",", ""));

                    using (AppDbContext context = new AppDbContext())
                    {
                        ShopeeVariationPrice result = context.ShopeeVariationPrices.SingleOrDefault(
                                x => x.SrcShopeeCountry == TxtSrcCountry.Text &&
                                x.SrcShopeeId == TxtSrcID.Text &&
                                x.TarShopeeCountry == TxtTarCountry.Text &&
                                x.TarShopeeId == TxtTarId.Text &&
                                x.variation_id == variation_id
                                && x.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.TarNetPrice = tarPrice;
                            result.TarRetail_price = tarOriginalPrice;
                            context.SaveChanges();                            
                        }
                    }
                }

                MessageBox.Show("저장하였습니다.","저장완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
