using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TS_RGB.Fuction;
using System.IO;

namespace TS_RGB
{
    public partial class frm_map : Form
    {
        #region params

        public int lineNo;//线别
        public static int map_width = 1115;
        public static int map_height = 306;

        bool MoveMap = false;
        Point MapLoction;
        Point Sta;
        Size OrdSize;
        List<Label> ShowRGV;
        List<Label> Show_Elevator;
        List<Label> Show_CallBox;
        List<Label> Show_Power;

        List<Label> Map_Show_No_S;
        int xOffset;
        int yOffset;
        int RGV_Num;
        int[] RGV_S_X;
        int[] RGV_S_Y;
        int RFID_Num;
        int[] RFID_Code;
        int[] RFID_X;
        int[] RFID_Y;

        List<Point> CallBox_Map;
        List<Point> Elevator_Map;
        List<Point> Power_Map;

        string ctrl_Name = "";
        string ctrl_Text = "";

        int mouswell;

        #endregion

        public frm_map()
        {
            InitializeComponent();
            img_Map.BackgroundImage = Image.FromFile(Application.StartupPath + "\\img\\HSE.PNG");
            img_Map.BackgroundImageLayout = ImageLayout.Zoom;
        }
        private void frm_map_Load(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            mouswell = 0;
            MapLoction = img_Map.Location;
            OrdSize = img_Map.Size;
            Show_Elevator = new List<Label>();
            Show_CallBox = new List<Label>();
            Show_Power = new List<Label>();
            ShowRGV = new List<Label>();
            Map_Show_No_S = new List<Label>();
            //ShowElevator();
            ShowPower();
            ShowAGV();
            ShowRfid();
            ShowCallBox();
            timer1.Start();
        }
        private void frm_map_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        //禁用Close按钮
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


