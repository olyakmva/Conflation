using AlgorithmsLibrary;
using DotSpatial.Data;
using MainForm.Controls;
using SupportLib;
using ConflationLib;
using KeyPointApp.Controls;

namespace KeyPointApp
{
    public partial class MainForm : Form
    {
        private readonly Map _map;
        private readonly List<Layer> _layers;
        readonly GraphicsState _state = new();
        private List<AlgParamControl> _listCtrls;
        private readonly string _applicationPath;
        private readonly int startX = 0;
        private int startY = 10;
        private readonly int ctrlHeight = 135;
        private int afterBtnProcY = 480;
        private readonly int layerCtrlHeight = 45;
        private TableLayoutPanel table;
        public List<KeyPoint> keyPoints;
        public Dictionary<int, List<PointVectorRelation>> map2Vectors, map1Vectors;
        ParamControl paramControl;
        public MainForm()
        {
            InitializeComponent();
            _listCtrls = new List<AlgParamControl>();
            _layers = new List<Layer>();
            _map = new Map();
            _state.Scale = 2;
            mapPictureBox.MouseWheel += MapPictureBoxMouseWheel;
            _applicationPath = Environment.CurrentDirectory;
            Colors.Init();
            paramControl = new ParamControl
            {
                Location = new Point(startX, 280)
            };
            mainContainer.Panel1 .Controls .Add(paramControl);
            mainContainer.SplitterDistance  = paramControl.Width;
            //InitTable();
        }
        private void InitTable()
        {
            table = new TableLayoutPanel
            {
                RowCount = 1,
                ColumnCount = 4,
                Width = 250,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.0F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.0F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.0F));
            ;
            var lblId1 = new Label() { Text = "ID1" };
            var lblId2 = new Label() { Text = "ID2" };
            var lblDist = new Label() { Text = "KeyPNum" };

            table.Controls.Add(lblId1, 0, 0);
            table.Controls.Add(lblId2, 1, 0);
            table.Controls.Add(lblDist, 2, 0);

            table.Location = new Point(0, ctrlHeight * 5);
            mainContainer.Panel1.Controls.Add(table);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = Path.Combine(_applicationPath, "Data"),
                Filter = @"shape files (*.shp)|*.shp|All files (*.*)|*.*",
                DefaultExt = "*.shp"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                string shapeFileName = openFileDialog.FileName;
                var inputShape = FeatureSet.Open(shapeFileName);
                var inputMap = Converter.ToMapData(inputShape);
                _map.Add(inputMap);
                SetGraphicsParams(_map);
                inputMap.FileName = Path.GetFileName(openFileDialog.FileName);
                inputMap.FileName = inputMap.FileName.Remove(inputMap.FileName.Length - 4);
                AlgParamControl algParamControl = new()
                {
                    mapData = inputMap,
                    Location = new Point(startX, startY),
                    LayerName = inputMap.FileName
                };
                startY += ctrlHeight;
                _listCtrls.Add(algParamControl);
                mainContainer.Panel1.Controls.Add(algParamControl);
                var l = new Layer(inputMap, "in" + inputMap.FileName);
                _layers.Add(l);
                var layerCtrl = new LayerControl(l)
                {
                    Location = new Point(startX, afterBtnProcY)
                };
                layerCtrl.CheckedChanged += OnLayerVisibleChanged;
                afterBtnProcY += layerCtrlHeight;
                mainContainer.Panel1.Controls.Add(layerCtrl);
                mainContainer.SplitterDistance  = layerCtrl .Width;
                foreach (var ctrl in _listCtrls)
                {
                    foreach (var otherCtrl in _listCtrls)
                    {
                        if (ctrl == otherCtrl)
                            continue;
                        ctrl.CopyingParams += otherCtrl.OnCopyingParams;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, @"Error : " + ex.Message);
                return;
            }
            mapPictureBox.Invalidate();
        }
        private void SetGraphicsParams(Map map)
        {
            double xmin = map.Xmin;
            double ymin = map.Ymin;
            double xmax = map.Xmax;
            double ymax = map.Ymax;

            var g = CreateGraphics();
            if ((xmax - xmin) / mapPictureBox.Width > (ymax - ymin) / mapPictureBox.Height)
            {
                _state.Scale = (xmax - xmin) / (mapPictureBox.Width - 40);
                _state.InitG(g, 1);
            }
            else
            {
                _state.Scale = (ymax - ymin) / (mapPictureBox.Height - 40);
                _state.InitG(g, 2);
            }

            _state.CenterX = xmin;
            _state.CenterY = ymax;
            _state.DefscaleX = (xmax - xmin);
            _state.DefscaleY = (ymax - ymin);
            _state.DefCenterX = xmin;
            _state.DefCenferY = ymax;
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MapPictureBoxMouseWheel(object sender, MouseEventArgs e)
        {
            var x = 0;
            if (e.Delta > 0)
                x = -1;
            else if (e.Delta < 0)
                x = 1;
            _state.Zoom(x, mapPictureBox.Width, mapPictureBox.Height);
            mapPictureBox.Invalidate();
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var layer in _layers)
            {
                IFeatureSet fs = Converter.ToShape(layer.MapData);
                fs.SaveAs(layer.AlgorithmName + ".shp", true);
            }
        }

