using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;
using System.Timers;
using System.IO;

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

        public PipeEngine(string PipeName,string FileName = "python\\pipe.py",int IOBuffSize = 65535,int timeout = 1000)
        {
            this.timeout = timeout;
            this.PipeName = PipeName;
            this.IOBuffSize = IOBuffSize;
            Client = new NamedPipeClientStream(PipeName);
            result = new byte[IOBuffSize];
            P = new Process();
            P.StartInfo.FileName = "python.exe";   //设置要启动的应用程序
            P.StartInfo.Arguments = FileName+" \\\\.\\pipe\\" + PipeName; //设置启动参数， 详见pipe.py
            P.StartInfo.UseShellExecute = true;    //是否使用操作系统shell启动
            P.StartInfo.CreateNoWindow = true;  //不显示程序窗口
            P.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
            CheckPythonEnviroment();
        }

        private static void CheckPythonEnviroment()
        {
            if(!System.IO.File.Exists("PythonHasInstalled"))
            try
            {
                Process P1 = Process.Start("python.exe","python\\pakcheck.py");
                FileStream fs1 = new FileStream("PythonHasInstalled", FileMode.Create, FileAccess.Write);
                fs1.Close();
            }
            catch
            {
                Process P2 = Process.Start("resources\\python373.exe");
                P2.WaitForExit();
                Process P3 = Process.Start("python.exe", "python\\pakcheck.py");
                P3.WaitForExit();
            }
        }
        public void StartEngine()
        {
            try
            {   
                //启动程序
                P.Start();
                //P = Process.Start("python\\pipe.py", "\\\\.\\pipe\\" + PipeName);
                //Client.Connect(timeout*10);
                while (!Client.IsConnected)
                {
                    if (!P.HasExited)
                        try
                        {
                            Client.Connect(timeout * 10);
                        }
                        catch
                        {
                            ;
                        }
                    else break;
                }
                
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
            if (!P.HasExited) P.Kill();
            Client.Close();
        }
        ~PipeEngine()
        {
            if(!P.HasExited) P.Kill();
            Client.Close();
        }
    }
}
