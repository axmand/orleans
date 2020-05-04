using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Engine.Lexicon.Extend
{
    public static class StringExtend
    {
        /// <summary>
        /// 清洗文本
        /// 目前简单处理标点符号问题
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ClearPunctuation(this string text)
        {
            text = text.Replace("\t", "").
                Replace(" ", "");
            //text = text.Replace(",", "")
            //         .Replace("、", "")
            //         .Replace("\t", "")
            //         .Replace(" ", "")
            //         .Replace("\tab", "")
            //         .Replace("，", "")
            //         .Replace("“", "")
            //         .Replace("”", "")
            //         .Replace(".", "")
            //         .Replace("。", "")
            //         .Replace("!", "")
            //         .Replace("！", "")
            //         .Replace("?", "")
            //         .Replace("？", "")
            //         .Replace(":", "")
            //         .Replace("：", "")
            //         .Replace(";", "")
            //         .Replace("；", "")
            //         .Replace("～", "")
            //         .Replace("-", "")
            //         .Replace("_", "")
            //         .Replace("——", "")
            //         .Replace("—", "")
            //         .Replace("--", "")
            //         .Replace("【", "")
            //         .Replace("】", "")
            //         .Replace("[", "")
            //         .Replace("]", "")
            //         .Replace("\\", "")
            //         .Replace("(", "")
            //         .Replace(")", "")
            //         .Replace("（", "")
            //         .Replace("）", "")
            //         .Replace("#", "")
            //         .Replace("$", "");
            return text;
        }

        /// <summary>
        /// “SUTIME: A Library for Recognizing and Normalizing Time Expressions”
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TimeExpression(this string text)
        {
            //1. 如果有T，表示有时间需要转换
            text = text?.Replace('T', ' ');
            text= text.Replace("MO", "06:00"); //morning
            text = text.Replace("NI", "21:00");//night
            text = text.Replace("AF", "14:00"); //afternoon
            return text;
        }

        public static string InsertCompound(this string text, string word)
        {
            List<string> str = text.Trim().Split(' ').ToList();
            if (str.Count > 1)
                str.Insert(str.Count - 1, word);
            else
                str.Insert(0, word);
            return string.Join(" ", str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string text)
        {
            string[] patterns = new string[] { 
                "yyyy-MM-dd hh:mm:ss",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd hh:mm",
                "yyyy-MM-dd HH:mm",
                "yyyy-MM-dd hh",
                "yyyy-MM-dd HH",
                "yyyy-MM-dd",
                "yyyyMMdd",
                "yyyyMM",
                "yyyy",
            };
            DateTime.TryParseExact(text, patterns, null, DateTimeStyles.None, out DateTime dt);
            return dt;
        }
    }
}
