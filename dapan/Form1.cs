using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace dapan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread my = new Thread(fun);
            my.IsBackground = true;
            my.Start();
        }
        
        private void fun()
        {
            List<string> lcode = new List<string>();
            List<string> lji = new List<string>();

            StreamReader reader = new StreamReader("gu.txt", System.Text.Encoding.Default);
         
            while (true)
            {
                string strLine = reader.ReadLine();
                if (strLine == null)
                {
                    break;
                }
                else
                {
                    lcode.Add(strLine);
                }
            }
            reader.Close();

            reader = new StreamReader("ji.txt", System.Text.Encoding.Default);

            while (true)
            {
                string strLine = reader.ReadLine();
                if (strLine == null)
                {
                    break;
                }
                else
                {
                    lji.Add(strLine);
                }
            }
            reader.Close();


            DataTable dtWork = new DataTable();
            string[] strColumnsName = { "名称", "今开", "昨收", "当前点数", "最高", "最低", "涨跌额", "涨跌幅", "成交量(手)", "成交额(元)" };
            for (int i = 0; i < strColumnsName.Length; i++)
            {
                dtWork.Columns.Add(strColumnsName[i], typeof(String));
            }
            for (int i = 0; i < lcode.Count; i++)
            {
                try
                {

                    dtWork.Rows.Add();

                    for (int j = 0; j < strColumnsName.Length; j++)
                    {
                        dtWork.Rows[i][j] = "0";
                    }
                }
                catch (Exception eee) { }
            }

            this.Invoke((EventHandler)(delegate
            {
                dataGridView1.DataSource = dtWork;
                dataGridView1.AllowUserToAddRows = false;

                //自动适应宽度，需要同时设置以下两个属性
                dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Aqua;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                //dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("SimHei", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.Black;
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;
                //dataGridView1.RowsDefaultCellStyle.Font = new System.Drawing.Font("SimHei", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                dataGridView1.ClearSelection();
                dataGridView1.RowHeadersVisible = false;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//数据 界面充满
            }));

            DataTable dtWork1 = new DataTable();
            string[] strColumnsName1 = { "名称", "今天估值", "净值","昨日涨跌幅", "近1月", "近3月", "近6月", "近1年", "近3年", "成立来" };
            for (int i = 0; i < strColumnsName1.Length; i++)
            {
                dtWork1.Columns.Add(strColumnsName1[i], typeof(String));
            }
            for (int i = 0; i < lcode.Count; i++)
            {
                try
                {

                    dtWork1.Rows.Add();

                    for (int j = 0; j < strColumnsName1.Length; j++)
                    {
                        dtWork1.Rows[i][j] = "0";
                    }
                }
                catch (Exception eee) { }
            }

            this.Invoke((EventHandler)(delegate
            {
                dataGridView2.DataSource = dtWork1;
                dataGridView2.AllowUserToAddRows = false;

                //自动适应宽度，需要同时设置以下两个属性
                dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Aqua;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                //dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("SimHei", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.Black;
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;
                //dataGridView1.RowsDefaultCellStyle.Font = new System.Drawing.Font("SimHei", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                dataGridView2.ClearSelection();
                dataGridView2.RowHeadersVisible = false;

                //dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//数据 界面充满
            }));





            while (true)
            {
                for (int i=0;i<lcode.Count;i++)
                {
                    try
                    {
                        WebClient MyWebClient = new WebClient();
                        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                        Byte[] pageData = MyWebClient.DownloadData(lcode[i]); //从指定网站下载数据           
                        string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句   
                        string[] sp = pageHtml.Split('"');
                        string[] sp2 = sp[1].Split(',');
                        
                        for (int j = 0; j < strColumnsName.Length; j++)
                        {
                            dtWork.Rows[i][j] = sp2[j];
                        }
                        dtWork.Rows[i][6] =( Convert.ToDouble(sp2[3]) - Convert.ToDouble(sp2[2])).ToString("0.00");
                        dtWork.Rows[i][7] = ((Convert.ToDouble(sp2[3]) - Convert.ToDouble(sp2[2])) / Convert.ToDouble(sp2[2]) * 100).ToString("0.00")+"%";
                        
                    }
                    catch (Exception eee) { }
                }

                for (int i = 0; i < lji.Count; i++)
                {
                    try
                    {
                        WebClient MyWebClient = new WebClient();
                        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                        Byte[] pageData = MyWebClient.DownloadData(lji[i]); //从指定网站下载数据           
                        string pageHtml = Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句   
                                                                              //"funCur-FundName">



                      


                        pageHtml = pageHtml.Replace("<span","|");
                        string[] sp = pageHtml.Split('|');
                        for (int k=0;k<sp.Length;k++)
                        {
                            if (sp[k].Contains("fix_fname"))
                            {
                                dtWork1.Rows[i][0] = sp[k].Trim().Replace("class=\"fix_fname\">", "").Replace("</span>","");
                            }
                            if (sp[k].Contains("fix_dwjz"))
                            {
                                string strtmp= sp[k].Trim().Replace("class=\"fix_dwjz", "").Replace("</span", "");
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"),strtmp.Length- strtmp.IndexOf(">"));
                                dtWork1.Rows[i][2] = strtmp.Replace(">","");
                            }

                            if (sp[k].Contains("fix_zzl"))
                            {
                                string strtmp = sp[k].Trim().Replace("class=\"fix_zzl", "").Replace("</span", "");
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"), strtmp.Length - strtmp.IndexOf(">"));
                                dtWork1.Rows[i][3] = strtmp.Replace(">", "");
                            }
                            if (sp[k].Contains("近1月："))
                            {
                                string strtmp = sp[k+1].Trim();
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"), strtmp.Length - strtmp.IndexOf(">"));
                                strtmp = strtmp.Replace("</span></dd><dd>", "").Replace(">", "");
                                dtWork1.Rows[i][4] = strtmp;
                            }
                            if (sp[k].Contains("近3月："))
                            {
                                string strtmp = sp[k + 1].Trim();
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"), strtmp.Length - strtmp.IndexOf(">"));
                                strtmp = strtmp.Replace("</span></dd><dd>", "").Replace(">", "");
                                dtWork1.Rows[i][5] = strtmp;
                            }
                            if (sp[k].Contains("近6月："))
                            {
                                string strtmp = sp[k + 1].Trim();
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"), strtmp.Length - strtmp.IndexOf(">"));
                                strtmp = strtmp.Replace("</span></dd><dd>", "").Replace(">", "");
                                dtWork1.Rows[i][6] = strtmp;
                            }
                            if (sp[k].Contains("近1年："))
                            {
                                string strtmp = sp[k + 1].Trim();
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"), strtmp.Length - strtmp.IndexOf(">"));
                                strtmp = strtmp.Replace("</span></dd></dl>", "").Replace(">", "");
                                dtWork1.Rows[i][7] = strtmp;
                            }
                            if (sp[k].Contains("近3年："))
                            {
                                string strtmp = sp[k + 1].Trim();
                                strtmp = strtmp.Substring(strtmp.IndexOf(">"), strtmp.Length - strtmp.IndexOf(">"));
                                strtmp = strtmp.Replace("</span></dd></dl>", "").Replace(">", "");
                                dtWork1.Rows[i][8] = strtmp;
                            }
                            if (sp[k].Contains("成立来："))
                            {
                                string strtmp = sp[k + 1].Trim();
                                strtmp = strtmp.Substring(strtmp.IndexOf(">")+1, strtmp.IndexOf("%") - strtmp.IndexOf(">"));
                                dtWork1.Rows[i][9] = strtmp;
                            }
                        }
                    }
                    catch (Exception eee) { }
                }



                try
                {
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                    Byte[] pageData = MyWebClient.DownloadData("http://api.money.126.net/data/feed/1399001,1399300,0000001,HSRANK_COUNT_SHA,HSRANK_COUNT_SZA,HSRANK_COUNT_SH3?callback=ne_1618551845654&[object%20Object]"); //从指定网站下载数据           
                    string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句   
                    string[] sp = pageHtml.Split('{');
                    for (int k=0;k<sp.Length;k++)
                    {
                        #region
                        string[] sp2 = sp[k].Split('"');
                        if (k==4)
                        {

                            this.Invoke((EventHandler)(delegate
                            {
                                label1.Text = sp2[18].Trim().Replace(",", "").Replace(":", "") + (Convert.ToDouble(sp2[36].Trim().Replace(",", "").Replace(":", "")) > 0 ? "▲" : "▼");
                                label1.ForeColor = Convert.ToDouble(sp2[36].Trim().Replace(",", "").Replace(":", "")) > 0 ? Color.Red : Color.Green;
                                label2.ForeColor = label1.ForeColor;
                                label3.ForeColor = label1.ForeColor;
                                label2.Text = "涨跌额:" + (Convert.ToDouble(sp2[36].Trim().Replace(",", "").Replace(":", "")) > 0 ? "+" : "-") + sp2[36].Trim().Replace(",", "").Replace(":", "");
                                label3.Text = "涨跌幅:"+(Convert.ToDouble(sp2[6].Trim().Replace(",", "").Replace(":", "")) * 100).ToString("0.00") + "%";
                                label4.Text ="成交额(元):"+ sp2[88].Trim().Replace(",", "").Replace(":", "").Replace("}", "");
                            }));
                        }
                        if (k == 3)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                label14.Text = sp2[18].Trim().Replace(",", "").Replace(":", "") + (Convert.ToDouble(sp2[34].Trim().Replace(",", "").Replace(":", "")) > 0 ? "▲" : "▼");
                                label14.ForeColor = Convert.ToDouble(sp2[34].Trim().Replace(",", "").Replace(":", "")) > 0 ? Color.Red : Color.Green;
                                label12.ForeColor = label14.ForeColor;
                                label13.ForeColor = label14.ForeColor;
                                label13.Text = "涨跌额:" + (Convert.ToDouble(sp2[34].Trim().Replace(",", "").Replace(":", "")) > 0 ? "+" : "-") + sp2[34].Trim().Replace(",", "").Replace(":", "");
                                label12.Text = "涨跌幅:" + (Convert.ToDouble(sp2[6].Trim().Replace(",", "").Replace(":", "")) * 100).ToString("0.00") + "%";
                                label11.Text = "成交额(元):" + sp2[88].Trim().Replace(",", "").Replace(":", "").Replace("}", "");
                            }));
                        }
                        if (k == 2)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                label21.Text = sp2[18].Trim().Replace(",", "").Replace(":", "") + (Convert.ToDouble(sp2[34].Trim().Replace(",", "").Replace(":", "")) > 0 ? "▲" : "▼");
                                label21.ForeColor = Convert.ToDouble(sp2[34].Trim().Replace(",", "").Replace(":", "")) > 0 ? Color.Red : Color.Green;
                                label20.ForeColor = label21.ForeColor;
                                label19.ForeColor = label21.ForeColor;
                                label20.Text = "涨跌额:" + (Convert.ToDouble(sp2[34].Trim().Replace(",", "").Replace(":", "")) > 0 ? "+" : "-") + sp2[34].Trim().Replace(",", "").Replace(":", "");
                                label19.Text = "涨跌幅:" + (Convert.ToDouble(sp2[6].Trim().Replace(",", "").Replace(":", "")) * 100).ToString("0.00") + "%";
                                label18.Text = "成交额(元):" + sp2[88].Trim().Replace(",", "").Replace(":", "").Replace("}", "");
                            }));
                        }



                        if (k == 5)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                label7.Text ="涨:"+ sp2[4].Trim().Replace(",", "").Replace(":", "");
                                label6.Text ="平:"+ sp2[2].Trim().Replace(",", "").Replace(":", "");
                                label5.Text ="跌:"+ sp2[6].Trim().Replace(",", "").Replace(":", "");
                            }));
                        }
                        if (k == 6)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                label17.Text = "涨:" + sp2[4].Trim().Replace(",", "").Replace(":", "");
                                label16.Text = "平:" + sp2[2].Trim().Replace(",", "").Replace(":", "");
                                label15.Text = "跌:" + sp2[6].Trim().Replace(",", "").Replace(":", "");
                            }));
                        }
                        if (k == 7)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                label10.Text = "涨:" + sp2[4].Trim().Replace(",", "").Replace(":", "");
                                label9.Text = "平:" + sp2[2].Trim().Replace(",", "").Replace(":", "");
                                label8.Text = "跌:" + sp2[6].Trim().Replace(",", "").Replace(":", "");
                            }));
                        }
                        #endregion
                    }
                }
                catch (Exception eee) { }

                try
                {
                    string url = "http://img1.money.126.net/chart/hs/time/210x140/0000001.png?1618558181529";
                    string filepath = "0000001.png";
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(url, filepath);
                    this.Invoke((EventHandler)(delegate
                    {
                        pictureBox1.Load(filepath);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                catch (Exception eee) { }
                try
                {
                    string url = "http://img1.money.126.net/chart/hs/time/210x140/1399001.png?1618558181525";
                    string filepath = "1399001.png";
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(url, filepath);
                    this.Invoke((EventHandler)(delegate
                    {
                        pictureBox3.Load(filepath);
                        pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                catch (Exception eee) { }
                try
                {
                    string url = "http://img1.money.126.net/chart/hs/time/210x140/1399300.png?1618558181527";
                    string filepath = "1399300.png";
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(url, filepath);
                    this.Invoke((EventHandler)(delegate
                    {
                        pictureBox2.Load(filepath);
                        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                catch (Exception eee) { }


                //基金





                this.Invoke((EventHandler)(delegate
                {

                    this.Text = "刷新时间:" + DateTime.Now.ToString();
                }));
                
                Thread.Sleep(1000);
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                DataGridViewRow dr = (sender as DataGridView).Rows[e.RowIndex];
                
                if (dr.Cells["涨跌幅"].Value.ToString().Trim().Contains("-"))
                {
                    // 设置单元格的背景色
                    dr.Cells["涨跌幅"].Style.ForeColor = Color.Green;
                }
                else
                {
                    // 设置单元格的背景色
                    dr.Cells["涨跌幅"].Style.ForeColor = Color.Red;
                }
            }
            catch (Exception eee) { }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == DBNull.Value)
            {
                e.Cancel = true;
            }
        }

        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                DataGridViewRow dr = (sender as DataGridView).Rows[e.RowIndex];

                if (dr.Cells["昨日涨跌幅"].Value.ToString().Trim().Contains("-"))
                {
                    // 设置单元格的背景色
                    dr.Cells["昨日涨跌幅"].Style.ForeColor = Color.Green;
                }
                else
                {
                    // 设置单元格的背景色
                    dr.Cells["昨日涨跌幅"].Style.ForeColor = Color.Red;
                }
            }
            catch (Exception eee) { }

            try
            {
                DataGridViewRow dr = (sender as DataGridView).Rows[e.RowIndex];

                if (dr.Cells["今天估值"].Value.ToString().Trim().Contains("-"))
                {
                    // 设置单元格的背景色
                    dr.Cells["今天估值"].Style.ForeColor = Color.Green;
                }
                else
                {
                    // 设置单元格的背景色
                    dr.Cells["今天估值"].Style.ForeColor = Color.Red;
                }
            }
            catch (Exception eee) { }
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == DBNull.Value)
            {
                e.Cancel = true;
            }

        }
    }
}
