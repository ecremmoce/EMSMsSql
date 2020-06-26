using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;

namespace ShopeeManagement
{
    public partial class FormDownloadData : DevExpress.XtraEditors.XtraForm
    {
        public FormDownloadData()
        {
            // 대리자를 초기화한다.
			csst = new CSafeSetText(CrossSafeSetTextMethod);
			cssm = new CSafeSetMaximum(CrossSafeSetMaximumMethod);
			cssv = new CSafeSetValue(CrossSafeSetValueMethod);
			
			// 웹 클라이언트 개체를 초기화하고,
			wc = new WebClient();
            InitializeComponent();
        }
        private delegate void CSafeSetText(string text);
        private delegate void CSafeSetMaximum(Int32 value);
        private delegate void CSafeSetValue(Int32 value);


        private CSafeSetText csst;
        private CSafeSetMaximum cssm;
        private CSafeSetValue cssv;
        private WebClient wc;
        private Boolean setBaseSize;
        private Boolean nowDownloading;

        public string downloadAddr = "";
        public string templateName = "";
        public long templateCurVersion;
        public long templateNewVersion;
        public string country;

        private void FormDownloadData_Load(object sender, EventArgs e)
        {
            txtCurVersion.Text = templateCurVersion.ToString();
            txtUpdateVersion.Text = templateNewVersion.ToString();
            txtTemplateName.Text = templateName;

            // 이벤트를 연결한다.
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(fileDownloadCompleted);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(fileDownloadProgressChanged);
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (nowDownloading)
            {
                MessageBox.Show("이미 다운로드가 진행 중입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            String remoteAddress = downloadAddr;
            if (String.IsNullOrEmpty(remoteAddress))
            {
                MessageBox.Show("주소가 입력되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 파일이 저장될 위치를 저장한다.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\ShopeeManagement\template\";
            String fileName = String.Format(path + @"\{0}", System.IO.Path.GetFileName(remoteAddress));

            // 폴더가 존재하지 않는다면 폴더를 생성한다.
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            try
            {
                if(File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // 사용자 계정의 template 폴더에 파일 이름대로 저장한다.
                wc.DownloadFileAsync(new Uri(remoteAddress), fileName);

                // 다운로드 중이라는걸 알리기 위한 값을 설정하고,
                // 프로그레스바의 크기를 0으로 만든다.
                // 그리고, 텍스트박스 및 시작 버튼을 비활성화시켜서 작업에 중복이 되지 않도록 한다.
                progressBarControl1.Properties.Minimum = 0;
                setBaseSize = false;
                nowDownloading = true;
                btnDownload.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        void CrossSafeSetValueMethod(Int32 value)
        {
            if (progressBarControl1.InvokeRequired)
            {
                progressBarControl1.Invoke(cssm, value);
            }
            else
            {
                progressBarControl1.EditValue = value;
                progressBarControl1.Update();
            }
                
        }
        void CrossSafeSetMaximumMethod(Int32 value)
        {
            if (progressBarControl1.InvokeRequired)
                progressBarControl1.Invoke(cssm, value);
            else
                progressBarControl1.Properties.Maximum = value;
        }
        void CrossSafeSetTextMethod(String text)
        {
            if (this.InvokeRequired)
                this.Invoke(csst, text);
            else
                this.Text = text;
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
        void fileDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            // e.BytesReceived
            //   받은 데이터의 크기를 저장합니다.

            // e.TotalBytesToReceive
            //   받아야 할 모든 데이터의 크기를 저장합니다.

            // 프로그레스바의 최대 크기가 정해지지 않은 경우,
            // 받아야 할 최대 데이터 량으로 설정한다.
            if (!setBaseSize)
            {
                CrossSafeSetMaximumMethod((int)e.TotalBytesToReceive);
                setBaseSize = true;
            }

            // 받은 데이터 량을 나타낸다.
            CrossSafeSetValueMethod((int)e.BytesReceived);

            // 받은 데이터 / 받아야할 데이터 (퍼센트) 로 나타낸다.
            CrossSafeSetTextMethod(String.Format("{0:N0} / {1:N0} ({2:P})", e.BytesReceived, e.TotalBytesToReceive, (Double)e.BytesReceived / (Double)e.TotalBytesToReceive));
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

        void fileDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            nowDownloading = false;
            //파일이 다운로드 되었을 때 그 파일을 열어서 업로드 한다.

            Application.DoEvents();

            // 파일이 저장될 위치를 저장한다.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\ShopeeManagement\template\";
            String fileName = String.Format(path + "{0}", System.IO.Path.GetFileName(downloadAddr));            
            //파일 종류에 따라 저장 방법이 다르다.
            //카테고리 맵핑 데이터
            if (fileName.Contains("category_mapping_data.xlsx"))
            {
                var package = new ExcelPackage(new FileInfo(fileName));

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


                using (AppDbContext context = new AppDbContext())
                {
                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShopeeCategories");
                }

                using (AppDbContext context = new AppDbContext())
                {
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        ShopeeCategory newCategory = new ShopeeCategory
                        {
                            id_l1 = workSheet.Cells[i, 1].Value.ToString(),
                            id_l2 = workSheet.Cells[i, 2].Value.ToString(),
                            id_l3 = workSheet.Cells[i, 3].Value.ToString(),
                            my_l1 = workSheet.Cells[i, 4].Value.ToString(),
                            my_l2 = workSheet.Cells[i, 5].Value.ToString(),
                            my_l3 = workSheet.Cells[i, 6].Value.ToString(),
                            ph_l1 = workSheet.Cells[i, 7].Value.ToString(),
                            ph_l2 = workSheet.Cells[i, 8].Value.ToString(),
                            ph_l3 = workSheet.Cells[i, 9].Value.ToString(),
                            sg_l1 = workSheet.Cells[i, 10].Value.ToString(),
                            sg_l2 = workSheet.Cells[i, 11].Value.ToString(),
                            sg_l3 = workSheet.Cells[i, 12].Value.ToString(),
                            th_l1 = workSheet.Cells[i, 13].Value.ToString(),
                            th_l2 = workSheet.Cells[i, 14].Value.ToString(),
                            th_l3 = workSheet.Cells[i, 15].Value.ToString(),
                            tw_l1 = workSheet.Cells[i, 16].Value.ToString(),
                            tw_l2 = workSheet.Cells[i, 17].Value.ToString(),
                            tw_l3 = workSheet.Cells[i, 18].Value.ToString(),
                            vn_l1 = workSheet.Cells[i, 19].Value.ToString(),
                            vn_l2 = workSheet.Cells[i, 20].Value.ToString(),
                            vn_l3 = workSheet.Cells[i, 21].Value.ToString(),
                            cat_id = workSheet.Cells[i, 22].Value.ToString(),
                            cat_my = workSheet.Cells[i, 23].Value.ToString(),
                            cat_ph = workSheet.Cells[i, 24].Value.ToString(),
                            cat_sg = workSheet.Cells[i, 25].Value.ToString(),
                            cat_th = workSheet.Cells[i, 26].Value.ToString(),
                            cat_tw = workSheet.Cells[i, 27].Value.ToString(),
                            cat_vn = workSheet.Cells[i, 28].Value.ToString()
                        };

                        context.ShopeeCategories.Add(newCategory);

                    }
                    context.SaveChanges();

                    //버전 업데이트
                    var res = context.TemplateVersions.FirstOrDefault();
                    if (res == null)
                    {
                        TemplateVersion tv = new TemplateVersion
                        {
                            category_map = Convert.ToInt64(templateNewVersion)
                        };

                        context.TemplateVersions.Add(tv);
                        context.SaveChanges();
                    }
                    else
                    {
                        res.category_map = Convert.ToInt64(templateNewVersion);
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("카테고리 맵핑 데이터를 업데이트 하였습니다.", "업데이트 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (fileName.Contains("AttributeList_"))
            {
                progressBarControl1.EditValue = 0;
                progressBarControl1.Update();
                DataTable dt = ConvertCSVtoDataTable(fileName);
                if (dt != null)
                {
                    //정상적으로 읽어 온 후 삭제 한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        List<AllAttributeList> attributeList = new List<AllAttributeList>();
                        attributeList = context.AllAttributeLists
                                .Where(x => x.country == country).ToList();

                        if (attributeList.Count > 0)
                        {
                            context.AllAttributeLists.RemoveRange(attributeList);
                            context.SaveChanges();
                        }

                        progressBarControl1.Properties.Maximum = dt.Rows.Count;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            bool isMan = false;
                            if (dt.Rows[i][5].ToString().ToUpper() == "TRUE")
                            {
                                isMan = true;
                            }

                            AllAttributeList newData = new AllAttributeList
                            {
                                country = country,
                                category_id = Convert.ToInt64(dt.Rows[i][1].ToString()),
                                attribute_id = Convert.ToInt64(dt.Rows[i][2].ToString()),
                                attribute_name = dt.Rows[i][3].ToString(),
                                attribute_name_kor = dt.Rows[i][4].ToString(),
                                is_mandatory = isMan,
                                attribute_type = dt.Rows[i][6].ToString(),
                                input_type = dt.Rows[i][7].ToString(),
                                options = dt.Rows[i][8].ToString(),
                                options_kor = dt.Rows[i][9].ToString(),
                                values_original_value = dt.Rows[i][10].ToString(),
                                values_translate_value = dt.Rows[i][11].ToString(),
                                values_translate_kor = dt.Rows[i][12].ToString()
                            };

                            context.AllAttributeLists.Add(newData);
                            progressBarControl1.EditValue = i + 1;
                            progressBarControl1.Update();
                        }

                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            if (country == "ID")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_ID = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }
                            else if (country == "MY")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_MY = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }
                            else if (country == "SG")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_SG = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }
                            else if (country == "TH")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_TH = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }
                            else if (country == "TW")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_TW = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }
                            else if (country == "PH")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_PH = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }
                            else if (country == "VN")
                            {
                                TemplateVersion tv = new TemplateVersion
                                {
                                    attribute_VN = Convert.ToInt64(templateNewVersion)
                                };
                                context.TemplateVersions.Add(tv);
                                context.SaveChanges();
                            }


                        }
                        else
                        {
                            if (country == "ID")
                            {
                                res.attribute_ID = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                            else if (country == "MY")
                            {
                                res.attribute_MY = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                            else if (country == "SG")
                            {
                                res.attribute_SG = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                            else if (country == "TH")
                            {
                                res.attribute_TH = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                            else if (country == "TW")
                            {
                                res.attribute_TW = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                            else if (country == "PH")
                            {
                                res.attribute_PH = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                            else if (country == "VN")
                            {
                                res.attribute_VN = Convert.ToInt64(templateNewVersion);
                                context.SaveChanges();
                            }
                        }
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("업데이트를 완료 하였습니다.", "업데이트 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (fileName.Contains("attribute_name_map.xlsx"))
            {
                DataTable dt_excel = GetDataTableFromExcel(fileName, true);
                using (AppDbContext context = new AppDbContext())
                {
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
                            }
                        }
                    }

                    context.SaveChanges();
                    //버전 업데이트
                    var res = context.TemplateVersions.FirstOrDefault();
                    if (res == null)
                    {
                        TemplateVersion tv = new TemplateVersion
                        {
                            attribute_name_map = Convert.ToInt64(templateNewVersion)
                        };

                        context.TemplateVersions.Add(tv);
                        context.SaveChanges();
                    }
                    else
                    {
                        res.attribute_name_map = Convert.ToInt64(templateNewVersion);
                        context.SaveChanges();
                    }

                    MessageBox.Show("업데이트를 완료 하였습니다.", "업데이트 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (fileName.Contains("category.xlsx"))
            {
                using (AppDbContext context = new AppDbContext())
                {
                    var lstCategory = context.ShopeeCategorOnlys
                        .Where(x => x.Country == country
                    && x.UserId == global_var.userId).ToList();


                    if (lstCategory.Count > 0)
                    {
                        context.ShopeeCategorOnlys.RemoveRange(lstCategory);
                        context.SaveChanges();
                    }

                    DataTable dt_excel = GetDataTableFromExcel(fileName, true);

                    progressBarControl1.Properties.Maximum = dt_excel.Rows.Count;
                    for (int i = 0; i < dt_excel.Rows.Count; i++)
                    {
                        ShopeeCategoryOnly newCategory = new ShopeeCategoryOnly
                        {
                            CatLevel1 = dt_excel.Rows[i][0].ToString(),
                            CatLevel2 = dt_excel.Rows[i][1].ToString(),
                            CatLevel3 = dt_excel.Rows[i][2].ToString(),
                            CatLevel4 = dt_excel.Rows[i][3].ToString(),
                            CatLevel5 = dt_excel.Rows[i][4].ToString(),
                            CatId = dt_excel.Rows[i][5].ToString(),
                            Country = country
                        };

                        context.ShopeeCategorOnlys.Add(newCategory);
                        progressBarControl1.EditValue = i + 1;
                        progressBarControl1.Update();
                    }

                    context.SaveChanges();

                    //버전 업데이트
                    var res = context.TemplateVersions.FirstOrDefault();
                    if (res == null)
                    {
                        if (country == "ID")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_id = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else if (country == "MY")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_my = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else if (country == "SG")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_sg = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else if (country == "PH")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_ph = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else if (country == "TH")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_th = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else if (country == "TW")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_tw = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else if (country == "VN")
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                category_vn = Convert.ToInt64(templateNewVersion)
                            };
                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        if (country == "ID")
                        {
                            res.category_id = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                        else if (country == "MY")
                        {
                            res.category_my = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                        else if (country == "SG")
                        {
                            res.category_sg = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                        else if (country == "PH")
                        {
                            res.category_ph = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                        else if (country == "TH")
                        {
                            res.category_th = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                        else if (country == "TW")
                        {
                            res.category_tw = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                        else if (country == "VN")
                        {
                            res.category_vn = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                }

                MessageBox.Show("업데이트를 완료 하였습니다.", "업데이트 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(fileName.Contains("SHIPPING_RATE_"))
            {
                var package = new ExcelPackage(new FileInfo(fileName));
                OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                DataTable dTable = new DataTable("shipping_rate");

                using (AppDbContext context = new AppDbContext())
                {
                    if (country == "ID")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsIds");

                        DataRow dr;
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateSlsId newRate = new ShippingRateSlsId
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ZoneA = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString()),
                                ZoneB = Convert.ToDecimal(workSheet.Cells[i, 3].Value.ToString()),
                                ZoneC = Convert.ToDecimal(workSheet.Cells[i, 4].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(workSheet.Cells[i, 5].Value.ToString())
                            };

                            context.ShippingRateSlsIds.Add(newRate);
                        }

                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_ID = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_ID = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                    else if (country == "MY")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSLSMies");

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateSlsMy newRate = new ShippingRateSlsMy
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ZoneA = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString()),
                                ZoneB = Convert.ToDecimal(workSheet.Cells[i, 3].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(workSheet.Cells[i, 4].Value.ToString()),
                                BuyerShippingPrice = Convert.ToDecimal(workSheet.Cells[i, 5].Value.ToString())
                            };

                            context.ShippingRateSlsMys.Add(newRate);
                        }
                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_MY = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_MY = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                    else if (country == "SG")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsSgs");

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateSlsSg newRate = new ShippingRateSlsSg
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ShippingFeeAvg = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(workSheet.Cells[i, 3].Value.ToString())
                            };

                            context.ShippingRateSlsSgs.Add(newRate);
                        }

                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_SG = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_SG = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                    else if (country == "PH")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateSlsPhs");

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateSlsPh newRate = new ShippingRateSlsPh
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ZoneA = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString()),
                                ZoneB = Convert.ToDecimal(workSheet.Cells[i, 3].Value.ToString()),
                                ZoneC = Convert.ToDecimal(workSheet.Cells[i, 4].Value.ToString()),
                                HiddenFee = Convert.ToDecimal(workSheet.Cells[i, 5].Value.ToString())
                            };
                            context.ShippingRateSlsPhs.Add(newRate);
                        }

                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_PH = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_PH = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                    else if (country == "TH")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateDRThs");

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateDRTh newRate = new ShippingRateDRTh
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ShippingFeeAvg = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString()),
                                ShippingFeeAvg2 = Convert.ToDecimal(workSheet.Cells[i, 3].Value.ToString())
                            };
                            context.ShippingRateDRThs.Add(newRate);
                        }

                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_TH = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_TH = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                    else if (country == "TW")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateYTOTws");

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateYTOTw newRate = new ShippingRateYTOTw
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ShippingFeeAvg = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString())
                            };

                            context.ShippingRateYTOTws.Add(newRate);                            
                        }
                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_TW = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_TW = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                    else if (country == "VN")
                    {
                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE ShippingRateDRVns");

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            ShippingRateDRVn newRate = new ShippingRateDRVn
                            {
                                Weight = Convert.ToInt32(workSheet.Cells[i, 1].Value.ToString()),
                                ShippingFeeAvg = Convert.ToDecimal(workSheet.Cells[i, 2].Value.ToString())
                            };

                            context.ShippingRateDRVns.Add(newRate);
                        }
                        context.SaveChanges();

                        //버전 업데이트
                        var res = context.TemplateVersions.FirstOrDefault();
                        if (res == null)
                        {
                            TemplateVersion tv = new TemplateVersion
                            {
                                shipping_rate_VN = Convert.ToInt64(templateNewVersion)
                            };

                            context.TemplateVersions.Add(tv);
                            context.SaveChanges();
                        }
                        else
                        {
                            res.shipping_rate_VN = Convert.ToInt64(templateNewVersion);
                            context.SaveChanges();
                        }
                    }
                }
                MessageBox.Show("업데이트를 완료 하였습니다.", "업데이트 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}