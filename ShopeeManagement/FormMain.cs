using Auth0.OidcClient;
using MetroFramework;
using MetroFramework.Forms;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormMain : MetroForm
    {
        public FormMain(string lang)
        {
            InitializeComponent();
        }
        private string Lang = "";        
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            //var frm = Application.OpenForms.Cast<Form>().Where(x => x.Name == "Form1").FirstOrDefault();
            //if (null != frm)
            //{
            //    frm.Show();
            //}
        }
        private void validateAttributeList()
        {
            using (AppDbContext context = new AppDbContext())
            {
                var lst = context.AllAttributeLists.FirstOrDefault();
                if (lst == null)
                {
                    MessageBox.Show("속성 목록을 업로드 하여 주세요", "속성 목록 누락", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            Cursor.Current = Cursors.WaitCursor;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                metroLink1.Text = "V" + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                metroLink1.Text = "V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            global_var.ucloudbiz_account_json = "{\"auth\":{\"identity\":{\"methods\":[\"password\"],\"password\":{\"user\":{\"name\":\"ecremmoce.io@gmail.com\",\"domain\":{\"id\":\"3611a5632d434ba48f7d8982797be77c\"},\"password\":\"MTU2MTYwMDY3NjE1NjE1OTUyMjgxNTgw\"}}},\"scope\":{\"project\":{\"id\":\"8556e13c15264137a6a9e78811845b46\"}}}}";
            global_var.ucloudbiz_account = "AUTH_8556e13c15264137a6a9e78811845b46";
            //metroStyleManager.Theme = this.Theme;
            //metroStyleManager.Style = this.StyleManager.Style;


            //FormProductMapper child_diff = new FormProductMapper(Lang);
            //child_diff.MdiParent = this;
            //child_diff.WindowState = FormWindowState.Maximized;
            //child_diff.StyleManager = this.StyleManager;
            //child_diff.Theme = this.Theme;
            //child_diff.Show();


            //FormUploader child_diff = new FormUploader(Lang);
            //child_diff.MdiParent = this;
            //child_diff.WindowState = FormWindowState.Maximized;
            //child_diff.StyleManager = this.StyleManager;
            //child_diff.Theme = this.Theme;
            //child_diff.Show();

            expire_timer.Enabled = true;
            update_timer.Enabled = true;
            FormProductManage child_diff = new FormProductManage(Lang);
            child_diff.MdiParent = this;
            child_diff.WindowState = FormWindowState.Maximized;
            child_diff.StyleManager = this.StyleManager;
            child_diff.Theme = this.Theme;
            child_diff.Show();

            

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

        
        

        private void Tile_Config_Click(object sender, EventArgs e)
        {
            Tile_current_display.Text = "환경 설정";
            if (Search_frm("FormConfig"))
            {
                FormConfig child_diff = new FormConfig(Lang);
                child_diff.MdiParent = this;
                //child_diff.WindowState = FormWindowState.Maximized;
                //child_diff.StyleManager = this.StyleManager;
                //child_diff.Theme = this.Theme;
                child_diff.Show();
            }
        }

        bool Search_frm(string formname)
        {
            bool rtn = true;
            foreach (Form openForm in MdiChildren)
            {
                if (formname.Equals(openForm.Name))
                {
                    //openForm.Focus();
                    openForm.WindowState = FormWindowState.Maximized;
                    //openForm.Activate();
                    openForm.BringToFront();
                    openForm.FormBorderStyle = FormBorderStyle.None;
                    this.BorderStyle = MetroFormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    rtn = false;
                    break;
                }
            }
            return rtn;
        }

        private void Tile_Manage_Product_Click(object sender, EventArgs e)
        {
            Tile_current_display.Text = "상품 맵핑";
            if (Search_frm("FormProductMapper"))
            {
                FormProductMapper child_diff = new FormProductMapper(Lang);
                child_diff.MdiParent = this;
                child_diff.WindowState = FormWindowState.Maximized;
                child_diff.StyleManager = this.StyleManager;
                child_diff.Theme = this.Theme;
                child_diff.Show();
            }
        }

        private void Tile_mapping_category_Click(object sender, EventArgs e)
        {
            Tile_current_display.Text = "카테고리 맵핑";
            if (Search_frm("FormCategoryMapper"))
            {
                FormCategoryMapper child_diff = new FormCategoryMapper(Lang);
                child_diff.MdiParent = this;
                child_diff.WindowState = FormWindowState.Maximized;
                child_diff.StyleManager = this.StyleManager;
                child_diff.Theme = this.Theme;
                child_diff.Show();
            }
            
        }

        private void Tile_Set_Shipping_Fee_Click(object sender, EventArgs e)
        {
            Tile_current_display.Text = "배송비 설정";
            if (Search_frm("FormShippingSetting"))
            {
                FormShippingSetting child_diff = new FormShippingSetting(Lang);
                child_diff.MdiParent = this;
                child_diff.WindowState = FormWindowState.Maximized;
                child_diff.StyleManager = this.StyleManager;
                child_diff.Theme = this.Theme;
                child_diff.Show();
            }
            
        }

        private void Tile_Manage_Product_Click_1(object sender, EventArgs e)
        {
            if (Search_frm("FormProductManage"))
            {
                Tile_current_display.Text = "상품 정보 관리";
                FormProductManage child_diff = new FormProductManage(Lang);
                child_diff.MdiParent = this;
                child_diff.WindowState = FormWindowState.Maximized;
                child_diff.StyleManager = this.StyleManager;
                child_diff.Theme = this.Theme;
                child_diff.Show();
            }
        }

        private void Tile_Uploader_Click(object sender, EventArgs e)
        {
            Tile_current_display.Text = "복사 등록기";
            if (Search_frm("FormUploader"))
            {
                FormUploader child_diff = new FormUploader(Lang);
                child_diff.MdiParent = this;
                child_diff.WindowState = FormWindowState.Maximized;
                child_diff.StyleManager = this.StyleManager;
                child_diff.Theme = this.Theme;
                child_diff.Show();
            }
        }

        private void Tile_Update_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("Shopee Management Solution의 새로운 버전을 다운로드 할 수 없습니다. \n\n인터넷 연결을 확인후 다시 시도하여 주세요. 오류: " + dde.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Shopee Management Solution의 새 버전을 확인할 수 없습니다.\n\n배포프로그램이 손상되었을 수 있습니다. Shopee Management Solution를 다시 배포하고 시도하여 주시기 바랍니다. 오류: " + ide.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("이 응용 프로그램은 업데이트 할 수 없습니다. 이것은 배포용 응용프로그램이 아닙니다. 오류: " + ioe.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    bool doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        update_timer.Stop();
                        DialogResult dr = MessageBox.Show("Shopee Management Solution의 새로운 버전이 발견되었습니다. 지금 프로그램을 업데이트 하시겠습니까?", "업데이트 가능", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("현재 프로그램에서 필수 업데이트요소를 발견하였습니다. " +
                            "버전 정보 : " + info.MinimumRequiredVersion.ToString() +
                            ". 이제 응용 프로그램이 업데이트를 설치합니다.",
                            "업데이트 가능", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("현재 응용프로그램이 업데이트 되었습니다, 프로그램을 재시작하여 주세요.", "프로그램 재시작", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("응용 프로그램의 최신 버전을 설치 할 수 없습니다. \r\n네트워크 연결을 확인 후 다시 시도하여 주세요. 오류: " + dde, "오류발생", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else
            {
                //MessageBox.Show("새로운 업데이트가 없습니다.","업데이트 없음",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            Cursor = Cursors.Default;
        }

        private void Tile_Shipping_Store_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ecremmoce.io");
        }

        private void Tile_Manual_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ecremmoce.io");
        }

        private void Tile_Style_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager.Style = (MetroColorStyle)next;

            using (AppDbContext context = new AppDbContext())
            {
                DesignStyle result = context.DesignStyles.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result != null)
                {
                    result.styleNo = next;
                    context.SaveChanges();
                }
                else
                {
                    DesignStyle newDesign = new DesignStyle
                    {
                        styleNo = next,
                        UserId = global_var.userId
                    };
                    context.DesignStyles.Add(newDesign);
                    context.SaveChanges();
                }
            }
        }

        private void Tile_New_Product_Click(object sender, EventArgs e)
        {
            Tile_current_display.Text = "신상품 등록";
            if (Search_frm("FromAddProduct"))
            {
                FromAddProduct child_diff = new FromAddProduct();
                child_diff.MdiParent = this;
                child_diff.WindowState = FormWindowState.Maximized;
                child_diff.StyleManager = this.StyleManager;
                child_diff.Theme = this.Theme;
                child_diff.Show();
            }
        }

        private void Tile_exit_Click(object sender, EventArgs e)
        {
            FormClosingEventArgs ev = new FormClosingEventArgs(CloseReason.UserClosing, false);

            FormMain_FormClosing(null, ev);
        }

        private async void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult Result = MetroMessageBox.Show(this, "로그아웃 하시겠습니까?", "로그아웃", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                //로그아웃 처리방법
                //Auth0에서 Application 의 APP 세팅으로 들어간다.
                //Settings의 
                //Allowed Callback URLs : https://shopeesls.auth0.com/mobile
                //Allowed Logout URLs : https://shopeesls.auth0.com/mobile
                Auth0.OidcClient.Auth0Client client = new Auth0.OidcClient.Auth0Client(new Auth0ClientOptions
                {
                    Domain = "gov1-ecremmoce.auth0.com",
                    ClientId = "mDlzQg67jgRqnYMPdjirezyNLzJrA2Fh",
                    LoadProfile = true,
                    EnableTelemetry = true,
                    PostLogoutRedirectUri = "https://gov1-ecremmoce.auth0.com/mobile"
                });


                await client.LogoutAsync();
                var frm = Application.OpenForms.Cast<Form>().Where(x => x.Name == "FormStart").FirstOrDefault();
                if (null != frm)
                {
                    frm.Show();
                    frm.BringToFront();
                    //this.Close();
                    //this.Dispose();
                }
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
