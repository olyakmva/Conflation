using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyPointApp.Controls
{
    public partial class ParamControl : UserControl
    {
        public double Distance
        {
            get
            {
                return double.Parse(distanceNumUpDown.Text);
            }
        }

        public int PointRange
        {
            get
            {
                return int.Parse(numericUpDown2.Text);
            }
        }
        public double Angle
        {
            get
            {
                double grad = double.Parse(angleNumericUpDown.Text);
                return Math.Round((grad * Math.PI) / 180, 3);
            }
        }
        public bool IsVector
        {
            get { return vectorCheckBox.Checked; }
        }
        public ParamControl()
        {
            InitializeComponent();
        }
    }
}
