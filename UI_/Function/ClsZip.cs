using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justech
{
    /// <summary>
    /// 压缩的方法
    /// </summary>
    public  class ZipFloClass
    {
        public void ZipFile(string strFile, string strZip)
        {
            var len = strFile.Length;
            var strlen = strFile[len - 1];
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile += Path.DirectorySeparatorChar;
            }
            ZipOutputStream outstream = new ZipOutputStream(File.Create(strZip));
            outstream.SetLevel(6);
            zip(strFile, outstream, strFile);
            outstream.Finish();
            outstream.Close();
        }

        public void zip(string strFile, ZipOutputStream outstream, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            //获取指定目录下所有文件和子目录文件名称
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            //遍历文件
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    zip(file, outstream, staticFile);
                }
                //否则，直接压缩文件
                else
                {
                    //打开文件
                    FileStream fs = File.OpenRead(file);
                    //定义缓存区对象
                    byte[] buffer = new byte[fs.Length];
                    //通过字符流，读取文件
                    fs.Read(buffer, 0, buffer.Length);
                    //得到目录下的文件（比如:D:\Debug1\test）,test
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    outstream.PutNextEntry(entry);
                    //写文件
                    outstream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
   

    /// <summary>
    /// 解压方法
    /// </summary>
    public class UnZipFloClass
    {
        public string unZipFile(string TargetFile, string fileDir, ref string msg)
        {
            string rootFile = "";
            msg = "";
            try
            {
                //读取压缩文件（zip文件），准备解压缩
                ZipInputStream inputstream = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
                ZipEntry entry;
                string path = fileDir;
                //解压出来的文件保存路径
                string rootDir = "";
                //根目录下的第一个子文件夹的名称
                while ((entry = inputstream.GetNextEntry()) != null)
                {
                    rootDir = Path.GetDirectoryName(entry.Name);
                    //得到根目录下的第一级子文件夹的名称
                    if (rootDir.IndexOf("\\") >= 0)
                    {
                        rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                    }
                    string dir = Path.GetDirectoryName(entry.Name);
                    //得到根目录下的第一级子文件夹下的子文件夹名称
                    string fileName = Path.GetFileName(entry.Name);
                    //根目录下的文件名称
                    if (dir != "")
                    {
                        //创建根目录下的子文件夹，不限制级别
                        if (!Directory.Exists(fileDir + "\\" + dir))
                        {
                            path = fileDir + "\\" + dir;
                            //在指定的路径创建文件夹
                            Directory.CreateDirectory(path);
                        }
                    }
                    else if (dir == "" && fileName != "")
                    {
                        //根目录下的文件
                        path = fileDir;
                        rootFile = fileName;
                    }
                    else if (dir != "" && fileName != "")
                    {
                        //根目录下的第一级子文件夹下的文件
                        if (dir.IndexOf("\\") > 0)
                        {
                            //指定文件保存路径
                            path = fileDir + "\\" + dir;
                        }
                    }
                    if (dir == rootDir)
                    {
                        //判断是不是需要保存在根目录下的文件
                        path = fileDir + "\\" + rootDir;
                    }
 
                    //以下为解压zip文件的基本步骤
                    //基本思路：遍历压缩文件里的所有文件，创建一个相同的文件
                    if (fileName != String.Empty)
                    {
                        FileStream fs = File.Create(path + "\\" + fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = inputstream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                fs.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        fs.Close();
                    }
                }
                inputstream.Close();
                msg = "解压成功！";
                return rootFile;
            }
            catch (Exception ex)
            {
                msg = "解压失败，原因：" + ex.Message;
                return "1;" + ex.Message;
            }
        }
    }


    //压缩文件
     //private void btnZipFlo_Click(object sender, EventArgs e)
     //   {
     //       string[] strs = new string[2];
     //       //待压缩文件目录
     //       strs[0] = "D:\\DeBug1\\";
     //       //压缩后的目标文件
     //       strs[1] = "D:\\Debug2\\FrpTest.zip";
     //       ZipFloClass zc = new ZipFloClass();
     //       zc.ZipFile(strs[0], strs[1]);
     //   }

    /// <summary>
        /// 批量解压事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnBatchUnZipFlo_Click(object sender, EventArgs e)
        //{
        //    string msg = "";
        //    string path2 = "D:\\Debug2\\";
        //    string path3 = "D:\\Debug3\\";
        //    //获取指定目录下所有文件和子文件名称（所有待解压的压缩文件）
        //    string[] files = Directory.GetFileSystemEntries(path2);
        //    UnZipFloClass uzc = new UnZipFloClass();
        //    //遍历所有压缩文件路径
        //    foreach (string file in files)
        //    {
        //        //获取压缩包名称（包括后缀名）
        //        var filename  = file.Substring(file.LastIndexOf("\\") + 1);
        //        //得到压缩包名称（没有后缀）
        //        filename = filename.Substring(0, filename.LastIndexOf("."));
        //        //判断解压的路径是否存在
        //        if (!Directory.Exists(path3 + filename))
        //        {
        //            //没有，则创建这个路径
        //            Directory.CreateDirectory(path3 + filename);
        //        }
        //        //调用解压方法（参数1：待解压的压缩文件路径（带后缀名），参数2：解压后存放的文件路径，参数3：返工是否解压成功）
        //        uzc.unZipFile(file, path3 + filename, ref msg);
        //    }
        //    MessageBox.Show("批量解压成功");
        //}
    }