        private void BtnProcessClick(object sender, EventArgs e)
        {
            List<MapData> mapDatas = new();
            foreach (var ctrl in _listCtrls)
            {
                if (!ctrl.IsChecked)
                    continue;
                var map = ctrl.mapData.Clone();

                ISimplificationAlgm algm = ctrl.GetAlgorithm();
                algm.Options.PointNumberGap = 2.0;
                algm.Run(map);
                mapDatas.Add(map);
                var layerName = $"{ctrl.LayerName}{ctrl.Name.Substring(0, 4)}";
                var l = new Layer(map, layerName);
                _layers.Add(l);
                var layerCtrl = new LayerControl(l)
                {
                    Location = new Point(startX, afterBtnProcY),
                    BackColor = Color.FromName(l.Color)
                };
                layerCtrl.CheckedChanged += OnLayerVisibleChanged;
                afterBtnProcY += layerCtrlHeight;
                mainContainer.Panel1.Controls.Add(layerCtrl);
            }
            if (mapDatas.Count > 1)
            {
                double maxDistanceBetweenPoints = paramControl .Distance;
                var bendCharacteristics = new BendCharacteristics(mapDatas[0], mapDatas[1], maxDistanceBetweenPoints);
                bendCharacteristics.AngleBetweenVectors = paramControl.Angle;
                bendCharacteristics.Run(paramControl.IsVector);

                //var floatCharts = new FloatCharacteristics(mapDatas[0], mapDatas[1], maxDistanceBetweenPoints );
                //floatCharts .PointRange = paramControl .PointRange;
                //floatCharts.AngleBetweenVectors = paramControl.Angle;
                //floatCharts.Run(paramControl.IsVector);
                
                //map1Vectors = floatCharts.map1Vectors;
                //map2Vectors = floatCharts.map2Vectors;
                keyPoints = bendCharacteristics.keyPoints;

            }
            mapPictureBox.Invalidate();

        }
        private void MapPictureBoxPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            foreach (var layer in _layers)
            {
                if (!layer.Visible)
                    continue;
                var c = Color.FromName(layer.Color);
                var pen0 = new Pen(c, 1.75f);
                Display(g, layer.MapData, pen0);
            }
            var pen = new Pen(Color.DarkRed, 2.0f);
           // DisplayVectors(g, map1Vectors, pen);
            pen = new Pen(Color.DarkKhaki, 2.0f);
            //DisplayVectors(g, map2Vectors, pen);

            if (keyPoints != null && keyPoints.Count > 0)
            {
                var pen1 = new Pen(Color.Black, 2.0f);
                var pen2 = new Pen(Color.Green, 2.0f);
                foreach (var kpoint in keyPoints)
                {
                    var pt1 = _state.GetPoint(kpoint.PointVector1.Point, mapPictureBox.Height - 1);
                    var pointEnd = new MapPoint
                    {
                        X = kpoint.PointVector1.Point.X + kpoint.PointVector1.Vector.x,
                        Y = kpoint.PointVector1.Point.Y + kpoint.PointVector1.Vector.y
                    };
                    var pt2 = _state.GetPoint(pointEnd, mapPictureBox.Height - 1);
                    g.DrawLine(pen1, pt1, pt2);
                    pt1 = _state.GetPoint(kpoint.PointVector2.Point, mapPictureBox.Height - 1);
                    pointEnd = new MapPoint
                    {
                        X = kpoint.PointVector2.Point.X + kpoint.PointVector2.Vector.x,
                        Y = kpoint.PointVector2.Point.Y + kpoint.PointVector2.Vector.y
                    };
                    pt2 = _state.GetPoint(pointEnd, mapPictureBox.Height - 1);
                    g.DrawLine(pen2, pt1, pt2);
                }
            }

