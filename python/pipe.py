try:
    import random
    import os,sys
    import win32file
    import win32pipe
    import nltk
    from nltk.corpus import wordnet

except Exception as e:
    if os.path.isfile("pakfailed"):
        input("环境配置失败,请关闭窗口")
        exit()
    if os.path.isfile("pakchecked"):
        f = open("pakfailed","w")
        f.close()
    else:
        f = open("pakchecked","w")
        f.close()

    from pakcheck import check
    check()
    python = sys.executable
    os.execl(python, python, *sys.argv)

pos_dct = {'NN':'n', 'NNS':'n', 'NNP':'n', 'NNPS':'n',
           'VB':'v', 'VBD':'v', 'VBG':'v', 'VBN':'v', 'VBP':'v', 'VBZ':'v', 'MD':'v',
           'RB':'r', 'RBR':'r', 'RBS':'r',
           'JJ':'a', 'JJR':'a', 'JJS':'a'}

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
            input('管道连接失败，请关闭窗口')
            exit()

    def StartListening(self,func):
        while True:
            try:
                result = win32file.ReadFile(self.PipeServer, self.IOBuffSize, None)[1].decode('utf-8')
                win32file.WriteFile(self.PipeServer, bytes(func(result), encoding='utf-8'))
            except Exception as e:
                print(e)
                input("管道通信失败，请关闭窗口")
                exit()

def synonyms(word,pos,leng = 5):
    if pos not in pos_dct:
        pos = None
    else:
        pos = pos_dct[pos]
    res = word + ' '
    count = 0
    for i in set([j for i in wordnet.synsets(word,pos) for j in i._lemma_names]):
        res += i + ' '
        count += 1
        if count == leng:
            break
    print(res)
    return res


def func(string):
    try:
        tokens = nltk.word_tokenize(string)
        pos_tags = nltk.pos_tag(tokens)
        word,pos = pos_tags[-1]

        return synonyms(word,pos)

    except:
        return word

if __name__=="__main__":
    try:
        nltk.word_tokenize("hello world")
        wordnet.synsets("world")
        #initialize

        gpus = sys.argv[1]
        pipe = PipeEngine(gpus)
        print("running")
        pipe.StartListening(func)
        input('运行结束，请关闭窗口')

    except Exception as e:
        print(e)
        input("程序错误，请关闭窗口")