import os, sys
def check():
    try:
        import nltk
    except:
        os.system("pip3 install --user nltk -i https://pypi.tuna.tsinghua.edu.cn/simple/")

    try:
        import win32file
        import win32pipe
    except:
        os.system("pip3 install --user pywin32 -i https://pypi.tuna.tsinghua.edu.cn/simple/")

    try:
        import nltk
    except:
        os.system("pip install --user --default-timeout=1000 -U nltk")
        import shutil

        source_path = os.path.abspath(r'nltk_data')
        target_path = os.path.abspath(r'D:\nltk_data')

        if not os.path.exists(target_path):
            os.makedirs(target_path)

        if os.path.exists(source_path):
            # root 所指的是当前正在遍历的这个文件夹的本身的地址
            # dirs 是一个 list，内容是该文件夹中所有的目录的名字(不包括子目录)
            # files 同样是 list, 内容是该文件夹中所有的文件(不包括子目录)
            for root, dirs, files in os.walk(source_path):
                for file in files:
                    src_file = os.path.join(root, file)
                    shutil.copy(src_file, target_path)
                    print('copying'+src_file)

if __name__ == "__main__":
    try:
        check()
    except:
        input("环境配置失败,请关闭窗口")