            g.Flush();
        }

        private void DisplayVectors(Graphics g, Dictionary<int, List<PointVectorRelation>> map1Vectors, Pen pen)
        {

            if (map1Vectors != null && map1Vectors.Count > 0)
            {

                foreach (var pair in map1Vectors)
                {
                    var pointVectorList = pair.Value;
                    foreach (var item in pointVectorList)
                    {
                        var pt1 = _state.GetPoint(item.Point, mapPictureBox.Height - 1);
                        var pointEnd = new MapPoint
                        {
                            X = item.Point.X + item.Vector.x,
                            Y = item.Point.Y + item.Vector.y
                        };
                        var pt2 = _state.GetPoint(pointEnd, mapPictureBox.Height - 1);
                        g.DrawLine(pen, pt1, pt2);
                    }
                }
            }
        }

        /// <summary>
        /// Отображение MapData md на графике g
        /// </summary>
        /// <param name="g"></param>
        /// <param name="md"></param>
        /// <param name="pen">Цвет в случае отображения нескольких MapData на одном picturebox</param>
        private void Display(Graphics g, MapData md, Pen pen)
        {
            var brush = new SolidBrush(pen.Color);
            foreach (var list in md.MapObjDictionary.Values)
            {
                if (list.Count == 0)
                    continue;
                for (var j = 0; j < (list.Count - 1); j++)
                {
                    var pt1 = _state.GetPoint(list[j], mapPictureBox.Height - 1);
                    var pt2 = _state.GetPoint(list[j + 1], mapPictureBox.Height - 1);
                    g.FillRectangle(brush, pt1.X, pt1.Y, 5, 5);
                    g.DrawLine(pen, pt1, pt2);
                }
            }
        }
        private void MapPictureBoxMouseLeave(object sender, EventArgs e)
        {
            if (mapPictureBox.Focused)
                mapPictureBox.Parent.Focus();
        }

        private void MapPictureBoxMouseUp(object sender, MouseEventArgs e)
        {
            mapPictureBox.Invalidate();
        }

        private void MapPictureBoxMouseDown(object sender, MouseEventArgs e)
        {
            _state.Mousex = e.X;
            _state.Mousey = e.Y;
        }

        private void MapPictureBoxMouseEnter(object sender, EventArgs e)
        {
            if (!mapPictureBox.Focused)
                mapPictureBox.Focus();
        }

        private void MapPictureBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                _state.CenterX += (_state.Mousex - e.X) * _state.Scale;
                _state.CenterY -= (_state.Mousey - e.Y) * _state.Scale;
                _state.Mousex = e.X;
                _state.Mousey = e.Y;
            }
        }
        private void MapSplitContainerPanel1Resize(object sender, EventArgs e)
        {
            mapPictureBox.Width = Width;
            mapPictureBox.Height = Height;
            mapPictureBox.Invalidate();
        }
        private void MapPictureBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            _state.Scale = Math.Max(_state.DefscaleX / mapPictureBox.Width, _state.DefscaleY / mapPictureBox.Height);
            _state.CenterX = _state.DefCenterX;
            _state.CenterY = _state.DefCenferY;
            mapPictureBox.Invalidate();
        }
        private void OnLayerVisibleChanged(object sender, EventArgs e)
        {
            mapPictureBox.Invalidate();
        }

        private void ClearToolStripMenuItemClick(object sender, EventArgs e)
        {
            _listCtrls.Clear();
            _layers.Clear();
            _map.MapLayers.Clear();
            keyPoints?.Clear();
            map2Vectors?.Clear();
            map1Vectors?.Clear();
            _state.Scale = 2;
            Colors.Init();
            startY = 10;
            afterBtnProcY = 480;
            //table.Controls.Clear();
            // mainContainer.Panel1.Controls.Remove(table);
            mainContainer.Panel1.Controls.Clear();
            mainContainer.Panel1.Controls.Add(btnProcess);
            paramControl = new ParamControl
            {
                Location = new Point(startX, 280)
            };
            mainContainer.Panel1.Controls.Add(paramControl);
            mainContainer.SplitterDistance = paramControl.Width;
            //InitTable();
            mapPictureBox.Invalidate();
        }
    }
}