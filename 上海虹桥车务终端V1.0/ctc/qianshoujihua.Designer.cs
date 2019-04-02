namespace ctc
{
    partial class qianshoujihua
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
            this.阶段计划签收 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.车次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.接收方 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.下达时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.签收计划 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // 阶段计划签收
            // 
            this.阶段计划签收.Font = new System.Drawing.Font("宋体", 11F);
            this.阶段计划签收.Location = new System.Drawing.Point(220, 200);
            this.阶段计划签收.Name = "阶段计划签收";
            this.阶段计划签收.Size = new System.Drawing.Size(106, 30);
            this.阶段计划签收.TabIndex = 1;
            this.阶段计划签收.Text = "阶段计划签收";
            this.阶段计划签收.UseVisualStyleBackColor = true;
            this.阶段计划签收.Click += new System.EventHandler(this.阶段计划回执_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.车次号,
            this.接收方,
            this.下达时间,
            this.签收计划});
            this.dataGridView1.Location = new System.Drawing.Point(10, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(540, 150);
            this.dataGridView1.TabIndex = 4;
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            // 
            // 车次号
            // 
            this.车次号.HeaderText = "车次号";
            this.车次号.Name = "车次号";
            // 
            // 接收方
            // 
            this.接收方.HeaderText = "接收方";
            this.接收方.Name = "接收方";
            // 
            // 下达时间
            // 
            this.下达时间.HeaderText = "下达时间";
            this.下达时间.Name = "下达时间";
            // 
            // 签收计划
            // 
            this.签收计划.HeaderText = "签收计划";
            this.签收计划.Name = "签收计划";
            // 
            // qianshoujihua
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 262);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.阶段计划签收);
            this.Name = "qianshoujihua";
            this.Text = "签收计划";
            this.Load += new System.EventHandler(this.qianshoujihua_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button 阶段计划签收;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 车次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 接收方;
        private System.Windows.Forms.DataGridViewTextBoxColumn 下达时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 签收计划;

    }
}