using Auth0.OidcClient;
using IdentityModel.OidcClient;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormStart : MetroForm
    {
        string lang = "";
        public FormStart()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager;
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

        private static void CheckForShortcut()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                if (ad.IsFirstRun)
                {
                    Assembly code = Assembly.GetExecutingAssembly();
                    string company = string.Empty;
                    string description = string.Empty;

                    if (Attribute.IsDefined(code, typeof(AssemblyCompanyAttribute)))
                    {
                        AssemblyCompanyAttribute ascompany = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(code, typeof(AssemblyCompanyAttribute));
                        company = ascompany.Company;
                    }

                    if (Attribute.IsDefined(code, typeof(AssemblyDescriptionAttribute)))
                    {
                        AssemblyDescriptionAttribute asdescription = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(code, typeof(AssemblyDescriptionAttribute));
                        description = asdescription.Description;
                    }

                    if (company != string.Empty && description != string.Empty)
                    {
                        string desktopPath = string.Empty;
                        desktopPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            "\\", description, ".appref-ms");

                        string shortcutName = string.Empty;
                        shortcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                            "\\", company, "\\", description, ".appref-ms");
                        System.IO.File.Copy(shortcutName, desktopPath, true);
                    }
                }
            }
        }
        private void FormStart_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            set_double_buffer();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Label_version.Text = "V" + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                Label_version.Text = "V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            CheckForShortcut();

            using (AppDbContext context = new AppDbContext())
            {
                DesignStyle result = context.DesignStyles.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result != null)
                {
                    metroStyleManager.Style = (MetroColorStyle)result.styleNo;

                    if (result.themeName == "Dark")
                    {
                        metroStyleManager.Theme = MetroThemeStyle.Dark;
                    }
                    else
                    {
                        metroStyleManager.Theme = MetroThemeStyle.Light;
                    }
                }
                else
                {
                    metroStyleManager.Style = MetroColorStyle.Silver;
                    metroStyleManager.Theme = MetroThemeStyle.Light;
                }

                if (!context.Database.Exists())
                {
                    context.Database.CreateIfNotExists();
                }
                else
                {
                    //context.Database.pa
                    //MessageBox.Show("데이터베이스가 이미 존재합니다.","DB 존재", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Database.SetInitializer<AppDbContext>(new DropCreateDatabaseIfModelChanges<AppDbContext>());                      
                context.ShopeeAccounts.Where(x => x.UserId == global_var.userId).ToList();                
                //초기화 하고 멤버를 건드려 주지 않으면 Revision이 올라가지 않는다.
                
            }
            Cursor.Current = Cursors.Default;
        }
        Auth0Client client = new Auth0Client(new Auth0ClientOptions
        {
            Domain = "gov1-ecremmoce.auth0.com",
            ClientId = "mDlzQg67jgRqnYMPdjirezyNLzJrA2Fh",
            LoadProfile = true,
            EnableTelemetry = true,
            //PostLogoutRedirectUri = "https://gov1-ecremmoce.auth0.com/mobile"
            //PostLogoutRedirectUri = "https://gov1-ecremmoce.auth0.com/mobile"
        });
        private async void btn_login_Click(object sender, EventArgs e)
        {
            //인증창 길게 나올때 해결 방법
            //Auth0의 Universal 로그인으로 가서 인증창 html에서 스크립트 버전을 변경해 준다.
            //src="https://cdn.auth0.com/js/lock/11.15/lock.min.js">  ->길게 나옴
            //src="https://cdn.auth0.com/js/lock/11.3/lock.min.js" -> 알맞게 나옴
            Cursor.Current = Cursors.WaitCursor;
            LoginResult loginResult = await client.LoginAsync();

            if (!loginResult.IsError)
            {
                if (loginResult.IdentityToken.ToString() != string.Empty)
                {
                    global_var.auth0_accessToken = loginResult.IdentityToken;
                    global_var.auth0_expire_date = loginResult.AccessTokenExpiration;
                    FormMain fm = new FormMain(lang);
                    fm.StyleManager = this.StyleManager;
                    fm.Theme = this.Theme;
                    var userInfo = loginResult.User.Claims.ToList();
                    fm.txt_loginId.Text = userInfo[1].Value.ToString();
                    global_var.userId = userInfo[1].Value.ToString();
                    this.Hide();
                    fm.Show();
                    
                }
            }
            else
            {
                //MessageBox.Show("인증에 실패하였습니다.", "인증실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Dispose();
                //Application.Exit();
            }

            Cursor.Current = Cursors.Default;

        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btn_login_Click(null, null);
            }
        }

        private void FormStart_Activated(object sender, EventArgs e)
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    FormMain fm = new FormMain(lang);
            //    fm.StyleManager = this.StyleManager;
            //    fm.Theme = this.Theme;
            //    global_var.userId = "mina.ecremmoce@gmail.com";
            //    fm.Show();

            //    this.Hide();
                

            //}
            //else
            //{

            //}
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("chrome.exe", "http://www.ecremmoce.io/ems/");
        }

        private void BtnShippingStore_Click(object sender, EventArgs e)
        {

        }

        private void BtnCheckUpdate_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Shopee Shipping Manager의 새로운 버전을 다운로드 할 수 없습니다. \n\n인터넷 연결을 확인후 다시 시도하여 주세요. 오류: " + dde.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Shopee Shipping Manager의 새 버전을 확인할 수 없습니다.\n\n배포프로그램이 손상되었을 수 있습니다. Shopee Shipping Manager를 다시 배포하고 시도하여 주시기 바랍니다. 오류: " + ide.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        DialogResult dr = MessageBox.Show("Shopee Shipping Manager의 새로운 버전이 발견되었습니다. 지금 프로그램을 업데이트 하시겠습니까?", "업데이트 가능", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

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
    }
}
