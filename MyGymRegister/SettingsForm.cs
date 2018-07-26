using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGymRegister
{
    public partial class SettingsForm : Form
    {
        //List of all capturable devices
        public FilterInfoCollection Devices { get; private set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            //Get all the video input devices
            Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in Devices)
            {
                comboBox1.Items.Add(device.Name);
            }

            if (Settings.Instance.SelectedCameraID == null)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                int findIndex = 0;
                
                for(int i = 0; i < Devices.Count; i++)
                {
                    if(Devices[i].MonikerString == Settings.Instance.SelectedCameraID)
                    {
                        findIndex = i;
                        break;
                    }
                }

                comboBox1.SelectedIndex = findIndex;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.Instance.SelectedCameraID = Devices[comboBox1.SelectedIndex].MonikerString;
            Settings.Instance.SelectedCameraName = Devices[comboBox1.SelectedIndex].Name;
            Settings.Instance.SaveSettings();
            this.Close();
        }
    }
}
