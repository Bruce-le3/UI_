using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using System.Web;
using System.Xml;
/*********************************************
* 功能描述：webService通用读取类库。
* Copyright (c) 2010-2017 鹰之眼软件技术有限公司－软件三课
* 创 建 人:满赛
* 日    期:20161229
* 版    本:1.0.0.0
* 修 改 人:满赛
* 修改日期:20170322
* 修改描述:增加get与post通用读取方法
* 版    本:1.1.1.0
* 修 改 人:满赛
* 修改日期:20170510
* 修改描述:增加上传图片
* 版    本:1.1.2.0
* 修 改 人:满赛
* 修改日期:20180915
* 修改描述:1.增加编码配置方式及自动编码方式使用HttpClient
*          2.增加多文件上传方法UploadByHttpClient与下载方法DownLoadByHttpClient
*          3. 编码名称配置详见https://docs.microsoft.com/zh-cn/dotnet/api/system.text.encoding
* 版    本:1.1.2.1
* 修 改 人:满赛
* 修改日期:20180920
* 修改描述:根据客户要求，一次下载2张图片
* 版    本:1.1.2.2
* 修 改 人:满赛
* 修改日期:20181217
* 修改描述:Invoke方法增加自定义字串分隔
* 版    本:1.1.2.3
* 修 改 人:满赛
* 修改日期:20190109
* 修改描述:UploadByHttpClientForVMI方法增加非文件传参
* 版    本:1.1.3.0
* *******************************************/
namespace Justech
{
    /// <summary>
    /// WebService 代理类
    /// </summary>
    public class JstWebServiceAgent
    {
        private object agent;
        private Type agentType;
        private const string CODE_NAMESPACE = "WebServiceAgent.Dynamic";
        private static jsonVMI[] vMI;
        /// <summary<
        /// Constructor
        /// </summary<
        /// <param name="url"<</param<
        public JstWebServiceAgent(string url)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(url + "?wsdl");

                //建立 WSDL格式文档
                ServiceDescription sd = ServiceDescription.Read(reader);

                //客户端代理
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, null, null);

