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
