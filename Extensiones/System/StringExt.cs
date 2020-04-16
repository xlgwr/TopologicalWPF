using ManageServerClient.Shared.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System
{
    public static class StringExt
    {
        /// <summary>
        /// 目录新增
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool FileDirectoryCreateDirectory(this string m)
        {
            try
            {
                if (m.isNull())
                {
                    return false;
                }
                if (!Directory.Exists(m))
                {
                    Directory.CreateDirectory(m);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool FileDirectoryExists(this string m)
        {
            try
            {
                if (m.isNull())
                {
                    return false;
                }
                return Directory.Exists(m);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
