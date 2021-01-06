using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;




namespace InstallerApp
{
    
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
              

        private static string path = ".";
        private static string progressText = "";

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
           
            // Open directory exporer and save path to private string
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {

                MainWindow.path = "";
                
            } else
            {
                
                MainWindow.path = dialog.SelectedPath;
               
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Checks if target directory path is empty or is the same as source directory, if not, call DirectoryCopy method
            if (MainWindow.path.Equals(""))
            {
                System.Windows.MessageBox.Show("No folder chosen");
            } else if (MainWindow.path.Equals("."))
            {
                System.Windows.MessageBox.Show("Choose another folder");
            }
            else {

                DirectoryCopy(".", MainWindow.path, false);
            }

        }


        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(MainWindow.path);

            int i = 0;

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                
                i++;

                progressText = "Copying files " + i + "/" + files.Length + "...";
                _txt.Text = progressText;

                string tempPath = System.IO.Path.Combine(MainWindow.path, file.Name);
                Console.WriteLine(tempPath, file);

                string filename = file.Name;
              
                //Checks if target directory already contains the file
                 if (!File.Exists(tempPath))
                {
                    file.CopyTo(tempPath, false);
                } else
                {
                    System.Windows.MessageBox.Show(tempPath + " already contains " + filename);
                }
             
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }

            System.Windows.MessageBox.Show("Copying files completed");
        }
    }
}