        private void ShowRfid()
        {
            DataTable RFID_Table = Function.Select_RFID_MAP();
            if (RFID_Table != null && RFID_Table.Rows.Count > 0)
            {
                RFID_Num = RFID_Table.Rows.Count;
                RFID_Code = new int[RFID_Num];
                RFID_X = new int[RFID_Num];
                RFID_Y = new int[RFID_Num];
                for (int i = 0; i < RFID_Num; i++)
                {
                    RFID_Code[i] = int.Parse(RFID_Table.Rows[i][1].ToString());
                    RFID_X[i] = int.Parse(RFID_Table.Rows[i][2].ToString());
                    RFID_Y[i] = int.Parse(RFID_Table.Rows[i][3].ToString());
                    ////地图AGV LABEL
                    Label lb_RFID = new Label();
                    lb_RFID.AutoSize = true;
                    //lb_RFID.BackColor = System.Drawing.Color.LimeGreen;
                    lb_RFID.Font = new System.Drawing.Font("幼圆", 6, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                    lb_RFID.BackColor = Color.FromArgb(192, 192, 192);
                    lb_RFID.ForeColor = Color.White;
                    toolTip1.SetToolTip(lb_RFID, "地标值：" + RFID_Code[i].ToString());
                        

                    //lb_RFID.ForeColor = System.Drawing.Color.Black;
                    lb_RFID.Location = new System.Drawing.Point(RFID_X[i], RFID_Y[i]);
                    lb_RFID.Name = "RFID_Show_" + (i + 1).ToString();
                    lb_RFID.TabIndex = 1;
                    lb_RFID.Text = RFID_Code[i].ToString();
                    lb_RFID.BringToFront();
                    Map_Show_No_S.Add(lb_RFID);
                    //this.AGV_Show_No_S[i].Click += new EventHandler(frmOpreationS_DoubleClick);
                    img_Map.Controls.Add(Map_Show_No_S[i]);
                    Map_Show_No_S[i].ContextMenuStrip = contextMenuStrip2;
                }
            }
        }
        private void RefShowRfid()
        {
            DataTable RFID_Table = Function.Select_RFID_MAP();
            if (RFID_Table != null && RFID_Table.Rows.Count > 0 && Map_Show_No_S.Count < RFID_Table.Rows.Count)
            {
                RFID_Num = RFID_Table.Rows.Count;
                RFID_Code = new int[RFID_Num];
                RFID_X = new int[RFID_Num];
                RFID_Y = new int[RFID_Num];
                for (int i = 0; i < Map_Show_No_S.Count; i++)
                {
                    RFID_Code[i] = int.Parse(RFID_Table.Rows[i][1].ToString());
                    RFID_X[i] = int.Parse(RFID_Table.Rows[i][2].ToString());
                    RFID_Y[i] = int.Parse(RFID_Table.Rows[i][3].ToString());
                }
                for (int i = Map_Show_No_S.Count; i < RFID_Table.Rows.Count; i++)
                {
                    RFID_Code[i] = int.Parse(RFID_Table.Rows[i][1].ToString());
                    RFID_X[i] = int.Parse(RFID_Table.Rows[i][2].ToString());
                    RFID_Y[i] = int.Parse(RFID_Table.Rows[i][3].ToString());
                    ////地图AGV LABEL
                    Label lb_RFID = new Label();
                    lb_RFID.AutoSize = true;
                    //lb_RFID.BackColor = System.Drawing.Color.LimeGreen;
                    lb_RFID.Font = new System.Drawing.Font("幼圆", 6, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                    lb_RFID.BackColor = Color.FromArgb(192, 192, 192);
                    lb_RFID.ForeColor = Color.White;
                    toolTip1.SetToolTip(lb_RFID, "地标值：" + RFID_Code[i].ToString());
                        
                    //lb_RFID.ForeColor = System.Drawing.Color.Black;
                    lb_RFID.Location = new System.Drawing.Point(RFID_X[i], RFID_Y[i]);
                    lb_RFID.Name = "RFID_Show_" + (i + 1).ToString();
                    lb_RFID.TabIndex = 1;
                    lb_RFID.Text = RFID_Code[i].ToString();
                    lb_RFID.BringToFront();
                    Map_Show_No_S.Add(lb_RFID);
                    img_Map.Controls.Add(Map_Show_No_S[i]);
                    Map_Show_No_S[i].ContextMenuStrip = contextMenuStrip2;
                }
            }
            else if (RFID_Table != null && RFID_Table.Rows.Count > 0)
            {
                RFID_Num = RFID_Table.Rows.Count;
                RFID_Code = new int[RFID_Num];
                RFID_X = new int[RFID_Num];
                RFID_Y = new int[RFID_Num];
                for (int i = 0; i < Map_Show_No_S.Count; i++)
                {
                    RFID_Code[i] = int.Parse(RFID_Table.Rows[i][1].ToString());
                    RFID_X[i] = int.Parse(RFID_Table.Rows[i][2].ToString());
                    RFID_Y[i] = int.Parse(RFID_Table.Rows[i][3].ToString());
                }
            }
        }

        private void ShowAGV()
        {
            DataTable AGV_Table = Function.SELETE_AGV_INFO(lineNo);
            if (AGV_Table != null && AGV_Table.Rows.Count > 0)
            {
                RGV_Num = AGV_Table.Rows.Count;
                RGV_S_X = new int[RGV_Num];
                RGV_S_Y = new int[RGV_Num];
                for (int i = 0; i < RGV_Num; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV = new Label();

                    lb_RGV.AutoSize = true;

                    lb_RGV.BorderStyle = BorderStyle.FixedSingle;
                    lb_RGV.ForeColor = Color.White;
                    toolTip1.SetToolTip(lb_RGV, "设备类型：AGV" + "\r\n" +
                        "设备编号：AGV_" + AGV_Table.Rows[i][7].ToString().Trim().PadLeft(2, '0') + "\r\n" +
                        "设备IP：" + AGV_Table.Rows[i][1].ToString().Trim());
                    lb_RGV.Font = new System.Drawing.Font("宋体", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lb_RGV.Text = "A" + AGV_Table.Rows[i][7].ToString().Trim().PadLeft(2, '0');
                    lb_RGV.Name = "AGVNO" + AGV_Table.Rows[i][0].ToString().Trim();
                    lb_RGV.Location = new Point(55, 20 + i * 40);
                    lb_RGV.BackColor = AGV_Table.Rows[i][2].ToString().Trim() == "-1" ? Color.FromArgb(173, 173, 173) : Color.FromArgb(49, 89, 196);
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    ShowRGV.Add(lb_RGV);
                    img_Map.Controls.Add(ShowRGV[i]);
                }
            }
        }
        public void RefShowAGV()
        {
            DataTable AGV_Table = Function.SELETE_AGV_INFO(lineNo);
            if (AGV_Table != null && AGV_Table.Rows.Count > 0)
            {
                var p = img_Map.Size;
                if (true )//(p.Width == map_width)
                {
                    for (int i = 0; i < ShowRGV.Count(); i++)
                    {
                        if (Map_Show_No_S != null)
                        {
                            for (int j = 0; j < Map_Show_No_S.Count(); j++)
                            {
                                //刷新AGV位置
                                if (Map_Show_No_S[j].Text.Trim() == AGV_Table.Rows[i][3].ToString().Trim())
                                {
                                    RGV_S_X[i] = Map_Show_No_S[j].Location.X;
                                    RGV_S_Y[i] = Map_Show_No_S[j].Location.Y + 9;
                                    ShowRGV[i].Location = new Point(RGV_S_X[i], RGV_S_Y[i]);
                                    toolTip1.SetToolTip(ShowRGV[i], "设备类型：AGV" + "\r\n" +
                                    "设备编号：AGV_" + AGV_Table.Rows[i][7].ToString().Trim().PadLeft(2, '0') + "\r\n" +
                                    "设备IP：" + AGV_Table.Rows[i][1].ToString().Trim() + "\r\n" +
                                    "------------------------" + "\r\n" +
                                    "设备位置：" + AGV_Table.Rows[i]["AGV_RFID_Now"].ToString().Trim() + "\r\n" +
                                    "运行方向：" + ((AGV_Table.Rows[i]["AGV_FW"].ToString().Trim()) == "1" ? "正向" : "反向") + "\r\n" +
                                    "设备电量：" + AGV_Table.Rows[i]["AGV_Power"].ToString().Trim() + "\r\n" +
                                    "网络状态：" + ((AGV_Table.Rows[i]["AGV_AC"].ToString().Trim()) == "1" ? "在线" : "离线") + "\r\n" +
                                    "设备异常：" + Function.retErr(AGV_Table.Rows[i]["AGV_ErrorCord"].ToString().Trim()));
                                }
                            }
                        }
                        //异常
                        if (AGV_Table.Rows[i]["AGV_ErrorCord"].ToString().Trim() != "0")
                        {
                            if (ShowRGV[i].BackColor != Color.Red)
                            {
                                ShowRGV[i].BackColor = Color.Red;
                            }
                        }
                        //else
                        //{
                        //    if (ShowRGV[i].BackColor != Color.FromArgb(49, 89, 196))
                        //    {
                        //        ShowRGV[i].BackColor = Color.FromArgb(49, 89, 196);
                        //    }
                        //}
                        //离线
                        if (AGV_Table.Rows[i][2].ToString().Trim() == "-1")
                        {
                            if (ShowRGV[i].BackColor != Color.FromArgb(173, 173, 173))
                            {
                                ShowRGV[i].BackColor = Color.FromArgb(173, 173, 173);
                            }
                        }
                        //else
                        //{
                        //    if (ShowRGV[i].BackColor != Color.FromArgb(49, 89, 196))
                        //    {
                        //        ShowRGV[i].BackColor = Color.FromArgb(49, 89, 196);
                        //    }
                        //}
                        
                        //ShowRGV[i].BackColor = AGV_Table.Rows[i][2].ToString().Trim() == "-1" ? Color.WhiteSmoke : Color.Lime;
                        //ShowRGV[1].BackColor = Color.WhiteSmoke;
                    }
                }
            }
        }
        public void RefShowAGV_flash()
        {
            DataTable AGV_Table = Function.SELETE_AGV_INFO(lineNo);
            if (AGV_Table != null && AGV_Table.Rows.Count > 0)
            {
                for (int i = 0; i < ShowRGV.Count(); i++)
                {

                    ShowRGV[i].BackColor = Color.FromArgb(49, 89, 196);
                }
            }

        }
        


        private void ShowElevator()
        {
            DataTable ELE_Table = Function.PSELECT_ELC();
            if (ELE_Table != null && ELE_Table.Rows.Count > 0)
            {
                //
                Elevator_Map = new List<Point>();
                for (int i = 0; i < ELE_Table.Rows.Count; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV.AutoSize = true;

                    lb_RGV.BorderStyle = BorderStyle.FixedSingle;
                    lb_RGV.ForeColor = Color.White;
                    toolTip1.SetToolTip(lb_RGV, "设备类型：电梯" + "\r\n" +
                        "设备编号：Elevator_" + ELE_Table.Rows[i][1].ToString().Trim().PadLeft(2, '0') + "\r\n" +
                        "设备IP：" + ELE_Table.Rows[i][2].ToString().Trim());



                    lb_RGV.Font = new System.Drawing.Font("幼圆", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lb_RGV.Text = "E " + ELE_Table.Rows[i][1].ToString().Trim();
                    lb_RGV.Name = ELE_Table.Rows[i][2].ToString().Trim();
                    lb_RGV.Location = new Point(int.Parse(ELE_Table.Rows[i][3].ToString().Trim()), int.Parse(ELE_Table.Rows[i][4].ToString().Trim()));
                    //
                    Elevator_Map.Add(lb_RGV.Location);

                    lb_RGV.BackColor = ELE_Table.Rows[i][10].ToString().Trim() == "-1" ? Color.FromArgb(173, 173, 173) : Color.FromArgb(246, 135, 128);
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    Show_Elevator.Add(lb_RGV);
                    img_Map.Controls.Add(Show_Elevator[i]);
                    Show_Elevator[i].ContextMenuStrip = contextMenuStrip2;
                    Show_Elevator[i].MouseDoubleClick += new MouseEventHandler(Show_Lift_dd_Click);
                }
            }
        }
        private void RefShowElevator()
        {
            DataTable ELE_Table = Function.PSELECT_ELC();
            if (ELE_Table != null && Show_Elevator != null && ELE_Table.Rows.Count >= Show_Elevator.Count)
            {
                for (int i = 0; i < Show_Elevator.Count; i++)
                {
                    Show_Elevator[i].BackColor = ELE_Table.Rows[i][10].ToString().Trim() == "1" ? Color.FromArgb(246, 135, 128) : Color.FromArgb(173, 173, 173);
                }
                for (int i = Show_Elevator.Count; i < ELE_Table.Rows.Count; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV.AutoSize = true;
                    lb_RGV.Font = new System.Drawing.Font("幼圆", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                    lb_RGV.BorderStyle = BorderStyle.FixedSingle;
                    lb_RGV.ForeColor = Color.White;
                    toolTip1.SetToolTip(lb_RGV, "设备类型：电梯" + "\r\n" +
                        "设备编号：Elevator_" + ELE_Table.Rows[i][1].ToString().Trim().PadLeft(2, '0') + "\r\n" +
                        "设备IP：" + ELE_Table.Rows[i][2].ToString().Trim());


                    lb_RGV.Text = "E " + ELE_Table.Rows[i][1].ToString().Trim().PadLeft(2, '0');
                    lb_RGV.Name = ELE_Table.Rows[i][2].ToString().Trim();
                    lb_RGV.Location = new Point(int.Parse(ELE_Table.Rows[i][3].ToString().Trim()), int.Parse(ELE_Table.Rows[i][4].ToString().Trim()));
                    lb_RGV.BackColor = ELE_Table.Rows[i][10].ToString().Trim() == "-1" ? Color.FromArgb(173, 173, 173) : Color.FromArgb(246, 135, 128);
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    Show_Elevator.Add(lb_RGV);
                    img_Map.Controls.Add(Show_Elevator[i]);
                    Show_Elevator[i].ContextMenuStrip = contextMenuStrip2;
                    Show_Elevator[i].MouseDoubleClick += new MouseEventHandler(Show_Lift_dd_Click);
                }
            }
        }
        private void RefShowElevator_flash()
        {
            DataTable ELE_Table = Function.PSELECT_ELC();
            if (ELE_Table != null && Show_Elevator != null && ELE_Table.Rows.Count >= Show_Elevator.Count)
            {
                for (int i = 0; i < ELE_Table.Rows.Count; i++)
                {
                    Show_Elevator[i].BackColor = Color.FromArgb(246, 135, 128);
                }
            }
        }


        private void ShowCallBox()
        {
            DataTable CallBox_Table = Function.SELECT_CallBox();
            if (CallBox_Table != null && CallBox_Table.Rows.Count > 0)
            {
                CallBox_Map = new List<Point>();

                for (int i = 0; i < CallBox_Table.Rows.Count; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV.AutoSize = true;
                    lb_RGV.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lb_RGV.Text = "C" + CallBox_Table.Rows[i][1].ToString().Trim();//SCallBoxNo
                    lb_RGV.Name = CallBox_Table.Rows[i][2].ToString().Trim();//Sip
                    lb_RGV.Location = new Point(int.Parse(CallBox_Table.Rows[i][4].ToString().Trim()), int.Parse(CallBox_Table.Rows[i][5].ToString().Trim()));
                    CallBox_Map.Add(lb_RGV.Location);
                    lb_RGV.BackColor = CallBox_Table.Rows[i][6].ToString().Trim() == "1" ? Color.Lime : Color.Yellow;
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    Show_CallBox.Add(lb_RGV);
                    img_Map.Controls.Add(Show_CallBox[i]);
                    Show_CallBox[i].ContextMenuStrip = contextMenuStrip2;
                    Show_CallBox[i].MouseDoubleClick += new MouseEventHandler(Show_CallBox_Click);
                }
            }
        }
        private void RefShowCallBox()
        {
            DataTable CallBox_Table = Function.SELECT_CallBox();
            if (CallBox_Table != null && Show_CallBox != null && CallBox_Table.Rows.Count >= Show_CallBox.Count)
            {
                for (int i = 0; i < Show_CallBox.Count; i++)
                {
                    Show_CallBox[i].BackColor = CallBox_Table.Rows[i][6].ToString().Trim() == "1" ? Color.Lime : Color.Yellow;
                }
                for (int i = Show_CallBox.Count; i < CallBox_Table.Rows.Count; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV.AutoSize = true;
                    lb_RGV.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lb_RGV.Text = "C" + CallBox_Table.Rows[i][1].ToString().Trim().PadLeft(2, '0');
                    lb_RGV.Name = CallBox_Table.Rows[i][2].ToString().Trim();
                    lb_RGV.Location = new Point(int.Parse(CallBox_Table.Rows[i][4].ToString().Trim()), int.Parse(CallBox_Table.Rows[i][5].ToString().Trim()));
                    lb_RGV.BackColor = CallBox_Table.Rows[i][6].ToString().Trim() == "1" ? Color.Lime : Color.Yellow;
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    Show_CallBox.Add(lb_RGV);
                    img_Map.Controls.Add(Show_CallBox[i]);
                    Show_CallBox[i].ContextMenuStrip = contextMenuStrip2;
                    Show_CallBox[i].MouseDoubleClick += new MouseEventHandler(Show_CallBox_Click);
                }
            }
        }

        private void ShowPower()
        {
            DataTable ELE_Table = Function.SELECT_Power_INFO(lineNo);
            if (ELE_Table != null && ELE_Table.Rows.Count > 0)
            {
                //
                Power_Map = new List<Point>();
                for (int i = 0; i < ELE_Table.Rows.Count; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV.AutoSize = true;
                    lb_RGV.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lb_RGV.Text = "P" + ELE_Table.Rows[i][1].ToString().Trim();
                    lb_RGV.Name = ELE_Table.Rows[i][2].ToString().Trim();
                    lb_RGV.Location = new Point(int.Parse(ELE_Table.Rows[i][5].ToString().Trim()), int.Parse(ELE_Table.Rows[i][6].ToString().Trim()));
                    //
                    Power_Map.Add(lb_RGV.Location);

                    lb_RGV.BackColor = ELE_Table.Rows[i][15].ToString().Trim() == "-1" ? Color.WhiteSmoke : Color.Lime;
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    Show_Power.Add(lb_RGV);
                    img_Map.Controls.Add(Show_Power[i]);
                    Show_Power[i].ContextMenuStrip = contextMenuStrip2;
                    Show_Power[i].MouseDoubleClick += new MouseEventHandler(Show_Power_dd_Click);
                }
            }
        }
        private void RefShowPower()
        {
            DataTable ELE_Table = Function.SELECT_Power_INFO(lineNo);
            if (ELE_Table != null && Show_Power != null && ELE_Table.Rows.Count >= Show_Power.Count)
            {
                for (int i = 0; i < Show_Power.Count; i++)
                {
                    Show_Power[i].BackColor = ELE_Table.Rows[i][15].ToString().Trim() == "-1" ? Color.WhiteSmoke : Color.Lime;
                }
                for (int i = Show_Power.Count; i < ELE_Table.Rows.Count; i++)
                {
                    Label lb_RGV = new Label();
                    lb_RGV.AutoSize = true;
                    lb_RGV.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lb_RGV.Text = "P" + ELE_Table.Rows[i][1].ToString().Trim();
                    lb_RGV.Name = ELE_Table.Rows[i][2].ToString().Trim();
                    lb_RGV.Location = new Point(int.Parse(ELE_Table.Rows[i][5].ToString().Trim()), int.Parse(ELE_Table.Rows[i][6].ToString().Trim()));
                    lb_RGV.BackColor = ELE_Table.Rows[i][15].ToString().Trim() == "-1" ? Color.WhiteSmoke : Color.Lime;
                    lb_RGV.TextAlign = ContentAlignment.MiddleCenter;
                    Show_Power.Add(lb_RGV);
                    img_Map.Controls.Add(Show_Power[i]);
                    Show_Power[i].ContextMenuStrip = contextMenuStrip2;
                    Show_Power[i].MouseDoubleClick += new MouseEventHandler(Show_Power_dd_Click);
                }
            }
        }


        private void Show_Power_dd_Click(object sender, EventArgs e)
        {
            Label GET = (Label)sender;
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_Power Addelec = new frm_Add_Power(GET.Name, xOffset, yOffset - 25);
                Addelec.ShowDialog();
                RefShowPower();
            }
        }
        private void Show_Lift_dd_Click(object sender, EventArgs e)
        {
            Label GET = (Label)sender;
            var p = img_Map.Size;
            if(p.Width == map_width)
            {
                frm_Add_Elevator Addelec = new frm_Add_Elevator(GET.Name, xOffset, yOffset - 25);
                Addelec.ShowDialog();
                RefShowElevator();
            }
        }
        private void Show_CallBox_Click(object sender, EventArgs e)
        {
            Label GET = (Label)sender;
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_CallBox AddCallBox = new frm_Add_CallBox(GET.Text, xOffset, yOffset - 25);
                AddCallBox.ShowDialog();
                RefShowCallBox();
            }
        }


        //滚轮
        private void ShowRGVLocation()
        {
            var p = img_Map.Size;
            if (p.Width >= map_width && p.Height >= map_height)
            {
                for (int i = 0; i < RGV_Num; i++)
                {
                    if (RGV_S_X[i] != 0 && RGV_S_Y[i] != 0)
                    {
                        float Wth = (float)(((RGV_S_X[i] * p.Width) / map_width));
                        float Hht = (float)((RGV_S_Y[i] * p.Height / map_height));
                        if (p.Width != map_width)
                        {
                            ShowRGV[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)), Convert.ToInt32(Math.Round(Hht, 2)));
                        }
                        else
                        {
                            // ShowRGV[i].Location = new Point(RGV_S_X[i], RGV_S_Y[i]);
                        }
                    }
                    else
                    {
                        //float Wth = (float)(((20 + i * 40) * p.Width) / map_width);
                        //float Hht = (float)(740 * p.Height / map_height);
                        //ShowRGV[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)), Convert.ToInt32(Math.Round(Hht, 2)));
                        ShowRGV[i].Location = new Point(55, 20 + i * 40);

                    }
                }
                for (int i = 0; i < RFID_Num; i++)
                {
                    if (RFID_X[i] != 0 && RFID_Y[i] != 0)
                    {
                        float Wth = (float)(((RFID_X[i] * p.Width) / map_width));
                        float Hht = (float)((RFID_Y[i] * p.Height / map_height));
                        if (p.Width != map_width)
                        {
                            Map_Show_No_S[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)), Convert.ToInt32(Math.Round(Hht, 2)));
                        }
                        else
                        {
                            Map_Show_No_S[i].Location = new Point(RFID_X[i], RFID_Y[i]);
                        }

                    }
                    else
                    {
                        float Wth = (float)(((20 + i * 30) * p.Width) / map_width);
                        float Hht = (float)(50 * p.Height / map_height);
                        Map_Show_No_S[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)), Convert.ToInt32(Math.Round(Hht, 2)));
                    }
                }
                //CallBox
                if (CallBox_Map != null && CallBox_Map.Count > 0)
                {
                    for (int i = 0; i < CallBox_Map.Count; i++)
                    {
                        if (CallBox_Map[i].X != 0 && CallBox_Map[i].Y != 0)
                        {
                            float Wth = (float) (((CallBox_Map[i].X*p.Width)/map_width));
                            float Hht = (float) ((CallBox_Map[i].Y*p.Height/map_height));
                            if (p.Width != map_width)
                            {
                                Show_CallBox[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)),
                                    Convert.ToInt32(Math.Round(Hht, 2)));
                            }
                            else
                            {
                                Show_CallBox[i].Location = new Point(CallBox_Map[i].X, CallBox_Map[i].Y);
                            }

                        }
                        else
                        {
                            float Wth = (float) (((20 + i*30)*p.Width)/map_width);
                            float Hht = (float) (50*p.Height/map_height);
                            Show_CallBox[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)),
                                Convert.ToInt32(Math.Round(Hht, 2)));
                        }
                    }
                }
                //ele
                //for (int i = 0; i < Elevator_Map.Count; i++)
                //{
                //    if (Elevator_Map[i].X != 0 && Elevator_Map[i].Y != 0)
                //    {
                //        float Wth = (float)(((Elevator_Map[i].X * p.Width) / map_width));
                //        float Hht = (float)((Elevator_Map[i].Y * p.Height / map_height));
                //        if (p.Width != map_width)
                //        {
                //            Show_Elevator[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)), Convert.ToInt32(Math.Round(Hht, 2)));
                //        }
                //        else
                //        {
                //            Show_Elevator[i].Location = new Point(Elevator_Map[i].X, Elevator_Map[i].Y);
                //        }

                //    }
                //    else
                //    {
                //        float Wth = (float)(((20 + i * 30) * p.Width) / map_width);
                //        float Hht = (float)(50 * p.Height / map_height);
                //        Show_Elevator[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)), Convert.ToInt32(Math.Round(Hht, 2)));
                //    }
                //}
                //poewr
                if (Power_Map != null && Power_Map.Count > 0)
                {
                    for (int i = 0; i < Power_Map.Count; i++)
                    {
                        if (Power_Map[i].X != 0 && Power_Map[i].Y != 0)
                        {
                            float Wth = (float) (((Power_Map[i].X*p.Width)/map_width));
                            float Hht = (float) ((Power_Map[i].Y*p.Height/map_height));
                            if (p.Width != map_width)
                            {
                                Show_Power[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)),
                                    Convert.ToInt32(Math.Round(Hht, 2)));
                            }
                            else
                            {
                                Show_Power[i].Location = new Point(Power_Map[i].X, Power_Map[i].Y);
                            }

                        }
                        else
                        {
                            float Wth = (float) (((20 + i*30)*p.Width)/map_width);
                            float Hht = (float) (50*p.Height/map_height);
                            Show_Power[i].Location = new Point(Convert.ToInt32(Math.Round(Wth, 2)),
                                Convert.ToInt32(Math.Round(Hht, 2)));
                        }
                    }
                }
            }
        }
        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            mouswell = e.Delta;
            var p = img_Map.Size;
            float eldw = (float)(e.Delta * 0.85);
            float eldh = (float)(e.Delta * 0.5);
            p.Width += Convert.ToInt32(Math.Round(eldw, 2));
            p.Height += Convert.ToInt32(Math.Round(eldh, 2));
            if (p.Width >= map_width && p.Height >= map_height)
            {
                if (p.Width == map_width)
                {
                    img_Map.Location = new Point(0, 0);
                }
                img_Map.Size = p;
                ShowRGVLocation();
            }
        }
        private void img_Map_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveMap = true;
                MapLoction = img_Map.Location;
                Sta = MousePosition;
            }
            else if (e.Button == MouseButtons.Right)
            {
                xOffset = e.X;
                yOffset = e.Y;
                xToolStripMenuItem.Text = "当前坐标：(" + e.X.ToString() + "," + e.Y.ToString() + ")";
            }
        }
        private void img_Map_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveMap = false;
            }
        }
        private void img_Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MoveMap)
                    img_Map.Location = new Point(MapLoction.X + (MousePosition.X - Sta.X), MapLoction.Y + (MousePosition.Y - Sta.Y));
            }
        }

        //右击获取Name和Text
        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            string whichcontrol_name = (sender as ContextMenuStrip).SourceControl.Name;
            ctrl_Name = whichcontrol_name;
            ctrl_Text = (sender as ContextMenuStrip).SourceControl.Text;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ctrl_Name != "")
            {
                foreach (Control ctl in this.img_Map.Controls)
                {
                    if (ctl is Label)
                    {
                        if (ctl.Name == ctrl_Name)
                        {
                            int Rt = Function.DELETE_RFID_MAP(ctl.Text.Trim(), ctl.Location.X.ToString(), ctl.Location.Y.ToString());
                            Map_Show_No_S.Remove((Label)ctl);
                            this.img_Map.Controls.Remove(ctl);
                            RefShowRfid();
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefShowAGV();
            //RefShowElevator();
            RefShowPower();
            RefShowCallBox();
        }

        private void ts_AddELEC_Click(object sender, EventArgs e)
        {
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_Elevator Addelec = new frm_Add_Elevator("", xOffset, yOffset);
                Addelec.ShowDialog();
                RefShowElevator();
            }
            else
            {
                MessageBox.Show("请先将地图缩放至初始大小后，再增加电梯。", "提示");
            }
        }

        private void ts_AddAuto_Click(object sender, EventArgs e)
        {
            //var p = img_Map.Size;
            //if (p.Width == map_width)
            //{
            //    frm_Add_AutoDoor AddAuto = new frm_Add_AutoDoor("", xOffset, yOffset - 25);
            //    AddAuto.ShowDialog();
            //}
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_AutoDoor_New AddAuto = new frm_Add_AutoDoor_New();
                AddAuto.ShowDialog();
            }
        }

        private void ts_AddPower_Click(object sender, EventArgs e)
        {
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_Power AddPower = new frm_Add_Power("", xOffset, yOffset);
                AddPower.ShowDialog();
                RefShowPower();
            }
        }

        private void MI_RFIDAdd_Click(object sender, EventArgs e)
        {
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_RFID Addrfid = new frm_Add_RFID("", xOffset, yOffset);
                Addrfid.ShowDialog();
                RefShowRfid();
            }
            else
            {
                MessageBox.Show("请先将地图缩放至初始大小后，再增加地标。", "提示");
            }
        }

        private void 删除电梯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctrl_Name != "")
            {
                foreach (Control ctl in this.img_Map.Controls)
                {
                    if (ctl is Label)
                    {
                        if (ctl.Name == ctrl_Name)
                        {
                            int Rt = Function.DELETE_ELEVATOR_MAP(ctl.Name.Trim(), ctl.Location.X.ToString(), ctl.Location.Y.ToString());
                            Map_Show_No_S.Remove((Label)ctl);
                            this.img_Map.Controls.Remove(ctl);
                            RefShowElevator();
                        }
                    }
                }
            }
        }

        private void 增加呼叫盒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = img_Map.Size;
            if (p.Width == map_width)
            {
                frm_Add_CallBox AddCallBox = new frm_Add_CallBox("", xOffset, yOffset - 25);
                AddCallBox.ShowDialog();
                RefShowCallBox();
            }
        }

        private void 删除呼叫盒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctrl_Text != "")
            {
                foreach (Control ctl in this.img_Map.Controls)
                {
                    if (ctl is Label)
                    {
                        if (ctl.Text == ctrl_Text)
                        {
                            int Rt = Function.DELETE_CallBox_Map(ctl.Text.Trim().Substring(1, ctl.Text.Trim().Length - 1));
                            Map_Show_No_S.Remove((Label)ctl);
                            this.img_Map.Controls.Remove(ctl);
                            ShowCallBox();
                        }
                    }
                }
            }
            RefShowCallBox();
        }

        private void frm_map_Resize(object sender, EventArgs e)
        {
            //if (WindowState == FormWindowState.Minimized)
            //{
            //    this.Parent.Dock = DockStyle.Top;
            //}
            //if (WindowState == FormWindowState.Maximized)
            //{
            //    this.Parent.Dock = DockStyle.None;                
            //}
        }

        private void 删除充电站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctrl_Name != "")
            {
                foreach (Control ctl in this.img_Map.Controls)
                {
                    if (ctl is Label)
                    {
                        if (ctl.Name == ctrl_Name)
                        {
                            int Rt = Function.DELETE_POWER_MAP(ctl.Name.Trim(), ctl.Location.X.ToString(), ctl.Location.Y.ToString());
                            Map_Show_No_S.Remove((Label)ctl);
                            this.img_Map.Controls.Remove(ctl);
                            RefShowPower();
                        }
                    }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            RefShowAGV_flash();
            //RefShowElevator_flash();
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}
