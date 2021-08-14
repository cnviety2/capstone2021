using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.WebPages;

namespace Capstone2021.Utils
{
    public class StringUtils
    {
        /// <summary>
        /// Kiểm tra chuỗi có chứa ký tự đặc biệt ko,nếu có trả true
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool isContainSpecialCharacter(string input)
        {
            if (input.Contains('!') || input.Contains('@') || input.Contains('#') || input.Contains('$')
                || input.Contains('%') || input.Contains('^') || input.Contains('&') || input.Contains('*')
                || input.Contains('(') || input.Contains(')') || input.Contains('{') || input.Contains('}')
                || input.Contains(',') || input.Contains('.') || input.Contains('?') || input.Contains('/')
                || input.Contains('\\') || input.Contains('|') || input.Contains('`') || input.Contains('~'))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Return true nếu đúng format
        /// </summary>
        /// <param name="gmail"></param>
        /// <returns></returns>
        public static bool isValidEmailFormat(string gmail)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(gmail);
                string[] s = gmail.Split('.');
                if (s[s.Length - 1].IsEmpty())
                {
                    return false;
                }
                return addr.Address == gmail;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Return true nếu chuỗi chỉ chứa số
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Return true nếu đúng format http url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool isValidHttpUrl(string uri)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            string result = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            return result;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}