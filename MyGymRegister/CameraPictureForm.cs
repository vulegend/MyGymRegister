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
    public partial class CameraPictureForm : Form
    {
        private VideoCaptureDevice _videoDevice = null;
        private Bitmap _lastFrame = null;
        private Form1 _ref;
        private bool _paintOnce = false;

        public CameraPictureForm(Form1 refForm)
        {
            InitializeComponent();
            _ref = refForm;
        }

        private void StartCaptureDevice()
        {
            if (Settings.Instance.SelectedCameraID == null)
            {
                FilterInfoCollection devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo device in devices)
                {
                    Settings.Instance.SelectedCameraID = device.MonikerString;
                    Settings.Instance.SelectedCameraName = device.Name;
                    Settings.Instance.SaveSettings();
                    break;
                }
            }
            
            if(Settings.Instance.SelectedCameraID != null)
            {
                _videoDevice = new VideoCaptureDevice(Settings.Instance.SelectedCameraID);
                _videoDevice.NewFrame += _videoDevice_NewFrame;
                _videoDevice.Start();
            }
            else
            {
                MessageBox.Show("Instalirajte kameru da bi ste pristupili opciji");
            }
        }

        private void _videoDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            if (_lastFrame != null)
            {
                _lastFrame.Dispose();
            }

            _lastFrame = new Bitmap(eventArgs.Frame);

            if (pictureBox1.InvokeRequired)
                pictureBox1.BeginInvoke(new Action<Bitmap>(SetFrame), _lastFrame);
        }

        private void SetFrame(Bitmap bMap)
        {
            if(bMap != null)
                pictureBox1.Image = bMap;
          
        }

        private void CameraPictureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _videoDevice.SignalToStop();
            _videoDevice.NewFrame -= _videoDevice_NewFrame;
            _videoDevice = null;
        }

        private void CameraPictureForm_Load(object sender, EventArgs e)
        {
            StartCaptureDevice();
        }

        private void TakeSnapshot()
        {
            Bitmap getLastFrame = (Bitmap)pictureBox1.Image;
            try
            {
                GraphicsUnit units = GraphicsUnit.Point;

                RectangleF imgRectangleF = getLastFrame.GetBounds(ref units);
                Rectangle imgRectangle = Rectangle.Round(imgRectangleF);

                int width = 350;
                int height = 350;
                int x = imgRectangle.Width / 2 - width/2;
                int y = imgRectangle.Height / 2 - height/2;

                Bitmap croppedImage = getLastFrame.Clone(new Rectangle(x,y,width,height), getLastFrame.PixelFormat);
                croppedImage.Save("lastImage.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                _ref.TookPicture = true;
                this.Close();
            }
            catch(Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Invoke(new Action(TakeSnapshot));
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            int width = 350;
            int height = 350;

            int x = (pictureBox1.Width / 2) - (width/2);
            int y = (pictureBox1.Height / 2) - (height / 2);

            Rectangle rectangler = new Rectangle(x, y, width, height);
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, rectangler);
                
            }
        }
    }
}
