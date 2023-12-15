
namespace KeyPointApp.Controls
{
    public partial class ParamControl : UserControl
    {
        public double BendDistance
        {
            get
            {
                return double.Parse(distanceNumUpDown.Text);
            }
        }

        public int PointDistance
        {
            get
            {
                return int.Parse(numericUpDown2.Text);
            }
        }
        public double AngleGap
        {
            get
            {
                return double.Parse(angleNumericUpDown.Text); 
            }
        }
       
        public ParamControl()
        {
            InitializeComponent();
        }
    }
}
