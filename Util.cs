﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace TrabalhoThreads
{
    /* 
     * Classe com funções úteis para agilizar o desenvolvimento 
     * https://medium.com/@mteixeira.dev_48908/entendendo-as-tasks-no-c-e-o-paradigma-async-await-31b2f17cf807
     * https://stackoverflow.com/questions/4488054/merge-two-or-more-lists-into-one-in-c-sharp-net
     */
    
    //Classe auxiliar para armazenar cada informação relevante da manipulação
    public class FileInformations
    {
        public string Filename { get; set; }
        public int WordsCount { get; set; }
        public int VowelsCount { get; set; }
        public int ConsonantsCount { get; set; }
        public string WordMostFrequently { get; set; }
        public string VowelMostFrequently { get; set; }
        public string ConsonantMostFrequently { get; set; }
    }
    
    public class Util
    {
        private static string filepath = "";
        private static List<FileInformations> fileInformations = new List<FileInformations>();
        //Exibir um texto juntamente com uma entrada do usuário 
        public static string ConsoleInOut(string exibitionText)
        {
            Console.WriteLine(exibitionText);
            string s = Console.ReadLine();
            return s.ToLower(); //retorna tudo minúsculo para possíveis comparações
        } 

        //Preencher um vetor com números aleatórios até um tamanho definido
        public static void FillVector(List<int> v, int size)
        {
            for(int i = 0; i < size; ++i)
            {
                v.Add(new Random().Next(0, size));
            }
        }

        public static void PrintVector(List<int> v, string title)
        {
            Console.WriteLine(string.Format("-------- \t{0}\t --------", title));
            for(int i = 0; i < v.Count; ++i)
            {
                Console.WriteLine(string.Format("[{0}]{1}", i, v[i]));
            }
            Console.WriteLine("---------------------------------------");
        }

        private static List<int> _initialVector = new List<int>();
        private static List<int> _invertedVector = new List<int>();

        private static void InvertVectorStepByStep()
        {
            for (int i = _initialVector.Count - 1; i >= 0; --i)
                _invertedVector.Add(_initialVector[i]);
        }
        
        public static List<int> InvertVector(List<int> initialVector, int threadsNumber = 0)
        {
            List<int> invertedVector = new List<int>();
            _initialVector = initialVector;
            if(threadsNumber == 0)
            {
                for(int i = initialVector.Count - 1; i >= 0; --i)
                    invertedVector.Add(initialVector[i]);
            }
            else
            {
                for(int i = 0; i < threadsNumber; ++i)
                {
                    Thread t = new Thread(new ThreadStart(InvertVectorStepByStep));
                    t.Start();
                }
            }
            return _invertedVector;
        }
        
        //Define caminho de arquivos para utilizar nas funções de manipulação de arquivo
        public static void DefineFilePath(string fp)
        {
            filepath = fp;
            foreach (string f in Directory.GetFiles(filepath, "*.txt"))
                fileInformations.Add(new FileInformations { Filename = f });
        }

        public static void ShowFileInformations()
        {
            foreach(FileInformations f in fileInformations)
            {
                string[] aux = f.Filename.Split('\\');
                string filename = aux[aux.Length - 1];
                Console.WriteLine("");
                Console.WriteLine(string.Format("Arquivo ({0})\n({1}) palavras\n({2}) vogais\n({3}) consoantes\n" +
                    "Palavra mais frequente é ({4})\nVogal mais frequente é ({5})\nConsoante mais frequente é ({6})", 
                    filename, f.WordsCount, f.VowelsCount, f.ConsonantsCount, f.WordMostFrequently, 
                    f.VowelMostFrequently, f.ConsonantMostFrequently));
                Console.WriteLine("");
            }

        }

        public static void FileManipulation(int threadsNumber)
        {
            string[] vowels = new string[] { "a", "e", "i", "o", "u" };
            string[] consonants = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

            try
            {
                //for(int i = 0; i < threadsNumber; ++i)
                //{
                //    Task task = new Task(() => {
                        
                //    });
                //}
                foreach(FileInformations f in fileInformations)
                {
                    List<char> vowelsList = new List<char>();
                    List<char> consonantsList = new List<char>();
                    StreamReader r = File.OpenText(f.Filename);
                    if(r != null)
                    {
                        string allText = r.ReadToEnd().ToLower(); //evita diferenciação entre maiúscula e minúscula, deixando tudo minúsculo para comparações
                        string[] words = allText.Split(' ');
                        f.WordsCount = words.Length;

                        for(int i = 0; i < allText.Length; ++i)
                        {
                            for (int v = 0; v < vowels.Length; ++v)
                            {
                                if (allText[i] == Convert.ToChar(vowels[v]))
                                    vowelsList.Add(allText[i]);
                            }
                            for(int c = 0; c < consonants.Length; ++c)
                            {
                                if (allText[i] == Convert.ToChar(consonants[c]))
                                    consonantsList.Add(allText[i]);
                            }                            
                        }

                        f.VowelsCount = vowelsList.Count;
                        f.ConsonantsCount = consonantsList.Count;
                        f.WordMostFrequently = words.Where(x => x.Length > 1).GroupBy(x => x).Select(x => x).OrderByDescending(x => x.Count()).First().Key;
                        f.VowelMostFrequently = vowelsList.GroupBy(x => x).OrderByDescending(xs => xs.Count()).Select(xs => xs.Key).First().ToString();
                        f.ConsonantMostFrequently = consonantsList.GroupBy(x => x).OrderByDescending(xs => xs.Count()).Select(xs => xs.Key).First().ToString();
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}