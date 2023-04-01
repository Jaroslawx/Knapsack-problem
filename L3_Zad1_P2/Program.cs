using System;
using System.Diagnostics;
using System.Numerics;

namespace Problem2
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream outFile = new FileStream("Out0302.txt", FileMode.Truncate, FileAccess.Write); // otworzenie pliku i wyczyszczenie go
            using var writer = new StreamWriter(outFile);

            int n, W;

            int[] pTable = new int[0]; // tabela wartosci
            int[] wTable = new int[0]; // tabela wag

            using (TextReader reader = File.OpenText("In0302.txt")) // sczytanie zmiennych n i k z pliku
            {
                string text = reader.ReadLine();
                string[] variables = text.Split(' ');
                n = int.Parse(variables[0]);
                W = int.Parse(variables[1]);

                Array.Resize(ref pTable, n);
                Array.Resize(ref wTable, n);

                for (int i = 0; i < n; i++)
                {
                    text = reader.ReadLine();
                    variables = text.Split(' ');
                    pTable[i] = int.Parse(variables[0]);
                    wTable[i] = int.Parse(variables[1]);

                }
            }

            int[,] knapsack = new int[n + 1, W + 1]; // tabela z plecakiem

            for (int i = 0; i <= n; i++)
            {
                for (int w = 0; w <= W; w++)
                {
                    if (i == 0 || w == 0)
                    {
                        knapsack[i, w] = 0;
                    }
                    else if (wTable[i - 1] > w && i != 0)
                    {
                        knapsack[i, w] = knapsack[i - 1, w];
                    }
                    else if (wTable[i - 1] <= w && i != 0)
                    {
                        knapsack[i, w] = Math.Max(knapsack[i - 1, w], knapsack[i - 1, w - wTable[i - 1]] + pTable[i - 1]);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= W; j++)
                {
                    Console.Write("{0} ", knapsack[i, j]);
                }
                Console.WriteLine();
            }


            int[,] odp = new int[n * n, n];

            int max = knapsack[n, W];
            int mHelp = max;
            int wHelp = W;
            int nHelp = n;
            int temp = 0, temp2 = 0;
            int save = 0; // zmiena zapisująca dodany ostatni przedmiot

            while (knapsack[nHelp, W] == max)
            {
                mHelp = max;
                for (int i = nHelp; i > 0; i--)
                {
                    for (int j = W; j > 0; j--)
                    {
                        if (knapsack[i, j] == mHelp)
                        {
                            if (mHelp - pTable[i - 1] >= 0)
                            {
                                if ((knapsack[i, j] != knapsack[i - 1, j] || knapsack[i, j] == max) && i != save)
                                {
                                    //Console.Write(i + " ");
                                    odp[temp, temp2] = i;
                                    j = j - wTable[i - 1];
                                    mHelp = mHelp - pTable[i - 1];
                                    save = i;
                                    temp2++;
                                }

                            }
                        }

                    }
                }
                //Console.WriteLine();
                nHelp--;
                temp++;
                temp2 = 0;
            }

            /*int[,] test = new int[n,n];
            int[] check = new int[n];
            for (int j = 0; j < temp; j++)
            {
                for (int i = n - 1, x = 0; i >= 0; i--, x++)
                {
                    test[j,x] = odp[j,i];
                }
            }

            for (int j = 0; j < temp; j++)
            {
                for (int i = n - 1, x = 0; i >= 0; i--, x++)
                {
                    if (test[j, x] == odp[j, i])
                    {

                    }
                }
            }*/

            for (int j = 0; j < temp; j++)
            {
                for (int i = n - 1; i >= 0; i--)
                {
                    if (odp[j, i] != 0)
                    {
                        writer.Write("{0} ", odp[j, i]);
                        //Console.Write("{0} ", odp[j, i]);
                    }
                }
                writer.WriteLine();
            }

        }
    }
}
