using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static WpfClient.PasswordAdvisor;

namespace WpfClient
{
    public class Password
    {
        public string Value { get; set; }
        public int Score { get; set; }
        public string Message { get; set; }
        public Brush MessageColor { get; set; }

        public bool Validate()
        {

            PasswordScore passwordStrengthScore = PasswordAdvisor.CheckStrength(this);

            switch (passwordStrengthScore)
            {
                case PasswordScore.Blank:
                    this.Message = "Password Invalid";
                    this.MessageColor = Brushes.Red;
                    return false;
                case PasswordScore.VeryWeak:
                    this.Message = "Password Very Weak";
                    this.MessageColor = Brushes.OrangeRed;
                    return false;
                case PasswordScore.Weak:
                    this.Message = "Password Weak";
                    this.MessageColor = Brushes.Orange;
                    return true;
                case PasswordScore.Medium:
                    this.Message = "Password Medium";
                    this.MessageColor = Brushes.Yellow;
                    return true;
                case PasswordScore.Strong:
                    this.Message = "Password Strong";
                    this.MessageColor = Brushes.GreenYellow;
                    return true;
                case PasswordScore.VeryStrong:
                    this.Message = "Password Very Strong";
                    this.MessageColor = Brushes.Green;
                    return true;
            }
            return false;
        }
    }

    
}
