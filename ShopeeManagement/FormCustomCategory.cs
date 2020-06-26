using MetroFramework.Forms;
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
    public partial class FormCustomCategory : MetroForm
    {
        public FormProductMapper2 fp;
        public FormCustomCategory()
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
        private void FormCustomCategory_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            cboSearchArea.SelectedIndex = 0;
            using (AppDbContext context = new AppDbContext())
            {
                if (TxtSrcCountry.Text == "ID")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories                        
                        .Where(x => x.cat_id == TxtSrcCategoryId.Text.Trim())                        
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)                        
                        .ToList();


                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();

                    if (TxtTarCountry.Text == "SG")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                                categoryListDist[i].cat_id,
                                categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3, categoryListDist[i].cat_sg);
                        }
                    }
                    else if (TxtTarCountry.Text == "MY")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                                categoryListDist[i].cat_id,
                                categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3, categoryListDist[i].cat_my);
                        }
                    }
                    else if (TxtTarCountry.Text == "PH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                                categoryListDist[i].cat_id,
                                categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3, categoryListDist[i].cat_ph);
                        }
                    }
                    else if (TxtTarCountry.Text == "TH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                                categoryListDist[i].cat_id,
                                categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3, categoryListDist[i].cat_th);
                        }
                    }
                    else if (TxtTarCountry.Text == "TW")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                                categoryListDist[i].cat_id,
                                categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3, categoryListDist[i].cat_tw);
                        }
                    }
                    else if (TxtTarCountry.Text == "VN")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                                categoryListDist[i].cat_id,
                                categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3, categoryListDist[i].cat_vn);
                        }
                    }
                }
                else if (TxtSrcCountry.Text == "MY")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(x => x.cat_my == TxtSrcCategoryId.Text.Trim())
                        .OrderBy(x => x.my_l1).ThenBy(x => x.my_l2).ThenBy(x => x.my_l3)
                        .ToList();

                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();

                    if (TxtTarCountry.Text == "SG")
                    {
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                                categoryListDist[i].cat_my,
                                categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3, categoryListDist[i].cat_sg);
                        }
                    }
                    else if (TxtTarCountry.Text == "ID")
                    {
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                                categoryListDist[i].cat_my,
                                categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3, categoryListDist[i].cat_id);
                        }
                    }
                    else if (TxtTarCountry.Text == "PH")
                    {
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                                categoryListDist[i].cat_my,
                                categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3, categoryListDist[i].cat_ph);
                        }
                    }
                    else if (TxtTarCountry.Text == "TH")
                    {
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                                categoryListDist[i].cat_my,
                                categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3, categoryListDist[i].cat_th);
                        }
                    }
                    else if (TxtTarCountry.Text == "TW")
                    {
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                                categoryListDist[i].cat_my,
                                categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3, categoryListDist[i].cat_tw);
                        }
                    }
                    else if (TxtTarCountry.Text == "VN")
                    {
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                                categoryListDist[i].cat_my,
                                categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3, categoryListDist[i].cat_vn);
                        }
                    }
                }
                else if (TxtSrcCountry.Text == "PH")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(x => x.cat_ph == TxtSrcCategoryId.Text.Trim())
                        .OrderBy(x => x.ph_l1).ThenBy(x => x.ph_l2).ThenBy(x => x.ph_l3)
                        .ToList();

                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    
                    if (TxtTarCountry.Text == "SG")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                                categoryListDist[i].cat_ph,
                                categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3, categoryListDist[i].cat_sg);
                        }
                    }
                    else if (TxtTarCountry.Text == "ID")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                                categoryListDist[i].cat_ph,
                                categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3, categoryListDist[i].cat_id);
                        }
                    }
                    else if (TxtTarCountry.Text == "MY")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                                categoryListDist[i].cat_ph,
                                categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3, categoryListDist[i].cat_my);
                        }
                    }
                    else if (TxtTarCountry.Text == "TH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                                categoryListDist[i].cat_ph,
                                categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3, categoryListDist[i].cat_th);
                        }
                    }
                    else if (TxtTarCountry.Text == "TW")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                                categoryListDist[i].cat_ph,
                                categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3, categoryListDist[i].cat_tw);
                        }
                    }
                    else if (TxtTarCountry.Text == "VN")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                                categoryListDist[i].cat_ph,
                                categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3, categoryListDist[i].cat_vn);
                        }
                    }
                }
                else if (TxtSrcCountry.Text == "SG")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(x => x.cat_sg == TxtSrcCategoryId.Text.Trim())
                        .OrderBy(x => x.sg_l1).ThenBy(x => x.sg_l2).ThenBy(x => x.sg_l3)
                        .ToList();

                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();

                    if (TxtTarCountry.Text == "PH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                                categoryListDist[i].cat_sg,
                                categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3, categoryListDist[i].cat_ph);
                        }
                    }
                    else if (TxtTarCountry.Text == "ID")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                                categoryListDist[i].cat_sg,
                                categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3, categoryListDist[i].cat_id);
                        }
                    }
                    else if (TxtTarCountry.Text == "MY")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                                categoryListDist[i].cat_sg,
                                categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3, categoryListDist[i].cat_my);
                        }
                    }
                    else if (TxtTarCountry.Text == "TH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                                categoryListDist[i].cat_sg,
                                categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3, categoryListDist[i].cat_th);
                        }
                    }
                    else if (TxtTarCountry.Text == "TW")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                                categoryListDist[i].cat_sg,
                                categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3, categoryListDist[i].cat_tw);
                        }
                    }
                    else if (TxtTarCountry.Text == "VN")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                                categoryListDist[i].cat_sg,
                                categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3, categoryListDist[i].cat_vn);
                        }
                    }
                }
                else if (TxtSrcCountry.Text == "TH")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(x => x.cat_th == TxtSrcCategoryId.Text.Trim())
                        .OrderBy(x => x.th_l1).ThenBy(x => x.th_l2).ThenBy(x => x.th_l3)
                        .ToList();

                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();

                    if (TxtTarCountry.Text == "PH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                                categoryListDist[i].cat_th,
                                categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3, categoryListDist[i].cat_ph);
                        }
                    }
                    else if (TxtTarCountry.Text == "ID")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                                categoryListDist[i].cat_th,
                                categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3, categoryListDist[i].cat_id);
                        }
                    }
                    else if (TxtTarCountry.Text == "MY")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                                categoryListDist[i].cat_th,
                                categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3, categoryListDist[i].cat_my);
                        }
                    }
                    else if (TxtTarCountry.Text == "SG")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                                categoryListDist[i].cat_th,
                                categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3, categoryListDist[i].cat_sg);
                        }
                    }
                    else if (TxtTarCountry.Text == "TW")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                                categoryListDist[i].cat_th,
                                categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3, categoryListDist[i].cat_tw);
                        }
                    }
                    else if (TxtTarCountry.Text == "VN")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                                categoryListDist[i].cat_th,
                                categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3, categoryListDist[i].cat_vn);
                        }
                    }
                }
                else if (TxtSrcCountry.Text == "TW")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(x => x.cat_tw == TxtSrcCategoryId.Text.Trim())
                        .OrderBy(x => x.tw_l1).ThenBy(x => x.tw_l2).ThenBy(x => x.tw_l3)
                        .ToList();

                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();

                    if (TxtTarCountry.Text == "PH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                                categoryListDist[i].cat_tw,
                                categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3, categoryListDist[i].cat_ph);
                        }
                    }
                    else if (TxtTarCountry.Text == "ID")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                                categoryListDist[i].cat_tw,
                                categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3, categoryListDist[i].cat_id);
                        }
                    }
                    else if (TxtTarCountry.Text == "MY")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                                categoryListDist[i].cat_tw,
                                categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3, categoryListDist[i].cat_my);
                        }
                    }
                    else if (TxtTarCountry.Text == "SG")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                                categoryListDist[i].cat_tw,
                                categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3, categoryListDist[i].cat_sg);
                        }
                    }
                    else if (TxtTarCountry.Text == "TH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                                categoryListDist[i].cat_tw,
                                categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3, categoryListDist[i].cat_th);
                        }
                    }
                    else if (TxtTarCountry.Text == "VN")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }
                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                                categoryListDist[i].cat_tw,
                                categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3, categoryListDist[i].cat_vn);
                        }
                    }
                }
                else if (TxtSrcCountry.Text == "VN")
                {
                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(x => x.cat_vn == TxtSrcCategoryId.Text.Trim())
                        .OrderBy(x => x.vn_l1).ThenBy(x => x.vn_l2).ThenBy(x => x.vn_l3)
                        .ToList();

                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();

                    if (TxtTarCountry.Text == "PH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3,
                                categoryListDist[i].cat_vn,
                                categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3, categoryListDist[i].cat_ph);
                        }
                    }
                    else if (TxtTarCountry.Text == "ID")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3,
                                categoryListDist[i].cat_vn,
                                categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3, categoryListDist[i].cat_id);
                        }
                    }
                    else if (TxtTarCountry.Text == "MY")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3,
                                categoryListDist[i].cat_vn,
                                categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3, categoryListDist[i].cat_my);
                        }
                    }
                    else if (TxtTarCountry.Text == "SG")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3,
                                categoryListDist[i].cat_vn,
                                categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3, categoryListDist[i].cat_sg);
                        }
                    }
                    else if (TxtTarCountry.Text == "TH")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3,
                                categoryListDist[i].cat_vn,
                                categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3, categoryListDist[i].cat_th);
                        }
                    }
                    else if (TxtTarCountry.Text == "TW")
                    {
                        if (categoryList.Count == 0)
                        {
                            categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        else
                        {
                            categoryListDist = categoryList.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();
                        }

                        for (int i = 0; i < categoryListDist.Count; i++)
                        {
                            dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].tw_l2, categoryListDist[i].vn_l3,
                                categoryListDist[i].cat_vn,
                                categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3, categoryListDist[i].cat_tw);
                        }
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgSrcItemList.Rows.Clear();
            if (TxtSearchText.Text.Trim() == string.Empty)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    if (TxtSrcCountry.Text == "ID")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();
                        
                        if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "MY")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.my_l1).ThenBy(x => x.my_l2).ThenBy(x => x.my_l3).ToList();

                        if (categoryList.Count == 0)
                        {
                            categoryList = context.ShopeeCategories.OrderBy(x => x.my_l1).ThenBy(x => x.my_l2).ThenBy(x => x.my_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }
                        if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "PH")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.ph_l1).ThenBy(x => x.ph_l2).ThenBy(x => x.ph_l3).ToList();

                        if (categoryList.Count == 0)
                        {
                            categoryList = context.ShopeeCategories.OrderBy(x => x.ph_l1).ThenBy(x => x.ph_l2).ThenBy(x => x.ph_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }

                        if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "SG")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.sg_l1).ThenBy(x => x.sg_l2).ThenBy(x => x.sg_l3).ToList();

                        if (categoryList.Count == 0)
                        {
                            categoryList = context.ShopeeCategories.OrderBy(x => x.sg_l1).ThenBy(x => x.sg_l2).ThenBy(x => x.sg_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "TH")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.th_l1).ThenBy(x => x.th_l2).ThenBy(x => x.th_l3).ToList();

                        if (categoryList.Count == 0)
                        {
                            categoryList = context.ShopeeCategories.OrderBy(x => x.th_l1).ThenBy(x => x.th_l2).ThenBy(x => x.th_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "TW")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.tw_l1).ThenBy(x => x.tw_l2).ThenBy(x => x.tw_l3).ToList();

                        if (categoryList.Count == 0)
                        {
                            categoryList = context.ShopeeCategories.OrderBy(x => x.tw_l1).ThenBy(x => x.tw_l2).ThenBy(x => x.tw_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "VN")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                            .OrderBy(x => x.vn_l1).ThenBy(x => x.vn_l2).ThenBy(x => x.vn_l3).ToList();

                        if (categoryList.Count == 0)
                        {
                            categoryList = context.ShopeeCategories.OrderBy(x => x.vn_l1).ThenBy(x => x.vn_l2).ThenBy(x => x.vn_l3).ToList();
                            TxtNotice.Text = "원본 카테고리를 찾을 수 없어 모든 카테고리를 로드하였습니다.";
                        }

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].tw_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                    }
                }
            }
            else
            {
                string searchText = TxtSearchText.Text.Trim();

                using (AppDbContext context = new AppDbContext())
                {
                    if (TxtSrcCountry.Text == "ID")
                    {

                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();

                        if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3,
                                    categoryList[i].cat_id,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "MY")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();

                        
                        if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3,
                                    categoryList[i].cat_my,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "PH")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();

                        if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3,
                                    categoryList[i].cat_ph,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "SG")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();


                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3,
                                    categoryList[i].cat_sg,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "TH")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();

                        

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3,
                                    categoryList[i].cat_th,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "TW")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();

                        

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "VN")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3,
                                    categoryList[i].cat_tw,
                                    categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3, categoryList[i].cat_vn);
                            }
                        }
                    }
                    else if (TxtSrcCountry.Text == "VN")
                    {
                        List<ShopeeCategory> categoryList = context.ShopeeCategories
                        .Where(s =>
                        s.cat_id.Contains(searchText) ||
                        s.cat_my.Contains(searchText) ||
                        s.cat_ph.Contains(searchText) ||
                        s.cat_sg.Contains(searchText) ||
                        s.cat_th.Contains(searchText) ||
                        s.cat_tw.Contains(searchText) ||
                        s.cat_vn.Contains(searchText) ||
                        s.id_l1.Contains(searchText) ||
                        s.id_l2.Contains(searchText) ||
                        s.id_l3.Contains(searchText) ||
                        s.my_l1.Contains(searchText) ||
                        s.my_l2.Contains(searchText) ||
                        s.my_l3.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.ph_l1.Contains(searchText) ||
                        s.sg_l1.Contains(searchText) ||
                        s.sg_l2.Contains(searchText) ||
                        s.sg_l3.Contains(searchText) ||
                        s.th_l1.Contains(searchText) ||
                        s.th_l2.Contains(searchText) ||
                        s.th_l3.Contains(searchText) ||
                        s.tw_l1.Contains(searchText) ||
                        s.tw_l2.Contains(searchText) ||
                        s.tw_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText) ||
                        s.vn_l3.Contains(searchText))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3)
                        .ToList();

                        if (TxtTarCountry.Text == "PH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].ph_l1, categoryList[i].ph_l2, categoryList[i].ph_l3, categoryList[i].cat_ph);
                            }
                        }
                        else if (TxtTarCountry.Text == "ID")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].id_l1, categoryList[i].id_l2, categoryList[i].id_l3, categoryList[i].cat_id);
                            }
                        }
                        else if (TxtTarCountry.Text == "MY")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].my_l1, categoryList[i].my_l2, categoryList[i].my_l3, categoryList[i].cat_my);
                            }
                        }
                        else if (TxtTarCountry.Text == "SG")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].sg_l1, categoryList[i].sg_l2, categoryList[i].sg_l3, categoryList[i].cat_sg);
                            }
                        }
                        else if (TxtTarCountry.Text == "TH")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].vn_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].th_l1, categoryList[i].th_l2, categoryList[i].th_l3, categoryList[i].cat_th);
                            }
                        }
                        else if (TxtTarCountry.Text == "TW")
                        {
                            for (int i = 0; i < categoryList.Count; i++)
                            {
                                dgSrcItemList.Rows.Add(i + 1, categoryList[i].vn_l1, categoryList[i].tw_l2, categoryList[i].vn_l3,
                                    categoryList[i].cat_vn,
                                    categoryList[i].tw_l1, categoryList[i].tw_l2, categoryList[i].tw_l3, categoryList[i].cat_tw);
                            }
                        }
                    }
                }
            }
        }

        private void BtnSetCategory_Click(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                CustomCategoryData newCustomCategory = new CustomCategoryData
                {
                    SrcShopeeCountry = TxtSrcCountry.Text,
                    TarShopeeCountry = TxtTarCountry.Text,
                    SrcCategoryId = TxtSrcCategoryId.Text,
                    TarCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Tar_cat3_id"].Value.ToString()
                };
                context.CustomCategoryDatas.Add(newCustomCategory);
                context.SaveChanges();
            }
                
            fp.TempCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Tar_cat3_id"].Value.ToString();
            this.Close();
        }

        private void dgSrcItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnSetCategory_Click(null, null);
        }
    }
}
