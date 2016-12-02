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


namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        private Password password1;
        private Password password2;
         
        CspParameters cspp = new CspParameters();
        const string keyName = "Key01";
        FileCryptography.Encryption fileCrypto;
        string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";// @"^ (?=.*[a - z])(?=.*[A - Z])(?=.*\d)(?=.*[^\da - zA - Z]).{ 8,124}$";
        Regex rgx;

        public MainWindow()
        {
            InitializeComponent();
            fileCrypto = new FileCryptography.Encryption();
            encryptButton.IsEnabled = false;
            decryptBtton.IsEnabled = false; 
            rgx = new Regex(regex);
            password1 = new Password();
            password2 = new Password();
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
            // Pass the file name without the path.
            string name = fInfo.FullName;
            //fileCrypto.EncryptFile(name);
            if (password1.Value == password2.Value)
            {
                string copyFile = name + ".crstr";
                fileTextBox.Text = copyFile;
                System.IO.File.Copy(name, copyFile);
                Encryption.EncryptFile(name, copyFile, passwordBox1.Password);
                System.IO.File.Delete(name);

                encryptButton.IsEnabled = true;
                decryptBtton.IsEnabled = false;
                passwordBox2.IsEnabled = true;

                //fileCrypto.EncryptFile(name, password1);
            }
            else
            {
                passwordBox1.Background = Brushes.Red;
                passwordBox2.Background = Brushes.Red;
            }

        }

        private void decryptBtton_Click(object sender, RoutedEventArgs e)
        {
            //fileCrypto.DecryptFile(fileTextBox.Text);
            if (password1.Value == password2.Value)
            {
                string filepath = fileTextBox.Text.Substring(0,fileTextBox.Text.LastIndexOf('.'));

                //fileTextBox.Text = filepath;
                System.IO.File.Copy(fileTextBox.Text, filepath);
                Encryption.DecryptFile(fileTextBox.Text, filepath, passwordBox1.Password);
                System.IO.File.Delete(fileTextBox.Text);
                fileTextBox.Text = filepath;

                encryptButton.IsEnabled = false;
                decryptBtton.IsEnabled = true;
                passwordBox2.IsEnabled = false;
                //fileCrypto.DecryptFile(fileTextBox.Text, password1);
            }
            else
            {
                passwordBox1.Background = Brushes.Red;
                passwordBox2.Background = Brushes.Red;
            }
        }

        private void passwordSaveButton_Click(object sender, RoutedEventArgs e)
        {

            //keyTextBox.Text = fileCrypto.CreateAsymetricKey();

            //keyTextBox.Text = Encryption.AutoGenerateKey();

        }

        private void passwordBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            //password1 = passwordBox1.Password;

            //if (password1 != password2 || ValidatePassword(password1))
            //{
            //    passwordBox1.Background = Brushes.Red;
            //}
            //else
            //{
            //    passwordBox1.Background = Brushes.White;
            //    passwordBox2.Background = Brushes.White;
            //}
        }

        private void passwordBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            //password2 = passwordBox2.Password;

            //if (password1 != password2 || ValidatePassword(password2))
            //{
            //    passwordBox2.Background = Brushes.Red;
            //}
            //else
            //{
            //    passwordBox1.Background = Brushes.White;
            //    passwordBox2.Background = Brushes.White;
            //}
        }

        private void fileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(fileTextBox.Text))
            {
                password1.Value = passwordBox1.Password;
                


                if (fileTextBox.Text.EndsWith(".crstr") && ValidatePassword(password1))
                {
                    encryptButton.IsEnabled = false;
                    decryptBtton.IsEnabled = true;
                    passwordBox2.IsEnabled = false; 
                }
                else if(password1.Value == password2.Value && ValidatePassword(password1) )
                {
                    encryptButton.IsEnabled = true;
                    decryptBtton.IsEnabled = false;
                    passwordBox2.IsEnabled = true;
                }
                
            }
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password1.Value = passwordBox1.Password;
            ValidatePassword(password1); 
            pass1Label.Content = password1.Message;
            pass1Label.Foreground = password1.MessageColor;


        }

        private void passwordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password2.Value = passwordBox2.Password;
            ValidatePassword(password2);
            pass2Label.Content = password2.Message;
            pass2Label.Foreground = password2.MessageColor;
        }

        private bool ValidatePassword(Password password)
        {
            //MatchCollection matches = rgx.Matches(password);
            //if (matches.Count > 0)
            //{
            //    return true;
            //}
            //return false;

            //String password = "MyDummy_Password"; // Substitute with the user input string
            PasswordScore passwordStrengthScore = PasswordAdvisor.CheckStrength(password);

            switch (passwordStrengthScore)
            {
                case PasswordScore.Blank:
                    password.Message = "Password Invalid";
                    password.MessageColor = Brushes.Red;
                    return false;
                case PasswordScore.VeryWeak: 
                    password.Message = "Password Very Weak";
                    password.MessageColor = Brushes.OrangeRed;
                    return false;
                case PasswordScore.Weak:
                    password.Message = "Password Weak";
                    password.MessageColor = Brushes.Orange; 
                    return true;
                case PasswordScore.Medium:
                    password.Message = "Password Medium";
                    password.MessageColor = Brushes.Yellow;
                    return true;
                case PasswordScore.Strong:
                    password.Message = "Password Strong";
                    password.MessageColor = Brushes.GreenYellow;
                    return true;
                case PasswordScore.VeryStrong:
                    password.Message = "Password Very Strong";
                    password.MessageColor = Brushes.Green;
                    return true; 
            }
            return false;
        }

        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public class PasswordAdvisor
        { 
            public static PasswordScore CheckStrength(Password password)
            {
                password.Score = 0;
                if (password.Value.Length < 1)
                {
                    password.Score++;
                    return PasswordScore.Blank;
                }
                if (password.Value.Length < 4)
                { 
                    password.Score++;
                    return PasswordScore.VeryWeak;
                }
                if (password.Value.Length >= 8)
                {
                    password.Score++;
                }
                if (password.Value.Length >= 12)
                {
                    password.Score++;
                }
                if (Regex.Match(password.Value, @"\d+", RegexOptions.ECMAScript).Success)
                {
                    password.Score++;
                }
                if (Regex.Match(password.Value, @"[a-z]", RegexOptions.ECMAScript).Success &&
                   Regex.Match(password.Value, @"[A-Z]", RegexOptions.ECMAScript).Success)
                {
                    password.Score++;
                }
                if (Regex.Match(password.Value, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                {
                    password.Score++;
                } 
                return (PasswordScore)password.Score;
            }
        }

        public class Password
        {
            public string Value { get; set; }
            public int Score { get; set; }
            public string Message { get; set; }
            public Brush MessageColor { get; set; }
        }
    }
}
