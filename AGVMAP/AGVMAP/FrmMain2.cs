using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGVMAP.Dialog;
using AGVMAP.HelpClass;
using Canvas;
using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using Canvas.EditTools;
using Canvas.Layers;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using Model.MDM;
using Model.MSM;
using SimulationModel;
using Tools;

namespace AGVMAP
{
    public partial class FrmMain2 : BaseForm, ICanvasOwner, IEditToolOwner
    {
        private static string Path = Application.StartupPath + @"\AGV_Set.ini";

        private CanvasCtrller m_canvas;

        private DataModel m_data;

        private string m_filename = string.Empty;

        private bool IsSaveStore = false;

        private System.Windows.Forms.Timer FreshData_Timer = new System.Windows.Forms.Timer();

        private Simulator simula = null;

        private bool IsRunninSimula = false;

        private Color DefaultColor;

        public FrmMain2()
        {
            InitializeComponent();
            try
            {

                Global.path = Application.StartupPath + @"\AGV_Set.ini";
                UserLookAndFeel.Default.SetSkinStyle(FileControl.SetFileControl.ReadIniValue("STYLE", "Style", Path));
                string dataBase = FileControl.SetFileControl.ReadIniValue("DBSETUP", "DATABASE", Path);
                string server = FileControl.SetFileControl.ReadIniValue("DBSETUP", "SERVER", Path);
                string maxPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MaxPoolSize", Path);
                string minPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MinPoolSize", Path);
                string uid = FileControl.SetFileControl.ReadIniValue("DBSETUP", "UID", Path);
                string pwd = FileControl.SetFileControl.ReadIniValue("DBSETUP", "PWD", Path);
                SqlDBControl._defultConnectionString = string.Format(
                    "database={0};server={1};Max Pool Size={2};Min Pool Size={3};uid={4};pwd={5}", dataBase, server,
                    maxPool, minPool, uid, pwd);

                using (new WaitDialogForm("正在启动,请稍后...", "提示"))
                {
                    InitCanvas("", false);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow.Alert(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                DefaultColor = btnlandmark2.ItemAppearance.Hovered.ForeColor;
                DataTable dtArea = Function.GetDataInfo("PR_SELECT_AREA_INFO");
                luArea.Properties.DataSource = dtArea;

                FreshData_Timer.Interval = 2000;
                FreshData_Timer.Tick += new EventHandler(FreshData_Timer_Tick);

                new Thread(new ThreadStart(UpdateStock))
                {
                    IsBackground = true
                }.Start();

                //BackgroundLayer backgroundLayer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
                //GridLayer gridLayer = m_canvas.Model.GridLayer as GridLayer;
                //DrawingLayer drawingLayer = m_canvas.Model.ActiveLayer as DrawingLayer;
                //if (backgroundLayer != null && gridLayer != null && drawingLayer != null)
                //{
                //    gridLayer.Enabled =
                //        bool.Parse(FileControl.SetFileControl.ReadIniValue("OPTION", "UseCoord", Global.path));
                //    string style = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordType", Global.path);
                //    gridLayer.GridStyle = style.ToLower() == "lines" ? GridLayer.eStyle.Lines : GridLayer.eStyle.Dots;
                //    string color = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordColor", Global.path);
                //    string[] colorArray = color.Split(',');
                //    gridLayer.Color = Color.FromArgb(int.Parse(colorArray[0]), int.Parse(colorArray[1]),
                //        int.Parse(colorArray[2]), int.Parse(colorArray[3]));
                //    color = FileControl.SetFileControl.ReadIniValue("OPTION", "BgColorB", Global.path);
                //    colorArray = color.Split(',');
                //    backgroundLayer.Color = Color.FromArgb(int.Parse(colorArray[0]), int.Parse(colorArray[1]),
                //        int.Parse(colorArray[2]), int.Parse(colorArray[3]));
                //    color = FileControl.SetFileControl.ReadIniValue("OPTION", "PenColor", Global.path);
                //    colorArray = color.Split(',');
                //    drawingLayer.Color = Color.FromArgb(int.Parse(colorArray[0]), int.Parse(colorArray[1]),
                //        int.Parse(colorArray[2]), int.Parse(colorArray[3]));
                //    drawingLayer.Width =
                //        float.Parse(FileControl.SetFileControl.ReadIniValue("OPTION", "PenSize", Global.path));
                //    m_canvas.DoInvalidate(true);
                //}
            }
            catch (Exception ex)
            {
                MessageBoxShow.Alert(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void FreshData_Timer_Tick(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(UpdateStock))
            {
                IsBackground = true
            }.Start();
            //new Thread(new ThreadStart(UpdateCar))
            //{
            //    IsBackground = true
            //}.Start();
            ClearMemory();
        }

        private void FrmMain2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBoxShow.Alert("确定退出当前系统?", MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                FileControl.SetFileControl.WriteIniValue("STYLE", "Style", UserLookAndFeel.Default.ActiveSkinName, Global.path);
            }
        }

        #region Menu
        private void btnOpenMap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Cad XML files (*.agv)|*.agv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                InitCanvas(openFileDialog.FileName, true);
            }
        }

        private void btnNewMap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InitCanvas("", true);
        }

