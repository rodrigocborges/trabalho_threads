using System;
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
                        //Cria diretório upper para duplicar os arquivos de texto só que com o conteúdo em maiúsuclo
                        string[] aux = f.Filename.Split('\\');
                        string filename = aux[aux.Length - 1];
                        string dir = filepath + "\\upper\\";
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        File.WriteAllText(dir + filename, allText.ToUpper());
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Classe auxiliar para definir os limites de inicio e fim pegando uma parte do vetor ou matriz para dividir entre threads
        public static void SetSteps(int[] steps, int size, int nThreads)
        {
            int diff = size / nThreads;
            for (int i = 0; i < steps.Length; ++i)
                steps[i] = diff * i;

            int rest = size % nThreads;
            steps[steps.Length - 1] += rest;

            if(rest > 1)
            {
                int aux = 0;
                while(rest > 1)
                {
                    for (int i = steps.Length - 2; i > (1 + aux); --i)
                        steps[i] += 1;
                    --rest;
                    ++aux;
                }
            }
        }

    }
}