                //使用CodeDom 动态编译代理类
                CodeNamespace cn = new CodeNamespace(CODE_NAMESPACE);
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                Microsoft.CSharp.CSharpCodeProvider icc = new Microsoft.CSharp.CSharpCodeProvider();
                CompilerParameters cp = new CompilerParameters();
                CompilerResults cr = icc.CompileAssemblyFromDom(cp, ccu);
                agentType = cr.CompiledAssembly.GetTypes()[0];
                agent = Activator.CreateInstance(agentType);
                WriteTextLog("日志", "初始化成功", DateTime.Now);
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }

        ///<summary>
        ///调用指定的方法
        ///</summary>
        ///<param name="methodName">方法名</param>
        ///<param name="args">动态参数</param>
        ///<returns>返回 Web service</returns>
        public string Invoke(string methodName, string args)
        {
            try
            {
                MethodInfo mi = agentType.GetMethod(methodName);
                ParameterInfo[] arrpi = mi.GetParameters();
                var pars = args.Trim().Split(',');
                ArrayList arr = new ArrayList();
                for (int i = 0; i < pars.Length; i++)
                {
                    if (arrpi[i].ParameterType != typeof(string))
                    {

                        arr.Add(Convert.ChangeType(pars[i], arrpi[i].ParameterType));
                    }
                    else
                    {
                        arr.Add(pars[i].Replace("#",","));
                    }
                }
                object ob = this.Invoke(mi, arr.ToArray());
                WriteTextLog("日志", ob.ToString(), DateTime.Now);
                return ob.ToString();
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        /// <summary>
        /// 调用指定的方法
        /// </summary>
        /// <param name="split">分隔符号</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">动态参数</param>
        /// <returns></returns>
        public string Invoke(string split, string methodName, string args)
        {
            try
            {
                MethodInfo mi = agentType.GetMethod(methodName);
                ParameterInfo[] arrpi = mi.GetParameters();
                var pars = args.Trim().Split(new[] { split }, StringSplitOptions.None);
                ArrayList arr = new ArrayList();
                for (int i = 0; i < pars.Length; i++)
                {
                    if (arrpi[i].ParameterType != typeof(string))
                    {

                        arr.Add(Convert.ChangeType(pars[i], arrpi[i].ParameterType));
                    }
                    else
                    {
                        arr.Add(pars[i]);
                    }
                }
                object ob = this.Invoke(mi, arr.ToArray());
                WriteTextLog("日志", ob.ToString(), DateTime.Now);
                return ob.ToString();
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        ///<summary>
        ///调用方法
        ///</summary>
        ///<param name="method">方法</param>
        ///<param name="args">参数</param>
        ///<returns>返回 Web service</returns>
        public object Invoke(MethodInfo method, params object[] args)
        {
            return method.Invoke(agent, args);
        }

        public MethodInfo[] Methods
        {
            get
            {
                return agentType.GetMethods();
            }
        }
        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"NetLog\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + time.ToString("yyyyMMdd") + ".txt";
            StringBuilder str = new StringBuilder();//什么时候开始使用stringbuilder
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 以Post方式提交命令
        /// </summary>
        public static string Post(string apiurl, string jsonString)
        {
            return Post(apiurl, jsonString, "utf-8");
        }
        /// <summary>
        /// 以Post方式提交命令
        /// </summary>
        public static string Post(string apiurl, string jsonString, string codingName)
        {
            try
            {
                WebRequest request = WebRequest.Create(@apiurl);
                request.Method = "POST";
                request.ContentType = "application/json";

                byte[] bs = Encoding.UTF8.GetBytes(jsonString);
                request.ContentLength = bs.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(bs, 0, bs.Length);
                newStream.Close();

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(codingName));
                string resultJson = reader.ReadToEnd();
                return resultJson;
            }
            catch (Exception ex)
            {

                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        public static string Post(string url)
        {
            return PostByCoding(url, "utf-8");
        }
        public static string PostByCoding(string url, string codingName)
        {   
            try
            {
                WebRequest request = WebRequest.Create(@url);
                byte[] bs = Encoding.UTF8.GetBytes(string.Empty);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;
                request.Method = "POST";

                System.IO.Stream RequestStream = request.GetRequestStream();
                RequestStream.Write(bs, 0, bs.Length);
                RequestStream.Close();

                System.Net.WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(codingName));
                string ReturnVal = reader.ReadToEnd();
                reader.Close();
                response.Close();

                return ReturnVal;
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        /// <summary>
        /// Get方式提交
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Get(url, "utf-8");

        }
        /// <summary>
        /// 异步Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetByHttpClient(string url)
        {
            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).Result;
            string msg = response.Content.ReadAsStringAsync().Result;
            return msg;
        }
        /// <summary>
        /// 异步Post
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PostByHttpClient(string url, string pars)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            string[] splits = pars.Split('&');
            foreach (var item in splits)
            {
                string[] keys = item.Split('=');
                string value = string.Empty;
                if (keys.Length > 1)
                {
                    value = keys[1];
                }
                form.Add(new StringContent(value), keys[0]);
            }
            var response = httpClient.PostAsync(new Uri(url), form).Result;
            string msg = response.Content.ReadAsStringAsync().Result;
            return msg;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json">json字符串</param>
        /// <param name="filePath">文件保存路径</param>
        /// <returns></returns>
        public static string DownLoadByHttpClient(string url, string json, string filePath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                StringContent content = new StringContent(json);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = httpClient.PostAsync(new Uri(url), content).Result;
                var stream = response.Content.ReadAsStreamAsync().Result;
                StreamToFile(stream, filePath);
                return filePath;
            }
            catch (Exception ex)
            {                
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                return "";
            }
           
        }
        /// <summary>
        /// VMI个案开发
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath">保存的文件夹路径</param>
        /// <returns></returns>
        public static string DownLoadForVMI(string url, string filePath)
        {
            try
            {
                if (vMI == null)
                {
                    return "";
                }
                else
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(vMI[0]);
                    string result1=DownLoadByHttpClient(url + "/" + vMI[0].image, json, filePath+ vMI[0].image);
                    json=js.Serialize(vMI[1]);
                    string result2=DownLoadByHttpClient(url + "/" + vMI[1].image, json, filePath + vMI[1].image);
                    vMI = null;
                    return result1+","+ result2;
                }
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }
            
        }
        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
        /// <summary>
        /// Get方式提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="codingName">utf-8</param>
        /// <returns></returns>
        public static string Get(string url, string codingName)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("charset", codingName);

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding(codingName));
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {

                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PostData(string url, string Pars)
        {
            return PostData(url, Pars, "utf-8");
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PostData(string url, string Pars, string codingName)
        {
            try
            {
                WebRequest request = WebRequest.Create(@url);
                byte[] bs = Encoding.UTF8.GetBytes(Pars);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;
                request.Method = "POST";

                System.IO.Stream RequestStream = request.GetRequestStream();
                RequestStream.Write(bs, 0, bs.Length);
                RequestStream.Close();

                System.Net.WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(codingName));
                string ReturnVal = reader.ReadToEnd();
                reader.Close();
                response.Close();

                return ReturnVal;
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        public static string UploadByWebClient(string url, string Pars, string fileFormName, string filePath)
        {
            return UploadByWebClient(url, Pars, filePath, "utf-8");
        }
        public static string UploadByWebClient(string url, string Pars, string fileFormName, string filePath, string codingName)
        {
            try
            {
                var webclient = new WebClient();
                NameValueCollection nvc = new NameValueCollection();
                string[] splits = Pars.Split('&');
                foreach (var item in splits)
                {
                    string[] keys = item.Split('=');
                    string value = string.Empty;
                    if (keys.Length > 1)
                    {
                        value = keys[1];
                    }
                    nvc.Add(keys[0], value);
                }
                webclient.QueryString = nvc;
                byte[] buffer = webclient.UploadFile(url, "POST", filePath);
                var msg = Encoding.GetEncoding(codingName).GetString(buffer);
                return msg;
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Pars"></param>
        /// <param name="fileFormName">服务端标记key（不是文件名）</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string UploadPostData(string url, string Pars, string fileFormName, string filePath)
        {
            if (Directory.Exists(filePath))
            {
                filePath = Directory.GetFiles(filePath).First();
            }
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            /*wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;*/
            Stream rs = request.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            string[] splits = Pars.Split('&');
            foreach (var item in splits)
            {
                string[] keys = item.Split('=');
                string value = string.Empty;
                if (keys.Length > 1)
                {
                    value = keys[1];
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, keys[0], value);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            if (!string.IsNullOrEmpty(filePath))
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                /*string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, fileFormName, fileFormName, "text/html");*/
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n\r\n";
                string header = string.Format(headerTemplate, fileFormName, Path.GetFileName(filePath));
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);
                //读取图片流
                byte[] byteFile = File.ReadAllBytes(filePath);
                rs.Write(byteFile, 0, byteFile.Length);
            }
            //结束分隔标记
            byte[] endBoundary = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(endBoundary, 0, endBoundary.Length);
            rs.Close();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                string msg = reader2.ReadToEnd();
                return msg;
            }
            catch (Exception ex)
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                return ex.ToString();
            }
            finally
            {
                request = null;
            }
        }
        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pars">附加参数格式：key=value&key=value</param>
        /// <param name="fileFormName">文件key多个用逗号分开</param>
        /// <param name="filePath">文件路径多个用逗号分开</param>
        /// <returns></returns>
        public static string UploadByHttpClient(string url, string pars, string fileFormName, string filePath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                if (pars.Length > 0)
                {
                    string[] splits = pars.Split('&');
                    foreach (var item in splits)
                    {
                        string[] keys = item.Split('=');
                        string value = string.Empty;
                        if (keys.Length > 1)
                        {
                            value = keys[1];
                        }
                        form.Add(new StringContent(value), keys[0]);
                    }
                }
                string[] fileFormNames = fileFormName.Split(',');
                string[] filePaths = filePath.Split(',');
                for (int i = 0; i < fileFormNames.Length; i++)
                {
                    form.Add(new ByteArrayContent(File.ReadAllBytes(filePaths[i])), fileFormNames[i], Path.GetFileName(filePaths[i]));
                }
                var response = httpClient.PostAsync(new Uri(url), form).Result;
                WriteTextLog("状态", response.ToString(), DateTime.Now);
                string msg = response.Content.ReadAsStringAsync().Result;
                return msg;
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }
        }
        public static string UploadByHttpClientForVMI(string url, string pars, string fileFormName, string filePath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                if (pars.Length > 0)
                {
                    string[] splits = pars.Split('&');
                    foreach (var item in splits)
                    {
                        string[] keys = item.Split('=');
                        string value = string.Empty;
                        if (keys.Length > 1)
                        {
                            value = keys[1];
                        }
                        form.Add(new StringContent(value), keys[0]);
                    }
                }
                string[] fileFormNames = fileFormName.Split(',');
                string[] filePaths = filePath.Split(',');
                for (int i = 0; i < fileFormNames.Length; i++)
                {
                    if (filePaths[i].IndexOf(":")>-0 && filePaths[i].IndexOf(".")>0)
                    {
                        form.Add(new ByteArrayContent(File.ReadAllBytes(filePaths[i])), fileFormNames[i], Path.GetFileName(filePaths[i]));
                    }
                    else
                    {
                        form.Add(new StringContent(filePaths[i]), fileFormNames[i]);
                    }
                    
                }
                var response = httpClient.PostAsync(new Uri(url), form).Result;                
                string str = response.ToString();
                str = str.Substring(0,str.IndexOf("Version")-2);
                WriteTextLog("状态", str, DateTime.Now);
                string msg = response.Content.ReadAsStringAsync().Result;
                
                //[{"elapsed_time":23.23218607902527,"front_new":0,"front_old":0,"image":"HZMC18102600WD_Pre_0120181027_083248_annotated.jpg","side":"FRONT","start_time":"2018-10-29 11:25:44.423392","token":"709c3ba8cf6ef60a80e44191cf58509c"},{"back_new":0,"back_old":0,"elapsed_time":23.23218607902527,"image":"HZMC18102600WD_Pre_0220181027_083304_annotated.jpg","side":"BACK","start_time":"2018-10-29 11:25:44.423392","token":"5ad24ec9fb478beab520c354c9d9bf3c"}]
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                try
                {
                    
                    vMI = js.Deserialize<jsonVMI[]>(msg);
                    list = js.Deserialize<List<Dictionary<string, object>>>(msg);
                    foreach (var item in list)
                    {
                        item.Remove("image");
                        item.Remove("token");
                        item.Remove("side");
                    }
                }
                catch
                {

                }
                WriteTextLog("记录", str + ";" + msg, DateTime.Now);
                return str + ";"+js.Serialize(list);
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }
        }
        /// <summary>
        /// VMI个案开发
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileFormName">文件key多个用逗号分开</param>
        /// <param name="filePath">文件路径多个用逗号分开</param>
        /// <returns></returns>
        public static string UploadForVMI(string url, string fileFormName, string filePath)
        {
            try
            {                                               
                string msg = UploadByHttpClientForVMI(url, "", fileFormName, filePath);                                           
                return msg;
            }
            catch (Exception ex)
            {
                WriteTextLog("错误", ex.ToString(), DateTime.Now);
                throw;
            }

        }
        class jsonVMI
        {
            public int decision { get; set; }
            public string image { get; set; }
            public string side { get; set; }            
            public string token { get; set; }
        }
    }
}
