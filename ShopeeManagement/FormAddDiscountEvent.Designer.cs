namespace ShopeeManagement
{
    partial class FormAddDiscountEvent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dt_end_date = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dt_start_date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_discount_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_create_discount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dt_end_date
            // 
            this.dt_end_date.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dt_end_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_end_date.Location = new System.Drawing.Point(86, 99);
            this.dt_end_date.Name = "dt_end_date";
            this.dt_end_date.Size = new System.Drawing.Size(147, 21);
            this.dt_end_date.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "종료일자";
            // 
            // dt_start_date
            // 
            this.dt_start_date.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dt_start_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_start_date.Location = new System.Drawing.Point(86, 72);
            this.dt_start_date.Name = "dt_start_date";
            this.dt_start_date.Size = new System.Drawing.Size(147, 21);
            this.dt_start_date.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "시작일자";
            // 
            // txt_discount_name
            // 
            this.txt_discount_name.Location = new System.Drawing.Point(86, 36);
            this.txt_discount_name.Name = "txt_discount_name";
            this.txt_discount_name.Size = new System.Drawing.Size(236, 21);
            this.txt_discount_name.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "할인명";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(345, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "이벤트가 생성되고 활성화 되기까지 몇분의 시간이 소요됩니다.";
            // 
            // btn_create_discount
            // 
            this.btn_create_discount.BackColor = System.Drawing.Color.Linen;
            this.btn_create_discount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_create_discount.Image = global::ShopeeManagement.Properties.Resources.add_icon;
            this.btn_create_discount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_create_discount.Location = new System.Drawing.Point(219, 134);
            this.btn_create_discount.Name = "btn_create_discount";
            this.btn_create_discount.Size = new System.Drawing.Size(103, 29);
            this.btn_create_discount.TabIndex = 21;
            this.btn_create_discount.Text = "할인 생성";
            this.btn_create_discount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_create_discount.UseVisualStyleBackColor = false;
            this.btn_create_discount.Click += new System.EventHandler(this.btn_create_discount_Click);
            // 
            // FormAddDiscountEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 215);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_create_discount);
            this.Controls.Add(this.dt_end_date);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dt_start_date);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_discount_name);
            this.Controls.Add(this.label1);
            this.Name = "FormAddDiscountEvent";
            this.Resizable = false;
            this.Load += new System.EventHandler(this.FormAddDiscountEvent_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_create_discount;
        public System.Windows.Forms.DateTimePicker dt_end_date;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.DateTimePicker dt_start_date;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txt_discount_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}