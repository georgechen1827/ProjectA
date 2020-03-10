using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Diagnostics;

namespace ProjectA
{
    class PipeEngine
    {
        private string PipeName;
        private int IOBuffSize;
        private NamedPipeClientStream Client;
        private Process P;
        public byte[] result;
        public int resultLength;

        public PipeEngine(string PipeName,int IOBuffSize = 65535)
        {
            this.PipeName = PipeName;
            this.IOBuffSize = IOBuffSize;
            Client = new NamedPipeClientStream(PipeName);
            result = new byte[IOBuffSize];
            P = new Process();  
            P.StartInfo.FileName = "python\\pipe.py";   //设置要启动的应用程序
            P.StartInfo.Arguments = "\\\\.\\pipe\\" + PipeName; //设置启动参数， 详见pipe.py
            P.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            P.StartInfo.CreateNoWindow = true;  //不显示程序窗口
        }

        public void StartEngine()
        {
            
            P = Process.Start("python\\pipe.py", "\\\\.\\pipe\\" + PipeName);
            Client.Connect();
            string s = "csharpPipe";

            result.Initialize();
            resultLength = Encoding.UTF8.GetBytes(s, 0, s.Length, result, 0);
            Client.Write(result, 0, resultLength);

            

            result.Initialize();
            resultLength = Client.Read(result, 0, IOBuffSize);
            s =  Encoding.UTF8.GetString(result, 0, resultLength);

            if (s != "pythonPipe") throw new Exception("Connection Failed");

            

            
            //启动程序
            //P.Start();
        }

        public string Request(string request)
        {
            result.Initialize();
            resultLength = Encoding.UTF8.GetBytes(request,0,request.Length,result,0);
            Client.Write(result, 0, resultLength);

            result.Initialize();
            resultLength = Client.Read(result, 0, IOBuffSize);
            return Encoding.UTF8.GetString(result, 0, resultLength);
        }

        ~PipeEngine()
        {
            P.Close();
            Client.Close();
        }
    }
}
