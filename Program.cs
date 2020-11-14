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
        /*
         Faça um programa que inverta a ordem dos elementos de um vetor de inteiros com N valores. 
        Por exemplo, se o vetor contiver os elementos 1, 2, 3, 4, 5 o vetor de saída deverá 
        ser 5, 4, 3, 2, 1. O tamanho do vetor e o número de threads devem ser informados pelo 
        usuário. Os elementos do vetor devem ser gerados de forma aleatória pelo programa. 
        O programa deverá imprimir na tela o vetor de entrada e o vetor de saída.
         */
        PutVectorSize:
            string sizeV = Util.ConsoleInOut("Informe o tamanho do vetor a ser invertido: ");

            if(Convert.ToInt32(sizeV) <= 0)
            {
                Console.WriteLine("O tamanho do vetor tem que ser maior que 0, tente novamente!");
                goto PutVectorSize;
            }

        PutThreadNumber:
            string inputThread = Util.ConsoleInOut("Número de threads: ");
            int nThreads = Convert.ToInt32(inputThread);

            if (nThreads < 1)
            {
                Console.WriteLine("O número de threads tem que ser maior ou igual a 1, tente novamente!");
                goto PutThreadNumber;
            }

            Vector vectorA = new Vector(Convert.ToInt32(sizeV));
            Vector vectorB = new Vector(Convert.ToInt32(sizeV));

            vectorA.Fill();
            
            if (nThreads > Convert.ToInt32(sizeV))
                nThreads = Convert.ToInt32(sizeV);

            int[] steps = new int[nThreads + 1];
            Util.SetSteps(steps, Convert.ToInt32(sizeV), nThreads);

            Thread[] threads = new Thread[nThreads];
            int start, end;

            for(int i = 0; i < nThreads; ++i)
            {
                start = steps[i];
                end = steps[i + 1];

                threads[i] = new Thread( () => Vector.Invert(vectorA, vectorB, start, end) );
                threads[i].Start();
                threads[i].Join();
            }

            vectorA.Print();
            vectorB.Print();

        }

        static void MatrixMult()
        {
            /*
             Faça um programa que multiplique duas matrizes A e B, cujos dimensões são MxN e NxP, onde M pode ou não 
            ser igual a P. O tamanho das matrizes e o número de threads devem ser informados pelo usuário. Os valores
            das matrizes devem ser gerados de forma aleatória pelo programa. O programa deverá imprimir na tela as
            matrizes A e B bem como o resultado da sua multiplicação.
             */
            SetMatrixInfo:
            string inputMatrixA = Util.ConsoleInOut("Digite a quantidade de linhas e colunas da matriz A (nesse formato, exs: 3x3, 2x2, 6x6):");
            string inputMatrixB = Util.ConsoleInOut("Digite a quantidade de linhas e colunas da matriz B: ");
            string inputThreads = Util.ConsoleInOut("Digite a quantidade de threads: ");
            string[] rowColumnA = inputMatrixA.Split('x');
            string[] rowColumnB = inputMatrixB.Split('x');

            //O número de colunas da matriz A tem que ser igual ao número de linhas da matriz B
            if(Convert.ToInt32(rowColumnA[1]) != Convert.ToInt32(rowColumnB[0]))
            {
                Console.WriteLine("O número de colunas da matriz A tem que ser igual ao número de linhas da matriz B, digite novamente!");
                goto SetMatrixInfo;
            }

            Matrix matrixA = new Matrix(Convert.ToInt32(rowColumnA[0]), Convert.ToInt32(rowColumnA[1]));
            Matrix matrixB = new Matrix(Convert.ToInt32(rowColumnB[0]), Convert.ToInt32(rowColumnB[1]));
            Matrix matrixC = new Matrix(matrixA.rows, matrixB.columns);

            matrixA.Fill();
            matrixB.Fill();
            matrixA.Print();
            matrixB.Print();

            int nThreads = Convert.ToInt32(inputThreads);
            if (nThreads > matrixA.rows)
                nThreads = matrixA.rows;

            Thread[] threads = new Thread[nThreads];
            int[] steps = new int[nThreads + 1];
            Util.SetSteps(steps, matrixA.rows, nThreads);

            int start, end, x = 0;
            for(int i = 0; i < nThreads; ++i)
            {
                start = steps[i];
                end = steps[i + 1];

                threads[i] = new Thread( () => Matrix.Mult(matrixA, matrixB, matrixC, start, end) );
                threads[i].Start();
                threads[i].Join();

                x++;
            }
            matrixC.Print();

        }

        static void FileManip()
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
                FileManipulation fm = new FileManipulation(filepath);

                List<Thread> threads = new List<Thread>();
                
                // O número de threads vai ser igual ao número de arquivos
                foreach(FileInformations f in fm.fileInformations)
                {
                    threads.Add(new Thread( () => {
                        fm.Manipulate(f);
                    } ));
                }

                for(int i = 0; i < threads.Count; ++i)
                {
                    threads[i].Start();
                    threads[i].Join();
                }

                fm.ShowFileInformations();

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
                FileManip();
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
