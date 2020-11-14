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
        //Exibir um texto juntamente com uma entrada do usuário 
        public static string ConsoleInOut(string exibitionText)
        {
            Console.WriteLine(exibitionText);
            string s = Console.ReadLine();
            return s.ToLower(); //retorna tudo minúsculo para possíveis comparações
        } 

        //Função auxiliar para definir os limites de inicio e fim pegando uma parte do vetor ou matriz para dividir entre threads
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
