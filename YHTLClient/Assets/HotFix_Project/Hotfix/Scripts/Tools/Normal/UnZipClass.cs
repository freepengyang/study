
///
/// 解压文件
///
using System.IO;
using System;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;

namespace FileCompress
{
    ///
    /// 功能：解压文件
    /// creator chaodongwang 2009-11-11
    ///
    public class UnRarClass
    {
        private int curSize;
        public int CurSize { get { return curSize; } }
        private int totalSize;
        public int TotalSize { get { return totalSize; } }
        private bool isSucceed = false;
        public bool IsSucceed { get { return isSucceed; } }
        private bool isComplete = false;
        public bool IsComplete { get { return isComplete; } }

        public void StartUnRar(string[] strArr, int totalSize)
        {
            this.totalSize = totalSize;

            Thread thread = new Thread(new ParameterizedThreadStart(UnRar));
            thread.IsBackground = true;
            thread.Start(strArr);
        }

        ///
        /// 功能：解压Rar格式的文件。
        ///
        ///RarFilePath压缩文件路径
        ///unRarDir解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹 ///出错信息 /// 解压是否成功
        //public void UnRar(object RarFilePath, object unRarDir)
        private void UnRar(object obj)
        {
            try
            {
                string[] strArr = obj as string[];
                if (strArr.Length != 2)
                {
                    FNDebug.Log("UnRar Parameters error");
                }

                string RarFilePath = strArr[0];
                string unRarDir = strArr[1];

                if (RarFilePath == string.Empty)
                {
                    throw new Exception("压缩文件不能为空！");
                }
                if (!File.Exists(RarFilePath))
                {
                    throw new System.IO.FileNotFoundException("压缩文件不存在！");
                }
                //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
                if (unRarDir == string.Empty)
                    unRarDir = RarFilePath.Replace(Path.GetFileName(RarFilePath), Path.GetFileNameWithoutExtension(RarFilePath));
                if (!unRarDir.EndsWith("/"))
                    unRarDir += "/";
                if (!Directory.Exists(unRarDir))
                    Directory.CreateDirectory(unRarDir);

                using (ZipInputStream s = new ZipInputStream(File.OpenRead(RarFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unRarDir + directoryName);
                        }
                        if (!directoryName.EndsWith("/"))
                            directoryName += "/";

                        string entryPath = unRarDir + theEntry.Name;
                        if (File.Exists(entryPath))
                        {
                            File.Delete(entryPath);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unRarDir + theEntry.Name))
                            {
                                int size = 4096;
                                byte[] data = new byte[4096];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);

                                    curSize += size;

                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                isSucceed = true;
            }
            catch(System.Exception ex)
            {
                FNDebug.Log("解压资源失败: " + ex.Message);
            }

            isComplete = true;
        }
    }
}