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
    public partial class FormSubLogin : Form
    {
        public string LoginId { get; set; }

        public FormSubLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string ID = TxtLoginID.Text;
            string PW = TxtLoginPW.Text;

            using (AppDbContext context = new AppDbContext())
            {
                Auth0ErrorLogin data = context.Auth0ErrorLogin.Where(Id => Id.LoginId.Equals(ID)).FirstOrDefault();

                if (data != null)
                {
                    if (data.LoginPW.Equals(PW))
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        DialogResult = DialogResult.Abort;
                    }
                }
                else
                {
                    DialogResult = DialogResult.Abort;
                }
            }

            Close();
        }

        private void TxtLoginPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnLogin_Click(null, null);
            }
        }
    }
}
