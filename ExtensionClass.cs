 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ExtensionClass
    {

        public static decimal ToDecimal(this string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return 0;
            }
            return decimal.Parse(str);
        }
        public static bool IntToBool(this int i)
        {
            if (i == 1)
            {
                return true;
            }
            return false;
        }
        public static bool IntToBool(this int? i)
        {
            if (i == 1)
            {
                return true;
            }
            return false;
        }


        public static string BoolToIntString(this bool b)
        {
            if (b)
            {
                return "1";
            }
            return "0";

        }
        public static string BoolToIntString(this bool? b)
        {
            if (b == true)
            {
                return "1";
            }
            return "0";

        }
        public static string ToJsonString(this object obj)
        {

            return JsonConvert.SerializeObject(obj);

        }

        public static T JsonStringToObj<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static object JsonStringToObj(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }
        public static object JsonStringToObj(string value, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, type, settings);
        }

        public static object JsonStringToObj(this string str)
        {
            return JsonConvert.DeserializeObject(str);
        }
        public static bool IsNullOrWhiteSpace(this string str)
        {

            return string.IsNullOrWhiteSpace(str);

        }

        public static bool IsNotNullOrWhiteSpace(this string str)
        {

            return !string.IsNullOrWhiteSpace(str);

        }

        public static decimal StringToDecimal(this string str)
        {
            return decimal.Parse(str);
        }
        public static int StringToInt(this string str)
        {
            return int.Parse(str);
        }

        public static string ToNotNullString(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return obj.ToString();
        }
        public static DateTime ToSettleDT(this string str)
        {
            return DateTime.ParseExact(str, ConstantSUtil.DateTimeFormatSettle, System.Globalization.CultureInfo.CurrentCulture);
        }
        /// <summary>
        /// 时间补0
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeConverter(this string time)
        {
            if (time == null) return string.Empty;
            if (time.Equals("0")) return string.Empty;

            if (time != null && time.Length != 6)
            {
                time = $"0{time}";
            }
            return time;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="str">20190101</param>
        /// <param name="formatstr">ConstantSUtil</param>
        /// <returns></returns>
        public static string ToStrDateTime(this string str, string formatstr = "yyyyMMdd HHmmss", string toformatstr = "yyyy-MM-dd HH:mm:ss")
        {
            if (str.isNull())
            {
                return "";
            }
            try
            {
                var dd = DateTime.ParseExact(str, formatstr, System.Globalization.CultureInfo.CurrentCulture);
                return dd.ToString(toformatstr);

            }
            catch (Exception ex)
            {
                return str;
            }
        }
        public static DateTime? StrToDateTime(this string str, string formatstr = "yyyyMMdd HHmmss")
        {
            if (str.isNull())
            {
                return null;
            }
            try
            {
                var dd = DateTime.ParseExact(str, formatstr, System.Globalization.CultureInfo.CurrentCulture);
                return dd;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        ///  日期加时间转换标准模式
        /// </summary>
        /// <param name="strDate">20190101</param>
        /// <param name="strTime">080808</param>
        /// <returns></returns>
        public static string ToStrDateAddTime(this string strDate, string strTime, string formatstr = "yyyyMMdd HHmmss", string toformatstr = "yyyy-MM-dd HH:mm:ss")
        {
            return $"{strDate} {strTime.TimeConverter()}".ToStrDateTime(formatstr, toformatstr);
        }

        public static int TryToInt(this string str)
        {
            int result = default(int);
            if (string.IsNullOrWhiteSpace(str))
            {
                return result;
            }

            int.TryParse(str, out result);
            return result;
        }


        public static string TryGetArrayByIndex(this string[] strArr, int index)
        {
            if (strArr != null && strArr.Length > index)
            {
                return strArr[index];
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 补空后面数组
        /// </summary>
        /// <param name="strArr"></param>
        /// <param name="allCount">总长度</param>
        /// <param name="defaultstr">默认值</param>
        /// <returns></returns>
        public static string[] AddItemByCount(this string[] strArr, int allCount, string defaultstr = "", bool toUTF8 = true)
        {
            if (strArr != null && strArr.Length < allCount)
            {
                var getAlllist = strArr.ToList();
                for (int i = strArr.Length; i < allCount; i++)
                {
                    getAlllist.Add(defaultstr);
                }
                //if (toUTF8)
                //{
                //    var getStr = string.Join(",", getAlllist).get_uft8();
                //    return getStr.Split(',');
                //}
                return getAlllist.ToArray();
            }
            return strArr;
        }

        public static string TryToString(this object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString();
        }

        public static double TryToDouble(this object obj, double defaultvalue = 0)
        {
            if (obj == null)
                return default(double);
            else
            {
                double tmp = 0;
                double.TryParse(obj.TryToString(), out tmp);
                return tmp;
            }
        }
        public static long TryToLong(this object obj, double defaultvalue = 0)
        {
            if (obj == null)
                return default(long);
            else
            {
                long tmp = 0;
                long.TryParse(obj.TryToString(), out tmp);
                return tmp;
            }
        }
        public static decimal TryToDecimal(this object obj)
        {
            if (obj == null)
                return default(decimal);
            else
            {
                decimal tmp;
                decimal.TryParse(obj.TryToString(), out tmp);
                return tmp;
            }
        }

        public static string TryToInterStr(this string obj)
        {
            var deci = obj.TryToDecimal();
            return ((int)deci).ToString();
        }


        public static bool HasItem<TSource>(this IEnumerable<TSource> source)
        {

            if (source == null)
            {
                return false;
            }
            else
            {
                return source.Any();
            }
        }

        public static ObservableCollection<TSource> AddToObservableCollection<TSource>(this IEnumerable<TSource> source, ObservableCollection<TSource> target)
        {
            if (target == null || source == null)
            {
                return target;
            }
            foreach (var item in source)
            {
                target.Add(item);
            }
            return target;
        }


        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Parse(str);
        }


        public static DateTime AddWorkDays(this DateTime dt, decimal value)
        {
            if (value > 0)
            {
                for (int i = 0; i < value;)
                {
                    dt = dt.AddDays(1);
                    if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Monday)
                    {
                        i++;
                    }
                }
            }
            else
            {
                for (int i = 0; i > value;)
                {
                    dt = dt.AddDays(-1);
                    if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Monday)
                    {
                        i--;
                    }
                }
            }
            return dt;
        }

        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };
        #region 是否判断
        public static string getIfStr(this int m, string other = "", int isYesI = 1, int isNoI = 0, string isyes = "是", string isNo = "否")
        {
            return m == isYesI ? isyes : m == isNoI ? isNo : other;
        }
        #endregion

        #region 字符串
        public static double ToDouble(this string m, double defaultV = 0)
        {
            double result = 0.00;
            if (double.TryParse(m, out result))
            {
                return result;
            };
            return defaultV;
        }
        public static int ToInt(this string m, int defaultV = 0)
        {
            int result = 0;
            if (int.TryParse(m, out result))
            {
                return result;
            };
            return defaultV;
        }
        public static bool Contains(this string m, List<string> checkV)
        {
            if (m.isNull() || checkV == null)
            {
                return false;
            }
            foreach (var item in checkV)
            {
                if (m.ContainsWithLower(item))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="m"></param>
        /// <param name="checkV"></param>
        /// <returns></returns>
        public static bool ContainsWithLower(this string m, string checkV)
        {
            if (m.isNull() || checkV.isNull())
            {
                return false;
            }
            if (m.ToLower().Contains(checkV.ToLower()))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 文件路径获取内容，（文件）
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string FilePathGetContent(this string m, Encoding encoder)
        {
            string currcontent = string.Empty;
            try
            {
                if (!File.Exists(m))
                {
                    return currcontent;
                }
                using (FileStream fs = File.Open(m, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs, encoder))
                {
                    currcontent = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return currcontent;
            }
            return currcontent;
        }
        public static bool FilePathSaveContent(this string m, string content, Encoding encoder)
        {
            try
            {
                File.WriteAllText(m, content, encoder);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool FileExists(this string m)
        {
            try
            {
                if (m.isNull())
                {
                    return false;
                }
                return File.Exists(m);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool FileDelete(this string m)
        {
            try
            {
                if (m.isNull())
                {
                    return false;
                }
                if (m.FileExists())
                {
                    File.Delete(m);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        ///   <summary>
        ///   给一个字符串进行MD5加密
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        public static string MD5Encrypt(this string strText)
        {
            if (strText.isNull())
            {
                return strText;
            }
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(strText));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }

        /// <summary>
        /// 编码：Base64编码
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string ToBase64String(this string m)
        {
            if (m.isNull())
            {
                return "";
            }
            byte[] bytes = Encoding.Default.GetBytes(m);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 解码：Base64编码
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string Base64GetString(this string m)
        {
            if (m.isNull())
            {
                return "";
            }
            byte[] outputb = Convert.FromBase64String(m);
            return Encoding.Default.GetString(outputb);
        }
        public static string Replace(this string m, List<string> toReplace, string foValue = "")
        {
            if (m.isNull())
            {
                return "";
            }
            foreach (var item in toReplace)
            {
                m.Replace(item, foValue);
            }
            return m;
        }
        public static bool isNull(this string m)
        {
            if (string.IsNullOrEmpty(m) || string.IsNullOrWhiteSpace(m))
            {
                return true;
            }
            return false;
        }
        public static string NullToStr(this object m, string defaultvalue = "", char trimChar = '、')
        {
            if (m == null)
            {
                return defaultvalue;
            }
            return m.ToString().Trim(trimChar);
        }
        /// <summary>
        /// 重复生成记录空白
        /// </summary>
        /// <param name="m"></param>
        /// <param name="genNum"></param>
        /// <param name="formatStr"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string StrFormatNum(this string m, int genNum = 3, string formatStr = "   {0}   {1}", char splitChar = '、')
        {
            if (m == null)
            {
                m = m.NullToStr();
            }
            var splitResult = m.SplitTrim(splitChar);
            string result = "";
            for (int i = 0; i < genNum; i++)
            {
                string nameValue = "      ";
                if (i < splitResult.Count)
                {
                    nameValue = splitResult[i];
                }
                result += string.Format(formatStr, nameValue, splitChar);
            }
            return result.Trim(splitChar);
        }
        /// <summary>
        /// 重复生成字符
        /// </summary>
        /// <param name="m"></param>
        /// <param name="genNum"></param>
        /// <param name="formatStr"></param>
        /// <returns></returns>
        public static string StrFormatNum(this string m, double genNum = 3, string formatStr = " ")
        {
            if (m == null)
            {
                m = m.NullToStr();
            }
            string result = "";
            for (int i = 0; i < genNum; i++)
            {
                result += formatStr;
            }
            return result;
        }
        /// <summary>
        /// 去空，去空格
        /// </summary>
        /// <param name="m"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitTrim(this string m, params char[] separator)
        {
            if (m == null)
            {
                return null;
            }
            var result = new List<string>();
            var resultSplit = m.Split(separator);
            foreach (var item in resultSplit)
            {
                if (!item.isNull())
                {
                    result.Add(item.Trim());
                }
            }
            return result;
        }

        #endregion

        #region 字典相关
        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false) dict.Add(key, value);
            return dict;
        }
        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return !key.NullToStr().isNull() && dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        #endregion
        #region 时间相关
        public static string toFormat(this DateTime m, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (m == null)
            {
                return "";
            }
            return m.ToString(format);
        }
        #endregion

        #region 数据转换
        public static string toEmptyStr(this double m, double defaultV = 0)
        {
            if (m <= defaultV)
            {
                return "";
            }
            return m.ToString();
        }
        #endregion

        #region SQL Getn
        /// <summary>
        /// 生成like sql 语句
        /// </summary>
        /// <param name="m"></param>
        /// <param name="isAnd">0: ,1:and,2:or</param>
        /// <param name="flag">0: XX%,1:%XX,other:%XX%</param>
        /// <returns></returns>
        public static string toLikeSql(this string m, string vaule, int isAnd = 0, int flag = 0)
        {
            if (m.isNull() || vaule.isNull())
            {
                return "";
            }
            string isOr = isAnd == 0 ? "" : isAnd == 1 ? "AND" : "OR";
            switch (flag)
            {
                case 0:
                    return string.Format(" {0} {1} like N'{2}%' ", isOr, m, vaule);
                case 1:
                    return string.Format(" {0} {1} like N'%{2}' ", isOr, m, vaule);
                default:
                    return string.Format(" {0} {1} like N'%{2}%' ", isOr, m, vaule);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="vaule"></param>
        /// <param name="isAnd">0:空,1:and,2:or</param>
        /// <param name="isEquleGeLt">3:=,4:>,5:<,6:!=<,7: is null,8:is NOT NULL /param>
        /// <returns></returns>
        public static string toAndOrSql(this string m, string vaule, int isAnd = 0, int isEquleGeLt = 3)
        {
            if (m.isNull())
            {
                return "";
            }
            string isOr = isAnd == 0 ? "" : isAnd == 1 ? "AND" : "OR";
            var strResult = "";
            switch (isEquleGeLt)
            {
                case 3:
                    strResult = string.Format(" {0} {1} = N'{2}' ", isOr, m, vaule);
                    break;
                case 4:
                    strResult = string.Format(" {0} {1} > '{2}' ", isOr, m, vaule);
                    break;
                case 5:
                    strResult = string.Format(" {0} {1} < '{2}' ", isOr, m, vaule);
                    break;
                case 6:
                    strResult = string.Format(" {0} {1} != N'{2}' ", isOr, m, vaule);
                    break;
                case 7:
                    strResult = string.Format(" {0} {1} is NULL ", isOr, m, vaule);
                    break;
                case 8:
                    strResult = string.Format(" {0} {1} is NOT NULL ", isOr, m, vaule);
                    break;
                default:
                    strResult = string.Format(" {0} {1} = N'{2}' ", isOr, m, vaule);
                    break;
            }

            return strResult;

        }
        #endregion
        public static T JsonTo<T>(this object m)
        {
            if (m == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(m.NullToStr(), settings);
        }
        public static string toJsonStr(this object m)
        {
            if (m == null)
            {
                return "";
            }
            return JsonConvert.SerializeObject(m, settings);
        }
        public static int TryToInt(this object obj)
        {
            int tmp = default(int);
            if (obj == null)
                return tmp;
            else
            {
                int.TryParse(obj.TryToString(), out tmp);
            }
            return tmp;
        }

        public static decimal? TryToDecimalOrNull(this object obj)
        {
            if (obj == null)
                return null;
            else
            {
                decimal tmp;
                if (decimal.TryParse(obj.TryToString(), out tmp))
                    return tmp;
                else
                    return null;
            }
        }
        public static decimal TryToDecimal(this object obj, decimal defaultvalue = 0)
        {
            if (obj == null)
                return defaultvalue;
            else
            {
                decimal tmp;
                if (decimal.TryParse(obj.TryToString(), out tmp))
                    return tmp;
                else
                    return defaultvalue;
            }
        }

        public static sbyte TryToSByte(this object obj)
        {
            if (obj == null)
                return default(sbyte);
            else
            {
                sbyte tmp;
                sbyte.TryParse(obj.TryToString(), out tmp);
                return tmp;
            }
        }

        public static DateTime? TryToDetaTime(this object obj)
        {
            if (obj == null)
                return null;
            else
            {
                try
                {
                    DateTime tmp;
                    tmp = DateTime.ParseExact(obj.TryToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    return tmp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //public static DataTable CopyToDataTableExt(this DataRow[] rows, DataTable defaultTable)
        //{
        //    if (rows == null || rows.Length == 0)
        //    {
        //        return defaultTable.Clone();
        //    }
        //    return rows.CopyToDataTable();
        //}

        /// <summary>
            /// 获取枚举的描述信息
            /// </summary>
        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }
    }

    public class ConstantSUtil
    {
        /// <summary>
        /// 时间格式
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DateTimeFileFormat = "yyyy-MM-dd HHmmss";
        public const string DateTimeFormatShot = "yyyy-MM-dd";
        public const string DateTimeFormatSettle = "yyyyMMdd";
        public const string DateTimeFormatAll = "yyyy-MM-dd HH:mm:ss.fff";
        public const string UserID = "UserID";
        public const string RecoverData = "RecoverData";

    }
    public enum DateTimeFormatEnum
    {
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        [Description("yyyy-MM-dd HH:mm:ss")]
        DateTimeFormat = 0,
        /// <summary>
        /// yyyy-MM-dd HHmmss
        /// </summary>
        [Description("yyyy-MM-dd HHmmss")]
        DateTimeFileFormat = 1,
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        [Description("yyyy-MM-dd")]
        DateTimeFormatShot = 2,
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        [Description("yyyyMMdd")]
        DateTimeFormatSettle = 3,
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        [Description("yyyy-MM-dd HH:mm:ss.fff")]
        DateTimeFormatAll = 4,

    }
    /// <summary>
    /// 时间拓展累
    /// </summary>
    public static class DateTimeExtend
    {
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatEnum"></param>
        /// <returns></returns>
        public static string FormatExt(this DateTime obj, DateTimeFormatEnum formatEnum)
        {
            if (obj == null) return string.Empty;
            return obj.ToString(formatEnum.GetDescription());
        }
        public static string ToyyyyMMddExt(this DateTime obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString("yyyyMMdd");
        }

        public static string ToyyyyMMddExt(this DateTime? obj)
        {
            if (obj == null)
                return string.Empty;
            else
            {
                DateTime dt = (DateTime)obj;
                return dt.ToString("yyyyMMdd");
            }
        }

        public static string ToHHmmssExt(this DateTime obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString("HHmmss");
        }

        public static string ToHHmmssfffExt(this DateTime obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString("HHmmssfff");
        }

        public static string ToyyyyMMddHHmmssExt(this DateTime obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString("yyyyMMddHHmmss");
        }

        public static DateTime? TryParseExt(this string dateStr)
        {
            DateTime dateTime = DateTime.Now;
            if (DateTime.TryParse(dateStr, out dateTime))
            {
                return dateTime;
            }
            return null;
        }

        /// <summary>
        /// 时分秒时间补零  传入数据为93000等格式,即为9点30，返回093000
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回补零后的字符串</returns>
        public static string TimeStrAddZeroExt(this string str)
        {
            if (str != null || str.Length < 6)
            {
                StringBuilder sb = new StringBuilder();
                var length = str.Length;
                for (int i = length; i < 6; i++)
                {
                    sb.Append("0");
                }

                return $"{sb.ToString()}{str}";
            }
            return str;
        }
        public static string get_uft82(this string unicodeString)
        {
            if (unicodeString.isNull())
            {
                return "";
            }
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }
        /// <summary>
        /// 以UTF-8带BOM格式返回utf8编码
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string get_uft8(this string text)
        {

            if (text.isNull())
            {
                return "";
            }
            //声明字符集   
            System.Text.Encoding utf8, utf8bom;

            //Default   //以UTF-8带BOM格式读取文件内容
            utf8bom = new UTF8Encoding(true);
            //utf8   
            utf8 = new UTF8Encoding(false);
            byte[] gb;
            gb = utf8bom.GetBytes(text);
            gb = System.Text.Encoding.Convert(utf8bom, utf8, gb);
            //返回转换后的字符   
            var result = utf8.GetString(gb);

            //using (var sink = new StreamWriter("FoobarNoBom.txt", false, utf8WithoutBom))
            //{
            //    sink.WriteLine(result);
            //}
            //using (var sink = new StreamWriter("FoobarBom.txt", false, utf8bom))
            //{
            //    sink.WriteLine(result);
            //}

            return result;
        }
        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string gb2312_utf8(this string text)
        {
            if (text.isNull())
            {
                return "";
            }
            //声明字符集   
            System.Text.Encoding utf8, gb2312;

            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(this string text)
        {
            if (text.isNull())
            {
                return "";
            }
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }

        /// <summary>
        /// 转换成json树状结构的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertJsonString(this string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }

    public static class GetDescriptionExt
    {

        /// <summary>
        /// 获取类属性对应
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDescriptionAll(this object classObject)
        {
            Type type = classObject.GetType();
            var fds = type.GetProperties();
            var dicResult = new Dictionary<string, string>();
            foreach (var fd in fds)
            {
                if (fd == null)
                    continue;
                object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string name = string.Empty;
                foreach (DescriptionAttribute attr in attrs)
                {
                    name = attr.Description;
                }
                dicResult[fd.Name] = name;
            }
            return dicResult;
        }
        /// <summary>
        /// 获取类属性对应Description
        /// </summary>
        /// <param name="classObject"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetDescriptionAll(this object classObject, string fieldName)
        {
            Type type = classObject.GetType();
            var fd = type.GetProperty(fieldName);
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }
        public static Dictionary<string, string> GetKeyDescFromDic(this Dictionary<string, string> dic, Dictionary<string, string> sources)
        {
            var result = new Dictionary<string, string>();
            foreach (var item in dic)
            {
                var key = $"{item.Key}:{sources.GetValue(item.Key, "")}".Trim(':');
                result.Add(key, item.Value);
            }
            return result;
        } 
    }
}
