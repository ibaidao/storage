using System;
using System.Text.RegularExpressions;

namespace Utilities
{
    public class Validate
    {        //"^\d+$"　　//非负整数（正整数 + 0） 
        //"^[0-9]*[1-9][0-9]*$"　　//正整数 
        //"^((-\d+)|(0+))$"　　//非正整数（负整数 + 0） 
        //"^-[0-9]*[1-9][0-9]*$"　　//负整数 
        //"^-?\d+$"　　　　//整数 
        //"^\d+(\.\d+)?$"　　//非负浮点数（正浮点数 + 0） 
        //"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$"　　//正浮点数 
        //"^((-\d+(\.\d+)?)|(0+(\.0+)?))$"　　//非正浮点数（负浮点数 + 0） 
        //"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$"　　//负浮点数 
        //"^(-?\d+)(\.\d+)?$"　　//浮点数 
        //"^[A-Za-z]+$"　　//由26个英文字母组成的字符串 
        //"^[A-Z]+$"　　//由26个英文字母的大写组成的字符串 
        //"^[a-z]+$"　　//由26个英文字母的小写组成的字符串 
        //"^[A-Za-z0-9]+$"　　//由数字和26个英文字母组成的字符串 
        //"^\w+$"　　//由数字、26个英文字母或者下划线组成的字符串 
        //"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"　　　　//email地址 
        //"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$"　　//url  
        /// <summary>
        /// 是否空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(string value)
        {
            return string.IsNullOrEmpty(value);
        }
        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return (Regex.IsMatch(value, @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$") | Regex.IsMatch(value, @"^\d+$"));
        }
        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger(string value)
        {
            return Regex.IsMatch(value, @"^\d+$");
        }
        /// <summary>
        /// 是否非负数数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^/d*[.]?/d*$");
        }
        /// <summary>
        ///是否是Email
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            return Regex.IsMatch(value, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        }
        /// <summary>
        ///是否是URL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsURL(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$");
        }

        /// <summary>
        /// 是否是时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(string value)
        {
            return Regex.IsMatch(value, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
        }

        /// <summary>
        ///是否是英文字母
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEnglishWords(string value)
        {
            return Regex.IsMatch(value, @"^[A-Za-z]+$");
        }
        /// <summary>
        ///是否是汉字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsChineseWords(string value)
        {
            return Regex.IsMatch(value, @"^[\u4E00-\u9FFF]+$");
        }
    }
}
