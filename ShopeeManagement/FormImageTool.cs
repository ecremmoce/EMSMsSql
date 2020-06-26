using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormImageTool : MetroForm
    {
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // SetIniValue를 위해
            String section, String key, String val, String filePath);
        public FormImageTool()
        {
            InitializeComponent();
        }

        public string ImagePath;
        public Image im;
        int selectX;
        int selectY;
        int selectWidth;
        int selectHeight;
        public Pen selectPen;
        bool start = false;
        Point po1 = new Point();
        Point po2 = new Point();

        private void draw_thumb_position()
        {
            if (pic.Image != null && pic_all.Image != null)
            {
                pic_all.Refresh();
                selectPen = new Pen(Color.Blue, 3);
                selectPen.DashStyle = DashStyle.Solid;
                //배율을 계산한다. 이미지 전체 길이 / 썸네일 전체 길이
                float ratio = ((float)pic.Height / (float)pic_all.Height);
                int y1 = Convert.ToInt32((panel1.VerticalScroll.Value / ratio));
                int y2 = Convert.ToInt32(((panel1.Height + panel1.VerticalScroll.Value) / ratio));
                int height = y2 - y1;
                pic_all.CreateGraphics().DrawRectangle(selectPen, 1, y1 + 1, pic_all.Width - 3, height - 3);
                this.Invalidate();
            }
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
        private void LoadExistImage()
        {
            if (Directory.Exists(ImagePath))
            {
                int i = 1;
                WebClient wc = new WebClient();
                DirectoryInfo di = new DirectoryInfo(ImagePath);
                foreach (var item in di.GetFiles())
                {
                    var content = wc.DownloadData(item.FullName);
                    using (var stream = new MemoryStream(content))
                    {
                        Image im = Image.FromStream(stream);
                        dgcolor_detail.Rows.Add("", i++, false, item.Name, item.FullName);
                    }
                }
            }

        }
        private void FormImageTool_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            cmbEdgeDetection.SelectedIndex = 2;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel1, new object[] { true });
            set_double_buffer();

            pic.Image = im;
            pic_all.Image = im;
            txt_image_width.Text = im.Width.ToString();
            txt_image_height.Text = im.Height.ToString();

            draw_thumb_position();


            //기존에 이미지를 자른 것이 있으면 로드해 준다.
            LoadExistImage();
            Cursor = Cursors.Default;
        }
        public void SetIniValue(String Section, String Key, String Value, String iniPath)
        {
            WritePrivateProfileString(Section, Key, Value, iniPath);
        }
        public String GetIniValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, string.Empty, temp, 255, iniPath);
            return temp.ToString();
        }
        private void set_double_buffer()
        {
            Control[] controls = GetAllControlsUsingRecursive(this);
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i].GetType() == typeof(DataGridView))
                {
                    ((DataGridView)controls[i]).DoubleBuffered(true);
                }
                else if (controls[i].GetType() == typeof(PictureBox))
                {
                    ((PictureBox)controls[i]).DoubleBuffered(true);
                }

            }
        }

        static Control[] GetAllControlsUsingRecursive(Control containerControl)
        {
            List<Control> allControls = new List<Control>();
            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);
            while (queue.Count > 0)
            {
                Control.ControlCollection controls = queue.Dequeue();
                if (controls == null || controls.Count == 0) continue;
                foreach (Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }
            return allControls.ToArray();
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (rd_real.Checked)
            {
                if (cmbEdgeDetection.SelectedIndex == 0)
                {
                    MessageBox.Show("이미지 영상처리 알고리즘을 선택하세요.", "알고리즘 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //validate when user right-click
                if (!start)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        pic.Refresh();
                        //starts coordinates for rectangle                    
                        selectX = e.X;
                        txt_x.Text = e.X.ToString();
                        selectY = e.Y;
                        txt_y.Text = e.Y.ToString();
                        selectPen = new Pen(Color.Blue, 1);
                        selectPen.DashStyle = DashStyle.Solid;

                        pic.CreateGraphics().DrawLine(selectPen, 0, e.Y, pic.Width, e.Y);
                        po1.X = 0;
                        po1.Y = e.Y;
                        start = true;
                        txt_cap_start.BackColor = Color.GreenYellow;
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        pic.Refresh();
                        start = false;
                        txt_cap_start.BackColor = Color.Gainsboro;
                        txt_cap_end.BackColor = Color.Gainsboro;
                    }
                }
                else
                {
                    //validate if there is image
                    if (pic.Image == null)
                        return;
                    //same functionality when mouse is over
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        //pic.Refresh();
                        selectWidth = e.X - selectX;
                        selectHeight = e.Y - selectY;
                        //pic.CreateGraphics().DrawRectangle(selectPen, selectX, selectY, selectWidth, selectHeight);
                        selectPen = new Pen(Color.Red, 1);
                        //pic.CreateGraphics().DrawLine(selectPen, 0, e.Y, pic.Width, e.Y);
                        po2.X = pic.Width;
                        po2.Y = e.Y;
                        txt_cap_end.BackColor = Color.Red;
                        start = false;
                        //function save image to clipboard                        
                        SaveToClipboard_rect();
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        pic.Refresh();
                        start = false;
                        txt_cap_start.BackColor = Color.Gainsboro;
                        txt_cap_end.BackColor = Color.Gainsboro;
                    }
                }
            }
        }

        private void SaveToClipboard_rect()
        {
            Cursor = Cursors.WaitCursor;
            int new_width = 0;
            int new_height = 0;

            int startx = 0;
            int starty = 0;
            if (po1.Y > po2.Y)
            {
                new_width = pic.Width;
                new_height = po1.Y - po2.Y;
                startx = 0;
                starty = po2.Y;
            }
            else
            {
                new_width = pic.Width;
                new_height = po2.Y - po1.Y;
                startx = po1.X;
                starty = po1.Y;
            }
            Rectangle rect = new Rectangle(startx, starty, new_width, new_height);
            //create bitmap with original dimensions
            Bitmap OriginalImage = new Bitmap(pic.Image, pic.Width, pic.Height);
            //create bitmap with selected dimensions

            //1차로 선택한 영역 사이즈로 커팅한다.
            Bitmap _img = new Bitmap(new_width, new_height);
            Graphics g = Graphics.FromImage(_img);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
            //1차 커팅 완료

            //이미지의 경계를 추출하여 좌표를 얻어 온후 또 자른다.            
            Rectangle rect_adj = draw_edge(_img);
            Bitmap img_result = new Bitmap(_img.Width, _img.Height);
            Graphics g_result = Graphics.FromImage(img_result);

            g_result.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g_result.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g_result.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g_result.DrawImage(_img, 0, 0, rect_adj, GraphicsUnit.Pixel);

            if (chk_auto_cut.Checked)
            {
                //정사각형이 아닌경우 정사각형으로 만들어 주는 코드
                if (rect_adj.Width > 0)
                {
                    int base_width = 0;
                    int small_side = 0;
                    int margin = (int)ud_margin.Value;
                    bool is_wide = false;
                    if (rect_adj.Width > rect_adj.Height)
                    {
                        base_width = rect_adj.Width + (margin * 2);
                        small_side = rect_adj.Height;
                        is_wide = true;
                    }
                    else
                    {
                        base_width = rect_adj.Height + (margin * 2);
                        small_side = rect_adj.Width;
                        is_wide = false;
                    }
                    //크기의 차이를 구한다.
                    int diff_size = base_width - small_side;
                    //더해야 할 사이즈를 구한다.
                    int add_size = diff_size / 2;
                    Rectangle new_rect = new Rectangle(0, 0, rect_adj.Width, rect_adj.Height);
                    Bitmap img_adj = new Bitmap(base_width, base_width);

                    //create graphic variable
                    Graphics g_end = Graphics.FromImage(img_adj);
                    //set graphic attributes
                    g_end.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g_end.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g_end.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    SolidBrush Brsh_end = new SolidBrush(Color.White);
                    g_end.FillRectangle(Brsh_end, 0, 0, base_width, base_width);
                    if (is_wide)
                    {
                        g_end.DrawImage(img_result, margin, add_size, new_rect, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g_end.DrawImage(img_result, add_size, margin, new_rect, GraphicsUnit.Pixel);
                    }

                    //insert image stream into clipboard

                    //이미지를 그렸으면 긴축을 기준으로 잡는다.                    
                    txt_cap_start.BackColor = Color.Gainsboro;
                    txt_cap_end.BackColor = Color.Gainsboro;
                    int i_resize = (int)ud_image_size.Value;

                    if (chk_for_big.Checked)
                    {
                        if (img_adj.Width < i_resize)
                        {
                            Bitmap bmp_strech = ResizeImage(img_adj, i_resize, i_resize);
                            //sharpen 적용여부
                            if (chk_sharpen.Checked)
                            {
                                Bitmap bmp_sharpen = Sharpen(bmp_strech, (double)ud_sharpen.Value);
                                pic_cap.Image = bmp_sharpen;
                                Clipboard.SetImage(bmp_sharpen);
                            }
                            else
                            {
                                pic_cap.Image = bmp_strech;
                                //Clipboard.SetImage(bmp_strech);
                            }
                        }
                        else
                        {
                            //sharpen 적용여부
                            if (chk_sharpen.Checked)
                            {
                                Bitmap bmp_sharpen = Sharpen(img_adj, (double)ud_sharpen.Value);
                                pic_cap.Image = bmp_sharpen;
                                Clipboard.SetImage(bmp_sharpen);
                            }
                            else
                            {
                                pic_cap.Image = img_adj;
                                Clipboard.SetImage(img_adj);
                            }
                        }
                    }
                    else
                    {
                        if (chk_sharpen.Checked)
                        {
                            Bitmap bmp_sharpen = Sharpen(img_adj, (double)ud_sharpen.Value);
                            pic_cap.Image = bmp_sharpen;
                            Clipboard.SetImage(bmp_sharpen);
                        }
                        else
                        {
                            pic_cap.Image = img_adj;
                            Clipboard.SetImage(img_adj);
                        }
                    }

                    //이미지 저장 루틴
                    string saveFileName = $"slice_{dgcolor_detail.Rows.Count + 1:000}.jpg";
                    Bitmap saveBmp = (Bitmap)pic_cap.Image;
                    saveBmp.Save(ImagePath + saveFileName);
                    dgcolor_detail.Rows.Add("", dgcolor_detail.Rows.Count + 1,
                        false, saveFileName, ImagePath + saveFileName);
                    pic_color_image.Image = saveBmp;

                    dgcolor_detail.Rows[dgcolor_detail.Rows.Count - 1].Selected = true;


                    //get_color_detail();
                    //if (dgcolor_detail.Rows.Count > 0)
                    //{
                    //    dgcolor_detail.Rows[dgcolor_detail.Rows.Count - 1].Selected = true;
                    //    //DataGridViewCellEventArgs es = new DataGridViewCellEventArgs(3, dgcolor_detail.Rows.Count - 1);
                    //    dgcolor_detail_CellClick(null, null);
                    //}
                    btn_save_image_Click(null, null);
                }
            }
            else
            {
                pic_cap.Image = img_result;
                txt_cap_start.BackColor = Color.Gainsboro;
                txt_cap_end.BackColor = Color.Gainsboro;
                btn_save_image_Click(null, null);
            }

            if (pic_cap.Image != null)
            {
                txt_cap_width.Text = pic_cap.Image.Width.ToString();
                txt_cap_height.Text = pic_cap.Image.Height.ToString();
            }
            Cursor = Cursors.Default;
        }

        public static Bitmap Sharpen(Bitmap image, double strength)
        {
            using (var bitmap = image)
            {
                if (bitmap != null)
                {
                    Bitmap sharpenImage = bitmap.Clone() as Bitmap;

                    int width = image.Width;
                    int height = image.Height;

                    // Create sharpening filter.
                    const int filterSize = 5;

                    var filter = new double[,]
                {
                    {-1, -1, -1, -1, -1},
                    {-1,  2,  2,  2, -1},
                    {-1,  2, 16,  2, -1},
                    {-1,  2,  2,  2, -1},
                    {-1, -1, -1, -1, -1}
                };

                    double bias = 1.0 - strength;
                    double factor = strength / 16.0;

                    const int s = filterSize / 2;

                    var result = new Color[image.Width, image.Height];

                    // Lock image bits for read/write.
                    if (sharpenImage != null)
                    {
                        BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height),
                                                                    ImageLockMode.ReadWrite,
                                                                    PixelFormat.Format24bppRgb);

                        // Declare an array to hold the bytes of the bitmap.
                        int bytes = pbits.Stride * height;
                        var rgbValues = new byte[bytes];

                        // Copy the RGB values into the array.
                        Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

                        int rgb;
                        // Fill the color array with the new sharpened color values.
                        for (int x = s; x < width - s; x++)
                        {
                            for (int y = s; y < height - s; y++)
                            {
                                double red = 0.0, green = 0.0, blue = 0.0;

                                for (int filterX = 0; filterX < filterSize; filterX++)
                                {
                                    for (int filterY = 0; filterY < filterSize; filterY++)
                                    {
                                        int imageX = (x - s + filterX + width) % width;
                                        int imageY = (y - s + filterY + height) % height;

                                        rgb = imageY * pbits.Stride + 3 * imageX;

                                        red += rgbValues[rgb + 2] * filter[filterX, filterY];
                                        green += rgbValues[rgb + 1] * filter[filterX, filterY];
                                        blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                                    }

                                    rgb = y * pbits.Stride + 3 * x;

                                    int r = Math.Min(Math.Max((int)(factor * red + (bias * rgbValues[rgb + 2])), 0), 255);
                                    int g = Math.Min(Math.Max((int)(factor * green + (bias * rgbValues[rgb + 1])), 0), 255);
                                    int b = Math.Min(Math.Max((int)(factor * blue + (bias * rgbValues[rgb + 0])), 0), 255);

                                    result[x, y] = Color.FromArgb(r, g, b);
                                }
                            }
                        }

                        // Update the image with the sharpened pixels.
                        for (int x = s; x < width - s; x++)
                        {
                            for (int y = s; y < height - s; y++)
                            {
                                rgb = y * pbits.Stride + 3 * x;

                                rgbValues[rgb + 2] = result[x, y].R;
                                rgbValues[rgb + 1] = result[x, y].G;
                                rgbValues[rgb + 0] = result[x, y].B;
                            }
                        }

                        // Copy the RGB values back to the bitmap.
                        Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
                        // Release image bits.
                        sharpenImage.UnlockBits(pbits);
                    }

                    return sharpenImage;
                }
            }
            return null;
        }
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private Rectangle draw_edge(Bitmap selectedSource)
        {
            //이미지의 경계선을 그리는 모듈                        
            Bitmap bitmapResult = null;
            Rectangle rect_result = new Rectangle();

            if (selectedSource != null)
            {
                if (cmbEdgeDetection.SelectedItem.ToString() == "None")
                {
                    bitmapResult = selectedSource;
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 3x3")
                {
                    bitmapResult = selectedSource.Laplacian3x3Filter(false);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 3x3 Grayscale")
                {
                    bitmapResult = selectedSource.Laplacian3x3Filter(true);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 5x5")
                {
                    bitmapResult = selectedSource.Laplacian5x5Filter(false);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 5x5 Grayscale")
                {
                    bitmapResult = selectedSource.Laplacian5x5Filter(true);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian of Gaussian")
                {
                    bitmapResult = selectedSource.LaplacianOfGaussianFilter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 3x3 of Gaussian 3x3")
                {
                    bitmapResult = selectedSource.Laplacian3x3OfGaussian3x3Filter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 3x3 of Gaussian 5x5 - 1")
                {
                    bitmapResult = selectedSource.Laplacian3x3OfGaussian5x5Filter1();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 3x3 of Gaussian 5x5 - 2")
                {
                    bitmapResult = selectedSource.Laplacian3x3OfGaussian5x5Filter2();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 5x5 of Gaussian 3x3")
                {
                    bitmapResult = selectedSource.Laplacian5x5OfGaussian3x3Filter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 5x5 of Gaussian 5x5 - 1")
                {
                    bitmapResult = selectedSource.Laplacian5x5OfGaussian5x5Filter1();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Laplacian 5x5 of Gaussian 5x5 - 2")
                {
                    bitmapResult = selectedSource.Laplacian5x5OfGaussian5x5Filter2();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Sobel 3x3")
                {
                    bitmapResult = selectedSource.Sobel3x3Filter(false);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Sobel 3x3 Grayscale")
                {
                    bitmapResult = selectedSource.Sobel3x3Filter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Prewitt")
                {
                    bitmapResult = selectedSource.PrewittFilter(false);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Prewitt Grayscale")
                {
                    bitmapResult = selectedSource.PrewittFilter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Kirsch")
                {
                    bitmapResult = selectedSource.KirschFilter(false);
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Kirsch Grayscale")
                {
                    bitmapResult = selectedSource.KirschFilter();
                }
                if (cmbEdgeDetection.SelectedIndex != 0)
                {
                    //경계선을 검출한 이미지를 보내서 경계선까지 자르고 액기스 좌표를 구한다.
                    rect_result = detect_bound(bitmapResult);
                }

                //pictureBox1.Image = bitmapResult;
            }

            return rect_result;
        }

        private Rectangle detect_bound(Bitmap bmp)
        {
            //상 하 좌 우 4번을 검색한다.            
            Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            int pos_top = 0;
            int pos_bottom = 0;
            int pos_left = 0;
            int pos_right = 0;
            bool found_top = false;
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            int numBytes = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                if (found_top)
                {
                    break;
                }
                else
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        numBytes = (y * (bmp.Width * 4)) + (x * 4);

                        if (rgbValues[numBytes] > 90 && rgbValues[numBytes + 1] > 90 && rgbValues[numBytes + 2] > 90)
                        {
                            //Blue, Green, Red 순서임
                            //rgbValues[numBytes] = 0;
                            //rgbValues[numBytes + 1] = 0;
                            //rgbValues[numBytes + 2] = 0;
                            pos_top = y;
                            found_top = true;
                            break;
                        }
                    }
                }
            }

            bool found_bottom = false;
            for (int y = bmp.Height - 1; y >= 0; y--)
            {
                if (found_bottom)
                {
                    break;
                }
                else
                {
                    for (int x = bmp.Width - 1; x >= 0; x--)
                    {
                        numBytes = (y * (bmp.Width * 4)) + (x * 4);

                        if (rgbValues[numBytes] > 90 && rgbValues[numBytes + 1] > 90 && rgbValues[numBytes + 2] > 90)
                        {
                            //rgbValues[numBytes] = 0;
                            //rgbValues[numBytes + 1] = 0;
                            //rgbValues[numBytes + 2] = 255;
                            pos_bottom = y;
                            found_bottom = true;
                            break;
                        }
                    }
                }
            }

            bool found_left = false;
            for (int y = 0; y < bmp.Width; y++)
            {
                if (found_left)
                {
                    break;
                }
                else
                {
                    for (int x = 0; x < bmp.Height; x++)
                    {
                        numBytes = (x * (bmp.Width * 4)) + (y * 4);

                        if (rgbValues[numBytes] > 90 && rgbValues[numBytes + 1] > 90 && rgbValues[numBytes + 2] > 90)
                        {
                            //rgbValues[numBytes] = 0;
                            //rgbValues[numBytes + 1] = 0;
                            //rgbValues[numBytes + 2] = 255;
                            pos_left = y;
                            found_left = true;
                            break;
                        }
                    }
                }
            }

            bool found_right = false;
            for (int y = bmp.Width - 1; y >= 0; y--)
            {
                if (found_right)
                {
                    break;
                }
                else
                {
                    for (int x = bmp.Height - 1; x >= 0; x--)
                    {
                        numBytes = (x * (bmp.Width * 4)) + (y * 4);

                        if (rgbValues[numBytes] > 90 && rgbValues[numBytes + 1] > 90 && rgbValues[numBytes + 2] > 90)
                        {
                            //rgbValues[numBytes] = 0;
                            //rgbValues[numBytes + 1] = 0;
                            //rgbValues[numBytes + 2] = 255;
                            pos_right = y;
                            found_right = true;
                            break;
                        }
                    }
                }
            }
            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            //// Unlock the bits.
            bmp.UnlockBits(bmpData);

            //create graphic variable
            //Graphics g = Graphics.FromImage(_img);
            //int new_height = pos_bottom - pos_top;
            //int new_width = pos_right - pos_left;
            Rectangle rect = new Rectangle(pos_left, pos_top, pos_right - pos_left, pos_bottom - pos_top);
            return rect;
        }

        private void btn_save_image_Click(object sender, EventArgs e)
        {

        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            if (rd_real.Checked)
            {
                Cursor = Cursors.Cross;
            }
            else
            {
                Cursor = Cursors.No;
            }
        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            pic.Refresh();
            Cursor = Cursors.Default;
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (rd_real.Checked)
            {
                if (pic.Image == null)
                    return;

                if (chk_view_guide_line.Checked)
                {
                    Cursor = Cursors.Cross;
                    selectPen = new Pen(Color.Red, 1);
                    pic.Refresh();
                    pic.CreateGraphics().DrawLine(selectPen, 0, e.Y, pic.Width, e.Y);
                }
                draw_thumb_position();
                //validate if right-click was trigger
                //if (start)
                //{
                //    //refresh picture box
                //    pic.Refresh();
                //    //set corner square to mouse coordinates
                //    selectWidth = e.X - selectX;
                //    txt_width.Text = selectWidth.ToString();
                //    selectHeight = e.Y - selectY;
                //    txt_height.Text = selectHeight.ToString();
                //    //draw dotted rectangle
                //    pic.CreateGraphics().DrawRectangle(selectPen,
                //              selectX, selectY, selectWidth, selectHeight);
                //}
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgcolor_detail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            WebClient wc = new WebClient();
            string destPath = dgcolor_detail.SelectedRows[0].Cells["dgcolor_detail_path"].Value.ToString();
            var content = wc.DownloadData(destPath);
            using (var stream = new MemoryStream(content))
            {
                Image im = Image.FromStream(stream);
                pic_color_image.Image = im;
            }

            if (e.RowIndex == -1)
            {
                if (e.ColumnIndex == 2)
                {
                    bool val = !(bool)dgcolor_detail.Rows[0].Cells["dgcolor_detail_check"].Value;
                    for (int i = 0; i < dgcolor_detail.Rows.Count; i++)
                    {
                        dgcolor_detail.Rows[i].Cells["dgcolor_detail_check"].Value = val;
                    }
                    groupBox5.Select();
                }
            }
        }

        private void btn_single_up_Click(object sender, EventArgs e)
        {
            if (dgcolor_detail.SelectedRows.Count > 0)
            {
                change_order(dgcolor_detail, dgcolor_detail.SelectedRows[0].Index, true);
            }
        }

        private void change_order(DataGridView dg, int selected_idx, bool is_up)
        {
            if (is_up)
            {
                //올려야 할놈이 제일 첫번째 인지 검사.
                if (selected_idx > 0)
                {
                    int pre_idx = selected_idx - 1;
                    string pre_db_idx = dg.Rows[pre_idx].Cells[0].Value.ToString();
                    string pre_image_name = dg.Rows[pre_idx].Cells[3].Value.ToString();
                    string pre_image_path = dg.Rows[pre_idx].Cells[4].Value.ToString();

                    string cur_db_idx = dg.Rows[selected_idx].Cells[0].Value.ToString();
                    string cur_image_name = dg.Rows[selected_idx].Cells[3].Value.ToString();
                    string cur_image_path = dg.Rows[selected_idx].Cells[4].Value.ToString();

                    //바꾼다.
                    dg.Rows[pre_idx].Cells[0].Value = cur_db_idx;
                    dg.Rows[pre_idx].Cells[3].Value = cur_image_name;
                    dg.Rows[pre_idx].Cells[4].Value = cur_image_path;

                    dg.Rows[selected_idx].Cells[0].Value = pre_db_idx;
                    dg.Rows[selected_idx].Cells[3].Value = pre_image_name;
                    dg.Rows[selected_idx].Cells[4].Value = pre_image_path;

                    //위에놈을 선택하여준다.
                    dg.Rows[pre_idx].Selected = true;
                    dg.FirstDisplayedScrollingRowIndex = pre_idx;                    
                }
            }
            else
            {
                if (selected_idx < dg.Rows.Count - 1)
                {
                    int next_idx = selected_idx + 1;
                    string next_db_idx = dg.Rows[next_idx].Cells[0].Value.ToString();
                    string next_image_name = dg.Rows[next_idx].Cells[3].Value.ToString();
                    string next_image_path = dg.Rows[next_idx].Cells[4].Value.ToString();

                    string cur_db_idx = dg.Rows[selected_idx].Cells[0].Value.ToString();
                    string cur_image_name = dg.Rows[selected_idx].Cells[3].Value.ToString();
                    string cur_image_path = dg.Rows[selected_idx].Cells[4].Value.ToString();

                    //바꾼다.
                    dg.Rows[next_idx].Cells[0].Value = cur_db_idx;
                    dg.Rows[next_idx].Cells[3].Value = cur_image_name;
                    dg.Rows[next_idx].Cells[4].Value = cur_image_path;

                    dg.Rows[selected_idx].Cells[0].Value = next_db_idx;
                    dg.Rows[selected_idx].Cells[3].Value = next_image_name;
                    dg.Rows[selected_idx].Cells[4].Value = next_image_path;

                    //아래놈을 선택하여준다.
                    dg.Rows[next_idx].Selected = true;
                    dg.FirstDisplayedScrollingRowIndex = next_idx;
                }
            }
        }

        private void btn_image_delete_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("체크한 파일을 삭제 하시겠습니까?", "파일 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgcolor_detail.Rows.Count; i++)
                {
                    if (dgcolor_detail.Rows[i].Cells["dgcolor_detail_check"].Value.ToString() == "True")
                    {
                        string file_name = dgcolor_detail.Rows[i].Cells["dgcolor_detail_name"].Value.ToString().Trim();
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 파일을 모두 삭제하였습니다.", "체크파일 삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_single_dn_Click(object sender, EventArgs e)
        {
            if (dgcolor_detail.SelectedRows.Count > 0)
            {
                change_order(dgcolor_detail, dgcolor_detail.SelectedRows[0].Index, false);
            }
        }

        private void btn_color_detail_top_Click(object sender, EventArgs e)
        {
            if (dgcolor_detail.SelectedRows.Count > 0)
            {
                change_top_bottom(dgcolor_detail, dgcolor_detail.SelectedRows[0].Index, true);
                DataGridViewCellEventArgs es = new DataGridViewCellEventArgs(1, 0);
                dgcolor_detail_CellClick(null, es);
            }
        }

        private void change_top_bottom(DataGridView dg, int selected_idx, bool is_top)
        {
            if (is_top)
            {
                //올려야 할놈이 제일 첫번째 인지 검사.
                if (selected_idx > 0)
                {
                    //현재 데이터를 복사한다.
                    DataGridViewRow clone_row = (DataGridViewRow)dg.Rows[selected_idx].Clone();

                    int intColIndex = 0;
                    foreach (DataGridViewCell cell in dg.SelectedRows[0].Cells)
                    {
                        clone_row.Cells[intColIndex].Value = cell.Value;
                        intColIndex++;
                    }
                    string cur_db_idx = dg.Rows[selected_idx].Cells[0].Value.ToString();
                    string cur_image_name = dg.Rows[selected_idx].Cells[3].Value.ToString();
                    string cur_image_path = dg.Rows[selected_idx].Cells[4].Value.ToString();

                    //현재를 지운다.
                    dg.Rows.RemoveAt(selected_idx);

                    //제일위에 삽입한다.
                    dg.Rows.Insert(0, clone_row);
                    dg.Rows[0].Selected = true;
                }
            }
        }
    }
}
