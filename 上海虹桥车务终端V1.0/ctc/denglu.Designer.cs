namespace ctc
{
    partial class denglu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(denglu));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_number = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.button_登录 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(30, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户工号：";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 11F);
            this.label2.Location = new System.Drawing.Point(30, 350);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户密码：";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 11F);
            this.label3.Location = new System.Drawing.Point(30, 295);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户姓名：";
            // 
            // textBox_number
            // 
            this.textBox_number.Font = new System.Drawing.Font("宋体", 11F);
            this.textBox_number.Location = new System.Drawing.Point(130, 245);
            this.textBox_number.Multiline = true;
            this.textBox_number.Name = "textBox_number";
            this.textBox_number.Size = new System.Drawing.Size(120, 30);
            this.textBox_number.TabIndex = 3;
            // 
            // textBox_name
            // 
            this.textBox_name.Font = new System.Drawing.Font("宋体", 11F);
            this.textBox_name.Location = new System.Drawing.Point(130, 295);
            this.textBox_name.Multiline = true;
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(120, 30);
            this.textBox_name.TabIndex = 4;
            // 
            // textBox_password
            // 
            this.textBox_password.Font = new System.Drawing.Font("宋体", 11F);
            this.textBox_password.Location = new System.Drawing.Point(130, 350);
            this.textBox_password.Multiline = true;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(120, 30);
            this.textBox_password.TabIndex = 5;
            // 
            // button_登录
            // 
            this.button_登录.Font = new System.Drawing.Font("宋体", 11F);
            this.button_登录.Location = new System.Drawing.Point(100, 390);
            this.button_登录.Name = "button_登录";
            this.button_登录.Size = new System.Drawing.Size(80, 30);
            this.button_登录.TabIndex = 6;
            this.button_登录.Text = "登录";
            this.button_登录.UseVisualStyleBackColor = true;
            this.button_登录.Click += new System.EventHandler(this.button_登录_Click);
            // 
            // denglu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(304, 442);
            this.Controls.Add(this.button_登录);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.textBox_number);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "denglu";
            this.Text = "登录界面";
            this.Load += new System.EventHandler(this.denglu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_number;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Button button_登录;
    }
}