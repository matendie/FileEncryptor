using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        public EncryptionPassword(Password password1, Password password2, Action<Password, Password> callback)
        {
            InitializeComponent();
            _password1 = password1;
            _password2 = password2; 
            this.Closed += EncryptionPassword_Closed;
            _callback = callback;
        }

        private void EncryptionPassword_Closed(object sender, EventArgs e)
        {
            _callback(_password1, _password2);
        }
         
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password1.Value = passwordBox.Password;
            _password1.Validate();
            pass1Label.Content = _password1.Message;
            pass1Label.Foreground = _password1.MessageColor;


            Color red = (Color)ColorConverter.ConvertFromString("#FF0000");
            SolidColorBrush redBrush = new SolidColorBrush(red);

            Color orange = (Color)ColorConverter.ConvertFromString("#FF9300");
            SolidColorBrush orangeBrush = new SolidColorBrush(orange);

            Color yellow = (Color)ColorConverter.ConvertFromString("#FFD040");
            SolidColorBrush yellowBrush = new SolidColorBrush(yellow);

            Color lightGreen = (Color)ColorConverter.ConvertFromString("#A4FF72");
            SolidColorBrush lightGreenBrush = new SolidColorBrush(lightGreen);

            Color green = (Color)ColorConverter.ConvertFromString("#06CB00");
            SolidColorBrush greenBrush = new SolidColorBrush(green);

            Color darkGreen = (Color)ColorConverter.ConvertFromString("#00A109");
            SolidColorBrush darkGreenBrush = new SolidColorBrush(darkGreen);

            Color gray = (Color)ColorConverter.ConvertFromString("#D3D3D3");
            SolidColorBrush grayBrush = new SolidColorBrush(gray);
            pass2Label.Content = _password1.Score + " - " + _password1.Value;
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
        }
    }
}
