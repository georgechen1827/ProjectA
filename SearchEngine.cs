using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectA
{
    class SearchEngine
    {
        private static string Docs = "resources/Docs.txt";
        private static string Index = "resources/idx";
        private static string PipeName = "ProjectAPipe";
        private static bool UsingPipeEngine = true;

        private PipeEngine PipeEngine;
        private LucenceEngine LucenceEngine;

        public SearchEngine()
        {
            if (System.IO.Directory.Exists(Index) == false) LucenceEngine.ReadDocs(Docs,Index);
            LucenceEngine = new LucenceEngine(Index);

            PipeEngine = null;
            if(UsingPipeEngine)
                try
                {
                    PipeEngine = new PipeEngine(PipeName);
                    PipeEngine.StartEngine();
                }
                catch(Exception e)
                {
                    throw e;
                    UsingPipeEngine = false;
                    PipeEngine = null;
                }
        }

        public string[] Search(string query)
        {
            return LucenceEngine.Search(query);
        }
    }
}
