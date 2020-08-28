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

            Auth0ErrorLogin Ids = JsonConvert.DeserializeObject<Auth0ErrorLogin>(Properties.Resources.Auth0ErrorLoginIds);

            foreach (Auth0ErrorLoginIds Id in Ids.Ids)
            {
                if (Id.LoginId.Equals(ID) && Id.LoginPw.Equals(PW))
                {
                    LoginId = ID;
                    DialogResult = DialogResult.OK;

                    break;
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
