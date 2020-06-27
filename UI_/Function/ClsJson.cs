using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ControlIni;
using Newtonsoft.Json;

namespace Justech
{   
    public class ClsJson
    {
        #region Json类
        public class HiveMachineState
        {
            public int machine_state { get; set; }
            public string state_change_time { get; set; }
            public HiveMachineData data { get; set; }
        }
        public class HiveMachineData
        {
            public string message { get; set; }
            public string code { get; set; }
        }
        public class HiveMachineErrState
        {
            public string message { get; set; }
            public string code { get; set; }
            public string severity { get; set; }
            public string occurrence_time { get; set; }
            public string resolved_time { get; set; }
            public HiveMachineErrDate data { get; set; }
        }

        public class HiveMachineErrDate
        {
            public string key { get; set; }
        }
        #endregion
        public static string strStateChangeTime = "";
        public static string strResolvedTime = "";
        public static string strMessage = "";
        public static string strCode = "";
        public static int intMachineState = 0;

        public static string strKey = "";
        public static string strSeverity = "";
        public static int errorCode = -1;
        public enum MachineState //five machine state
        {
            running = 1,
            idle, //空闲
            engineering,
            plannedDowntime,
            down
        }
        public enum errSeverity //the severity of the erro
        {
            warning=1,
            error,
            critical
        }
        public static string GetmachineTime()//返回系统时间
        {
            return System.DateTime.Now.ToString("yyyy/MM/ddTHH:mm:ss.ff+0800"); 
        }
       
        HiveMachineState hMs = new HiveMachineState();
        HiveMachineData hMd = new HiveMachineData();
        public void UpLoadMachineState(int machineState,string stateChangeTime,string message,string code,int errCode)
        {        
            if(CAMiClsVariable.isHIVEOpen)
            {
                if(errCode != errorCode)
                {
                    hMs.state_change_time = stateChangeTime;
                    hMs.machine_state = machineState;
                    hMd.message = message;
                    hMd.code = code;
                    hMs.data = hMd;
                    errorCode = errCode;
                    //string datatransfer = Convert.ToString(objects1);
                    // var objects = new { message = messagetext, code = codetext, severity = severitytext, occurrence_time = occurrence_timetext, resolved_time = resolved_timetext, data = objects1 };                       
                    //string posturl = "http://10.72.184.161:5000/hello";
                    string postcontent = JsonConvert.SerializeObject(hMs);  //将一个json对象转为json字符串 
                    JstWebServiceAgent.Post(CAMiClsVariable.webServiceStatePath, postcontent);
                }               
            }            
        }

        HiveMachineErrState hMes = new HiveMachineErrState();
        HiveMachineErrDate hMed = new HiveMachineErrDate();
        public void UpLoadMachineErrState(string message, string code, string severity, string occurrence_time, string resolved_time, string key)
        {
            if (CAMiClsVariable.isHIVEOpen)
            {
                //rb1.occurrence_time = System.DateTime.Now.ToString("yyyy/MM/ddTHH:mm:ss.ff+0800");  
                //rb1.message = "Emergency stop button";
                //rb1.code = "F02ESSP-01-01";
                //rb1.severity = "critical";
                //rb1.resolved_time = System.DateTime.Now.ToString("yyyy/MM/ddTHH:mm:ss.ff+0800");
                //dt1.key = "example";
                //rb1.data = dt1;
                hMes.occurrence_time = occurrence_time;
                hMes.resolved_time = resolved_time;
                hMes.message = message;
                hMes.code = code;
                hMed.key = key;
                hMes.data = hMed;
                //var objects1 = new { key = keytext };
                //string datatransfer = JsonConvert.SerializeObject(objects1); //将一个json对象转为json字符串
                ////string datatransfer = Convert.ToString(objects1);
                //var objects = new { message = messagetext, code = codetext, severity = severitytext, occurrence_time = occurrence_timetext, resolved_time = resolved_timetext, data = objects1 };           
                //string postcontent = Convert.ToString(objects);
                //string posturl = "http://10.72.184.161:5000/hello";
                string postcontent = JsonConvert.SerializeObject(hMes);  //将一个json对象转为json字符串
                JstWebServiceAgent.Post(CAMiClsVariable.webServiceEmgPath, postcontent);
            }           
        }      
        public void MachineRunningInfoGet()//running 信息上传
        {
            ClsJson.intMachineState = (int)ClsJson.MachineState.running;
            ClsJson.strStateChangeTime = ClsJson.GetmachineTime();
            ClsJson.strMessage = "Machine is running";
            ClsJson.strCode = "running";

           // UpLoadMachineState(ClsJson.intMachineState, ClsJson.strStateChangeTime, ClsJson.strMessage, ClsJson.strCode);
        }
    }   
}
