import sys
import win32file
import win32pipe
import time

class PipeEngine:
    __slots__ = ['PipeName','PipeServer','IOBuffSize']
    def __init__(self,PipeName):
        try:
            self.IOBuffSize = 65535
            self.PipeName = PipeName
            self.PipeServer = win32pipe.CreateNamedPipe(PipeName,
                                               win32pipe.PIPE_ACCESS_DUPLEX,
                                               win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_WAIT | win32pipe.PIPE_READMODE_MESSAGE,
                                               win32pipe.PIPE_UNLIMITED_INSTANCES,
                                               self.IOBuffSize,
                                               self.IOBuffSize, 500, None)

            win32pipe.ConnectNamedPipe(self.PipeServer)

            result = win32file.ReadFile(self.PipeServer, self.IOBuffSize, None)[1].decode('utf-8')
            assert result == "csharpPipe";

            win32file.WriteFile(self.PipeServer, bytes("pythonPipe", encoding='utf-8'))

        except Exception as e:
            print(e)
            print(PipeName)

    def StartListening(self,func):
        while True:
            try:
                result = win32file.ReadFile(self.PipeServer, self.IOBuffSize, None)[1].decode('utf-8')
                win32file.WriteFile(self.PipeServer, bytes(func(result), encoding='utf-8'))
            except Exception as e:
                print(e)



def func(string):
    return string

if __name__=="__main__":
    gpus = sys.argv[1]
    pipe = PipeEngine(gpus)
    print("running")
    pipe.StartListening(func)
    x = input()