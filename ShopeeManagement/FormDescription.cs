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
    public partial class FormDescription : MetroForm
    {
        public FormDescription()
        {
            InitializeComponent();
        }
        public long ItemId = 0;
        public int maxLen;
        public int curLen;
        private void FormDescription_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            lblCurrentLen.Text = "현재 : " + TxtProductDesc.Text.Length.ToString() + "자" ;

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
        public FormProductManage fp;
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
        private void BtnSetCategory_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("상품 설명을 저장 하시겠습니까?", "상품 설명 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    
                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                    b => b.item_id == ItemId && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.description = TxtProductDesc.Text;
                        result.isChanged = true;
                        context.SaveChanges();
                        fp.isChanged = true;
                        MessageBox.Show("저장하였습니다.", "저장완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LblCurrentLen_Click(object sender, EventArgs e)
        {

        }

        private void TxtProductDesc_TextChanged(object sender, EventArgs e)
        {
            lblCurrentLen.Text = "현재 : " + string.Format("{0:n0}", TxtProductDesc.Text.Length) + "자";

            if(TxtProductDesc.Text.Length > maxLen)
            {
                MessageBox.Show("최대 글자수를 초과하였습니다.","최대 글자수 초과",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
