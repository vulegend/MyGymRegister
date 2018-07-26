using AForge.Video.DirectShow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGymRegister
{
    public partial class Form1 : Form
    {
        private Bitmap _lastFrame = null;
        private FilterInfoCollection _devices = null;
        public bool TookPicture { get; set; }
        public List<CountryWithExtension> CountriesWithExtension = new List<CountryWithExtension>();

        public Form1()
        {
            InitializeComponent();
            DisableGroups();
            RetreiveCountryInfo();
        }

        public void SaveCountriesList()
        {
            using (FileStream fs = new FileStream("countries.dat", FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, CountriesWithExtension);
            }
        }

        public List<CountryWithExtension> LoadCountries()
        {
            using (FileStream fs = new FileStream("countries.dat", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (List<CountryWithExtension>)bf.Deserialize(fs);
            }
        }

        public void RetreiveCountryInfo()
        {
            CountriesWithExtension = new List<CountryWithExtension>(LoadCountries());


            int indexToSet = CountriesWithExtension.FindIndex(x => x.CountryCode == "RS");

            foreach(var country in CountriesWithExtension)
            {
                string extension = country.CountryExtension;

                if (country.CountryExtension[1] == '+')
                {
                    extension = extension.Substring(1);
                }

                countryExtensionCombobox.Items.Add(country.CountryName + " - " + country.CountryCode);
                phoneExtCombobox.Items.Add(country.CountryCode + ": " + extension);
            }

            Console.Write("");
        }

        public void RefreshForm()
        {
            DisableGroups();
            nameTextbox.Clear();
            addressTetbox.Clear();
            jmbgTextbox.Clear();
            mailTextbox.Clear();
            pictureBox1.Image = null;
            phoneTextbox.Clear();
            surnameTetbox.Clear();
            genderCombobox.SelectedIndex = -1;
        }

        public void EnableGroups()
        {
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            registerUserButton.Enabled = true;
        }

        public void DisableGroups()
        {
            registerUserButton.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
            toolStripLabel2.Enabled = false;
            settingsForm.FormClosed += SettingsForm_FormClosed;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            toolStripLabel2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableGroups();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {    
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CameraPictureForm cForm = new CameraPictureForm(this);
            cForm.Show();
            cForm.FormClosed += CForm_FormClosed;
        }

        private void CForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TookPicture)
            {
                Bitmap toLoad = (Bitmap)Bitmap.FromFile("lastImage.jpeg");
                pictureBox1.Image = toLoad;
            }
        }

        private void registerUserButton_Click(object sender, EventArgs e)
        {
            if(!TookPicture || nameTextbox.Text == string.Empty || surnameTetbox.Text == string.Empty || jmbgTextbox.Text == string.Empty || mailTextbox.Text == string.Empty || addressTetbox.Text == string.Empty || genderCombobox.SelectedIndex < 0 || phoneTextbox.Text == string.Empty)
            {
                MessageBox.Show("Ispunite sve podatke pre unosa");
                return;
            }

            if(!phoneTextbox.Text.IsDigitsOnly())
            {
                MessageBox.Show("Telefon mora sadrzati samo brojeve!");
                return;
            }

            if(!mailTextbox.Text.Contains("@"))
            {
                MessageBox.Show("E-Mail nije u ispravnom formatu!");
                return;
            }

            //Passed all the fail safes. Upload picture to picture repo and push data to main server
            User toSend = new User(nameTextbox.Text,surnameTetbox.Text,jmbgTextbox.Text,phoneTextbox.Text,addressTetbox.Text,genderCombobox.GetItemText(genderCombobox.SelectedItem),mailTextbox.Text);

            string jsonData = JsonConvert.SerializeObject(toSend);

            string response = WCFHandler.Instance.SendPOSTRequest("json/registeruser", jsonData);

            if(response.Contains("true"))
            {
                MessageBox.Show("Uspesno unet korisnik!");
                string userID = response.Between("[", "]");

                UploadImage(pictureBox1.Image as Bitmap, userID);

                RefreshForm();
            }
            else
            {
                MessageBox.Show(response);
            }
        }

        public void UploadImage(Bitmap toUpload, string fileName)
        {
            FileStream fStream = new FileStream("lastImage.jpeg", FileMode.Open, FileAccess.Read);
            FileInfo fInfo = new FileInfo("lastImage.jpeg");
            long numBytes = fInfo.Length;
            BinaryReader bReader = new BinaryReader(fStream);
            byte[] buffer = bReader.ReadBytes((int)numBytes);
            bReader.Close();
            
            WCFHandler.Instance.UploadPicture(buffer, fileName);

            fStream.Close();
            fStream.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }

    [System.Serializable]
    public class CountryWithExtension
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryExtension { get; set; }
    }
}
