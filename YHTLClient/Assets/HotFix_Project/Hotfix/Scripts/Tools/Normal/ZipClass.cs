using System;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
namespace FileCompress
{
    ///
    /// 功能：压缩文件
    /// creator chaodongwang 2009-11-11
    ///
    public class RarClass
    {
        ///
        /// 压缩单个文件
        ///
        ///FileToRar被压缩的文件名称(包含文件路径)
        ///RaredFile压缩后的文件名称(包含文件路径)
        ///CompressionLevel压缩率0（无压缩）-9（压缩率最高）
        ///缓存大小
        public void RarFile(string FileToRar, string RaredFile, int CompressionLevel)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(FileToRar))
            {
                throw new System.IO.FileNotFoundException("文件：" + FileToRar + "没有找到！");
            }
            if (RaredFile == string.Empty)
            {
                RaredFile = Path.GetFileNameWithoutExtension(FileToRar) + ".zip";
            }
            if (Path.GetExtension(RaredFile) != ".zip")
            {
                RaredFile = RaredFile + ".zip";
            }
            ////如果指定位置目录不存在，创建该目录
            //string RaredDir = RaredFile.Substring(0,RaredFile.LastIndexOf(“/”));
            //if (!Directory.Exists(RaredDir))
            // Directory.CreateDirectory(RaredDir);
            //被压缩文件名称
            string filename = FileToRar.Substring(FileToRar.LastIndexOf("//") + 1);
            System.IO.FileStream StreamToRar = new System.IO.FileStream(FileToRar, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream RarFile = System.IO.File.Create(RaredFile);
            ZipOutputStream RarStream = new ZipOutputStream(RarFile);
            ZipEntry RarEntry = new ZipEntry(filename);
            RarStream.PutNextEntry(RarEntry);
            RarStream.SetLevel(CompressionLevel);
            byte[] buffer = new byte[2048];
            System.Int32 size = StreamToRar.Read(buffer, 0, buffer.Length);
            RarStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToRar.Length)
                {
                    int sizeRead = StreamToRar.Read(buffer, 0, buffer.Length);
                    RarStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                RarStream.Finish();
                RarStream.Close();
                StreamToRar.Close();
            }
        }

        ///
        /// 压缩文件夹的方法
        ///
        public void RarDir(string DirToRar, string RaredFile, int CompressionLevel)
        {
            //压缩文件为空时默认与压缩文件夹同一级目录
            if (RaredFile == string.Empty)
            {
                RaredFile = DirToRar.Substring(DirToRar.LastIndexOf("/") + 1);
                RaredFile = DirToRar.Substring(0, DirToRar.LastIndexOf("/")) + "//" + RaredFile + ".zip";
            }
            if (Path.GetExtension(RaredFile) != ".zip")
            {
                RaredFile = RaredFile + ".zip";
            }
            /*using (ZipOutputStream Raroutputstream = new ZipOutputStream(File.Create(RaredFile)))
            {
                Raroutputstream.SetLevel(CompressionLevel);
                Crc32 crc = new Crc32();
                Hashtable fileList = getAllFies(DirToRar);
                foreach (DictionaryEntry item in fileList)
                {
                    FileStream fs = File.OpenRead(item.Key.ToString());
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToRar.Length + 1));
                    entry.DateTime = (DateTime)item.Value;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    Raroutputstream.PutNextEntry(entry);
                    Raroutputstream.Write(buffer, 0, buffer.Length);
                }
            }*/
        }
        ///
        /// 获取所有文件
        ///
        ///
        private Hashtable getAllFies(string dir)
        {
            Hashtable FilesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }
            this.getAllDirFiles(fileDire, FilesList);
            this.getAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        ///
        /// 获取一个文件夹下的所有文件夹里的文件
        ///
        private void getAllDirsFiles(DirectoryInfo[] dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                this.getAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        ///
        /// 获取一个文件夹下的文件
        ///
        ///目录名称 ///文件列表HastTable 
        private void getAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }
    }
}