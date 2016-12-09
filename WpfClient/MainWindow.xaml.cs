using FileCryptography;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using static WpfClient.PasswordAdvisor;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        internal Password _password1;
        internal Password _password2;
           
        WpfClient.EncryptionPassword encryptWindow;

        public MainWindow()
        {
            InitializeComponent(); 
            encryptButton.IsEnabled = false;
            decryptBtton.IsEnabled = false; 
             
            _password1 = new Password();
            _password2 = new Password();

            
             
        }

        
        private void EncryptWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
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
                                fileTextBox.Text = fName;
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

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            FileInfo fInfo = new FileInfo(fileTextBox.Text); 
            string name = fInfo.FullName; 
            if (_password1.Value == _password2.Value)
            {
                string copyFile = name + ".crstr";
                fileTextBox.Text = copyFile;
                System.IO.File.Copy(name, copyFile);
                Encryption.EncryptFile(name, copyFile, passwordBox1.Password);
                System.IO.File.Delete(name);

                encryptButton.IsEnabled = true;
                decryptBtton.IsEnabled = false;
                passwordBox2.IsEnabled = true;
                 
            }
            else
            {
                passwordBox1.Background = Brushes.Red;
                passwordBox2.Background = Brushes.Red;
            }

        }

        private void decryptBtton_Click(object sender, RoutedEventArgs e)
        { 
            if (_password1.Value == _password2.Value)
            {
                string filepath = fileTextBox.Text.Substring(0,fileTextBox.Text.LastIndexOf('.'));
                 
                System.IO.File.Copy(fileTextBox.Text, filepath);
                Encryption.DecryptFile(fileTextBox.Text, filepath, passwordBox1.Password);
                System.IO.File.Delete(fileTextBox.Text);
                fileTextBox.Text = filepath;

                encryptButton.IsEnabled = false;
                decryptBtton.IsEnabled = true;
                passwordBox2.IsEnabled = false; 
            }
            else
            {
                passwordBox1.Background = Brushes.Red;
                passwordBox2.Background = Brushes.Red;
            }
        }
         
        private void fileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(fileTextBox.Text))
            {
                _password1.Value = passwordBox1.Password;
                


                if (fileTextBox.Text.EndsWith(".crstr") && _password1.Validate())
                {
                    encryptButton.IsEnabled = false;
                    decryptBtton.IsEnabled = true;
                    passwordBox2.IsEnabled = false; 
                }
                else if(_password1.Value == _password2.Value && _password1.Validate())
                {
                    encryptButton.IsEnabled = true;
                    decryptBtton.IsEnabled = false;
                    passwordBox2.IsEnabled = true;
                }
                
            }
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password1.Value = passwordBox1.Password;
            _password1.Validate(); 
            pass1Label.Content = _password1.Message;
            pass1Label.Foreground = _password1.MessageColor;


        }

        private void passwordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password2.Value = passwordBox2.Password;
            _password2.Validate();
            pass2Label.Content = _password2.Message;
            pass2Label.Foreground = _password2.MessageColor;
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

            passwordBox1.Password = _password1.Value;
            passwordBox2.Password = _password2.Value;
        }
    }
}
