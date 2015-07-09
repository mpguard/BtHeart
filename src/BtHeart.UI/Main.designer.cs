namespace BtHeart.UI
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.gbSet = new System.Windows.Forms.GroupBox();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStopBit = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDataBit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBaundrate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tp蓝牙原始数据 = new System.Windows.Forms.TabPage();
            this.gbReceive = new System.Windows.Forms.GroupBox();
            this.gbSend = new System.Windows.Forms.GroupBox();
            this.cmbOrder = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvFineTrajectory = new System.Windows.Forms.DataGridView();
            this.gbSet3 = new System.Windows.Forms.GroupBox();
            this.cmbEncoding = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkHEXShow = new System.Windows.Forms.CheckBox();
            this.chkHEXSend = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSendReceive = new System.Windows.Forms.Button();
            this.btnClearSend = new System.Windows.Forms.Button();
            this.btnRefreshPort = new System.Windows.Forms.Button();
            this.tpBLE数据 = new System.Windows.Forms.TabPage();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn原始数据解析 = new System.Windows.Forms.Button();
            this.btn滤波解析 = new System.Windows.Forms.Button();
            this.txtBleText = new System.Windows.Forms.RichTextBox();
            this.btnBle解析 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tb心电图 = new System.Windows.Forms.TabPage();
            this.ChartHeart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chkFilter = new System.Windows.Forms.CheckBox();
            this.btn滤波保存 = new System.Windows.Forms.Button();
            this.gbSet.SuspendLayout();
            this.tp蓝牙原始数据.SuspendLayout();
            this.gbReceive.SuspendLayout();
            this.gbSend.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFineTrajectory)).BeginInit();
            this.gbSet3.SuspendLayout();
            this.tpBLE数据.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tb心电图.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartHeart)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSet
            // 
            this.gbSet.Controls.Add(this.cmbParity);
            this.gbSet.Controls.Add(this.label5);
            this.gbSet.Controls.Add(this.cmbStopBit);
            this.gbSet.Controls.Add(this.label4);
            this.gbSet.Controls.Add(this.cmbDataBit);
            this.gbSet.Controls.Add(this.label3);
            this.gbSet.Controls.Add(this.cmbBaundrate);
            this.gbSet.Controls.Add(this.label2);
            this.gbSet.Controls.Add(this.cmbPort);
            this.gbSet.Controls.Add(this.label1);
            this.gbSet.Location = new System.Drawing.Point(12, 12);
            this.gbSet.Name = "gbSet";
            this.gbSet.Size = new System.Drawing.Size(157, 156);
            this.gbSet.TabIndex = 1;
            this.gbSet.TabStop = false;
            this.gbSet.Text = "串口配置";
            // 
            // cmbParity
            // 
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Location = new System.Drawing.Point(71, 127);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(80, 20);
            this.cmbParity.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "校验：";
            // 
            // cmbStopBit
            // 
            this.cmbStopBit.FormattingEnabled = true;
            this.cmbStopBit.Location = new System.Drawing.Point(71, 101);
            this.cmbStopBit.Name = "cmbStopBit";
            this.cmbStopBit.Size = new System.Drawing.Size(80, 20);
            this.cmbStopBit.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "停止位：";
            // 
            // cmbDataBit
            // 
            this.cmbDataBit.FormattingEnabled = true;
            this.cmbDataBit.Location = new System.Drawing.Point(71, 75);
            this.cmbDataBit.Name = "cmbDataBit";
            this.cmbDataBit.Size = new System.Drawing.Size(80, 20);
            this.cmbDataBit.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "数据位：";
            // 
            // cmbBaundrate
            // 
            this.cmbBaundrate.FormattingEnabled = true;
            this.cmbBaundrate.Location = new System.Drawing.Point(71, 49);
            this.cmbBaundrate.Name = "cmbBaundrate";
            this.cmbBaundrate.Size = new System.Drawing.Size(80, 20);
            this.cmbBaundrate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "波特率：";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(71, 23);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(80, 20);
            this.cmbPort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口：";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConnect.Location = new System.Drawing.Point(12, 268);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(157, 29);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "打开串口";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tp蓝牙原始数据
            // 
            this.tp蓝牙原始数据.Controls.Add(this.gbReceive);
            this.tp蓝牙原始数据.Location = new System.Drawing.Point(4, 22);
            this.tp蓝牙原始数据.Name = "tp蓝牙原始数据";
            this.tp蓝牙原始数据.Padding = new System.Windows.Forms.Padding(3);
            this.tp蓝牙原始数据.Size = new System.Drawing.Size(825, 639);
            this.tp蓝牙原始数据.TabIndex = 0;
            this.tp蓝牙原始数据.Text = "蓝牙原始数据";
            this.tp蓝牙原始数据.UseVisualStyleBackColor = true;
            // 
            // gbReceive
            // 
            this.gbReceive.Controls.Add(this.gbSend);
            this.gbReceive.Controls.Add(this.txtReceive);
            this.gbReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReceive.Location = new System.Drawing.Point(3, 3);
            this.gbReceive.Name = "gbReceive";
            this.gbReceive.Size = new System.Drawing.Size(819, 633);
            this.gbReceive.TabIndex = 5;
            this.gbReceive.TabStop = false;
            this.gbReceive.Text = "接收区：已接收0字节";
            // 
            // gbSend
            // 
            this.gbSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSend.Controls.Add(this.cmbOrder);
            this.gbSend.Controls.Add(this.label6);
            this.gbSend.Controls.Add(this.btnSend);
            this.gbSend.Controls.Add(this.txtSend);
            this.gbSend.Location = new System.Drawing.Point(3, 487);
            this.gbSend.Name = "gbSend";
            this.gbSend.Size = new System.Drawing.Size(810, 140);
            this.gbSend.TabIndex = 7;
            this.gbSend.TabStop = false;
            this.gbSend.Text = "发送区：已发送0字节";
            // 
            // cmbOrder
            // 
            this.cmbOrder.FormattingEnabled = true;
            this.cmbOrder.Location = new System.Drawing.Point(55, 108);
            this.cmbOrder.Name = "cmbOrder";
            this.cmbOrder.Size = new System.Drawing.Size(186, 20);
            this.cmbOrder.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "指令：";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(247, 103);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(87, 29);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.BackColor = System.Drawing.Color.White;
            this.txtSend.Location = new System.Drawing.Point(0, 20);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(797, 77);
            this.txtSend.TabIndex = 0;
            // 
            // txtReceive
            // 
            this.txtReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceive.BackColor = System.Drawing.Color.White;
            this.txtReceive.Location = new System.Drawing.Point(3, 17);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ReadOnly = true;
            this.txtReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceive.Size = new System.Drawing.Size(808, 464);
            this.txtReceive.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvFineTrajectory);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(825, 639);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "精确空间轨迹";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvFineTrajectory
            // 
            this.dgvFineTrajectory.AllowDrop = true;
            this.dgvFineTrajectory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFineTrajectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFineTrajectory.Location = new System.Drawing.Point(3, 3);
            this.dgvFineTrajectory.Name = "dgvFineTrajectory";
            this.dgvFineTrajectory.RowTemplate.Height = 23;
            this.dgvFineTrajectory.Size = new System.Drawing.Size(819, 633);
            this.dgvFineTrajectory.TabIndex = 2;
            // 
            // gbSet3
            // 
            this.gbSet3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSet3.Controls.Add(this.cmbEncoding);
            this.gbSet3.Controls.Add(this.label7);
            this.gbSet3.Controls.Add(this.chkHEXShow);
            this.gbSet3.Controls.Add(this.chkHEXSend);
            this.gbSet3.Location = new System.Drawing.Point(12, 174);
            this.gbSet3.Name = "gbSet3";
            this.gbSet3.Size = new System.Drawing.Size(157, 88);
            this.gbSet3.TabIndex = 7;
            this.gbSet3.TabStop = false;
            this.gbSet3.Text = "辅助";
            // 
            // cmbEncoding
            // 
            this.cmbEncoding.FormattingEnabled = true;
            this.cmbEncoding.Location = new System.Drawing.Point(71, 15);
            this.cmbEncoding.Name = "cmbEncoding";
            this.cmbEncoding.Size = new System.Drawing.Size(80, 20);
            this.cmbEncoding.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "编码：";
            // 
            // chkHEXShow
            // 
            this.chkHEXShow.AutoSize = true;
            this.chkHEXShow.Location = new System.Drawing.Point(26, 63);
            this.chkHEXShow.Name = "chkHEXShow";
            this.chkHEXShow.Size = new System.Drawing.Size(66, 16);
            this.chkHEXShow.TabIndex = 2;
            this.chkHEXShow.Text = "HEX显示";
            this.chkHEXShow.UseVisualStyleBackColor = true;
            // 
            // chkHEXSend
            // 
            this.chkHEXSend.AutoSize = true;
            this.chkHEXSend.Location = new System.Drawing.Point(26, 41);
            this.chkHEXSend.Name = "chkHEXSend";
            this.chkHEXSend.Size = new System.Drawing.Size(66, 16);
            this.chkHEXSend.TabIndex = 1;
            this.chkHEXSend.Text = "HEX发送";
            this.chkHEXSend.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(12, 408);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(157, 29);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "重新计数";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSendReceive
            // 
            this.btnSendReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSendReceive.Location = new System.Drawing.Point(12, 373);
            this.btnSendReceive.Name = "btnSendReceive";
            this.btnSendReceive.Size = new System.Drawing.Size(157, 29);
            this.btnSendReceive.TabIndex = 9;
            this.btnSendReceive.Text = "清接收区";
            this.btnSendReceive.UseVisualStyleBackColor = true;
            this.btnSendReceive.Click += new System.EventHandler(this.btnSendReceive_Click);
            // 
            // btnClearSend
            // 
            this.btnClearSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSend.Location = new System.Drawing.Point(12, 338);
            this.btnClearSend.Name = "btnClearSend";
            this.btnClearSend.Size = new System.Drawing.Size(157, 29);
            this.btnClearSend.TabIndex = 7;
            this.btnClearSend.Text = "清发送区";
            this.btnClearSend.UseVisualStyleBackColor = true;
            this.btnClearSend.Click += new System.EventHandler(this.btnClearSend_Click);
            // 
            // btnRefreshPort
            // 
            this.btnRefreshPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshPort.Location = new System.Drawing.Point(12, 303);
            this.btnRefreshPort.Name = "btnRefreshPort";
            this.btnRefreshPort.Size = new System.Drawing.Size(157, 29);
            this.btnRefreshPort.TabIndex = 12;
            this.btnRefreshPort.Text = "刷新串口";
            this.btnRefreshPort.UseVisualStyleBackColor = true;
            this.btnRefreshPort.Click += new System.EventHandler(this.btnRefreshPort_Click);
            // 
            // tpBLE数据
            // 
            this.tpBLE数据.Controls.Add(this.btn滤波保存);
            this.tpBLE数据.Controls.Add(this.btn保存);
            this.tpBLE数据.Controls.Add(this.btn原始数据解析);
            this.tpBLE数据.Controls.Add(this.btn滤波解析);
            this.tpBLE数据.Controls.Add(this.txtBleText);
            this.tpBLE数据.Controls.Add(this.btnBle解析);
            this.tpBLE数据.Location = new System.Drawing.Point(4, 22);
            this.tpBLE数据.Name = "tpBLE数据";
            this.tpBLE数据.Padding = new System.Windows.Forms.Padding(3);
            this.tpBLE数据.Size = new System.Drawing.Size(825, 639);
            this.tpBLE数据.TabIndex = 2;
            this.tpBLE数据.Text = "串口数据";
            this.tpBLE数据.UseVisualStyleBackColor = true;
            // 
            // btn保存
            // 
            this.btn保存.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn保存.Location = new System.Drawing.Point(433, 604);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(84, 29);
            this.btn保存.TabIndex = 6;
            this.btn保存.Text = "数据保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn原始数据解析
            // 
            this.btn原始数据解析.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn原始数据解析.Location = new System.Drawing.Point(43, 604);
            this.btn原始数据解析.Name = "btn原始数据解析";
            this.btn原始数据解析.Size = new System.Drawing.Size(87, 29);
            this.btn原始数据解析.TabIndex = 5;
            this.btn原始数据解析.Text = "原始数据解析";
            this.btn原始数据解析.UseVisualStyleBackColor = true;
            this.btn原始数据解析.Click += new System.EventHandler(this.btn原始数据解析_Click);
            // 
            // btn滤波解析
            // 
            this.btn滤波解析.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn滤波解析.Location = new System.Drawing.Point(285, 604);
            this.btn滤波解析.Name = "btn滤波解析";
            this.btn滤波解析.Size = new System.Drawing.Size(116, 29);
            this.btn滤波解析.TabIndex = 4;
            this.btn滤波解析.Text = "开源滤波数据解析";
            this.btn滤波解析.UseVisualStyleBackColor = true;
            this.btn滤波解析.Click += new System.EventHandler(this.btn滤波解析_Click);
            // 
            // txtBleText
            // 
            this.txtBleText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBleText.Location = new System.Drawing.Point(6, 6);
            this.txtBleText.Name = "txtBleText";
            this.txtBleText.Size = new System.Drawing.Size(811, 592);
            this.txtBleText.TabIndex = 3;
            this.txtBleText.Text = "";
            // 
            // btnBle解析
            // 
            this.btnBle解析.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBle解析.Location = new System.Drawing.Point(166, 604);
            this.btnBle解析.Name = "btnBle解析";
            this.btnBle解析.Size = new System.Drawing.Size(87, 29);
            this.btnBle解析.TabIndex = 2;
            this.btnBle解析.Text = "滤波解析";
            this.btnBle解析.UseVisualStyleBackColor = true;
            this.btnBle解析.Click += new System.EventHandler(this.btnBle解析_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpBLE数据);
            this.tabControl1.Controls.Add(this.tb心电图);
            this.tabControl1.Location = new System.Drawing.Point(175, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(833, 665);
            this.tabControl1.TabIndex = 5;
            // 
            // tb心电图
            // 
            this.tb心电图.Controls.Add(this.ChartHeart);
            this.tb心电图.Location = new System.Drawing.Point(4, 22);
            this.tb心电图.Name = "tb心电图";
            this.tb心电图.Padding = new System.Windows.Forms.Padding(3);
            this.tb心电图.Size = new System.Drawing.Size(825, 639);
            this.tb心电图.TabIndex = 3;
            this.tb心电图.Text = "心电图";
            this.tb心电图.UseVisualStyleBackColor = true;
            // 
            // ChartHeart
            // 
            chartArea1.Name = "ChartArea1";
            this.ChartHeart.ChartAreas.Add(chartArea1);
            this.ChartHeart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartHeart.Location = new System.Drawing.Point(3, 3);
            this.ChartHeart.Name = "ChartHeart";
            this.ChartHeart.Size = new System.Drawing.Size(819, 633);
            this.ChartHeart.TabIndex = 1;
            // 
            // chkFilter
            // 
            this.chkFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFilter.AutoSize = true;
            this.chkFilter.Location = new System.Drawing.Point(26, 455);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Size = new System.Drawing.Size(48, 16);
            this.chkFilter.TabIndex = 1;
            this.chkFilter.Text = "滤波";
            this.chkFilter.UseVisualStyleBackColor = true;
            this.chkFilter.CheckedChanged += new System.EventHandler(this.chbMean_CheckedChanged);
            // 
            // btn滤波保存
            // 
            this.btn滤波保存.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn滤波保存.Location = new System.Drawing.Point(536, 604);
            this.btn滤波保存.Name = "btn滤波保存";
            this.btn滤波保存.Size = new System.Drawing.Size(97, 29);
            this.btn滤波保存.TabIndex = 7;
            this.btn滤波保存.Text = "滤波数据保存";
            this.btn滤波保存.UseVisualStyleBackColor = true;
            this.btn滤波保存.Click += new System.EventHandler(this.btn滤波保存_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 681);
            this.Controls.Add(this.btnRefreshPort);
            this.Controls.Add(this.chkFilter);
            this.Controls.Add(this.gbSet3);
            this.Controls.Add(this.btnSendReceive);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnClearSend);
            this.Controls.Add(this.gbSet);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "移动心电调试工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.gbSet.ResumeLayout(false);
            this.gbSet.PerformLayout();
            this.tp蓝牙原始数据.ResumeLayout(false);
            this.gbReceive.ResumeLayout(false);
            this.gbReceive.PerformLayout();
            this.gbSend.ResumeLayout(false);
            this.gbSend.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFineTrajectory)).EndInit();
            this.gbSet3.ResumeLayout(false);
            this.gbSet3.PerformLayout();
            this.tpBLE数据.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tb心电图.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChartHeart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSet;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStopBit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDataBit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBaundrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TabPage tp蓝牙原始数据;
        private System.Windows.Forms.GroupBox gbReceive;
        private System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.GroupBox gbSet3;
        private System.Windows.Forms.ComboBox cmbEncoding;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkHEXShow;
        private System.Windows.Forms.CheckBox chkHEXSend;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSendReceive;
        private System.Windows.Forms.Button btnClearSend;
        private System.Windows.Forms.GroupBox gbSend;
        private System.Windows.Forms.ComboBox cmbOrder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnRefreshPort;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvFineTrajectory;
        private System.Windows.Forms.TabPage tpBLE数据;
        private System.Windows.Forms.RichTextBox txtBleText;
        private System.Windows.Forms.Button btnBle解析;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tb心电图;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartHeart;
        private System.Windows.Forms.CheckBox chkFilter;
        private System.Windows.Forms.Button btn滤波解析;
        private System.Windows.Forms.Button btn原始数据解析;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn滤波保存;
    }
}

