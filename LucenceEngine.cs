﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Index;

namespace ProjectA
{
    class LucenceEngine
    {
        private static Lucene.Net.Util.Version Version = Lucene.Net.Util.Version.LUCENE_CURRENT;
        private static StandardAnalyzer Analyzer = new StandardAnalyzer(Version);
        private static string FieldName = "content";

        private string IndexDir;
        private IndexSearcher Searcher;
        private QueryParser Parser;
        private Sort Sort;


        public LucenceEngine(string IndexDir)
        {
            // IndexWriter.MaxFieldLength maxFieldLength = new IndexWriter.MaxFieldLength(int <len>);
            try
            {
                if (System.IO.Directory.Exists(IndexDir) == false) throw new Exception(IndexDir + " Not Exist");
                this.IndexDir = IndexDir;
                Searcher = new IndexSearcher(new SimpleFSDirectory(new DirectoryInfo(IndexDir), new SimpleFSLockFactory())); //,readOnly) 为boolean值
                //Searcher = new IndexSearcher(new MMapDirectory(new DirectoryInfo(IndexDir), new SimpleFSLockFactory())); //,readOnly) 为boolean值
                Sort = new Sort(); //(new SortField(FieldName, SortField.SCORE, true));   //fieldName can be null
                Parser = new QueryParser(Version, FieldName, Analyzer);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void ReadDocs(string DocsDir, string IndexDir, bool isNewCreate = true)
        {
            try
            {
                if (System.IO.File.Exists(DocsDir) == false) throw new Exception(DocsDir + " Not Exist");
                IndexWriter Writer = InitWriter(IndexDir, isNewCreate);

                FileStream fs = new FileStream(DocsDir, FileMode.Open);

                //int count = 1;

                StreamReader Reader = new StreamReader(fs);
                while (!Reader.EndOfStream)
                {
                    //Document doc = new Document();
                    //Field field = new Field("name", "content", Field.Store.YES, Field.Index.ANALYZED);
                    //doc.Add(field);
                    //writer.AddDocument(doc);

                    Document d = new Document();
                    d.Add(new Field(FieldName, Reader.ReadLine(), Field.Store.YES, Field.Index.ANALYZED));
                    Writer.AddDocument(d);

                    //if (count == 200000) break;
                    //count++;
                }

                Reader.Close();
                fs.Close();

                Writer.Optimize();
                Writer.Commit();
                Writer.Rollback();
                Writer.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static IndexWriter InitWriter(string IndexDir, bool isNewCreate = true)
        {
            IndexWriter.MaxFieldLength maxFieldLength = IndexWriter.MaxFieldLength.LIMITED;
            Lucene.Net.Store.Directory Directory = new MMapDirectory(new DirectoryInfo(IndexDir), new SimpleFSLockFactory()); //using SimpleFSDirectory if memory is not enough
            return new IndexWriter(Directory, Analyzer, isNewCreate, maxFieldLength);
        }

        public string[] Search(string s, int MaxDoc = 10)
        {
            // MaxDoc = Searcher.MaxDoc;
            Query q = Parser.Parse(s);
            TopFieldDocs hits = Searcher.Search(q, null, MaxDoc, Sort);
            ScoreDoc[] scoreDocs = hits.ScoreDocs;
            int docCount = scoreDocs.Length;
            string[] result = new string[docCount];
            for (int i = 0; i < docCount; i += 1) result[i] = Searcher.Doc(scoreDocs[i].Doc).Get(FieldName);

            return result;
        }

        public string[] MultiSearch(string s1,string s2, int MaxDoc = 5)
        {
            // MaxDoc = Searcher.MaxDoc;
            BooleanQuery q = new BooleanQuery();
            q.Add(Parser.Parse(s1), Occur.MUST);
            q.Add(Parser.Parse(s2), Occur.MUST);
            TopFieldDocs hits = Searcher.Search(q, null, MaxDoc, Sort);
            ScoreDoc[] scoreDocs = hits.ScoreDocs;
            int docCount = scoreDocs.Length;
            string[] result = new string[docCount];
            for (int i = 0; i < docCount; i += 1) result[i] = Searcher.Doc(scoreDocs[i].Doc).Get(FieldName);

            return result;
        }

        ~LucenceEngine()
        {
            Searcher.Dispose();
        }
    }
}
