using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace TrabalhoThreads
{
    //Classe auxiliar para manipular arquivos
    public class FileManipulation
    {
        public List<FileInformations> fileInformations = new List<FileInformations>();
        
        private string directory = string.Empty;
        private string directoryUpper = string.Empty;

        private string[] vowels = new string[] { "a", "e", "i", "o", "u" };
        private string[] consonants = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

        public FileManipulation(string dir)
        {
            directory = dir;
            directoryUpper = directory + "\\upper\\";
            try
            {
                if (!Directory.Exists(directoryUpper))
                    Directory.CreateDirectory(directoryUpper);

                //Pega todos os arquivos txt do diretório e adiciona a lista de FileInformations preenchendo o nome do arquivo
                foreach (string f in Directory.GetFiles(directory, "*.txt"))
                    fileInformations.Add(new FileInformations { Filename = f });
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ShowFileInformations()
        {
            foreach (FileInformations f in fileInformations)
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

        public void Manipulate(FileInformations f)
        {
            try
            {
                List<char> vowelsList = new List<char>();
                List<char> consonantsList = new List<char>();
                StreamReader r = File.OpenText(f.Filename);
                if (r != null)
                {
                    string allText = r.ReadToEnd().ToLower(); //evita diferenciação entre maiúscula e minúscula, deixando tudo minúsculo para comparações
                    string[] words = allText.Split(' ');
                    f.WordsCount = words.Length;

                    for (int i = 0; i < allText.Length; ++i)
                    {
                        for (int v = 0; v < vowels.Length; ++v)
                        {
                            if (allText[i] == Convert.ToChar(vowels[v]))
                                vowelsList.Add(allText[i]);
                        }
                        for (int c = 0; c < consonants.Length; ++c)
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
                    //Duplica os arquivos de texto só que com o conteúdo em maiúsuclo
                    string[] aux = f.Filename.Split('\\');
                    string filename = aux[aux.Length - 1];
                    File.WriteAllText(directoryUpper + filename, allText.ToUpper());
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int GetFilesCount()
        {
            return fileInformations.Count;
        }
    }
}
