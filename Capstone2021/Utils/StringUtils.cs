using System;
using System.Linq;

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
    }
}