        private void btnCoorCompare_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmCoorCompare frm = new FrmCoorCompare())
            {
                frm.ShowDialog();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBoxShow.Alert("确认保存当前地图?", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IList<IDrawObject> list = (from p in m_data.ActiveLayer.Objects
                                           where p.Id == "StorageTool"
                                           select p).ToList();
                if (list.Count > 0 && MessageBoxShow.Alert("是否保存更新储位信息?", MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    IsSaveStore = true;
                }
                else
                {
                    IsSaveStore = false;
                }
                Save();
            }
        }

        private void btnSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveAs();
        }

        private void btnSaveTask_ItemClick(object sender, ItemClickEventArgs e)
        {
            LandMarkTool landMarkTool = m_data.SelectedObjects.FirstOrDefault() as LandMarkTool;
            if (landMarkTool == null)
            {
                MessageBoxShow.Alert("请先选择地标", MessageBoxIcon.Exclamation);
                return;
            }
            Function.Insert_tbOrder(landMarkTool.LandCode);
            MessageBoxShow.Alert("保存任务成功", MessageBoxIcon.Asterisk);
        }

        private void btnQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBoxShow.Alert("确定退出当前系统?", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.ExitThread();
                //Application.Exit();
                this.Close();
                this.Dispose();
            }
        }

        private void btnExchangeInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmDbSetup frm = new FrmDbSetup())
            {
                frm.ShowDialog();
            }
        }

        private void btnAgvSetUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAgvInfo frm = new FrmAgvInfo())
            {
                frm.ShowDialog();
            }
        }

        private void btnSysInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmSysPara frm = new FrmSysPara())
            {
                frm.ShowDialog();
            }
        }

        private void btnCallBox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmCallBoxInfo frm = new FrmCallBoxInfo())
            {
                frm.ShowDialog();
            }
        }

        private void btnTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmTaskSet frm = new FrmTaskSet())
            {
                frm.ShowDialog();
            }
        }

        private void btnArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAreaInfo frm = new FrmAreaInfo())
            {
                frm.ShowDialog();
            }
        }

        private void btnAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAction frm = new FrmAction())
            {
                frm.ShowDialog();
            }
        }

        private void btnMaterial_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmMaterial frm = new FrmMaterial())
            {
                frm.ShowDialog();
            }
        }

        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (new WaitDialogForm("正在启动,请稍后...", "提示"))
            {
                m_canvas.IsChooseSpecial = true;
                FreshData_Timer.Enabled = true;
                dockPanel1.Enabled = false;
                ChangeEnable(false);
                dockPanel2.Enabled = false;
                barSubItem1.Enabled = false;
                barSubItem5.Enabled = false;
                barSubItem6.Enabled = false;
                barSubItem7.Enabled = false;
                IniMoni();
                IsRunninSimula = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private void btnStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (new WaitDialogForm("正在停止,请稍后...", "提示"))
            {
                FreshData_Timer.Enabled = false;
                m_canvas.IsChooseSpecial = false;
                ChangeEnable(true);
                dockPanel1.Enabled = true;
                dockPanel2.Enabled = true;
                barSubItem1.Enabled = true;
                barSubItem5.Enabled = true;
                barSubItem6.Enabled = true;
                barSubItem7.Enabled = true;
                simula.StopSimula();
                IsRunninSimula = false;

                IEnumerable<IDrawObject> enumerable = from p in m_data.ActiveLayer.Objects
                                                      where p.Id == "AGVTool"
                                                      select p;

                for (int i = enumerable.Count(); i > 0; i = enumerable.Count())
                {
                    m_data.DeleteObjects(enumerable);
                    m_canvas.DoInvalidate(true);
                    enumerable = from p in m_data.ActiveLayer.Objects
                                 where p.Id == "AGVTool"
                                 select p;
                }

                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void btnOption_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BackgroundLayer backgroundLayer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
            GridLayer gridLayer = m_canvas.Model.GridLayer as GridLayer;
            DrawingLayer drawingLayer = m_canvas.Model.ActiveLayer as DrawingLayer;
            if (backgroundLayer != null && gridLayer != null && drawingLayer != null)
            {
                using (
                    FrmOption frmOption = new FrmOption(gridLayer.Enabled, gridLayer.GridStyle, gridLayer.Color,
                        backgroundLayer.Color, drawingLayer.Color, drawingLayer.Width))
                {
                    if (frmOption.ShowDialog() == DialogResult.OK)
                    {
                        gridLayer.Enabled = frmOption.GridEnable;
                        gridLayer.GridStyle = frmOption.GridStyle;
                        gridLayer.Color = frmOption.GridColor;
                        backgroundLayer.Color = frmOption.BackGroudColor;
                        drawingLayer.Color = frmOption.PenColor;
                        drawingLayer.Width = frmOption.PenWidth;
                        m_canvas.DoInvalidate(true);
                    }
                }
            }
        }
        #endregion

        private void Property_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            m_canvas.Focus();
            UpdateLayerUI();
        }

        private void Property_CellValueChanging(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            //if (e.Row.Properties.Caption == "MarkValue")
            //{
            //    int num = (from p in m_data.ActiveLayer.Objects
            //               where p.Id == "LandMark" && (p as LandMarkTool).LandCode == e.Value.ToString()
            //               select p).Count<IDrawObject>();
            //    if (num > 0)
            //    {
            //        lblMaxLand.Caption = "地标号:" + e.Value + "已存在!";
            //    }
            //    else
            //    {
            //        lblMaxLand.Caption = "";
            //    }
            //}
        }

        private void luArea_EditValueChanged(object sender, EventArgs e)
        {
            if (m_data.SelectedObjects.Any())
            {
                StorageTool storageTool = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                if (storageTool != null)
                {
                    LookUpEdit lookUpEdit = sender as LookUpEdit;
                    if (lookUpEdit != null)
                    {
                        int ownArea = int.Parse(lookUpEdit.EditValue.ToString());
                        storageTool.OwnArea = ownArea;
                    }
                }
            }
        }

        private void OnToolSelect(object sender, NavBarLinkEventArgs e)
        {
            string text = string.Empty;
            bool handleImmediately = false;
            if (sender is NavBarItem)
            {
                text = ((NavBarItem)sender).Tag.ToString();
                handleImmediately = true;
            }
            if (text == string.Empty)
            {
                return;
            }
            if (text == "select")
            {
                m_canvas.CommandEscape(true);
            }
            else if (text == "pan")
            {
                m_canvas.CommandPan();
            }
            else if (text == "move")
            {
                m_canvas.CommandMove(handleImmediately);
            }
            else
            {
                m_canvas.CommandSelectDrawTool(text);
            }
        }

        private void OnToolSelect2(object sender, ItemClickEventArgs e)
        {
            string text = string.Empty;
            bool handleImmediately = false;
            if (e.Item is BarLargeButtonItem)
            {
                text = e.Item.Tag.ToString();
                if (text == "move" && !m_data.SelectedObjects.Any())
                {
                    MessageBoxShow.Alert("请选择移动对象", MessageBoxIcon.Exclamation);
                    return;
                }
                handleImmediately = true;
                Color currentColor = e.Item.ItemAppearance.Hovered.ForeColor;
                if (currentColor != Color.Blue)
                {
                    DevExpress.XtraBars.Controls.CustomBarControl barControl = bar1.GetBarControl();
                    BarLargeButtonItemLink barLargeButtonItem;
                    foreach (var c in barControl.VisibleLinks)
                    {
                        barLargeButtonItem = c as BarLargeButtonItemLink;
                        if (barLargeButtonItem != null)
                        {
                            barLargeButtonItem.Item.ItemAppearance.Hovered.ForeColor =
                                barLargeButtonItem.Item.ItemAppearance.Normal.ForeColor = currentColor;
                        }
                    }

                    e.Item.ItemAppearance.Hovered.ForeColor = e.Item.ItemAppearance.Normal.ForeColor = Color.Blue;
                }
            }
            if (text == string.Empty)
            {
                return;
            }
            if (text == "select")
            {
                m_canvas.CommandEscape(true);
            }
            else if (text == "pan")
            {
                m_canvas.CommandPan();
            }
            else if (text == "move")
            {
                m_canvas.CommandMove(handleImmediately);
            }
            else
            {
                m_canvas.CommandSelectDrawTool(text);
            }
        }

        private void OnEditToolSelect(object sender, NavBarLinkEventArgs e)
        {
            string editid = string.Empty;
            if (sender is NavBarItem)
            {
                editid = ((NavBarItem)sender).Tag.ToString();
            }
            m_canvas.CommandEdit(editid);
        }

        private void OnEditToolSelect2(object sender, ItemClickEventArgs e)
        {
            string editid = string.Empty;
            if (e.Item is BarLargeButtonItem)
            {
                editid = e.Item.Tag.ToString();
                Color currentColor = e.Item.ItemAppearance.Hovered.ForeColor;
                if (currentColor != Color.Blue)
                {
                    DevExpress.XtraBars.Controls.CustomBarControl barControl = bar1.GetBarControl();
                    BarLargeButtonItemLink barLargeButtonItem;
                    foreach (var c in barControl.VisibleLinks)
                    {
                        barLargeButtonItem = c as BarLargeButtonItemLink;
                        if (barLargeButtonItem != null)
                        {
                            barLargeButtonItem.Item.ItemAppearance.Hovered.ForeColor =
                                barLargeButtonItem.Item.ItemAppearance.Normal.ForeColor = currentColor;
                        }
                    }

                    e.Item.ItemAppearance.Hovered.ForeColor = e.Item.ItemAppearance.Normal.ForeColor = Color.Blue;
                }
            }
            m_canvas.CommandEdit(editid);
        }

        private void OnRedo(object sender, EventArgs e)
        {
            if (m_data.DoRedo())
            {
                m_canvas.DoInvalidate(true);
            }
        }

        private void OnUndo(object sender, EventArgs e)
        {
            if (m_data.DoUndo())
            {
                m_canvas.DoInvalidate(true);
            }
        }

        private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_data.SelectedObjects.Count() == 1)
            {
                IDrawObject drawObject = m_data.SelectedObjects.FirstOrDefault();
                if (drawObject != null && drawObject.Id == "ButtonTool" && IsRunninSimula)
                {
                    using (FrmSimulationCallBox frmSimulationCallBox = new FrmSimulationCallBox((drawObject as ButtonTool).CallBoxID, simula))
                    {
                        frmSimulationCallBox.ShowDialog();
                    }
                }
            }
        }

        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        {
            luArea.EditValueChanged -= new EventHandler(luArea_EditValueChanged);
            try
            {
                Property.SelectedObject = null;
                Property.UpdateRows();
                if (m_data.SelectedCount > 0)
                {
                    Property.SelectedObject = m_data.SelectedObjects.FirstOrDefault();
                    StorageTool storageTool = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                    if (storageTool != null)
                    {
                        luArea.EditValue = storageTool.OwnArea;
                        pnlChoose.Visible = true;
                    }
                    else
                    {
                        pnlChoose.Visible = false;
                    }
                }
                else
                {
                    pnlChoose.Visible = false;
                }
                UpdateLayerUI();
                List<IDrawObject> list = (from p in m_data.ActiveLayer.Objects
                                          where p.Id == "LandMark" || p.Id == "AGVTool"
                                          select p).ToList();
                m_data.DeleteObjects(list);
                List<IDrawObject> list2 = list.Where(p => p.Id == "LandMark").ToList();

                foreach (IDrawObject current in list2)
                {
                    m_data.AddObject(m_data.ActiveLayer, current);
                }
                List<IDrawObject> list3 = list.Where(p => p.Id == "AGVTool").ToList();
                foreach (IDrawObject current2 in list3)
                {
                    m_data.AddObject(m_data.ActiveLayer, current2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                luArea.EditValueChanged += new EventHandler(luArea_EditValueChanged);
            }
        }

        private void OnCanvasKeyDown(object sender, KeyEventArgs e)
        {
        }

        #region Function
        public void SetPositionInfo(UnitPoint unitpos)
        {
        }

        public void SetSnapInfo(ISnapPoint snap)
        {
        }

        public void SetHint(string text)
        {
            lblCoorateInfo.Caption = text;
        }

        private void InitCanvas(string filename, bool IsNew)
        {
            if (!IsNew)
            {
                DataTable dtPlanSet = Function.GetDataInfo("PR_SELECT_TBPLANSET");
                DataRow[] dataRows = dtPlanSet.Select("[fileName]='temSet.agv'");
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temSet.agv";

                if (dataRows.Length > 0)
                {
                    byte[] array = (byte[])dataRows[0]["FileContent"];
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                        binaryWriter.Write(array, 0, array.Length);
                        binaryWriter.Close();
                    }
                }
                if (File.Exists(path))
                {
                    filename = path;
                }
            }

            m_data = new DataModel();
            if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename))
            {
                m_filename = filename;
            }
            m_canvas = new CanvasCtrller(this, m_data);
            m_canvas.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(m_canvas);
            m_canvas.SetCenter(new UnitPoint(0.0, 0.0));
            m_canvas.RunningSnaps = new Type[]
                {
                    typeof(VertextSnapPoint),
                    typeof(MidpointSnapPoint),
                    typeof(IntersectSnapPoint),
                    typeof(QuadrantSnapPoint),
                    typeof(CenterSnapPoint),
                    typeof(DivisionSnapPoint)
                };
            m_canvas.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
            m_canvas.MouseUp += new MouseEventHandler(OnCanvasMouseUp);
            m_canvas.MouseDoubleClick += new MouseEventHandler(OnCanvasMouseDoubleClick);
            m_canvas.m_commandTypeChange += new EventHandler(m_canvas_m_commandTypeChange);
            SetupDrawTools();
            SetupEditTools();
            UpdateLayerUI();
        }

        void m_canvas_m_commandTypeChange(object sender, EventArgs e)
        {
            UpdateCurrentOper();
        }

        private void SetupDrawTools()
        {
            btnLine.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnLine.Tag = "Line";
            m_data.AddDrawTool(btnLine.Tag.ToString(), new LineTool(LineType.Line));
            btnPointLine.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnPointLine.Tag = "PointLine";
            m_data.AddDrawTool(btnPointLine.Tag.ToString(), new LineTool(LineType.PointLine));
            btnImg.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnImg.Tag = "imgTool";
            m_data.AddDrawTool(btnImg.Tag.ToString(), new ImgeTool());
            btntxt.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btntxt.Tag = "txtTool";
            m_data.AddDrawTool(btntxt.Tag.ToString(), new TextTool());
            btnlandmark.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnlandmark.Tag = "LandMark";
            m_data.AddDrawTool(btnlandmark.Tag.ToString(), new LandMarkTool());
            btnBesize.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnBesize.Tag = "BezierTool";
            m_data.AddDrawTool(btnBesize.Tag.ToString(), new BezierTool(BezierType.Bezier));
            btnPointBesize.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnPointBesize.Tag = "PointBezier";
            m_data.AddDrawTool(btnPointBesize.Tag.ToString(), new BezierTool(BezierType.PointBezier));
            btnStock.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnStock.Tag = "StorageTool";
            m_data.AddDrawTool(btnStock.Tag.ToString(), new StorageTool());
            btnCallBox.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnCallBox.Tag = "ButtonTool";
            m_data.AddDrawTool(btnCallBox.Tag.ToString(), new ButtonTool());
            btnselect.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnselect.Tag = "select";
            btnHand.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnHand.Tag = "pan";
            btnMove.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnMove.Tag = "move";

            btnLine2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnLine2.Tag = "Line";
            m_data.AddDrawTool(btnLine2.Tag.ToString(), new LineTool(LineType.Line));
            btnPointLine2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnPointLine2.Tag = "PointLine";
            m_data.AddDrawTool(btnPointLine2.Tag.ToString(), new LineTool(LineType.PointLine));
            btnImg2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnImg2.Tag = "imgTool";
            m_data.AddDrawTool(btnImg2.Tag.ToString(), new ImgeTool());
            btntxt2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btntxt2.Tag = "txtTool";
            m_data.AddDrawTool(btntxt2.Tag.ToString(), new TextTool());
            btnlandmark2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnlandmark2.Tag = "LandMark";
            m_data.AddDrawTool(btnlandmark2.Tag.ToString(), new LandMarkTool());
            btnBesize2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnBesize2.Tag = "BezierTool";
            m_data.AddDrawTool(btnBesize2.Tag.ToString(), new BezierTool(BezierType.Bezier));
            btnPointBesize2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnPointBesize2.Tag = "PointBezier";
            m_data.AddDrawTool(btnPointBesize2.Tag.ToString(), new BezierTool(BezierType.PointBezier));
            btnStock2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnStock2.Tag = "StorageTool";
            m_data.AddDrawTool(btnStock2.Tag.ToString(), new StorageTool());
            btnCallBox2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnCallBox2.Tag = "ButtonTool";
            m_data.AddDrawTool(btnCallBox2.Tag.ToString(), new ButtonTool());
            btnselect2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnselect2.Tag = "select";
            btnHand2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnHand2.Tag = "pan";
            btnMove2.ItemClick += new ItemClickEventHandler(OnToolSelect2);
            btnMove2.Tag = "move";
        }

        private void SetupEditTools()
        {
            m_data.AddEditTool("meet2lines", new LinesMeetEditTool(this));
            m_data.AddEditTool("shrinkextend", new LineShrinkExtendEditTool(this));
        }

        private void UpdateLayerUI()
        {
            string str = "1";
            if (m_data.ActiveLayer.Objects.Any())
            {
                if ((from p in m_data.ActiveLayer.Objects
                     where p.Id == "LandMark"
                     select p).Any())
                {
                    IEnumerable<IDrawObject> objects =
                    from p in m_data.ActiveLayer.Objects
                    where p.Id == "LandMark"
                    select p;
                    str = objects.Max(p => Convert.ToDecimal((p as LandMarkTool).LandCode)).ToString();
                }
            }
            lblCoorateInfo.Caption = "当前最大地标号:" + str;
            m_canvas.DoInvalidate(true);
        }

        private void Save()
        {
            try
            {
                UpdateData();
                if (m_filename.Length == 0)
                {
                    SaveAs();
                }
                else
                {
                    m_filename = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temSet.agv";
                    m_data.Save(m_filename);
                }
                if (!string.IsNullOrEmpty(m_filename))
                {
                    OperateReturnInfo msg;
                    using (new WaitDialogForm("正在保存,请稍后...", "提示"))
                    {
                        string fileName = "";
                        if (m_filename.Contains("\\"))
                        {
                            fileName = m_filename.Substring(m_filename.LastIndexOf('\\')).Replace("\\", "");
                        }
                        else
                        {
                            fileName = m_filename;
                        }
                        IList<IDrawObject> list = (from p in m_data.ActiveLayer.Objects
                                                   where p.Id == "LandMark"
                                                   select p).ToList();
                        IList<LandmarkInfo> list2 = new List<LandmarkInfo>();
                        foreach (IDrawObject current in list)
                        {
                            LandMarkTool landMarkTool = current as LandMarkTool;
                            if (landMarkTool != null)
                            {
                                list2.Add(new LandmarkInfo
                                {
                                    LandX = ((float)landMarkTool.Location.X),
                                    LandY = ((float)landMarkTool.Location.Y),
                                    LandMidX = ((float)landMarkTool.MidPoint.X),
                                    LandMidY = ((float)landMarkTool.MidPoint.Y),
                                    LandmarkCode = landMarkTool.LandCode,
                                    LandmarkName = landMarkTool.LandName,
                                    IsWork =(int) landMarkTool.IsWorkStation,
                                    WorkDirect =(int) landMarkTool.WorkDirect,
                                    ActionDirect =(int) landMarkTool.ActionDirect
                                });
                            }
                        }
                        List<IGrouping<string, LandmarkInfo>> list3 = (from p in list2
                                                                       group p by p.LandmarkCode
                                                                           into p
                                                                           where p.Count() > 1
                                                                           select p).ToList();
                        if (list3.Count > 0)
                        {
                            string text = "";
                            foreach (IGrouping<string, LandmarkInfo> current2 in list3)
                            {
                                text = text + current2.Key + ",";
                                if (text.Length > 20)
                                {
                                    text += "\r\n";
                                }
                            }
                            if (!string.IsNullOrEmpty(text))
                            {
                                MessageBoxShow.Alert("有重复地标:" + text, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        IList<Model.MDM.StorageInfo> list4 = new List<Model.MDM.StorageInfo>();
                        bool isSaveStore = IsSaveStore;
                        if (isSaveStore)
                        {
                            IList<IDrawObject> list5 = (from p in m_data.ActiveLayer.Objects
                                                        where p.Id == "StorageTool"
                                                        select p).ToList();
                            foreach (IDrawObject current3 in list5)
                            {
                                StorageTool storageTool = current3 as StorageTool;
                                if (storageTool != null)
                                {
                                    list4.Add(new Model.MDM.StorageInfo
                                    {
                                        ID = storageTool.StcokID,
                                        StorageName = storageTool.StorageName,
                                        OwnArea = storageTool.OwnArea,
                                        SubOwnArea = storageTool.SubOwnArea,
                                        matterType = storageTool.matterType,
                                        MaterielType = storageTool.MaterielType,
                                        LankMarkCode = storageTool.LankMarkCode,
                                        StorageState = storageTool.StorageState
                                    });
                                }
                            }
                        }
                        IList<AllSegment> list6 = new List<AllSegment>();
                        if (list2.Count > 0)
                        {
                            IList<IDrawObject> list7 = (from p in m_data.ActiveLayer.Objects
                                                        where p.Id == "LineTool" || p.Id == "BezierTool"
                                                        select p).ToList();
                            foreach (IDrawObject current4 in list7)
                            {
                                if (current4 is LineTool)
                                {
                                    LineTool line = current4 as LineTool;
                                    if (line.Type != LineType.Line)
                                    {
                                        LandmarkInfo BegLand = (from p in list2
                                                                where Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P1.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P1.Y, 1, MidpointRounding.AwayFromZero)
                                                                select p).FirstOrDefault<LandmarkInfo>();
                                        LandmarkInfo EndLand = (from p in list2
                                                                where Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P2.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P2.Y, 1, MidpointRounding.AwayFromZero)
                                                                select p).FirstOrDefault<LandmarkInfo>();
                                        if (BegLand != null && EndLand != null)
                                        {
                                            if (!(from p in list6
                                                  where p.BeginLandMakCode == BegLand.LandmarkCode && p.EndLandMarkCode == EndLand.LandmarkCode
                                                  select p).Any())
                                            {
                                                list6.Add(new AllSegment
                                                {
                                                    BeginLandMakCode = BegLand.LandmarkCode,
                                                    EndLandMarkCode = EndLand.LandmarkCode,
                                                    BeginLandMak = BegLand,
                                                    EndLandMark = EndLand,
                                                    Length = line.Lenth,
                                                    ExcuteAngle = line.ExcuteAngle,
                                                    Go_Direction = EndLand.IsWork==1?EndLand.WorkDirect:-1,
                                                    ActionDirect = EndLand.IsWork == 1 ? EndLand.ActionDirect : -1,
                                                });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (current4 is BezierTool)
                                    {
                                        BezierTool Bezier = current4 as BezierTool;
                                        if (Bezier.Type != BezierType.Bezier)
                                        {
                                            LandmarkInfo BegLand = (from p in list2
                                                                    where Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P1.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P1.Y, 1, MidpointRounding.AwayFromZero)
                                                                    select p).FirstOrDefault<LandmarkInfo>();
                                            LandmarkInfo EndLand = (from p in list2
                                                                    where Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P2.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P2.Y, 1, MidpointRounding.AwayFromZero)
                                                                    select p).FirstOrDefault<LandmarkInfo>();
                                            if (BegLand != null && EndLand != null)
                                            {
                                                if (!(from p in list6
                                                      where p.BeginLandMakCode == BegLand.LandmarkCode && p.EndLandMarkCode == EndLand.LandmarkCode
                                                      select p).Any())
                                                {
                                                    list6.Add(new AllSegment
                                                    {
                                                        BeginLandMakCode = BegLand.LandmarkCode,
                                                        EndLandMarkCode = EndLand.LandmarkCode,
                                                        BeginLandMak = BegLand,
                                                        EndLandMark = EndLand,
                                                        Length = Bezier.Lenth,
                                                        Go_Direction = EndLand.IsWork == 1 ? EndLand.WorkDirect : -1,
                                                        ActionDirect = EndLand.IsWork == 1 ? EndLand.ActionDirect : -1,
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            DataTable dtPara = Function.GetParByCondition("IsImportLenth");
                            if (dtPara != null && dtPara.Rows.Count > 0 && dtPara.Rows[0]["ParameterValue"].ToString() == "是" && list6.Count > 0)
                            {
                                if ((from p in list6 where p.Length == 0 select p).Any())
                                {
                                    MessageBoxShow.Alert("存在路径长度没维护!", MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }
                        }

                        msg = Function.SaveMap(m_filename, fileName, m_data.Zoom, list2, list4, list6);

                    }
                    if (msg.ReturnCode == OperateCodeEnum.Success)
                    {
                        MessageBoxShow.Alert("操作成功!", MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBoxShow.Alert(msg.ReturnInfo.ToString(), MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow.Alert(ex.Message + ex.StackTrace, MessageBoxIcon.Error);
            }
        }

        private void SaveAs()
        {
            UpdateData();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Cad XML files (*.agv)|*.agv";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = "temSet.agv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                m_filename = saveFileDialog.FileName;
                m_data.Save(m_filename);
            }
        }

        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        private void IniMoni()
        {
            simula = new Simulator();
            simula.Car_Move += new Simulator.CarMove(Simula_Car_Move);
            simula.Car_Ini += new Simulator.CarIni(Simula_Car_Ini);
            simula.Inital();
            m_canvas.DoInvalidate(true);
            lblInflection.Caption = "";
        }

        private void UpdateData()
        {
            m_data.CenterPoint = m_canvas.GetCenter();
        }

        private void UpdateStock()
        {
            DataTable dtLocation = Function.GetDataInfo("P_SELECT_TBLOCATION");
            IList<Model.MDM.StorageInfo> list = DataToObject.TableToEntity<Model.MDM.StorageInfo>(dtLocation);
            if (list != null)
            {
                StorageTool storageTool;
                foreach (Model.MDM.StorageInfo item in list)
                {
                    storageTool = (from p in m_data.ActiveLayer.Objects
                                   where p.Id == "StorageTool" && (p as StorageTool).StcokID == item.ID
                                   select p).FirstOrDefault<IDrawObject>() as StorageTool;
                    if (storageTool != null)
                    {
                        storageTool.OwnArea = item.OwnArea;
                        storageTool.SubOwnArea = item.SubOwnArea;
                        storageTool.matterType = item.matterType;
                        storageTool.StorageState = item.StorageState;
                        storageTool.LockState = item.LockState;
                        storageTool.MaterielType = item.MaterielType;
                    }
                }
            }
            DataTable dtSegment = Function.GetDataInfo("P_SELECT_TBALLSEGMENT");
            IList<AllSegment> list2 = DataToObject.TableToEntity<AllSegment>(dtSegment);
            if (list2 != null)
            {
                IDrawObject LandDraw1, LandDraw2;
                IList<IDrawObject> source;
                IDrawObject drawObject;
                foreach (AllSegment item in list2)
                {
                    LandDraw1 = (from p in m_data.ActiveLayer.Objects
                                 where p.Id == "LandMark" && (p as LandMarkTool).LandCode == item.BeginLandMakCode
                                 select p).FirstOrDefault<IDrawObject>();
                    LandDraw2 = (from p in m_data.ActiveLayer.Objects
                                 where p.Id == "LandMark" && (p as LandMarkTool).LandCode == item.EndLandMarkCode
                                 select p).FirstOrDefault<IDrawObject>();
                    if (LandDraw1 != null && LandDraw2 != null)
                    {
                        source = (from p in m_data.ActiveLayer.Objects
                                  where p.Id == "line" && (p as LineTool).Type == LineType.PointLine
                                  select p).ToList();
                        drawObject = (from q in source
                                      where Math.Abs(Math.Round((q as LineTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((q as LineTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((q as LineTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((q as LineTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02
                                      select q).FirstOrDefault<IDrawObject>();
                        if (drawObject != null)
                        {
                            (drawObject as LineTool).Lenth = item.Length;
                        }

                        source = (from p in m_data.ActiveLayer.Objects
                                  where p.Id == "BezierTool" && (p as BezierTool).Type == BezierType.PointBezier
                                  select p).ToList();
                        drawObject = (from q in source
                                      where Math.Abs(Math.Round((q as BezierTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((q as BezierTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((q as BezierTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((q as BezierTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02
                                      select q).FirstOrDefault<IDrawObject>();
                        if (drawObject != null)
                        {
                            (drawObject as BezierTool).Lenth = item.Length;
                        }
                    }
                }
            }
            m_canvas.DoInvalidate(true);
        }

        private void Simula_Car_Ini(object obj)
        {
            double num = 0.0;
            DataTable dtPara = Function.GetParByCondition("ScalingRate");
            if (dtPara != null && dtPara.Rows.Count > 0)
            {
                try
                {
                    num = Convert.ToDouble(dtPara.Rows[0]["ParameterValue"]);
                }
                catch
                {
                    return;
                }
            }
            if (num > 0.0)
            {
                IEnumerable<IDrawObject> objects = from p in m_data.ActiveLayer.Objects
                                                   where p.Id == "AGVTool"
                                                   select p;
                m_data.DeleteObjects(objects);
                List<CarMonitor> list = obj as List<CarMonitor>;
                if (list != null)
                {
                    foreach (CarMonitor current in list)
                    {
                        AGVTool aGVTool = new AGVTool();
                        aGVTool.Agv_id = current.AgvID.ToString();
                        aGVTool.Position = new UnitPoint((double)current.X / num, (double)current.Y / num);
                        m_data.AddObject(m_data.ActiveLayer, aGVTool);
                        m_canvas.DoInvalidate(true);
                    }
                }
            }
        }

        private void Simula_Car_Move(object obj)
        {
            if (obj != null)
            {
                double num = 0.0;
                DataTable dtPara = Function.GetParByCondition("ScalingRate");
                if (dtPara != null && dtPara.Rows.Count > 0)
                {
                    try
                    {
                        num = Convert.ToDouble(dtPara.Rows[0]["ParameterValue"]);
                    }
                    catch
                    {
                        return;
                    }
                }

                if (num > 0.0)
                {
                    CarMonitor car = obj as CarMonitor;
                    IEnumerable<IDrawObject> source = from a in m_data.ActiveLayer.Objects
                                                      where a.Id == "AGVTool" && (a as AGVTool).Agv_id == car.AgvID.ToString()
                                                      select a;
                    if (!source.Any())
                    {
                        AGVTool aGVTool = new AGVTool();
                        aGVTool.Agv_id = car.AgvID.ToString();
                        aGVTool.Position = new UnitPoint((double)car.X / num, (double)car.Y / num);
                        m_data.AddObject(m_data.ActiveLayer, aGVTool);
                        m_canvas.DoInvalidate(true);
                    }
                    else
                    {
                        AGVTool aGVTool2 = source.FirstOrDefault() as AGVTool;
                        if (aGVTool2 != null)
                        {
                            aGVTool2.Position = new UnitPoint((double)car.X / num, (double)car.Y / num);
                            lblInflection.Caption = "";
                            if (car.CurrLand.sway == SwayEnum.Left)
                            {
                                lblInflection.Caption = "左转";
                            }
                            else
                            {
                                if (car.CurrLand.sway == SwayEnum.Right)
                                {
                                    lblInflection.Caption = "右转";
                                }
                                else
                                {
                                    if (car.CurrLand.sway == SwayEnum.ExcuteLand)
                                    {
                                        lblInflection.Caption = "强制地标";
                                    }
                                }
                            }
                            if (car.CurrLand.sway > SwayEnum.None)
                            {
                                BarStaticItem barStaticItem = lblInflection;
                                barStaticItem.Caption += car.CurrLand.Angle.ToString();
                            }
                            m_canvas.DoInvalidate(true);
                        }
                    }
                }
            }
        }

        private void ChangeEnable(bool enable)
        {
            DevExpress.XtraBars.Controls.CustomBarControl barControl = bar1.GetBarControl();
            BarLargeButtonItemLink barLargeButtonItem;
            foreach (var c in barControl.VisibleLinks)
            {
                barLargeButtonItem = c as BarLargeButtonItemLink;
                if (barLargeButtonItem != null)
                {
                    barLargeButtonItem.Item.Enabled = enable;
                }
            }
        }

        private void UpdateCar()
        {
            if (simula != null)
            {
                simula.Inital();
                m_canvas.DoInvalidate(true);
            }
        }

        private void UpdateCurrentOper()
        {
            if (m_canvas.m_commandType == eCommandType.select || m_canvas.m_commandType == eCommandType.pan ||
                m_canvas.m_commandType == eCommandType.move)
            {
                DevExpress.XtraBars.Controls.CustomBarControl barControl = bar1.GetBarControl();
                BarLargeButtonItemLink barLargeButtonItem;
                foreach (var c in barControl.VisibleLinks)
                {
                    barLargeButtonItem = c as BarLargeButtonItemLink;
                    if (barLargeButtonItem != null)
                    {
                        barLargeButtonItem.Item.ItemAppearance.Hovered.ForeColor =
                            barLargeButtonItem.Item.ItemAppearance.Normal.ForeColor = DefaultColor;
                    }
                }
                switch (m_canvas.m_commandType)
                {
                    case eCommandType.select:
                        btnselect2.ItemAppearance.Hovered.ForeColor =
                            btnselect2.ItemAppearance.Normal.ForeColor = Color.Blue;
                        break;
                    case eCommandType.pan:
                        btnHand2.ItemAppearance.Hovered.ForeColor =
                            btnHand2.ItemAppearance.Normal.ForeColor = Color.Blue;
                        break;
                    case eCommandType.move:
                        btnMove2.ItemAppearance.Hovered.ForeColor =
                            btnMove2.ItemAppearance.Normal.ForeColor = Color.Blue;
                        break;
                }
            }
        }
        #endregion
    }
}
