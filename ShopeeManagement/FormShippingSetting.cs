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
    public partial class FormShippingSetting : MetroForm
    {
        public FormShippingSetting(string lang)
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
        private void FormShippingSetting_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            CboShopeeCountry.SelectedIndex = 0;
            CboShopeeCountry2.SelectedIndex = 0;
        }

        private void btnUploadShippingRate_Click(object sender, EventArgs e)
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
                    gridRate.DataSource = null;
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                    int colCount = workSheet.Dimension.End.Column;
                                        
                    DataTable dTable = new DataTable("ShippingData");

                    if(CboShopeeCountry2.Text == "싱가폴")
                    {
                        if (colCount != 3)
                        {
                            MessageBox.Show("올바른 싱가폴 배송요율 엑셀이 아닙니다.","엑셀 요율파일 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_no = new DataColumn("No", typeof(string));
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비", typeof(string));
                            DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));

                            dTable.Columns.Add(dc_no);
                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ShippingFeeAvg);
                            dTable.Columns.Add(dc_HiddenFee);

                            DataRow dr;

                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["No"] = i - 1;
                                dr["무게(g)"] = string.Format("{0:n2}", workSheet.Cells[i, 1].Value);
                                dr["평균운임비"] = string.Format("{0:n2}",workSheet.Cells[i, 2].Value);
                                dr["Hidden Fee"] = string.Format("{0:n2}",workSheet.Cells[i, 3].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                    else if(CboShopeeCountry2.Text == "인도네시아")
                    {
                        if (colCount != 5)
                        {
                            MessageBox.Show("올바른 싱가폴 배송요율 엑셀이 아닙니다.", "엑셀 요율파일 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_no = new DataColumn("No", typeof(string));
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ZonA = new DataColumn("Zone A", typeof(string));
                            DataColumn dc_ZonB = new DataColumn("Zone B", typeof(string));
                            DataColumn dc_ZonC = new DataColumn("Zone C", typeof(string));
                            DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));

                            dTable.Columns.Add(dc_no);
                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ZonA);
                            dTable.Columns.Add(dc_ZonB);
                            dTable.Columns.Add(dc_ZonC);
                            dTable.Columns.Add(dc_HiddenFee);

                            DataRow dr;
                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["No"] = i - 1;
                                dr["무게(g)"] = string.Format("{0:n0}", workSheet.Cells[i, 1].Value);
                                dr["Zone A"] = string.Format("{0:n0}", workSheet.Cells[i, 2].Value);
                                dr["Zone B"] = string.Format("{0:n0}", workSheet.Cells[i, 3].Value);
                                dr["Zone C"] = string.Format("{0:n0}", workSheet.Cells[i, 4].Value);
                                dr["Hidden Fee"] = string.Format("{0:n0}", workSheet.Cells[i, 5].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                    else if (CboShopeeCountry2.Text == "말레이시아")
                    {
                        if (colCount != 2)
                        {
                            MessageBox.Show("올바른 말레이시아 배송요율 엑셀이 아닙니다.", "엑셀 요율파일 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비", typeof(string));

                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ShippingFeeAvg);

                            DataRow dr;

                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["무게(g)"] = string.Format("{0:n0}", workSheet.Cells[i, 1].Value);
                                dr["평균운임비"] = string.Format("{0:n0}", workSheet.Cells[i, 2].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }

                    else if (CboShopeeCountry2.Text == "태국")
                    {
                        if (colCount != 3)
                        {
                            MessageBox.Show("올바른 태국 배송요율 엑셀이 아닙니다.", "엑셀 요율파일 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ShippingFeeAvg = new DataColumn("화장품, 식품, 전자제품, 의약품", typeof(string));
                            DataColumn dc_ShippingFeeAvg2 = new DataColumn("non controlled item (패션잡화, home & living 등)", typeof(string));

                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ShippingFeeAvg);
                            dTable.Columns.Add(dc_ShippingFeeAvg2);

                            DataRow dr;

                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["무게(g)"] = string.Format("{0:n0}", workSheet.Cells[i, 1].Value);
                                dr["화장품, 식품, 전자제품, 의약품"] = string.Format("{0:n0}", workSheet.Cells[i, 2].Value);
                                dr["non controlled item (패션잡화, home & living 등)"] = string.Format("{0:n0}", workSheet.Cells[i, 3].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[1].Width = 300;
                            gridRate.Columns[2].Width = 300;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                    else if (CboShopeeCountry2.Text == "대만")
                    {
                        if (colCount != 2)
                        {
                            MessageBox.Show("올바른 대만 배송요율 엑셀이 아닙니다.", "엑셀 요율파일 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비", typeof(string));

                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ShippingFeeAvg);

                            DataRow dr;

                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["무게(g)"] = string.Format("{0:n0}", workSheet.Cells[i, 1].Value);
                                dr["평균운임비"] = string.Format("{0:n0}", workSheet.Cells[i, 2].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                    else if (CboShopeeCountry2.Text == "베트남")
                    {
                        if (colCount != 2)
                        {
                            MessageBox.Show("올바른 베트남 배송요율 엑셀이 아닙니다.", "엑셀 요율파일 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비", typeof(string));

                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ShippingFeeAvg);

                            DataRow dr;

                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["무게(g)"] = string.Format("{0:n0}", workSheet.Cells[i, 1].Value);
                                dr["평균운임비"] = string.Format("{0:n0}", workSheet.Cells[i, 2].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                    else if (CboShopeeCountry2.Text == "필리핀")
                    {
                        if (colCount != 5)
                        {
                            MessageBox.Show("올바른 필리핀 배송요율 엑셀이 아닙니다.", "엑셀 요율파일 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            DataColumn dc_no = new DataColumn("No", typeof(string));
                            DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                            DataColumn dc_ZonA = new DataColumn("Zone A", typeof(string));
                            DataColumn dc_ZonB = new DataColumn("Zone B", typeof(string));
                            DataColumn dc_ZonC = new DataColumn("Zone C", typeof(string));
                            DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));

                            dTable.Columns.Add(dc_no);
                            dTable.Columns.Add(dc_weight);
                            dTable.Columns.Add(dc_ZonA);
                            dTable.Columns.Add(dc_ZonB);
                            dTable.Columns.Add(dc_ZonC);
                            dTable.Columns.Add(dc_HiddenFee);

                            DataRow dr;
                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                dr = dTable.NewRow();
                                dr["No"] = i - 1;
                                dr["무게(g)"] = string.Format("{0:n0}", workSheet.Cells[i, 1].Value);
                                dr["Zone A"] = string.Format("{0:n0}", workSheet.Cells[i, 2].Value);
                                dr["Zone B"] = string.Format("{0:n0}", workSheet.Cells[i, 3].Value);
                                dr["Zone C"] = string.Format("{0:n0}", workSheet.Cells[i, 4].Value);
                                dr["Hidden Fee"] = string.Format("{0:n0}", workSheet.Cells[i, 5].Value);
                                dTable.Rows.Add(dr);
                            }

                            gridRate.DataSource = dTable;
                            gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            gridRate.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }



                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSaveShippingRate_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show(CboShopeeCountry2.Text = " 배송요율 정보를 저장 하시겠습니까?\r\n기존 데이터는 삭제됩니다.", "배송요율 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (CboShopeeCountry2.Text == "싱가폴")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsSgs");
                    }
                }
                else if(CboShopeeCountry2.Text == "인도네시아")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsIds");
                    }
                }

                else if (CboShopeeCountry2.Text == "말레이시아")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateYSMies");
                    }
                }

                else if (CboShopeeCountry2.Text == "태국")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateDRThs");
                    }
                }

                else if (CboShopeeCountry2.Text == "대만")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateYTOTws");
                    }
                }
                else if (CboShopeeCountry2.Text == "필리핀")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsPhs");
                    }
                }


                for (int i = 0; i < gridRate.Rows.Count; i++)
                {
                    if (CboShopeeCountry2.Text == "싱가폴")
                    {
                        double dblWeight = Convert.ToDouble(gridRate.Rows[i].Cells[1].Value.ToString());


                        using (AppDbContext context = new AppDbContext())
                        {
                            ShippingRateSlsSg newRate = new ShippingRateSlsSg
                            {
                                Weight = Convert.ToInt32(Math.Round(dblWeight)),
                                ShippingFeeAvg = Convert.ToDecimal(gridRate.Rows[i].Cells[2].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(gridRate.Rows[i].Cells[3].Value.ToString())
                            };

                            context.ShippingRateSlsSgs.Add(newRate);
                            context.SaveChanges();
                        }
                    }
                    else if (CboShopeeCountry2.Text == "인도네시아")
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ShippingRateSlsId newRate = new ShippingRateSlsId
                            {
                                Weight = Convert.ToInt32(gridRate.Rows[i].Cells[1].Value.ToString()),
                                ZoneA = Convert.ToDecimal(gridRate.Rows[i].Cells[2].Value.ToString()),
                                ZoneB = Convert.ToDecimal(gridRate.Rows[i].Cells[3].Value.ToString()),
                                ZoneC = Convert.ToDecimal(gridRate.Rows[i].Cells[4].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(gridRate.Rows[i].Cells[5].Value.ToString())
                            };

                            context.ShippingRateSlsIds.Add(newRate);
                            context.SaveChanges();
                        }
                    }
                    else if (CboShopeeCountry2.Text == "말레이시아")
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ShippingRateYSMy newRate = new ShippingRateYSMy
                            {
                                Weight = Convert.ToInt32(gridRate.Rows[i].Cells[0].Value.ToString()),
                                ShippingFeeAvg = Convert.ToDecimal(gridRate.Rows[i].Cells[1].Value.ToString())                                
                            };

                            context.ShippingRateYSMys.Add(newRate);
                            context.SaveChanges();
                        }
                    }
                    else if (CboShopeeCountry2.Text == "태국")
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ShippingRateDRTh newRate = new ShippingRateDRTh
                            {
                                Weight = Convert.ToInt32(gridRate.Rows[i].Cells[0].Value.ToString()),
                                ShippingFeeAvg = Convert.ToDecimal(gridRate.Rows[i].Cells[1].Value.ToString()),
                                ShippingFeeAvg2 = Convert.ToDecimal(gridRate.Rows[i].Cells[2].Value.ToString())
                            };

                            context.ShippingRateDRThs.Add(newRate);
                            context.SaveChanges();
                        }
                    }
                    else if (CboShopeeCountry2.Text == "대만")
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ShippingRateYTOTw newRate = new ShippingRateYTOTw
                            {
                                Weight = Convert.ToInt32(gridRate.Rows[i].Cells[0].Value.ToString().Replace(",","")),
                                ShippingFeeAvg = Convert.ToDecimal(gridRate.Rows[i].Cells[1].Value.ToString())
                            };

                            context.ShippingRateYTOTws.Add(newRate);
                            context.SaveChanges();
                        }
                    }
                    else if (CboShopeeCountry2.Text == "필리핀")
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ShippingRateSlsPh newRate = new ShippingRateSlsPh
                            {
                                Weight = Convert.ToInt32(gridRate.Rows[i].Cells[1].Value.ToString()),
                                ZoneA = Convert.ToDecimal(gridRate.Rows[i].Cells[2].Value.ToString()),
                                ZoneB = Convert.ToDecimal(gridRate.Rows[i].Cells[3].Value.ToString()),
                                ZoneC = Convert.ToDecimal(gridRate.Rows[i].Cells[4].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(gridRate.Rows[i].Cells[5].Value.ToString())
                            };

                            context.ShippingRateSlsPhs.Add(newRate);
                            context.SaveChanges();
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("배송요율 데이터를 저장하였습니다.", "배송요율 데이터 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLoadShippingRate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            gridRate.DataSource = null;
            if(CboShopeeCountry.Text == "싱가폴")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateSlsSg> lstShippingRateSGs = cc.GetShippingRateSG();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_no = new DataColumn("No", typeof(string));
                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비", typeof(string));
                    DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));

                    dTable.Columns.Add(dc_no);
                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_ShippingFeeAvg);
                    dTable.Columns.Add(dc_HiddenFee);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRateSGs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["No"] = i + 1;
                        dr["무게(g)"] = string.Format("{0:n2}", lstShippingRateSGs[i].Weight);
                        dr["평균운임비"] = string.Format("{0:n2}", lstShippingRateSGs[i].ShippingFeeAvg);
                        dr["Hidden Fee"] = string.Format("{0:n2}", lstShippingRateSGs[i].HiddenFee);
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                }
                catch (Exception ex)
                {
                }
            }
            else if(CboShopeeCountry.Text == "인도네시아")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateSlsId> lstShippingRateIDs = cc.GetShippingRateID();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_no = new DataColumn("No", typeof(string));
                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_ZoneA = new DataColumn("Zone A", typeof(string));
                    DataColumn dc_ZoneB = new DataColumn("Zone B", typeof(string));
                    DataColumn dc_ZoneC = new DataColumn("Zone C", typeof(string));
                    DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));

                    dTable.Columns.Add(dc_no);
                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_ZoneA);
                    dTable.Columns.Add(dc_ZoneB);
                    dTable.Columns.Add(dc_ZoneC);
                    dTable.Columns.Add(dc_HiddenFee);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRateIDs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["No"] = i + 1;
                        dr["무게(g)"] = string.Format("{0:n0}", lstShippingRateIDs[i].Weight);
                        dr["Zone A"] = string.Format("{0:n0}", lstShippingRateIDs[i].ZoneA);
                        dr["Zone B"] = string.Format("{0:n0}", lstShippingRateIDs[i].ZoneB);
                        dr["Zone C"] = string.Format("{0:n0}", lstShippingRateIDs[i].ZoneC);
                        dr["Hidden Fee"] = string.Format("{0:n0}", lstShippingRateIDs[i].HiddenFee);
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                catch (Exception ex)
                {
                }
            }
            else if (CboShopeeCountry.Text == "말레이시아")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateSlsMy> lstShippingRateMYs = cc.GetShippingRateMY();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_ZoneA = new DataColumn("ZoneA", typeof(string));
                    DataColumn dc_ZoneB = new DataColumn("ZoneB", typeof(string));
                    DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));
                    DataColumn dc_BuyerShippingPrice = new DataColumn("구매자 배송비", typeof(string));

                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_ZoneA);
                    dTable.Columns.Add(dc_ZoneB);
                    dTable.Columns.Add(dc_HiddenFee);
                    dTable.Columns.Add(dc_BuyerShippingPrice);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRateMYs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["무게(g)"] = string.Format("{0:n2}", lstShippingRateMYs[i].Weight);
                        dr["ZoneA"] = string.Format("{0:n2}", lstShippingRateMYs[i].ZoneA);
                        dr["ZoneB"] = string.Format("{0:n2}", lstShippingRateMYs[i].ZoneB);
                        dr["Hidden Fee"] = string.Format("{0:n2}", lstShippingRateMYs[i].HiddenFee);
                        dr["구매자 배송비"] = string.Format("{0:n2}", lstShippingRateMYs[i].BuyerShippingPrice);
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
                catch (Exception ex)
                {
                }
            }
            else if (CboShopeeCountry.Text == "태국")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateDRTh> lstShippingRateTHs = cc.GetShippingRateTH();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비(화장품, 식품, 전자제품, 의약품)", typeof(string));
                    DataColumn dc_ShippingFeeAvg2 = new DataColumn("non controlled item (패션잡화, home & living 등)", typeof(string));

                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_ShippingFeeAvg);
                    dTable.Columns.Add(dc_ShippingFeeAvg2);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRateTHs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["무게(g)"] = lstShippingRateTHs[i].Weight;
                        dr["평균운임비(화장품, 식품, 전자제품, 의약품)"] = string.Format("{0:n0}", lstShippingRateTHs[i].ShippingFeeAvg);
                        dr["non controlled item (패션잡화, home & living 등)"] = string.Format("{0:n0}", lstShippingRateTHs[i].ShippingFeeAvg2);
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    gridRate.Columns[1].Width = 300;
                    gridRate.Columns[2].Width = 300;

                }
                catch (Exception ex)
                {
                }
            }
            else if (CboShopeeCountry.Text == "대만")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateYTOTw> lstShippingRateTWs = cc.GetShippingRateTW();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_ShippingFeeAvg = new DataColumn("평균운임비", typeof(string));

                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_ShippingFeeAvg);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRateTWs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["무게(g)"] = string.Format("{0:n0}", lstShippingRateTWs[i].Weight);
                        dr["평균운임비"] = string.Format("{0:n0}", lstShippingRateTWs[i].ShippingFeeAvg);
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
                catch (Exception ex)
                {
                }
            }
            else if (CboShopeeCountry.Text == "필리핀")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateSlsPh> lstShippingRatePHs = cc.GetShippingRatePH();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_no = new DataColumn("No", typeof(string));
                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_ZoneA = new DataColumn("Zone A", typeof(string));
                    DataColumn dc_ZoneB = new DataColumn("Zone B", typeof(string));
                    DataColumn dc_ZoneC = new DataColumn("Zone C", typeof(string));
                    DataColumn dc_HiddenFee = new DataColumn("Hidden Fee", typeof(string));

                    dTable.Columns.Add(dc_no);
                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_ZoneA);
                    dTable.Columns.Add(dc_ZoneB);
                    dTable.Columns.Add(dc_ZoneC);
                    dTable.Columns.Add(dc_HiddenFee);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRatePHs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["No"] = i + 1;
                        dr["무게(g)"] = lstShippingRatePHs[i].Weight;
                        dr["Zone A"] = lstShippingRatePHs[i].ZoneA;
                        dr["Zone B"] = lstShippingRatePHs[i].ZoneB;
                        dr["Zone C"] = lstShippingRatePHs[i].ZoneC;
                        dr["Hidden Fee"] = lstShippingRatePHs[i].HiddenFee;
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                }
                catch (Exception ex)
                {
                }
            }

            else if (CboShopeeCountry.Text == "베트남")
            {
                try
                {
                    ClsShopee cc = new ClsShopee();
                    IList<ShippingRateDRVn> lstShippingRateVNs = cc.GetShippingRateVN();

                    DataTable dTable = new DataTable("ShippingData");

                    DataColumn dc_no = new DataColumn("No", typeof(string));
                    DataColumn dc_weight = new DataColumn("무게(g)", typeof(string));
                    DataColumn dc_rate = new DataColumn("요율", typeof(string));

                    dTable.Columns.Add(dc_no);
                    dTable.Columns.Add(dc_weight);
                    dTable.Columns.Add(dc_rate);

                    DataRow dr;
                    for (int i = 0; i < lstShippingRateVNs.Count; i++)
                    {
                        dr = dTable.NewRow();
                        dr["No"] = i + 1;
                        dr["무게(g)"] = string.Format("{0:n0}", lstShippingRateVNs[i].Weight);
                        dr["요율"] = string.Format("{0:n0}", lstShippingRateVNs[i].ShippingFeeAvg);
                        dTable.Rows.Add(dr);
                    }

                    gridRate.DataSource = dTable;
                    gridRate.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gridRate.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gridRate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                }
                catch (Exception ex)
                {
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnDelLogisticRate_Click(object sender, EventArgs e)
        {

            DialogResult Result = MessageBox.Show(CboShopeeCountry2.Text = " 배송요율 정보를 삭제 하시겠습니까?", "배송요율 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                gridRate.DataSource = null;
                if (CboShopeeCountry.Text == "싱가폴")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsSgs");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_SG = 0;
                            context.SaveChanges();
                        }
                    }
                }
                else if (CboShopeeCountry.Text == "인도네시아")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsIds");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_ID = 0;
                            context.SaveChanges();
                        }
                    }
                }

                else if (CboShopeeCountry.Text == "말레이시아")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateYSMies");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_MY = 0;
                            context.SaveChanges();
                        }
                    }
                }

                else if (CboShopeeCountry.Text == "태국")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateDRThs");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_TH = 0;
                            context.SaveChanges();
                        }
                    }
                }

                else if (CboShopeeCountry.Text == "대만")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateYTOTws");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_TW = 0;
                            context.SaveChanges();
                        }
                    }
                }
                else if (CboShopeeCountry.Text == "베트남")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateDRVns");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_VN = 0;
                            context.SaveChanges();
                        }
                    }
                }
                else if (CboShopeeCountry.Text == "필리핀")
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsPhs");

                        var result = context.TemplateVersions.FirstOrDefault();
                        if (result != null)
                        {
                            result.shipping_rate_PH = 0;
                            context.SaveChanges();
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnAutoUpdate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string versionUrl = "";
            string downloadAddr = "";
            long savedVer = 0;
            string country = "";
            using (AppDbContext context = new AppDbContext())
            {
                var result = context.TemplateVersions.FirstOrDefault();
                if (result != null)
                {
                    if (CboShopeeCountry.Text == "싱가폴")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_SG.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_SG_SLS.xlsx";
                        savedVer = result.shipping_rate_SG;
                        country = "SG";
                    }
                    else if (CboShopeeCountry.Text == "인도네시아")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_ID.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_ID_SLS.xlsx";
                        savedVer = result.shipping_rate_ID;
                        country = "ID";
                    }

                    else if (CboShopeeCountry.Text == "말레이시아")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_MY.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_MY_SLS.xlsx";
                        savedVer = result.shipping_rate_MY;
                        country = "MY";
                    }

                    else if (CboShopeeCountry.Text == "태국")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_TH.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_TH_DRL.xlsx";
                        savedVer = result.shipping_rate_TH;
                        country = "TH";
                    }

                    else if (CboShopeeCountry.Text == "대만")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_TW.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_TW_YTO.xlsx";
                        savedVer = result.shipping_rate_TW;
                        country = "TW";
                    }
                    else if (CboShopeeCountry.Text == "베트남")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_VN.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_VN_DRL.xlsx";
                        savedVer = result.shipping_rate_VN;
                        country = "VN";
                    }
                    else if (CboShopeeCountry.Text == "필리핀")
                    {
                        versionUrl = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/VERSION_PH.txt";
                        downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/shipping_rate/SHIPPING_RATE_PH_SLS.xlsx";
                        savedVer = result.shipping_rate_PH;
                        country = "PH";
                    }
                }   
            }


            FormDownloadData form = new FormDownloadData();
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] raw = wc.DownloadData(versionUrl);

            long version = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(raw));

            if (savedVer < version)
            {
                form.templateCurVersion = savedVer;
                form.templateNewVersion = version;
                form.templateName = CboShopeeCountry.Text + " 배송요율 템플릿";
                form.downloadAddr = downloadAddr;
                form.country = country;
                form.ShowDialog();
                btnLoadShippingRate_Click(null, null);
            }
            else
            {
                MessageBox.Show("현재 최신 데이터를 사용중입니다.", "최신데이터 사용중", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
