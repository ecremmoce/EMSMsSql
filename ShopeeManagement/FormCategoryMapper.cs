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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormCategoryMapper : MetroForm
    {
        public FormCategoryMapper(string lang)
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager;
        }

        private void FormCategoryMapper_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            set_double_buffer();
            btnLoadCategory_Click(null, null);
            Cursor.Current = Cursors.Default;
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
        private void btnUploadCategory_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    gridCategory.Rows.Clear();
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("category_data");

                    DataColumn dc_id_l1 = new DataColumn("id_l1", typeof(string));
                    DataColumn dc_id_l2 = new DataColumn("id_l2", typeof(string));
                    DataColumn dc_id_l3 = new DataColumn("id_l3", typeof(string));
                    DataColumn dc_my_l1 = new DataColumn("my_l1", typeof(string));
                    DataColumn dc_my_l2 = new DataColumn("my_l2", typeof(string));
                    DataColumn dc_my_l3 = new DataColumn("my_l3", typeof(string));
                    DataColumn dc_ph_l1 = new DataColumn("ph_l1", typeof(string));
                    DataColumn dc_ph_l2 = new DataColumn("ph_l2", typeof(string));
                    DataColumn dc_ph_l3 = new DataColumn("ph_l3", typeof(string));
                    DataColumn dc_sg_l1 = new DataColumn("sg_l1", typeof(string));
                    DataColumn dc_sg_l2 = new DataColumn("sg_l2", typeof(string));
                    DataColumn dc_sg_l3 = new DataColumn("sg_l3", typeof(string));
                    DataColumn dc_th_l1 = new DataColumn("th_l1", typeof(string));
                    DataColumn dc_th_l2 = new DataColumn("th_l2", typeof(string));
                    DataColumn dc_th_l3 = new DataColumn("th_l3", typeof(string));
                    DataColumn dc_tw_l1 = new DataColumn("tw_l1", typeof(string));
                    DataColumn dc_tw_l2 = new DataColumn("tw_l2", typeof(string));
                    DataColumn dc_tw_l3 = new DataColumn("tw_l3", typeof(string));
                    DataColumn dc_vn_l1 = new DataColumn("vn_l1", typeof(string));
                    DataColumn dc_vn_l2 = new DataColumn("vn_l2", typeof(string));
                    DataColumn dc_vn_l3 = new DataColumn("vn_l3", typeof(string));
                    DataColumn dc_cat_id = new DataColumn("cat_id", typeof(string));
                    DataColumn dc_cat_my = new DataColumn("cat_my", typeof(string));
                    DataColumn dc_cat_ph = new DataColumn("cat_ph", typeof(string));
                    DataColumn dc_cat_sg = new DataColumn("cat_sg", typeof(string));
                    DataColumn dc_cat_th = new DataColumn("cat_th", typeof(string));
                    DataColumn dc_cat_tw = new DataColumn("cat_tw", typeof(string));
                    DataColumn dc_cat_vn = new DataColumn("cat_vn", typeof(string));

                    dTable.Columns.Add(dc_id_l1);
                    dTable.Columns.Add(dc_id_l2);
                    dTable.Columns.Add(dc_id_l3);
                    dTable.Columns.Add(dc_my_l1);
                    dTable.Columns.Add(dc_my_l2);
                    dTable.Columns.Add(dc_my_l3);
                    dTable.Columns.Add(dc_ph_l1);
                    dTable.Columns.Add(dc_ph_l2);
                    dTable.Columns.Add(dc_ph_l3);
                    dTable.Columns.Add(dc_sg_l1);
                    dTable.Columns.Add(dc_sg_l2);
                    dTable.Columns.Add(dc_sg_l3);
                    dTable.Columns.Add(dc_th_l1);
                    dTable.Columns.Add(dc_th_l2);
                    dTable.Columns.Add(dc_th_l3);
                    dTable.Columns.Add(dc_tw_l1);
                    dTable.Columns.Add(dc_tw_l2);
                    dTable.Columns.Add(dc_tw_l3);
                    dTable.Columns.Add(dc_vn_l1);
                    dTable.Columns.Add(dc_vn_l2);
                    dTable.Columns.Add(dc_vn_l3);
                    dTable.Columns.Add(dc_cat_id);
                    dTable.Columns.Add(dc_cat_my);
                    dTable.Columns.Add(dc_cat_ph);
                    dTable.Columns.Add(dc_cat_sg);
                    dTable.Columns.Add(dc_cat_th);
                    dTable.Columns.Add(dc_cat_tw);
                    dTable.Columns.Add(dc_cat_vn);
                    
                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["id_l1"] = workSheet.Cells[i, 1].Value;
                        dr["id_l2"] = workSheet.Cells[i, 2].Value;
                        dr["id_l3"] = workSheet.Cells[i, 3].Value;
                        dr["my_l1"] = workSheet.Cells[i, 4].Value;
                        dr["my_l2"] = workSheet.Cells[i, 5].Value;
                        dr["my_l3"] = workSheet.Cells[i, 6].Value;
                        dr["ph_l1"] = workSheet.Cells[i, 7].Value;
                        dr["ph_l2"] = workSheet.Cells[i, 8].Value;
                        dr["ph_l3"] = workSheet.Cells[i, 9].Value;
                        dr["sg_l1"] = workSheet.Cells[i, 10].Value;
                        dr["sg_l2"] = workSheet.Cells[i, 11].Value;
                        dr["sg_l3"] = workSheet.Cells[i, 12].Value;
                        dr["th_l1"] = workSheet.Cells[i, 13].Value;
                        dr["th_l2"] = workSheet.Cells[i, 14].Value;
                        dr["th_l3"] = workSheet.Cells[i, 15].Value;
                        dr["tw_l1"] = workSheet.Cells[i, 16].Value;
                        dr["tw_l2"] = workSheet.Cells[i, 17].Value;
                        dr["tw_l3"] = workSheet.Cells[i, 18].Value;
                        dr["vn_l1"] = workSheet.Cells[i, 19].Value;
                        dr["vn_l2"] = workSheet.Cells[i, 20].Value;
                        dr["vn_l3"] = workSheet.Cells[i, 21].Value;
                        dr["cat_id"] = workSheet.Cells[i, 22].Value;
                        dr["cat_my"] = workSheet.Cells[i, 23].Value;
                        dr["cat_ph"] = workSheet.Cells[i, 24].Value;
                        dr["cat_sg"] = workSheet.Cells[i, 25].Value;
                        dr["cat_th"] = workSheet.Cells[i, 26].Value;
                        dr["cat_tw"] = workSheet.Cells[i, 27].Value;
                        dr["cat_vn"] = workSheet.Cells[i, 28].Value;
                        dTable.Rows.Add(dr);
                    }

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        gridCategory.Rows.Add(i+1, dTable.Rows[i][0].ToString(),
                            dTable.Rows[i][1].ToString(),
                            dTable.Rows[i][2].ToString(),
                            dTable.Rows[i][3].ToString(),
                            dTable.Rows[i][4].ToString(),
                            dTable.Rows[i][5].ToString(),
                            dTable.Rows[i][6].ToString(),
                            dTable.Rows[i][7].ToString(),
                            dTable.Rows[i][8].ToString(),
                            dTable.Rows[i][9].ToString(),
                            dTable.Rows[i][10].ToString(),
                            dTable.Rows[i][11].ToString(),
                            dTable.Rows[i][12].ToString(),
                            dTable.Rows[i][13].ToString(),
                            dTable.Rows[i][14].ToString(),
                            dTable.Rows[i][15].ToString(),
                            dTable.Rows[i][16].ToString(),
                            dTable.Rows[i][17].ToString(),
                            dTable.Rows[i][18].ToString(),
                            dTable.Rows[i][19].ToString(),
                            dTable.Rows[i][20].ToString(),
                            dTable.Rows[i][21].ToString(),
                            dTable.Rows[i][22].ToString(),
                            dTable.Rows[i][23].ToString(),
                            dTable.Rows[i][24].ToString(),
                            dTable.Rows[i][25].ToString(),
                            dTable.Rows[i][26].ToString(),
                            dTable.Rows[i][27].ToString());
                    }
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLoadCategory_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            gridCategory.Rows.Clear();

            try
            {
                ClsShopee cc = new ClsShopee();
                IList<ShopeeCategory> ShopeeCategorys = cc.GetShopeeCategoryList();
                for (int i = 0; i < ShopeeCategorys.Count; i++)
                {
                    gridCategory.Rows.Add(i + 1,
                        ShopeeCategorys[i].id_l1,
                        ShopeeCategorys[i].id_l2,
                        ShopeeCategorys[i].id_l3,
                        ShopeeCategorys[i].my_l1,
                        ShopeeCategorys[i].my_l2,
                        ShopeeCategorys[i].my_l3,
                        ShopeeCategorys[i].ph_l1,
                        ShopeeCategorys[i].ph_l2,
                        ShopeeCategorys[i].ph_l3,
                        ShopeeCategorys[i].sg_l1,
                        ShopeeCategorys[i].sg_l2,
                        ShopeeCategorys[i].sg_l3,
                        ShopeeCategorys[i].th_l1,
                        ShopeeCategorys[i].th_l2,
                        ShopeeCategorys[i].th_l3,
                        ShopeeCategorys[i].tw_l1,
                        ShopeeCategorys[i].tw_l2,
                        ShopeeCategorys[i].tw_l3,
                        ShopeeCategorys[i].vn_l1,
                        ShopeeCategorys[i].vn_l1,
                        ShopeeCategorys[i].vn_l1,
                        ShopeeCategorys[i].cat_id,
                        ShopeeCategorys[i].cat_my,
                        ShopeeCategorys[i].cat_ph,
                        ShopeeCategorys[i].cat_sg,
                        ShopeeCategorys[i].cat_th,
                        ShopeeCategorys[i].cat_tw,
                        ShopeeCategorys[i].cat_vn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getShopeeAccount: " + ex.Message);
                Cursor.Current = Cursors.Default;
            }

            Cursor.Current = Cursors.Default;
        }

        private void btn_save_category_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Shopee 카테고리 연결 데이터 정보를 저장 하시겠습니까?\r\n기존 데이터는 삭제됩니다.", "Shopee 카테고리 연결 데이터 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                using (AppDbContext context = new AppDbContext())
                {
                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShopeeCategories");
                }

                for (int i = 0; i < gridCategory.Rows.Count; i++)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ShopeeCategory newCategory = new ShopeeCategory
                        {
                            
                            id_l1 = gridCategory.Rows[i].Cells["gridCategory_id_l1"].Value.ToString(),
                            id_l2 = gridCategory.Rows[i].Cells["gridCategory_id_l2"].Value.ToString(),
                            id_l3 = gridCategory.Rows[i].Cells["gridCategory_id_l3"].Value.ToString(),
                            my_l1 = gridCategory.Rows[i].Cells["gridCategory_my_l1"].Value.ToString(),
                            my_l2 = gridCategory.Rows[i].Cells["gridCategory_my_l2"].Value.ToString(),
                            my_l3 = gridCategory.Rows[i].Cells["gridCategory_my_l3"].Value.ToString(),
                            ph_l1 = gridCategory.Rows[i].Cells["gridCategory_ph_l1"].Value.ToString(),
                            ph_l2 = gridCategory.Rows[i].Cells["gridCategory_ph_l2"].Value.ToString(),
                            ph_l3 = gridCategory.Rows[i].Cells["gridCategory_ph_l3"].Value.ToString(),
                            sg_l1 = gridCategory.Rows[i].Cells["gridCategory_sg_l1"].Value.ToString(),
                            sg_l2 = gridCategory.Rows[i].Cells["gridCategory_sg_l2"].Value.ToString(),
                            sg_l3 = gridCategory.Rows[i].Cells["gridCategory_sg_l3"].Value.ToString(),
                            th_l1 = gridCategory.Rows[i].Cells["gridCategory_th_l1"].Value.ToString(),
                            th_l2 = gridCategory.Rows[i].Cells["gridCategory_th_l2"].Value.ToString(),
                            th_l3 = gridCategory.Rows[i].Cells["gridCategory_th_l3"].Value.ToString(),
                            tw_l1 = gridCategory.Rows[i].Cells["gridCategory_tw_l1"].Value.ToString(),
                            tw_l2 = gridCategory.Rows[i].Cells["gridCategory_tw_l2"].Value.ToString(),
                            tw_l3 = gridCategory.Rows[i].Cells["gridCategory_tw_l3"].Value.ToString(),
                            vn_l1 = gridCategory.Rows[i].Cells["gridCategory_vn_l1"].Value.ToString(),
                            vn_l2 = gridCategory.Rows[i].Cells["gridCategory_vn_l2"].Value.ToString(),
                            vn_l3 = gridCategory.Rows[i].Cells["gridCategory_vn_l3"].Value.ToString(),
                            cat_id = gridCategory.Rows[i].Cells["gridCategory_id"].Value.ToString(),
                            cat_my = gridCategory.Rows[i].Cells["gridCategory_my"].Value.ToString(),
                            cat_ph = gridCategory.Rows[i].Cells["gridCategory_ph"].Value.ToString(),
                            cat_sg = gridCategory.Rows[i].Cells["gridCategory_sg"].Value.ToString(),
                            cat_th = gridCategory.Rows[i].Cells["gridCategory_th"].Value.ToString(),
                            cat_tw = gridCategory.Rows[i].Cells["gridCategory_tw"].Value.ToString(),
                            cat_vn = gridCategory.Rows[i].Cells["gridCategory_vn"].Value.ToString()
                        };

                        context.ShopeeCategories.Add(newCategory);
                        context.SaveChanges();
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("카테고리 연결 데이터를 저장하였습니다.", "카테고리 연결 데이터 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void BtnDeleteMapData_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("카테고리 연결자료를 삭제 하시겠습니까?", "카테고리 연결 자료 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShopeeCategories");
                    gridCategory.Rows.Clear();

                    var result = context.TemplateVersions.FirstOrDefault();
                    if (result != null)
                    {
                        result.category_map = 0;
                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnGetCategoryData_Click(object sender, EventArgs e)
        {
            using(AppDbContext context = new AppDbContext())
            {
                FormDownloadData form = new FormDownloadData();
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] raw = wc.DownloadData("https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/category_map/version.txt");

                long version = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(raw));

                var result = context.TemplateVersions.FirstOrDefault();
                if(result != null)
                {
                    if(result.category_map < version)
                    {
                        form.templateCurVersion = result.category_map;
                        form.templateNewVersion = version;
                        form.templateName = "카테고리 연결 템플릿";
                        form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/category_map/category_mapping_data.xlsx";
                        form.ShowDialog();
                        btnLoadCategory_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("현재 최신 데이터를 사용중입니다.", "최신데이터 사용중", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {   
                    form.templateNewVersion = version;
                    form.templateCurVersion = 0;
                    form.templateName = "카테고리 연결 템플릿";
                    form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/category_map/category_mapping_data.xlsx";
                    form.ShowDialog();
                    btnLoadCategory_Click(null, null);
                }
            }
        }
    }
}
