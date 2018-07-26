using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyGymRegister
{
    public class Settings
    {
        public string SelectedCameraID { get; set; }
        public string SelectedCameraName { get; set; }

        private static Settings _instance;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings();

                return _instance;
            }
        }

        [Serializable]
        private class SerializationSettings
        {
            public string SelectedCameraID { get; set; } = null;
            public string SelectedCameraName { get; set; } = null;

            public SerializationSettings()
            {

            }

            public SerializationSettings(string selectedCameraID, string selectedCameraName)
            {
                SelectedCameraID = selectedCameraID;
                SelectedCameraName = selectedCameraName;
            }
        }

        public Settings()
        {
            //Try to load settings
            BinaryFormatter bf = new BinaryFormatter();
            SerializationSettings serSettings = new SerializationSettings();

            try
            {
                using (Stream str = File.Open("settings.dat", FileMode.Open))
                {
                    serSettings = (SerializationSettings)bf.Deserialize(str);
                }

                //Assign loaded values to statics
                SelectedCameraID = serSettings.SelectedCameraID;
                SelectedCameraName = serSettings.SelectedCameraName;
            }
            catch(Exception ex)
            {

            }
        }

        public void SaveSettings()
        {
            BinaryFormatter bf = new BinaryFormatter();
            SerializationSettings serSettings = new SerializationSettings(SelectedCameraID, SelectedCameraName);

            using (Stream str = File.Open("settings.dat", FileMode.Create))
            {
                bf.Serialize(str, serSettings);
            }
        }
    }


   
}
