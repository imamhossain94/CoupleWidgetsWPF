using CoupleWidgets.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;

namespace CoupleWidgets.Utils
{
    class DBHelper
    {

        public CoupleData coupleData { get; set; }

        public DBHelper()
        {
            readData();
            deleteImages();
        }

        // Ceate file if not exist
        private string getFilePath()
        {
            string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = baseDirectoryPath + "data.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();

                var data = new CoupleData()
                {
                    firstCoupleName = "Name",
                    secondCoupleName = "Name",
                    firstCoupleImage = "",
                    secondCoupleImage = "",
                    startDate = DateTime.Now.ToString(),
                    windowPositionX = 0.0,
                    windowPositionY = 0.0,
                    visibility = false
                };

                writeFile(filePath, data);
            }
            
            return filePath;
        }

        // Read all data
        public void readData()
        {
            try
            {
                string jsonFile = getFilePath();
                var json = File.ReadAllText(jsonFile);

                coupleData = new CoupleData(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

        }

        // Write file
        private void writeFile(string jsonFile, object data)
        {
            try
            {
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }


        // Delete unused images
        public void deleteImages()
        {
            string[] files = { coupleData.firstCoupleImage, coupleData.secondCoupleImage };
            string[] extentions = { ".jpg", ".png" };

            string[] filePaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);

            foreach (string filePath in filePaths)
            {
                var name = new FileInfo(filePath).Name;
                var ext = new FileInfo(filePath).Extension;

                if (!files.Contains(filePath) && extentions.Contains(ext))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch(Exception)
                    {

                    }
                }
            }

        }


        public void updateFirstName(string name)
        {
            readData();
            coupleData.firstCoupleName = name;
            writeFile(getFilePath(), coupleData);
        }

        public void updateSecondName(string name)
        {
            readData();
            coupleData.secondCoupleName = name;
            writeFile(getFilePath(), coupleData);
        }

        public void updateStartDate(string date)
        {
            readData();
            coupleData.startDate = date;
            writeFile(getFilePath(), coupleData);
        }

        public void updateFirstImage(string imagePath)
        {
            readData();
            coupleData.firstCoupleImage = copyFileToApplicationDirectory(imagePath);
            writeFile(getFilePath(), coupleData);
        }

        public void updateSecondImage(string imagePath)
        {
            readData();
            coupleData.secondCoupleImage = copyFileToApplicationDirectory(imagePath);
            writeFile(getFilePath(), coupleData);
        }

        public void updateVisivility(bool value)
        {
            readData();
            coupleData.visibility = value;
            writeFile(getFilePath(), coupleData);
        }

        public void updateWidgetPosition(double x, double y)
        {
            readData();
            coupleData.windowPositionX = x;
            coupleData.windowPositionY = y;
            writeFile(getFilePath(), coupleData);
        }


        private string copyFileToApplicationDirectory(string sourceFilePath)
        {
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(sourceFilePath);
            string destFilePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, filename);

            File.Copy(sourceFilePath, destFilePath, true);

            return destFilePath;
        }
    }
}
