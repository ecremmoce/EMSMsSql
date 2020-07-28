using MetroFramework.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormConfig : MetroForm
    {
        public FormConfig(string lang)
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
            AutoSize = true;
            AutoScroll = true;
            WindowState = FormWindowState.Normal;
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            getShopeeAccount();

            BtnAttributeNameMapLoad_Click(null, null);
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
        private void BtnDeleteAccount_Click(object sender, EventArgs e)
        {
            if(dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("Shopee 계정 정보를 삭제 하시겠습니까?", "Shopee 계정 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    string shopeeCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                    string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                    if (!string.IsNullOrEmpty(shopeeCountry) && !string.IsNullOrEmpty(shopeeId))
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ShopeeAccount result = context.ShopeeAccounts.SingleOrDefault(b => b.ShopeeCountry == shopeeCountry && b.ShopeeId == shopeeId && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                context.ShopeeAccounts.Remove(result);
                                context.SaveChanges();
                            }
                        }

                        getShopeeAccount();
                    }
                }
            }
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

        private void BtnUpdateShopeeAccount_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Shopee 계정 정보를 업데이트 하시겠습니까?", "Shopee 계정 정보 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                string shopeeCountry = "";

                if (rdCountrySg.Checked)
                {
                    shopeeCountry = "SG";
                }
                else if (rdCountryMy.Checked)
                {
                    shopeeCountry = "MY";
                }
                else if (rdCountryId.Checked)
                {
                    shopeeCountry = "ID";
                }
                else if (rdCountryTh.Checked)
                {
                    shopeeCountry = "TH";
                }
                else if (rdCountryPh.Checked)
                {
                    shopeeCountry = "PH";
                }
                else if (rdCountryTw.Checked)
                {
                    shopeeCountry = "TW";
                }

                using (AppDbContext context = new AppDbContext())
                {
                    ShopeeAccount result = context.ShopeeAccounts.SingleOrDefault(
                            b => b.ShopeeCountry == shopeeCountry &&
                            b.ShopeeId == TxtShopeeId.Text.Trim() &&
                            b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.ShopeeId = TxtShopeeId.Text.Trim();
                        result.PartnerId = TxtPartnerId.Text.Trim();
                        result.ShopId = TxtShopId.Text.Trim();
                        result.SecretKey = TxtSecretKey.Text.Trim();
                        context.SaveChanges();
                    }
                }

                getShopeeAccount();

            }
        }

        private void BtnAddShopeeAccount_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Shopee 계정 정보를 추가 하시겠습니까?", "Shopee 계정 정보 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    string shopeeCountry = "";
                    if (rdCountrySg.Checked)
                    {
                        shopeeCountry = "SG";
                    }
                    else if (rdCountryMy.Checked)
                    {
                        shopeeCountry = "MY";
                    }
                    else if (rdCountryId.Checked)
                    {
                        shopeeCountry = "ID";
                    }
                    else if (rdCountryTh.Checked)
                    {
                        shopeeCountry = "TH";
                    }
                    else if (rdCountryPh.Checked)
                    {
                        shopeeCountry = "PH";
                    }
                    else if (rdCountryTw.Checked)
                    {
                        shopeeCountry = "TW";
                    }
                    else if (rdCountryVn.Checked)
                    {
                        shopeeCountry = "VN";
                    }


                    var acc = context.ShopeeAccounts.FirstOrDefault(x => x.ShopeeId == TxtShopeeId.Text.Trim() && x.UserId == global_var.userId);
                    if(acc == null)
                    {
                        ShopeeAccount newAccount = new ShopeeAccount
                        {
                            ShopeeCountry = shopeeCountry,
                            ShopeeId = TxtShopeeId.Text.Trim(),
                            PartnerId = TxtPartnerId.Text.Trim(),
                            ShopId = TxtShopId.Text.Trim(),
                            SecretKey = TxtSecretKey.Text.Trim(),
                            UserId = global_var.userId
                        };
                        context.ShopeeAccounts.Add(newAccount);
                        context.SaveChanges();
                    }
                        
                    
                    TxtShopeeId.Text = "";
                    TxtPartnerId.Text = "";
                    TxtShopId.Text = "";
                    TxtSecretKey.Text = "";
                }

                getShopeeAccount();

            }
        }

        private void BtnManageAttribute_Click(object sender, EventArgs e)
        {
            FormAttributeManage fa = new FormAttributeManage();
            fa.ShowDialog();
        }

        private void dg_site_id_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1 && dg_site_id.Rows.Count > 0)
            {
                if(dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "SG")
                {
                    rdCountrySg.Checked = true;
                }
                else if (dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "MY")
                {
                    rdCountryMy.Checked = true;
                }
                else if (dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "ID")
                {
                    rdCountryId.Checked = true;
                }
                else if (dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "TH")
                {
                    rdCountryTh.Checked = true;
                }

                else if (dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "TW")
                {
                    rdCountryTw.Checked = true;
                }
                else if (dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "PH")
                {
                    rdCountryPh.Checked = true;
                }
                else if (dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() == "VN")
                {
                    rdCountryVn.Checked = true;
                }
                TxtShopeeId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                TxtPartnerId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString();
                TxtShopId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString();
                TxtSecretKey.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
            }
        }

        private void rdCountrySg_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }

        private void TxtShopeeId_TextChanged(object sender, EventArgs e)
        {
            if(TxtShopeeId.Text.Trim().ToUpper().Contains(".SG"))
            {
                rdCountrySg.Checked = true;
            }
            else if (TxtShopeeId.Text.Trim().ToUpper().Contains(".MY"))
            {
                rdCountryMy.Checked = true;
            }
            else if (TxtShopeeId.Text.Trim().ToUpper().Contains(".ID"))
            {
                rdCountryId.Checked = true;
            }
            else if (TxtShopeeId.Text.Trim().ToUpper().Contains(".TH"))
            {
                rdCountryTh.Checked = true;
            }
            else if (TxtShopeeId.Text.Trim().ToUpper().Contains(".TW"))
            {
                rdCountryTw.Checked = true;
            }
            else if (TxtShopeeId.Text.Trim().ToUpper().Contains(".PH"))
            {
                rdCountryPh.Checked = true;
            }
            else if (TxtShopeeId.Text.Trim().ToUpper().Contains(".VN"))
            {
                rdCountryVn.Checked = true;
            }
        }

        private void BtnDeleteAccountAll_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Shopee 계정 정보를 모두 삭제 하시겠습니까?", "Shopee 계정 전체 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    List<ShopeeAccount> accountList = new List<ShopeeAccount>();
                    accountList = context.ShopeeAccounts
                            .Where(x => x.UserId == global_var.userId).ToList();

                    if (accountList.Count > 0)
                    {
                        context.ShopeeAccounts.RemoveRange(accountList);
                        context.SaveChanges();
                    }
                    dg_site_id.Rows.Clear();
                    TxtShopeeId.Text = "";
                    TxtPartnerId.Text = "";
                    TxtShopId.Text = "";
                    TxtSecretKey.Text = "";
                }
            }
        }

        private void rdCountryId_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }

        private void rdCountryMy_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }

        private void rdCountryTh_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }

        private void rdCountryPh_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }

        private void rdCountryTw_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }

        private void rdCountryVn_Click(object sender, EventArgs e)
        {
            TxtShopeeId.Text = "";
            TxtPartnerId.Text = "";
            TxtShopId.Text = "";
            TxtSecretKey.Text = "";
        }
        
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            try
            {
                StreamReader sr = new StreamReader(strFilePath);
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
        

        private void BtnAttributeNameMapLoad_Click(object sender, EventArgs e)
        {
            getAttributeNameMap();
        }
        private void getAttributeNameMap()
        {
            DgAttributeMap.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<AttributeNameMap> mapList = context.AttributeNameMaps.OrderBy(x => x.KR).ToList();

                if (mapList.Count > 0)
                {
                    for (int i = 0; i < mapList.Count; i++)
                    {
                        DgAttributeMap.Rows.Add(i + 1, false,
                            mapList[i].KR,
                            mapList[i].SG,
                            mapList[i].MY,
                            mapList[i].ID,
                            mapList[i].TH,
                            mapList[i].TW,
                            mapList[i].VN,
                            mapList[i].PH,
                            mapList[i].Idx);
                    }
                }
            }
        }

        private void BtnUploadMappingData_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xlsx";
            dialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            dialog.FileName = "AttributeList_" +
                dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() +
                "_" + DateTime.Now.ToString("yyyy-MM-dd") +
                ".xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dt_excel = GetDataTableFromExcel(dialog.FileName, true);

                for (int i = 0; i < dt_excel.Rows.Count; i++)
                {
                    string KR = dt_excel.Rows[i][1].ToString().Trim();
                    string SG = dt_excel.Rows[i][2].ToString().Trim();
                    string MY = dt_excel.Rows[i][3].ToString().Trim();
                    string ID = dt_excel.Rows[i][4].ToString().Trim();
                    string TH = dt_excel.Rows[i][5].ToString().Trim();
                    string TW = dt_excel.Rows[i][6].ToString().Trim();
                    string VN = dt_excel.Rows[i][7].ToString().Trim();
                    string PH = dt_excel.Rows[i][8].ToString().Trim();

                    if (KR == string.Empty &&
                       SG == string.Empty &&
                       MY == string.Empty &&
                       ID == string.Empty &&
                       TH == string.Empty &&
                       TW == string.Empty &&
                       VN == string.Empty &&
                       PH == string.Empty)
                    {
                        return;
                    }

                    else
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                                b => b.KR == KR &&
                                                b.SG == SG &&
                                                b.MY == MY &&
                                                b.ID == ID &&
                                                b.TH == TH &&
                                                b.TW == TW &&
                                                b.VN == VN &&
                                                b.PH == PH);

                            if (result == null)
                            {
                                AttributeNameMap newData = new AttributeNameMap
                                {
                                    KR = KR,
                                    SG = SG,
                                    MY = MY,
                                    ID = ID,
                                    TH = TH,
                                    TW = TW,
                                    VN = VN,
                                    PH = PH
                                };

                                context.AttributeNameMaps.Add(newData);
                                context.SaveChanges();

                                DgAttributeMap.Rows.Add(DgAttributeMap.Rows.Count + 1, false,
                                    KR,
                                    SG,
                                    MY,
                                    ID,
                                    TH,
                                    TW,
                                    VN,
                                    PH);
                            }
                        }
                    }
                }
                getAttributeNameMap();
                MessageBox.Show("엑셀데이터를 업로드 하였습니다.", "엑셀 업로드", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                OfficeOpenXml.ExcelWorksheet ws = package.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                for (int i = 1; i < ws.Dimension.End.Column + 1; i++)
                {
                    if (ws.Cells[1, i].Value == null)
                    {
                        tbl.Columns.Add("컬럼" + i.ToString());
                    }
                    else
                    {
                        tbl.Columns.Add(ws.Cells[1, i].Value.ToString());
                    }

                }

                for (int i = 2; i <= ws.Dimension.End.Row; i++)
                {
                    DataRow workRow;
                    workRow = tbl.NewRow();
                    for (int j = 0; j < ws.Dimension.End.Column; j++)
                    {
                        if (ws.Cells[i, j + 1].Value == null)
                        {
                            workRow[j] = "";
                        }
                        else
                        {
                            workRow[j] = ws.Cells[i, j + 1].Value.ToString();
                        }

                    }
                    tbl.Rows.Add(workRow);
                }

                return tbl;
            }
        }

        private void BtnDownloadMappingData_Click(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                FormDownloadData form = new FormDownloadData();
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] raw = wc.DownloadData("https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute_name_map/version.txt");

                long version = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(raw));

                var result = context.TemplateVersions.FirstOrDefault();
                if (result != null)
                {
                    if (result.attribute_name_map < version)
                    {
                        form.templateCurVersion = result.attribute_name_map;
                        form.templateNewVersion = version;
                        form.templateName = "속성명 맵핑 템플릿";
                        form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute_name_map/attribute_name_map.xlsx";
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("현재 최신 템플릿을 사용중입니다.","최신템플릿",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
                else
                {

                    form.templateNewVersion = version;
                    form.templateCurVersion = 0;
                    form.templateName = "속성명 맵핑 템플릿";
                    form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute_name_map/attribute_name_map.xlsx";
                    form.ShowDialog();
                }
            }
        }

        private void BtnAddAttributeMapData_Click(object sender, EventArgs e)
        {
            string KR = txt_KR.Text.Trim();
            string SG = txt_SG.Text.Trim();
            string MY = txt_MY.Text.Trim();
            string ID = txt_ID.Text.Trim();
            string TH = txt_TH.Text.Trim();
            string TW = txt_TW.Text.Trim();
            string VN = txt_VN.Text.Trim();
            string PH = txt_PH.Text.Trim();

            if (KR == string.Empty &&
               SG == string.Empty &&
               MY == string.Empty &&
               ID == string.Empty &&
               TH == string.Empty &&
               TW == string.Empty &&
               VN == string.Empty &&
               PH == string.Empty)
            {
                return;
            }


            DialogResult Result = MessageBox.Show("맵핑 정보를 추가 하시겠습니까?", "맵핑정보 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                        b => b.KR == KR &&
                                        b.SG == SG &&
                                        b.MY == MY &&
                                        b.ID == ID &&
                                        b.TH == TH &&
                                        b.TW == TW &&
                                        b.VN == VN &&
                                        b.PH == PH);

                    if (result != null)
                    {
                        MessageBox.Show("동일한 데이터가 이미 존재합니다.", "데이터 중복", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        AttributeNameMap newData = new AttributeNameMap
                        {
                            KR = KR,
                            SG = SG,
                            MY = MY,
                            ID = ID,
                            TH = TH,
                            TW = TW,
                            VN = VN,
                            PH = PH
                        };

                        context.AttributeNameMaps.Add(newData);
                        context.SaveChanges();

                        txt_KR.Text = "";
                        txt_SG.Text = "";
                        txt_MY.Text = "";
                        txt_ID.Text = "";
                        txt_TH.Text = "";
                        txt_TW.Text = "";
                        txt_VN.Text = "";
                        txt_PH.Text = "";
                        getAttributeNameMap();

                        //DgAttributeMap.Rows.Add(DgAttributeMap.Rows.Count + 1, false,
                        //    KR,
                        //    SG,
                        //    MY,
                        //    ID,
                        //    TH,
                        //    TW,
                        //    VN,
                        //    PH);
                    }
                }
            }
        }

        private void BtnUpdateAttributeMapData_Click(object sender, EventArgs e)
        {
            string KR = txt_KR.Text.Trim();
            string SG = txt_SG.Text.Trim();
            string MY = txt_MY.Text.Trim();
            string ID = txt_ID.Text.Trim();
            string TH = txt_TH.Text.Trim();
            string TW = txt_TW.Text.Trim();
            string VN = txt_VN.Text.Trim();
            string PH = txt_PH.Text.Trim();

            if (KR == string.Empty &&
               SG == string.Empty &&
               MY == string.Empty &&
               ID == string.Empty &&
               TH == string.Empty &&
               TW == string.Empty &&
               VN == string.Empty &&
               PH == string.Empty)
            {
                return;
            }

            int idxNo = Convert.ToInt32(DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_Idx"].Value.ToString());
            DialogResult Result = MessageBox.Show("맵핑 정보를 수정 하시겠습니까?", "맵핑정보 수정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    AttributeNameMap resultCur = context.AttributeNameMaps.SingleOrDefault(
                                        b => b.Idx == idxNo);

                    if (resultCur != null)
                    {
                        resultCur.KR = KR;
                        resultCur.SG = SG;
                        resultCur.MY = MY;
                        resultCur.ID = ID;
                        resultCur.TH = TH;
                        resultCur.TW = TW;
                        resultCur.VN = VN;
                        resultCur.PH = PH;
                        context.SaveChanges();


                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_KR"].Value = KR;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_SG"].Value = SG;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_MY"].Value = MY;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_ID"].Value = ID;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TH"].Value = TH;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TW"].Value = TW;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_VN"].Value = VN;
                        DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_PH"].Value = PH;

                        MessageBox.Show("수정하였습니다.", "수정완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnDeleteAttributeMapData_Click(object sender, EventArgs e)
        {
            if (DgAttributeMap.Rows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("카테고리 정보를 저장 하시겠습니까?", "카테고리정보 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    for (int i = 0; i < DgAttributeMap.Rows.Count; i++)
                    {
                        if ((bool)DgAttributeMap.Rows[i].Cells["DgAttributeMap_Chk"].Value)
                        {
                            int Idx = Convert.ToInt32(DgAttributeMap.Rows[i].Cells["DgAttributeMap_Idx"].Value.ToString());

                            using (AppDbContext context = new AppDbContext())
                            {
                                AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                                    b => b.Idx == Idx);

                                if (result != null)
                                {
                                    context.AttributeNameMaps.Remove(result);
                                    context.SaveChanges();

                                    txt_KR.Text = "";
                                    txt_SG.Text = "";
                                    txt_MY.Text = "";
                                    txt_ID.Text = "";
                                    txt_TH.Text = "";
                                    txt_TW.Text = "";
                                    txt_VN.Text = "";
                                    txt_PH.Text = "";
                                }

                            }
                        }
                    }


                    getAttributeNameMap();
                    MessageBox.Show("체크한 맵핑 데이터를 삭제하였습니다.", "맵핑데이터 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DgAttributeMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (DgAttributeMap.Rows.Count > 0 && DgAttributeMap.SelectedRows.Count > 0)
                {
                    txt_KR.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_KR"].Value.ToString();
                    txt_SG.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_SG"].Value.ToString();
                    txt_MY.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_MY"].Value.ToString();
                    txt_ID.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_ID"].Value.ToString();
                    txt_TH.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TH"].Value.ToString();
                    txt_TW.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TW"].Value.ToString();
                    txt_VN.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_VN"].Value.ToString();
                    txt_PH.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_PH"].Value.ToString();
                }
            }
        }
    }
}
