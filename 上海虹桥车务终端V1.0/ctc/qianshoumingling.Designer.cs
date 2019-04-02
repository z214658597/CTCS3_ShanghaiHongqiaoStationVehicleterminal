namespace ctc
{
    partial class qianshoumingling
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
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.命令序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.命令编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.受令单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.签收命令 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.调度命令签收 = new System.Windows.Forms.Button();
            this.命令内容 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.命令序号,
            this.命令编号,
            this.受令单位,
            this.签收命令});
            this.dataGridView2.Location = new System.Drawing.Point(10, 20);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(445, 150);
            this.dataGridView2.TabIndex = 8;
            // 
            // 命令序号
            // 
            this.命令序号.HeaderText = "命令序号";
            this.命令序号.Name = "命令序号";
            // 
            // 命令编号
            // 
            this.命令编号.HeaderText = "命令编号";
            this.命令编号.Name = "命令编号";
            // 
            // 受令单位
            // 
            this.受令单位.HeaderText = "受令单位";
            this.受令单位.Name = "受令单位";
            // 
            // 签收命令
            // 
            this.签收命令.HeaderText = "签收命令";
            this.签收命令.Name = "签收命令";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 200);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(300, 150);
            this.textBox1.TabIndex = 10;
            // 
            // 调度命令签收
            // 
            this.调度命令签收.Font = new System.Drawing.Font("宋体", 11F);
            this.调度命令签收.Location = new System.Drawing.Point(333, 250);
            this.调度命令签收.Name = "调度命令签收";
            this.调度命令签收.Size = new System.Drawing.Size(106, 30);
            this.调度命令签收.TabIndex = 11;
            this.调度命令签收.Text = "调度命令签收";
            this.调度命令签收.UseVisualStyleBackColor = true;
            this.调度命令签收.Click += new System.EventHandler(this.调度命令回执_Click);
            // 
            // 命令内容
            // 
            this.命令内容.AutoSize = true;
            this.命令内容.Font = new System.Drawing.Font("宋体", 11F);
            this.命令内容.Location = new System.Drawing.Point(10, 180);
            this.命令内容.Name = "命令内容";
            this.命令内容.Size = new System.Drawing.Size(67, 15);
            this.命令内容.TabIndex = 12;
            this.命令内容.Text = "命令内容";
            // 
            // qianshoumingling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.命令内容);
            this.Controls.Add(this.调度命令签收);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView2);
            this.Name = "qianshoumingling";
            this.Text = "签收命令";
            this.Load += new System.EventHandler(this.qianshoumingling_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 命令序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 命令编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 受令单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 签收命令;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button 调度命令签收;
        private System.Windows.Forms.Label 命令内容;

    }
}