namespace ctc
{
    partial class qingqiumingling
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.命令类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.命令编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.请求结果 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.取消 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.发送请求 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.命令类型,
            this.命令编号,
            this.请求结果});
            this.dataGridView1.Location = new System.Drawing.Point(81, 206);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(445, 150);
            this.dataGridView1.TabIndex = 26;
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            // 
            // 命令类型
            // 
            this.命令类型.HeaderText = "命令类型";
            this.命令类型.Name = "命令类型";
            // 
            // 命令编号
            // 
            this.命令编号.HeaderText = "命令编号";
            this.命令编号.Name = "命令编号";
            // 
            // 请求结果
            // 
            this.请求结果.HeaderText = "请求结果";
            this.请求结果.Name = "请求结果";
            // 
            // 取消
            // 
            this.取消.Font = new System.Drawing.Font("宋体", 11F);
            this.取消.Location = new System.Drawing.Point(387, 386);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(80, 30);
            this.取消.TabIndex = 29;
            this.取消.Text = "取消";
            this.取消.UseVisualStyleBackColor = true;
            this.取消.Click += new System.EventHandler(this.取消_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11F);
            this.label3.Location = new System.Drawing.Point(47, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "命令正文";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(121, 76);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(350, 100);
            this.textBox2.TabIndex = 24;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(271, 26);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 30);
            this.textBox1.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11F);
            this.label2.Location = new System.Drawing.Point(197, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "命令名称";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13"});
            this.comboBox1.Location = new System.Drawing.Point(121, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(60, 20);
            this.comboBox1.TabIndex = 21;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(47, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "命令类型";
            // 
            // 发送请求
            // 
            this.发送请求.Font = new System.Drawing.Font("宋体", 11F);
            this.发送请求.Location = new System.Drawing.Point(127, 386);
            this.发送请求.Name = "发送请求";
            this.发送请求.Size = new System.Drawing.Size(80, 30);
            this.发送请求.TabIndex = 28;
            this.发送请求.Text = "发送请求";
            this.发送请求.UseVisualStyleBackColor = true;
            this.发送请求.Click += new System.EventHandler(this.发送请求_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11F);
            this.label4.Location = new System.Drawing.Point(8, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "请求状态";
            // 
            // qingqiumingling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 442);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.发送请求);
            this.Controls.Add(this.label4);
            this.Name = "qingqiumingling";
            this.Text = "请求命令";
            this.Load += new System.EventHandler(this.qingqiumingling_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 命令类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 命令编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 请求结果;
        private System.Windows.Forms.Button 取消;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 发送请求;
        private System.Windows.Forms.Label label4;

    }
}