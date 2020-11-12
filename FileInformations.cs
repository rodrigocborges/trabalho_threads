using System;
using System.Collections.Generic;
using System.Text;

namespace TrabalhoThreads
{
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
}
