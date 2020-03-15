import os,nltk

filePath = 'D:/pdf2txts/'
lst = os.listdir(filePath)
outlines = []

print(len(lst))
count = 0
for i in lst:
    try:
        count += 1
        f = open(filePath+i,'r',encoding='utf-8')
        lines = f.readlines()
        f.close()

        for line in lines:
            if line.strip() == '' or line[0] == '#' or line[0] == '.':
                continue
            for j in nltk.sent_tokenize(line):
                if(len(j)<=32):
                    continue
                    # print(j)
                outlines.append(j + '\n')

        print(count,' ',i)
    except:
        with open('process_failed.txt', 'a', encoding='utf-8') as f:
            f.write(filePath + i + '\n')
            f.close()

print('writing...')
outlines = [i for i in set(outlines)]
f = open('Docs.txt','w',encoding='utf-8')
f.writelines(outlines)
f.close()