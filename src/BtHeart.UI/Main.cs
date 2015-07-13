using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BtHeart.Controller;
using System.Windows.Forms.DataVisualization.Charting;
using System.Configuration;

namespace BtHeart.UI
{
    public partial class Main : Form
    {
        private HeartContext HeartContext;

        public Main()
        {
            InitializeComponent();

            HeartContext = new HeartContext();
            HeartContext.Processed += HeartContext_Processed;
            HeartContext.ComReceived += HeartContext_ComReceived;
            HeartContext.RateAnalyzed += HeartContext_RateAnalyzed;

            ChartHeart.Series.Clear();
            Series s1 = ChartHeart.Series.Add("heart");
            s1.ChartType = SeriesChartType.Line;
            s1.Color = Color.Red;
        }

        private void Config()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["portName"].Value = cmbPort.Text.Trim();
            config.AppSettings.Settings["baudRate"].Value = cmbBaundrate.Text.Trim();
            config.AppSettings.Settings["dataBits"].Value = cmbDataBit.Text.Trim();
            config.Save();
            ConfigurationManager.RefreshSection("AppSettings");
        }

        private void HeartContext_Processed(EcgPacket ecgPacket)
        {
            if (!this.IsHandleCreated)
                return;
            this.BeginInvoke(new Action(() =>
            {
                foreach(var data in ecgPacket.Data)
                    ChartHeart.Series[0].Points.AddY(data);
                if(ChartHeart.Series[0].Points.Count >= 1000)
                {
                    for(int i = 0; i < 12; i++)
                        ChartHeart.Series[0].Points.RemoveAt(0);
                }
                //ChartHeart.ResetAutoValues();
                ChartHeart.Invalidate();
            }));
        }

        private void HeartContext_ComReceived(byte[] buffer)
        {
            if (buffer == null || buffer.Length < 1)
                return;

            var line = "";
            if (chkHEXShow.Checked)
                line = string.Join(" ", buffer.Select(b => b.ToString("x2")));
            else
                line = Encoding.Default.GetString(buffer);

            if (!this.IsHandleCreated)
                return;
            this.BeginInvoke(new Action(() =>
            {
                txtBleText.AppendText(line + " ");
            }));
        }

        private void HeartContext_RateAnalyzed(int? rate)
        {
            if (!this.IsHandleCreated)
                return;
            this.BeginInvoke(new Action(() =>
            {
                lblRate.Text = rate.HasValue ? rate.Value.ToString() : "...";
            }));
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadInfo();
            LoadChartInfo();
        }

        #region 串口操作
        private void LoadInfo()
        {
            cmbPort.DataSource = SerialPort.GetPortNames();
            cmbPort.SelectedItem = ConfigurationManager.AppSettings["portName"];

            cmbStopBit.DataSource = Enum.GetValues(typeof(StopBits));
            cmbParity.DataSource = Enum.GetValues(typeof(Parity));

            cmbStopBit.SelectedItem = StopBits.One;

            cmbBaundrate.DataSource = new Int32[] { 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 194000 };
            cmbBaundrate.SelectedItem = Int32.Parse(ConfigurationManager.AppSettings["baudRate"]);

            cmbDataBit.DataSource = new Int32[] { 5, 6, 7, 8 };
            cmbDataBit.SelectedItem = Int32.Parse(ConfigurationManager.AppSettings["dataBits"]);

            cmbEncoding.DataSource = new String[] { Encoding.Default.WebName, Encoding.ASCII.WebName, Encoding.UTF8.WebName };
            cmbEncoding.SelectedItem = Encoding.Default.WebName;
            chkHEXShow.Checked = true;

            chkFilter.Checked = true;
        }

