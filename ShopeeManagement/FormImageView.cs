using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormImageView : MetroForm
    {
        public FormImageView()
        {
            InitializeComponent();
        }
        public Image im;
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
        private void FormImageView_Load(object sender, EventArgs e)
        {
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            pic.Image = im;
            txt_image_width.Text = string.Format("{0:n0}", im.Width);
            txt_image_height.Text = string.Format("{0:n0}", im.Height);
            //WebClient wc = new WebClient();
            //var content = wc.DownloadData(detail_image_path);
            //using (var stream = new MemoryStream(content))
            //{
            //    Image im = Image.FromStream(stream);
            //    Application.DoEvents();
            //    pic.Image = im;
            //    txt_image_width.Text = im.Width.ToString();
            //    txt_image_height.Text = im.Height.ToString();
            //}
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
