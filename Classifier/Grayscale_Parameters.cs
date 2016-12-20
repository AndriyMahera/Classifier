using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Classifier.Program;

namespace Classifier
{
    public partial class Grayscale_Parameters : Form
    {


        public Grayscale_Parameters()
        {
            InitializeComponent();
        }

        private void BTAccept_Click(object sender, EventArgs e)
        {

        }

        private void Grayscale_Parameters_Load(object sender, EventArgs e)
        {
            double black, white;
            if(Double.TryParse(TBblackPointPercent.Text,out black))
            {
                if(Double.TryParse(TBwhitePointPercent.Text,out white))
                {
                    
                    this.Close();
                }
            }
            
        }
    }
}
