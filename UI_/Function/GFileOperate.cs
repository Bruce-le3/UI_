
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Justech
{
    public sealed class GFileOperate
    {
        #region text操作                               
        public static bool GreatNewFolder(string path)// 根据路径创建路径的所有文件夹
        {
            try
            {
                string[] pathArray;
                string allPath;
                pathArray = path.Split('\\');
                allPath = pathArray[0];
                for (int i = 0; i < pathArray.Length - 1; i++)
                {
                    allPath = allPath + "\\" + pathArray[i + 1];
                    if (Directory.Exists(allPath) == false && pathArray[i + 1].IndexOf(".") == -1)
                    {
                        Directory.CreateDirectory(allPath);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void WriteDataToCsv(string filename, string strWriteData)//写数据到指定的csv文件中
        {
            GreatNewFolder(filename);
            StreamWriter sw;
            if (!File.Exists(filename))
            {
                sw = File.CreateText(filename);
            }
            else
            {
                sw = File.AppendText(filename);
            }
            sw.WriteLine(strWriteData);
            sw.Flush();
            sw.Close();
        }
        public static bool IsFileExist(string path)// 判断文件是否存在
        {
            return File.Exists(path);
        }
      
        public static bool DeleteFile(string path)// 删除指定文件
        {
            try
            {
                if (IsFileExist(path))
                {
                    File.Delete(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public static bool CopyFile(string sourceFileName, string destFileName)// 复制指定文件到目标文件,可更改文件名
        {
            try
            {
                if (IsFileExist(sourceFileName))
                {
                    File.Copy(sourceFileName, destFileName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
      
        public static bool MoveFile(string sourceFileName, string destFileName)// 剪切指定文件到指定路径,可更改文件名
        {
            try
            {
                if (IsFileExist(sourceFileName))
                {
                    File.Move(sourceFileName, destFileName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
       
        public static bool FolderExist(string folderPath)//判断文件夹是否存在
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if (folder.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public static bool DeleteFolder(string path)// 删除指定文件夹-包括里面的文件
        {
            try
            {
                DirectoryInfo folderPath = new DirectoryInfo(path);
                if (folderPath.Exists)
                {
                    folderPath.Delete(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
      
        public static bool CopyFolder(string sourcePath, string destPath)// 复制指定文件夹到指定路径
        {//有bug---------------------------------
            try
            {
                DirectoryInfo folderPath = new DirectoryInfo(sourcePath);
                if (folderPath.Exists)
                {
                    if (!Directory.Exists(destPath))
                        Directory.CreateDirectory(destPath);
                    foreach (string sub in Directory.GetDirectories(sourcePath))
                        CopyFolder(sub + "\\", destPath + Path.GetFileName(sub) + "\\");
                    foreach (string file in Directory.GetFiles(sourcePath))
                        File.Copy(file, destPath + Path.GetFileName(file), true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
       
        public static bool MoveFolder(string sourcePath, string destinationPath)// 剪切指定文件夹到目标路径
        {
            try
            {
                DirectoryInfo folderPath = new DirectoryInfo(sourcePath);
                if (folderPath.Exists)
                {
                    folderPath.MoveTo(destinationPath);
                    System.IO.Directory.Move(@sourcePath, @destinationPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetDefaultConfigPath(string fileName)// 获取默认相对路径下的文件
        {
            string filePath = Directory.GetCurrentDirectory() + @"\Config\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath += "\\" + fileName;
            return filePath;
        }

        public static string deleteOldFile(string fileName)// 删除绝对路径下的文件
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            return fileName;
        }
       
        public static void WritePressureToCSV(string filename, string writeString)
        {
            StreamWriter sw;
            sw = File.CreateText(writeString);
            sw.WriteLine(filename);
            sw.Flush();
            sw.Close();
        }

        public static void WriteTxt(string time, string writedata)// 写生产日志到TXT文件中
        {
            StreamWriter sw;
            string strdata = "";
            strdata = time + "," + writedata;
            string MDataFileName = Directory.GetCurrentDirectory() + @"\日志\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //string MDataFileName = "E:\" + "1.txt";
           // FileOperate.GreatNewFolder(MDataFileName);
            if (!File.Exists(MDataFileName))
            {
                sw = File.CreateText(MDataFileName);
            }
            else
            {
                sw = File.AppendText(MDataFileName);
            }
            sw.WriteLine(strdata);//开始写入值 
            sw.Flush();
            sw.Close();
        }

        public static void WriteTxtAlarm(string writedata)//写报警信息
        {
            StreamWriter sw;
            string strdata = "";
            strdata = writedata;
            string MDataFileName = @"d:\生产数据\报警文件\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (!File.Exists(MDataFileName))
            {
                sw = File.CreateText(MDataFileName);
            }
            else
            {
                sw = File.AppendText(MDataFileName);
            }
            sw.WriteLine(strdata);//开始写入值 
            sw.Flush();
            sw.Close();
        }
        #endregion
        #region  xml文件操作
        public static void CreateXMLFile()
        {
            if (!File.Exists(GetDefaultConfigPath("COCOB.xml")))// 创建默认路径下"COCOB.XML"文件
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration xmldel;
                XmlNode root;
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
        }
      
        public static void CreateXMLFileRP(string fileName)// 创建相对路径下的文件
        {
            if (!File.Exists(GetDefaultConfigPath(fileName + ".xml")))
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration xmldel;
                XmlNode root;
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
        }
       
        public static void CreateXMLFileAP(string fileName)// 创建绝对路径下的文件
        {
            if (!File.Exists(fileName + ".xml"))
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration xmldel;
                XmlNode root;
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
        }
      
        public static bool SaveDataToXMLDP(string fnodename, string cnodename, string cnodetext)// 保存在默认"COCOB.XML"文件中
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            if (!File.Exists(GetDefaultConfigPath("COCOB.xml")))
            {
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
            else
            {
                xml.Load(GetDefaultConfigPath("COCOB.xml"));
                root = xml.SelectSingleNode("Parameter");
                if (root == null)
                {
                    deleteOldFile(GetDefaultConfigPath("COCOB.xml"));
                    booladddata = false;
                    return booladddata;
                }
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    node2.InnerText = cnodetext;
                }
                else
                {
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
            }
            else
            {
                node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                root.AppendChild(node1);
                node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                node2.InnerText = cnodetext;
                node1.AppendChild(node2);
            }
            xml.Save(GetDefaultConfigPath("COCOB.xml"));
            return booladddata;
        }

        public static bool ReadDataFromXMLDP(string fnodename, string cnodename, ref string cnodetext)// 从默认"COCOB.XML"文件中读取
        {
            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(GetDefaultConfigPath("COCOB.xml"));
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            if (root == null)
            {
                boolreaddata = false;
                return boolreaddata;
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    cnodetext = node2.InnerText;
                }
                else
                {
                    cnodetext = "0";
                    boolreaddata = false;
                }
            }
            else
            {
                cnodetext = "0";
                boolreaddata = false;
            }
            return boolreaddata;
        }

        public static bool SaveDataToXMLRP(string filename, string fnodename, string cnodename, string cnodetext)// 保存在相对路径下的文件中
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            if (!File.Exists(GetDefaultConfigPath(filename + ".xml")))
            {
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
            else
            {
                xml.Load(GetDefaultConfigPath(filename + ".xml"));
                root = xml.SelectSingleNode("Parameter");
                if (root == null)
                {
                    deleteOldFile(GetDefaultConfigPath(filename + ".xml"));
                    booladddata = false;
                    return booladddata;
                }
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    node2.InnerText = cnodetext;
                }
                else
                {
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
            }
            else
            {
                node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                root.AppendChild(node1);
                node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                node2.InnerText = cnodetext;
                node1.AppendChild(node2);
            }
            xml.Save(GetDefaultConfigPath(filename + ".xml"));
            return booladddata;
        }

        public static bool ReadDataFromXMLRP(string filename, string fnodename, string cnodename, ref string cnodetext)// 从相对路径下的文件中读取
        {
            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(GetDefaultConfigPath(filename + ".xml"));
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            if (root == null)
            {
                boolreaddata = false;
                return boolreaddata;
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    if (node2.InnerText != "")
                    {
                        cnodetext = node2.InnerText;
                    }
                    else
                    {
                        cnodetext = "0";
                    }
                }
                else
                {
                    cnodetext = "0";
                    boolreaddata = false;
                }
            }
            else
            {
                cnodetext = "0";
                boolreaddata = false;
            }
            return boolreaddata;
        }

        public static bool SaveDataToXMLAP(string filename, string fnodename, string cnodename, string cnodetext)// 保存在绝对路径下的文件中
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            if (!File.Exists(filename + ".xml"))
            {
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
            else
            {
                xml.Load(filename + ".xml");
                root = xml.SelectSingleNode("Parameter");
                if (root == null)
                {
                    deleteOldFile(filename + ".xml");
                    booladddata = false;
                    return booladddata;
                }
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    node2.InnerText = cnodetext;
                }
                else
                {
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
            }
            else
            {
                node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                root.AppendChild(node1);
                node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                node2.InnerText = cnodetext;
                node1.AppendChild(node2);
            }
            xml.Save(filename + ".xml");
            return booladddata;
        }

        public static bool ReadDataFromXMLAP(string filename, string fnodename, string cnodename, ref string cnodetext)// 从绝对路径下的文件中读取
        {
            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(filename + ".xml");
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            if (root == null)
            {
                boolreaddata = false;
                return boolreaddata;
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    if (node2.InnerText != "")
                    {
                        cnodetext = node2.InnerText;
                    }
                    else
                    {
                        cnodetext = "0";
                    }
                }
                else
                {
                    boolreaddata = false;
                }
            }
            else
            {
                boolreaddata = false;
            }

            return boolreaddata;
        }
        #endregion
        
    }
}
