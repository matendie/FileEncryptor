using System.Text.RegularExpressions; 

namespace WpfClient
{
    public class PasswordAdvisor
    { 
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public static PasswordScore CheckStrength(Password password)
        {
            password.Score = 0;
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
            if (password.Value.Length < 5)
            {
                password.Score = 0;
                return (PasswordScore)password.Score;
            } 
            if (password.Value.Length < 9)
            {
                password.Score++;
                return (PasswordScore)password.Score;
            }
            if (password.Value.Length < 12)
            {
                password.Score++;
                return (PasswordScore)password.Score;
            }
            if (password.Value.Length >= 12)
            {
                password.Score++;
            }
            if (password.Value.Length >= 24)
            {
                password.Score++;
            }
            
            return (PasswordScore)password.Score;
        }
    }
}
