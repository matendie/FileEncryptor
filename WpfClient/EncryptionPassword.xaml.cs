using System;
using System.Windows;
using System.Windows.Media;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for EncryptionPassword.xaml
    /// </summary>
    public partial class EncryptionPassword : Window
    {
        Password _password1;
        Password _password2;
        private Action<Password, Password> _callback;

        SolidColorBrush redBrush       ;
        SolidColorBrush orangeBrush    ;
        SolidColorBrush yellowBrush    ;
        SolidColorBrush lightGreenBrush;
        SolidColorBrush greenBrush     ;
        SolidColorBrush darkGreenBrush ;
        SolidColorBrush grayBrush;

        Color red        ;
        Color orange     ;
        Color yellow     ;
        Color lightGreen ;
        Color green      ;
        Color darkGreen  ;
        Color gray;

        public EncryptionPassword(Password password1, Password password2, Action<Password, Password> callback)
        {
            InitializeComponent();
            _password1 = password1;
            _password2 = password2;  
            _callback = callback;
            encrypt_button.IsEnabled = false;

            red        = (Color)ColorConverter.ConvertFromString("#FF0000");
            orange     = (Color)ColorConverter.ConvertFromString("#FF9300");
            yellow     = (Color)ColorConverter.ConvertFromString("#FFD040");
            lightGreen = (Color)ColorConverter.ConvertFromString("#A4FF72");
            green      = (Color)ColorConverter.ConvertFromString("#06CB00");
            darkGreen  = (Color)ColorConverter.ConvertFromString("#00A109");
            gray       = (Color)ColorConverter.ConvertFromString("#D3D3D3");

            redBrush        = new SolidColorBrush(red);
            orangeBrush     = new SolidColorBrush(orange);
            yellowBrush     = new SolidColorBrush(yellow);
            lightGreenBrush = new SolidColorBrush(lightGreen);
            greenBrush      = new SolidColorBrush(green);
            darkGreenBrush  = new SolidColorBrush(darkGreen);
            grayBrush       = new SolidColorBrush(gray);
        }
          
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password1.Value = passwordBox.Password;
            _password1.Validate();
            pass1Label.Content = _password1.Message;
            pass1Label.Foreground = _password1.MessageColor; 

            ChangePasswordStrengthColor();
            if (passwordBox.Password == passwordBox1.Password)
            {
                encrypt_button.IsEnabled = true;
            }
            else
            {
                encrypt_button.IsEnabled = false;
            }
        }

        private void ChangePasswordStrengthColor()
        {
            switch (_password1.Score)
            {
                case 0:
                    passLevel0.Fill = redBrush;
                    passLevel1.Fill = grayBrush;
                    passLevel2.Fill = grayBrush;
                    passLevel3.Fill = grayBrush;
                    passLevel4.Fill = grayBrush;
                    passLevel5.Fill = grayBrush;
                    break;
                case 1:
                    passLevel0.Fill = redBrush;
                    passLevel1.Fill = orangeBrush;
                    passLevel2.Fill = grayBrush;
                    passLevel3.Fill = grayBrush;
                    passLevel4.Fill = grayBrush;
                    passLevel5.Fill = grayBrush;
                    break;
                case 2:
                    passLevel0.Fill = redBrush;
                    passLevel1.Fill = orangeBrush;
                    passLevel2.Fill = yellowBrush;
                    passLevel3.Fill = grayBrush;
                    passLevel4.Fill = grayBrush;
                    passLevel5.Fill = grayBrush;
                    break;
                case 3:
                    passLevel0.Fill = redBrush;
                    passLevel1.Fill = orangeBrush;
                    passLevel2.Fill = yellowBrush;
                    passLevel3.Fill = lightGreenBrush;
                    passLevel4.Fill = grayBrush;
                    passLevel5.Fill = grayBrush;
                    break;
                case 4:
                    passLevel0.Fill = redBrush;
                    passLevel1.Fill = orangeBrush;
                    passLevel2.Fill = yellowBrush;
                    passLevel3.Fill = lightGreenBrush;
                    passLevel4.Fill = greenBrush;
                    passLevel5.Fill = grayBrush;
                    break;
                case 5:
                    passLevel0.Fill = redBrush;
                    passLevel1.Fill = orangeBrush;
                    passLevel2.Fill = yellowBrush;
                    passLevel3.Fill = lightGreenBrush;
                    passLevel4.Fill = greenBrush;
                    passLevel5.Fill = darkGreenBrush;
                    break;
                default:
                    break;
            }
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password2.Value = passwordBox1.Password;
            _password2.Validate();
            pass2Label.Content = _password2.Message;
            pass2Label.Foreground = _password2.MessageColor;
            if(passwordBox.Password == passwordBox1.Password)
            {
                encrypt_button.IsEnabled = true;
            }
            else
            {
                encrypt_button.IsEnabled = false;
            }
        }

        private void encrypt_button_Click(object sender, RoutedEventArgs e)
        {
            _callback(_password1, _password2);
            this.Close();
        }
    }
}