        private void btnRefreshPort_Click(object sender, EventArgs e)
        {
            cmbPort.Text = "";
            cmbPort.DataSource = SerialPort.GetPortNames();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnConnect.Text == "打开串口")
                {
                    Config();
                    //var port = cmbPort.SelectedItem+"";
                    //if(port.IsEmpty())
                    //{
                    //    MessageBox.Show("请选择串口！","警告");
                    //    cmbPort.Focus();
                    //    return;
                    //}
                    ChartHeart.Series[0].Points.Clear();
                    HeartContext.Start();
                    btnConnect.Text = "关闭串口";
                }
                else
                {
                    HeartContext.Stop();
                    gbSet.Enabled = true;
                    btnConnect.Text = "打开串口";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnClearSend_Click(object sender, EventArgs e)
        {
            txtSend.Clear();
        }

        private void btnSendReceive_Click(object sender, EventArgs e)
        {
            txtBleText.Clear();
        }
        #endregion

        #region 运动数据表格

        private void btnBle解析_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBleText.Text))
                    return;

                Heart.ReadText(txtBleText.Text);
                Console.WriteLine("delta=" + (Heart.Datas.Max() - Heart.Datas.Min()));
                InitChartHeart(Heart.Datas);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void btn滤波解析_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBleText.Text))
                    return;

                Heart.ReadTextEx(txtBleText.Text);
                Console.WriteLine("delta=" + (Heart.Datas.Max() - Heart.Datas.Min()));
                InitChartHeart(Heart.Datas);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void btn原始数据解析_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBleText.Text))
                    return;

                Heart.ReadTextOriginal(txtBleText.Text);
                Console.WriteLine("delta=" + (Heart.Datas.Max() - Heart.Datas.Min()));
                InitChartHeart(Heart.Datas);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 图表
        private void InitChartHeart(List<double> a)
        {
            ChartHeart.Series.Clear();
            Series s1 = ChartHeart.Series.Add("heart");
            s1.ChartType = SeriesChartType.Line;
            s1.Color = Color.Red;
            s1.Points.DataBindY(a);

            ChartHeart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            ChartHeart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            ChartHeart.ChartAreas[0].RecalculateAxesScale();
            ChartHeart.Invalidate();
        }
        #endregion

        #region 滤波开关
        private void chbMean_CheckedChanged(object sender, EventArgs e)
        {
            HeartContext.EnabledFilter(chkFilter.Checked);
        }
        #endregion

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            HeartContext.Stop();
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = " csv files(*.csv)|*.csv|All files(*.*)|*.*";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = fileDialog.FileName;
                Heart.SaveTxt(txtBleText.Text, path);
            }
        }

        private void btn滤波保存_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = " csv files(*.csv)|*.csv|All files(*.*)|*.*";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = fileDialog.FileName;
                HeartContext.SaveTxt(path);
            }
        }

        private void LoadChartInfo()
        {
            double f = 500;
            ChartHeart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            ChartHeart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            ChartHeart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            ChartHeart.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.LightGray;
            ChartHeart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            ChartHeart.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            ChartHeart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            ChartHeart.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.LightGray;

            ChartHeart.ChartAreas[0].AxisX.Minimum = 0;
            ChartHeart.ChartAreas[0].AxisX.Maximum = 2*f; // 2s
            ChartHeart.ChartAreas[0].AxisX.IsStartedFromZero = true;
            ChartHeart.ChartAreas[0].AxisY.Minimum = -2;
            ChartHeart.ChartAreas[0].AxisY.Maximum = 2; // -2~2mv;
            ChartHeart.ChartAreas[0].AxisY.IsStartedFromZero = true;

            ChartHeart.ChartAreas[0].AxisX.IsLabelAutoFit = true;
            ChartHeart.ChartAreas[0].AxisY.IsLabelAutoFit = true;

            ChartHeart.ChartAreas[0].AxisX.MajorTickMark.Interval = 0.2 * f;
            ChartHeart.ChartAreas[0].AxisX.MinorTickMark.Interval = 0.04 * f;
            ChartHeart.ChartAreas[0].AxisY.MajorTickMark.Interval = 0.5;
            ChartHeart.ChartAreas[0].AxisY.MinorTickMark.Interval = 0.1;

            ChartHeart.ChartAreas[0].AxisX.MajorGrid.Interval = 0.2*f;
            ChartHeart.ChartAreas[0].AxisX.MinorGrid.Interval = 0.04*f;
            ChartHeart.ChartAreas[0].AxisY.MajorGrid.Interval = 0.5;
            ChartHeart.ChartAreas[0].AxisY.MinorGrid.Interval = 0.1;
        }
    }
}
