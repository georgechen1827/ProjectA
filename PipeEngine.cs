using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;
using System.Timers;

namespace ProjectA
{
    class PipeEngine
    {
        private string PipeName;
        private int IOBuffSize;
        private NamedPipeClientStream Client;
        private Process P;
        private int timeout;
        public byte[] result;
        public int resultLength;

        public PipeEngine(string PipeName,int IOBuffSize = 65535,int timeout = 1000)
        {
            this.timeout = timeout;
            this.PipeName = PipeName;
            this.IOBuffSize = IOBuffSize;
            Client = new NamedPipeClientStream(PipeName);
            result = new byte[IOBuffSize];
            P = new Process();  
            P.StartInfo.FileName = "python\\pipe.py";   //设置要启动的应用程序
            P.StartInfo.Arguments = "\\\\.\\pipe\\" + PipeName; //设置启动参数， 详见pipe.py
            P.StartInfo.UseShellExecute = true;    //是否使用操作系统shell启动
            P.StartInfo.CreateNoWindow = true;  //不显示程序窗口
        }

        public void StartEngine()
        {
            try
            {   
                //启动程序
                P.Start();
                //P = Process.Start("python\\pipe.py", "\\\\.\\pipe\\" + PipeName);
                Client.Connect(timeout);
                string s = "csharpPipe";

                result.Initialize();
                resultLength = Encoding.UTF8.GetBytes(s, 0, s.Length, result, 0);
                Client.Write(result, 0, resultLength);

                result.Initialize();
                Thread T = new Thread(()=> { resultLength = Client.Read(result, 0, IOBuffSize); });
                T.Start();
                T.Join(timeout);
                s =  Encoding.UTF8.GetString(result, 0, resultLength);

                if (s != "pythonPipe") throw new Exception("Connection Failed");
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public string Request(string request)
        {
            try
            {
                result.Initialize();
                resultLength = Encoding.UTF8.GetBytes(request, 0, request.Length, result, 0);
                Client.Write(result, 0, resultLength);

                result.Initialize();
                Thread T = new Thread(() => { resultLength = Client.Read(result, 0, IOBuffSize); });
                T.Start();
                T.Join(timeout);
                return Encoding.UTF8.GetString(result, 0, resultLength);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void CloseEngine()
        {
            P.Kill();
            Client.Close();
        }
        ~PipeEngine()
        {
            P.Kill();
            Client.Close();
        }
    }
}
