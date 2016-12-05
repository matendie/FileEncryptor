using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
}
