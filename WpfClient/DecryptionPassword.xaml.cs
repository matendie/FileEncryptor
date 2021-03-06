﻿using System;
using System.Windows;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for DecryptionPassword.xaml
    /// </summary>
    public partial class DecryptionPassword : Window
    {
        private Action<Password> _callback;
        private Password _password;
        public DecryptionPassword(Password password, Action<Password> callback)
        {
            InitializeComponent();
            _password = password;
            _callback = callback;
        }

         
        private void decrypt_button_Click(object sender, RoutedEventArgs e)
        {
            _callback(_password);
            this.Close();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password.Value = passwordBox.Password;
            _password.Validate(); 
             
            if (!String.IsNullOrEmpty(passwordBox.Password ))
            {
                decrypt_button.IsEnabled = true;
            }
            else
            {
                decrypt_button.IsEnabled = false;
            }
        }
    }
}
