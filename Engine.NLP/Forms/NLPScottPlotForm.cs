using System.Drawing;
using System.Windows.Forms;

namespace Engine.NLP.Forms
{
    public partial class NLPScottPlotForm : Form
    {
        public NLPScottPlotForm()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            //convert to v3.0.3
            Mainly_scottPlotUC.plt.Clear();
        }

        public void Title(string title)
        {
            Mainly_scottPlotUC.plt.Title(title);
            Mainly_scottPlotUC.plt.XLabel(title);
        }

        public void DrawPoint(double[] xy, Color color)
        {
            Mainly_scottPlotUC.plt.PlotPoint(xy[0], xy[1], color);
        }

        public void PrepareData(double[] x, double[] y)
        {
            Mainly_scottPlotUC.plt.PlotScatter(x, y);
            Mainly_scottPlotUC.plt.AxisAuto();
        }

        public void Render()
        {
            Mainly_scottPlotUC.Render();
        }

    }
}
