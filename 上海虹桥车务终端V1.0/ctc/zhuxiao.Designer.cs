namespace ctc
{
    partial class zhuxiao
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
            this.button_注销 = new System.Windows.Forms.Button();
            this.label_number = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_username = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_time = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_注销
            // 
            this.button_注销.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_注销.Location = new System.Drawing.Point(95, 190);
            this.button_注销.Name = "button_注销";
            this.button_注销.Size = new System.Drawing.Size(80, 30);
            this.button_注销.TabIndex = 0;
            this.button_注销.Text = "注销";
            this.button_注销.UseVisualStyleBackColor = true;
            this.button_注销.Click += new System.EventHandler(this.button_注销_Click);
            // 
            // label_number
            // 
            this.label_number.AutoSize = true;
            this.label_number.Font = new System.Drawing.Font("宋体", 11F);
            this.label_number.Location = new System.Drawing.Point(140, 30);
            this.label_number.Name = "label_number";
            this.label_number.Size = new System.Drawing.Size(47, 15);
            this.label_number.TabIndex = 2;
            this.label_number.Text = "label";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "用户工号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11F);
            this.label2.Location = new System.Drawing.Point(30, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "用户姓名：";
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Font = new System.Drawing.Font("宋体", 11F);
            this.label_username.Location = new System.Drawing.Point(140, 85);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(47, 15);
            this.label_username.TabIndex = 5;
            this.label_username.Text = "label";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11F);
            this.label4.Location = new System.Drawing.Point(30, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "登录时间：";
            // 
            // label_time
            // 
            this.label_time.AutoSize = true;
            this.label_time.Font = new System.Drawing.Font("宋体", 11F);
            this.label_time.Location = new System.Drawing.Point(140, 140);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(47, 15);
            this.label_time.TabIndex = 7;
            this.label_time.Text = "label";
            // 
            // zhuxiao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_number);
            this.Controls.Add(this.button_注销);
            this.Name = "zhuxiao";
            this.Text = "注销界面";
            this.Load += new System.EventHandler(this.Loaded_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_注销;
        private System.Windows.Forms.Label label_number;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_time;
    }
}