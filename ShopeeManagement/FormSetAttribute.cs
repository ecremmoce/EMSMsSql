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
    public partial class FormSetAttribute : MetroForm
    {
        public FormSetAttribute()
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
        private void FormSetAttribute_Load(object sender, EventArgs e)
        {
            set_double_buffer();

            using (AppDbContext context = new AppDbContext())
            {
                long ItemId = Convert.ToInt64(TxtSrcProductId.Text.ToString());
                ItemInfo result = context.ItemInfoes.SingleOrDefault(
                b => b.item_id == ItemId && b.UserId == global_var.userId);

                if (result != null)
                {
                    TxtProductDesc.Text = result.description.ToString();
                }

                List<ItemAttribute> attributeList = context.ItemAttributes
                                .Where(b => b.item_id == ItemId
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.is_mandatory).ToList();
            }
        }
    }
}
