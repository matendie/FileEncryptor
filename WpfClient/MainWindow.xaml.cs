using FileCryptography;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Password _password;
        internal Password _password1;
        internal Password _password2;
           
        EncryptionPassword encryptWindow;
        DecryptionPassword decryptWindow;
        ObservableCollection<Encryptable> items;
        Encryptable selectedFile;

        public MainWindow()
        {
            InitializeComponent(); 
             
            _password1 = new Password();
            _password2 = new Password();
            _password = new Password();

            encryptFileButton.IsEnabled = false;
            decryptFileButton.IsEnabled = false;

            items = new ObservableCollection<Encryptable>();
            filesListBox.ItemsSource = items;
            selectedFile = new Encryptable();
            selectedFile.ValueChanged += SelectedFile_ValueChanged;
        }
         
        private void SelectedFile_ValueChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(selectedFile.Name))
            {
                encryptFileButton.IsEnabled = false;
                decryptFileButton.IsEnabled = false;
            }
            else if (selectedFile.Name.EndsWith(".crstr"))
            {
                encryptFileButton.IsEnabled = false;
                decryptFileButton.IsEnabled = true;
            }
            else
            {
                encryptFileButton.IsEnabled = true;
                decryptFileButton.IsEnabled = false;

            }
        }

        private void EncryptWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Image Files(*.txt;*.csv;*.config)|*.txt;*.csv;*.config";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            string fName = openFileDialog1.FileName;
                            if (fName != null)
                            {
                                Encryptable newFile = new Encryptable()
                                { Name = System.IO.Path.GetFileName(openFileDialog1.FileName),
                                    FilePath = fName,
                                    Index = items.Count
                                };
                                items.Add(newFile); 
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
          
        private void encryptFileButton_Click(object sender, RoutedEventArgs e)
        { 
            encryptWindow = new EncryptionPassword(_password1, _password2, EncryptionPasswordCallBack);
            encryptWindow.ShowDialog();
            encryptWindow.Close();
        }

        private void EncryptionPasswordCallBack(Password password1, Password password2)
        {
            _password1 = password1;
            _password2 = password2;

            FileInfo fInfo = new FileInfo(selectedFile.FilePath);
            string name = fInfo.FullName;

            string copyFile = name + ".crstr";

            System.IO.File.Copy(name, copyFile);
            Encryption.EncryptFile(name, copyFile, _password1.Value);
            System.IO.File.Delete(name);
            int indexToRemove = selectedFile.Index;
            
            selectedFile.FilePath = copyFile;
            selectedFile.Name = System.IO.Path.GetFileName(copyFile);
            selectedFile.Index = items.Count - 1;
            items.Add(selectedFile);
            items.RemoveAt(indexToRemove);
        }

        private void decryptFileButton_Click(object sender, RoutedEventArgs e)
        {
            decryptWindow = new DecryptionPassword(_password, DecryptionPasswordCallBack);
            decryptWindow.ShowDialog();
            decryptWindow.Close();
        }

        private void DecryptionPasswordCallBack(Password password)
        {
            _password = password;

            string oldfilepath = selectedFile.FilePath.Substring(0, selectedFile.FilePath.LastIndexOf('.'));

            FileInfo fInfo = new FileInfo(selectedFile.FilePath);
            string name = fInfo.FullName;

            System.IO.File.Copy(name, oldfilepath);

            if (Encryption.DecryptFile(name, oldfilepath, _password.Value))
            {
                
                System.IO.File.Delete(name);
                int indexToRemove = selectedFile.Index;

                selectedFile.FilePath = oldfilepath;
                selectedFile.Name = System.IO.Path.GetFileName(oldfilepath);
                selectedFile.Index = items.Count - 1;
                items.Add(selectedFile);
                items.RemoveAt(indexToRemove);
            }
            else
            {
                System.IO.File.Delete(oldfilepath);
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock selected = (TextBlock)sender;
             
            Process.Start(System.IO.Path.GetDirectoryName(selected.Text));
        }

         
        private void filesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listview = (ListView)sender;
            Encryptable selected = (Encryptable)listview.SelectedItem;
            if (selected != null)
            {
                selectedFile.FilePath = selected.FilePath;
                selectedFile.Name = selected.Name;
                selectedFile.Index = selected.Index;
            }
        }
    }

    
}
