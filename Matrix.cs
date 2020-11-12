using System;
using System.Collections.Generic;
using System.Text;

namespace TrabalhoThreads
{
    //Classe auxiliar para realizar operações com as matrizes
    public class Matrix
    {
        public int[,] MatrixArr;
        private int rows = 0;
        private int columns = 0;
        public Matrix(int amountRows, int amountColumns)
        {
            rows = amountRows;
            columns = amountColumns;
            MatrixArr = new int[rows, columns];
        }

        //Preenche a matriz com números aleatórios de 0 ao valor definido na função Next do Random
        public void Fill()
        {
            for(int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                    MatrixArr[i, j] = (new Random()).Next(20);
            }
        }

        //Exibe a matriz para melhor visualização das colunas e linhas
        public void Print()
        {
            Console.WriteLine("Exibindo matriz");
            for(int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                    Console.Write(string.Format("{0}\t", MatrixArr[i, j]));
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Função responsável por multiplicar duas matrizes e retornar uma outra
        public static Matrix Mult(Matrix A, Matrix B, int start, int end)
        {
            Matrix r = new Matrix(A.rows, B.columns);
            for(int i = start; i < end; ++i)
            {
                for(int j = 0; j < B.rows; ++j)
                {
                    for (int x = 0; x < A.columns; ++x)
                        r.MatrixArr[i, j] += A.MatrixArr[i, x] * B.MatrixArr[x, j];
                        
                }
            }
            return r;
        }
    }
}
