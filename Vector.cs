using System;
using System.Collections.Generic;
using System.Text;

namespace TrabalhoThreads
{
    //Classe auxiliar para realizar operações com vetores
    public class Vector
    {
        public int size { get; private set; }
        public int[] VectorArr;
        public Vector(int size)
        {
            this.size = size;
            VectorArr = new int[size];
        }

        //Preenche o vetor com números aleatórios
        public void Fill()
        {
            for (int i = 0; i < VectorArr.Length; ++i)
                VectorArr[i] = (new Random()).Next(20);
        }

        //Exibe na tela os valores do vetor
        public void Print()
        {
            Console.WriteLine("Exibindo vetor");
            for (int i = 0; i < VectorArr.Length; ++i)
                Console.Write(string.Format("{0}\t", VectorArr[i]));
            Console.WriteLine();
        }

        //Função para realizar inversão do vetor e manipulando o vetor B por passagem por referência
        public static void Invert(Vector A, Vector B, int start, int end)
        {
            for (int i = start; i < end; ++i)
                B.VectorArr[B.VectorArr.Length - i - 1] = A.VectorArr[i];
        }
    }
}
