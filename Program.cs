using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace TrabalhoThreads
{
    class Program
    {
        static List<int> numbersVector = new List<int>();

        static void InvertVector()
        {
            PutVectorSize:
            string sizeV = Util.ConsoleInOut("Informe o tamanho do vetor a ser invertido: ");

            if (Convert.ToInt32(sizeV) < 10)
            {
                Console.WriteLine("O número de itens de um vetor tem que ser no mínimo 10, digite novamente!");
                goto PutVectorSize;
            }

            PutThreadNumber:
            string threadsN = Util.ConsoleInOut("Número de threads: (1, 2, 5)");

            if (threadsN.Equals("1") || threadsN.Equals("2") || threadsN.Equals("5"))
            {
                Util.FillVector(numbersVector, Convert.ToInt32(sizeV));
                Util.PrintVector(numbersVector, "Vetor original");
            }
            else
            {
                Console.WriteLine("Valor inválido de threads, digite novamente!");
                goto PutThreadNumber;
            }
        }

        static void MatrixMult()
        {
            
            

        }

        static void FileManipulation()
        {
        /*
         Faça um programa que, dado um diretório com arquivos de texto no formato .txt, calcule as 
        seguintes estatísticas para cada arquivo. Número de palavras, número de vogais, número de consoantes,
        palavra que apareceu mais vezes no arquivo, vogal mais frequente, consoantes mais frequente. 
        Além disso, para cada arquivo do diretório, o programa deverá gerar um novo arquivo,
        contendo o conteúdo do arquivo original escrito em letras maiúsculas.
         */
        SetFilePath:
            string filepath = Util.ConsoleInOut("Digite o local do arquivo: ");
            if (Directory.Exists(filepath))
            {
                Util.DefineFilePath(filepath);
                Console.WriteLine("----------------------");
                Console.WriteLine("Informações dos arquivos:");
                Util.FileManipulation(0);
                Util.ShowFileInformations();
                Console.WriteLine("----------------------");

            }
            else
            {
                Console.WriteLine("Diretório inválido, tente novamente");
                goto SetFilePath;
            }


        }

        static void Main(string[] args)
        {
            ChooseOption:
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Trabalho de Threads - opções disponíveis: ");
            Console.WriteLine("Dupla: Rodrigo Borges e José Victor");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("1 - Escrevendo um vetor de trás pra frente");
            Console.WriteLine("2 - Multiplicação de matrizes");
            Console.WriteLine("3 - Manipulação de arquivos");
            Console.WriteLine("----------------------------------------------");
            string option = Util.ConsoleInOut("Qual opção? ");
            if (option == "1")
                InvertVector();
            else if (option == "2")
                MatrixMult();
            else if (option == "3")
                FileManipulation();
            else
            {
                Console.WriteLine("Valor inválido, tente novamente!");
                goto ChooseOption;
            }
            Console.WriteLine("Aperte uma tecla para sair...");
            Console.ReadKey();
        }
    }
}
