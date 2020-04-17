using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static partial class StringExt
    {
        public static string OpenFileDialog(this string m, Action<string> action, string title = "布局", string filter = "json选择布局 (*.json)|*.json")
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Title = $"选择{title}";
            openFileDialog.InitialDirectory = m;
            if ((bool)openFileDialog.ShowDialog())
            {
                string tmpPath = openFileDialog.FileName;
                action?.Invoke(tmpPath);
                return tmpPath;
            }
            return "";
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="initialDirectory"></param>
        /// <param name="action"></param>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string SaveFileDialog(this string m, string initialDirectory, Action<string> action, string title = "布局", string filter = "json布局文件 (*.json)|*.json")
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = filter;
                saveFileDialog.Title = $"保存当前{title}";
                saveFileDialog.InitialDirectory = initialDirectory;
                var saveName = initialDirectory.Replace(@"\", "`").Replace(@":", "`").Replace(@"``", "`").Split('`');

                var saveNameLast = saveName[saveName.Length - 1];
                if (saveName.Length > 2)
                {
                    saveNameLast = saveName[0] + "_m_" + saveName[saveName.Length - 2] + "_" + saveName[saveName.Length - 1];
                }
                saveFileDialog.FileName = $"{title}_{saveNameLast}_" + DateTime.Now.Date.ToString("yyyyMMdd");
                if ((bool)(saveFileDialog.ShowDialog()))
                {
                    //获得文件路径
                    var localFilePath = saveFileDialog.FileName.ToString();
                    localFilePath.FilePathSaveContent(m, Encoding.UTF8);

                    action?.Invoke(localFilePath);

                    return localFilePath;
                }
                return "";
            }
            catch (Exception ex)
            {
                return m;
            }
        }
    }
}
