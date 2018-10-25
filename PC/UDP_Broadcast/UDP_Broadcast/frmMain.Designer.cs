namespace UDP_Broadcast
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList_16x16 = new System.Windows.Forms.ImageList(this.components);
            this.lb_path = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_state = new System.Windows.Forms.Label();
            this.lb_state_value = new System.Windows.Forms.Label();
            this.lb_info = new System.Windows.Forms.Label();
            this.lb_info_value = new System.Windows.Forms.Label();
            this.imageList_64x64 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.lb_path);
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Location = new System.Drawing.Point(12, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 502);
            this.panel1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(190, 29);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(565, 468);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList_16x16
            // 
            this.imageList_16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_16x16.ImageStream")));
            this.imageList_16x16.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_16x16.Images.SetKeyName(0, "disk");
            this.imageList_16x16.Images.SetKeyName(1, "folder");
            this.imageList_16x16.Images.SetKeyName(2, "folder_open");
            // 
            // lb_path
            // 
            this.lb_path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_path.Location = new System.Drawing.Point(190, 3);
            this.lb_path.Name = "lb_path";
            this.lb_path.Size = new System.Drawing.Size(565, 23);
            this.lb_path.TabIndex = 1;
            this.lb_path.Text = "label1";
            this.lb_path.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList_16x16;
            this.treeView1.Location = new System.Drawing.Point(4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(180, 494);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lb_state, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_state_value, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_info, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_info_value, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(550, 30);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lb_state
            // 
            this.lb_state.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_state.Location = new System.Drawing.Point(3, 0);
            this.lb_state.Name = "lb_state";
            this.lb_state.Size = new System.Drawing.Size(94, 30);
            this.lb_state.TabIndex = 0;
            this.lb_state.Text = "연결상태 :";
            this.lb_state.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_state_value
            // 
            this.lb_state_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_state_value.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_state_value.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lb_state_value.Location = new System.Drawing.Point(103, 0);
            this.lb_state_value.Name = "lb_state_value";
            this.lb_state_value.Size = new System.Drawing.Size(144, 30);
            this.lb_state_value.TabIndex = 1;
            this.lb_state_value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_info
            // 
            this.lb_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_info.Location = new System.Drawing.Point(273, 0);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(94, 30);
            this.lb_info.TabIndex = 2;
            this.lb_info.Text = "단말기 정보 :";
            this.lb_info.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_info_value
            // 
            this.lb_info_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_info_value.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_info_value.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lb_info_value.Location = new System.Drawing.Point(373, 0);
            this.lb_info_value.Name = "lb_info_value";
            this.lb_info_value.Size = new System.Drawing.Size(174, 30);
            this.lb_info_value.TabIndex = 3;
            this.lb_info_value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageList_64x64
            // 
            this.imageList_64x64.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_64x64.ImageStream")));
            this.imageList_64x64.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_64x64.Images.SetKeyName(0, "folder");
            this.imageList_64x64.Images.SetKeyName(1, "file");
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lb_state;
        private System.Windows.Forms.Label lb_state_value;
        private System.Windows.Forms.Label lb_info;
        private System.Windows.Forms.Label lb_info_value;
        private System.Windows.Forms.Label lb_path;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList_16x16;
        private System.Windows.Forms.ImageList imageList_64x64;
    }
}

