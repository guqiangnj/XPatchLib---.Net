﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using XPatchLib.UnitTest.Benchmarks;

namespace XPatchLib.TestConsole
{
    class Program
    {
        static void SaveLastResult()
        {
            string path = @"BenchmarkDotNet.Artifacts\results";

            string resultFile= @"BenchmarkDotNet.Artifacts\results\Result.md";

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "SerializeBenchmarks*.md",
                    SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        File.AppendAllLines(resultFile, new string[]
                        {
                            string.Format("## {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            System.Environment.NewLine,
                            File.ReadAllText(file),
                            System.Environment.NewLine
                        });
                    }
                }
            }
        }

        private static int TIMES = 100;

        static void Main(string[] args)
        {
#if Release
            SaveLastResult();
            string version = FileVersionInfo.GetVersionInfo(typeof(Serializer).Assembly.Location).FileVersion;
            Console.WriteLine("XPatchLib Version: " + version);
            Console.WriteLine(".NET Version: " + Environment.Version);

            new BenchmarkSwitcher(new[]
            {
                typeof(SerializeBenchmarks)
            }).Run(new[] {"*"});

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
#elif DOTTRACE
            for (int i = 0; i < TIMES; i++)
            {
                new SerializeBenchmarks().SerializeLargeXmlFile_XPatchLib();
            }
#elif STOPWATCH
            Stopwatch watch = new Stopwatch();
            List<long> xpatch = new List<long>(TIMES);
            List<long> xml = new List<long>(TIMES);
            SerializeBenchmarks b=new SerializeBenchmarks();
            for (int i=0;i<TIMES;i++)
            {
                Console.WriteLine("{0}/{1}", i, TIMES);
                watch.Restart();
                b.SerializeLargeXmlFile_XPatchLib();
                watch.Stop();
                Console.WriteLine("XPatchLib Serialize:{0}.", watch.ElapsedMilliseconds);
                xpatch.Add(watch.ElapsedMilliseconds);
                watch.Restart();
                b.SerializeLargeXmlFile_XmlSerializer();
                watch.Stop();
                Console.WriteLine("XmlSerializer:{0}.", watch.ElapsedMilliseconds);
                xml.Add(watch.ElapsedMilliseconds);
            }

            Console.WriteLine("XPatchLib Serialize avg:{0}.", xpatch.Average());
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
#endif
        }
    }
}