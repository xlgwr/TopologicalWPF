using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TopologicalWPF.common
{
    public class FileGetHelper
    {
        public static void OpenFolderAndSelectFile(String fileFullName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + fileFullName;
            System.Diagnostics.Process.Start(psi);
        }
        /// <summary>
        /// 最多50个exe,防爆
        /// </summary>
        public static int maxFileSize { get; set; } = 50;

        public static List<FileInfo> allGetFiles = new List<FileInfo>();

        /// <summary>
        /// 获得目录下所有文件或指定文件类型文件(包含所有子文件夹)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="extName">扩展名可以多个 例如 .exe</param>
        /// <returns>List<FileInfo></returns>
        public static void getFile(string path, string extName, List<string> notName)
        {
            try
            {
                string[] dir = Directory.GetDirectories(path); //文件夹列表   
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                //FileInfo[] file = Directory.GetFiles(path); //文件列表   
                if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空                   
                {
                    foreach (FileInfo f in file) //显示当前目录所有文件   
                    {
                        if (notName.ContainsReAll(f.Name))
                        {
                            continue;
                        }

                        if (extName.StartsWith("."))
                        {
                            if (string.IsNullOrEmpty(f.Extension))
                            {
                                continue;
                            }
                            if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                            {
                                allGetFiles.Add(f);
                                if (allGetFiles.Count >= maxFileSize)
                                {
                                    return;
                                }
                            }

                        }
                        else
                        {
                            if (extName.ToLower() == f.Name.ToLower())
                            {
                                allGetFiles.Add(f);
                                if (allGetFiles.Count >= maxFileSize)
                                {
                                    return;
                                }
                            }
                        }
                    }
                    foreach (string d in dir)
                    {
                        getFile(d, extName, notName);//递归   
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
