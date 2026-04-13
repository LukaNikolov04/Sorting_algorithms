using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19294_C__Drugi_zadatak
{
    internal class Program
    {
        public static readonly Random rand = new Random();
        public static readonly Stopwatch watch = new Stopwatch();

        static void BubbleSort(int[] niz)
        {
            for(int i = 0;i < niz.Length - 1;i++)
            {
                for(int j = niz.Length - 1;j > i;j--)
                {
                    if (niz[j] < niz[j - 1])
                    {
                        int temp = niz[j];
                        niz[j] = niz[j - 1];
                        niz[j - 1] = temp;
                    }
                }

            }
        }

        static void QuickSort(int[] niz,int p,int r)
        {
            if( p < r )
            {
                int x = niz[r];
                int i = p - 1;
                for(int j = p; j <= r - 1;j++)
                {
                    if (niz[j] <= x)
                    {
                        i++;

                        int temp = niz[j];
                        niz[j] = niz[i];
                        niz[i] = temp;
                    }
                }

                int temp1 = niz[i+1];
                niz[i + 1] = niz[r];
                niz[r] = temp1;

                QuickSort(niz, p, i);
                QuickSort(niz, i + 2, r);
            }
        }

        static void CountingSort(int[] niz,int pozicija)
        {
            int n = niz.Length;
            int i;
            int[] C = new int[10] { 0,0,0,0,0,0,0,0,0,0 };
            int[] B = new int[n];

            // (niz[i] / pozicija) % 10 -> izdvajamo samo cifru koju trenutno posmatramo
            // u prvom prolazu je cifra jedinice, posle cifra desetice itd.
            for(i = 0;i < n;i++)
            {
                C[(niz[i] / pozicija) % 10]++;
            }

            for(i = 1;i < 10;i++)
            {
                C[i] = C[i] + C[i - 1];
            }

            for(i = n-1; i >= 0;i--)
            {
                B[C[(niz[i] / pozicija) % 10] - 1] = niz[i];
                C[(niz[i] / pozicija) % 10]--;
            }

            for(i = 0;i<n;i++)
            {
                niz[i] = B[i];
            }
        }

        static void RadixSort(int[] niz)
        {
            // Odredjujemo max el. da bismo znali koliko cifara obradjujemo
            int max = niz[0];
            for(int i = 1;i<niz.Length;i++)
            {
                if (niz[i] > max)
                    max = niz[i];
            }

            // pozicija odredjuje koju cifru trenutno posmatramo
            // na pocetku je pozicija = 1 -> obradjujemo cifru jedinice
            for(int pozicija = 1; max / pozicija > 0;pozicija *= 10)
            {
                CountingSort(niz, pozicija); // koristim CountingSort jer je stabilan algoritam
            }
        }

        static int[] NapraviNiz(int N)
        {
            int[] niz = new int[N];
            for (int i = 0; i < N; i++)
            {
                niz[i] = rand.Next(0, 10001);
            }
            return niz;
        }

        static void RasporediPakete(int[] niz,int M)
        {
            if(niz.Length == 0)
            {
                Console.WriteLine("PRAZAN NIZ !!!");
                return;
            }

            int minPocetniIndex = -1;
            int minKrajIndex = -1;
            int minRazlika = -1;
            int Razlika;

            for(int i = 0; i <= niz.Length - M; i++)
            {
                if(i == 0)
                {
                    minRazlika = niz[i + M - 1] - niz[i];
                    minPocetniIndex = i;
                    minKrajIndex = i + M - 1;
                }
                else
                {
                    Razlika = niz[i + M - 1] - niz[i];
                    if(Razlika < minRazlika)
                    {
                        minRazlika = Razlika;
                        minPocetniIndex = i;
                        minKrajIndex = i + M - 1;
                    }
                }
            }

            Console.WriteLine("MinRazlika: " + minRazlika + '\t' + "MinPocetniIndex: " + minPocetniIndex + " = " + niz[minPocetniIndex] + '\t' + "MinKrajIndex: " + minKrajIndex + " = " + niz[minKrajIndex] + '\n');

        }

        static void Main(string[] args)
        {
            List<int[]> nizovi = new List<int[]>();

            int N;
            int M;
            for(N = 100;N <= 10000000;N *=10)
            {
                nizovi.Add(NapraviNiz(N));
            }

            N = 100;
            int[] copy1, copy2, copy3;

            for(int i = 0;i < nizovi.Count;i++)
            {
                copy1 = new int[nizovi.ElementAt(i).Length];
                copy2 = new int[nizovi.ElementAt(i).Length];
                copy3 = new int[nizovi.ElementAt(i).Length];
                for (int j = 0; j < nizovi.ElementAt(i).Length;j++)
                {
                    copy1[j] = copy2[j] = copy3[j] = nizovi.ElementAt(i)[j];
                }

                M = (int)(N * 0.3);
                Console.WriteLine("########## " + N + " elemenata ##########" + '\n');

                //Console.WriteLine(" PRE SORTIRANJA : \n");

                //for (int j = 0; j < nizovi[i].Length; j++)
                //{
                //    Console.WriteLine(copy1[j] + " - " + j);
                //}
                //Console.WriteLine();

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();



                long memPreFunkcije = GC.GetTotalMemory(false);
                watch.Restart();
                BubbleSort(copy1);
                watch.Stop();
                long memPosleFunkcije = GC.GetTotalMemory(false);
                long memKoriscena = memPosleFunkcije - memPreFunkcije;
                Console.WriteLine("Vreme izvrsenja BubbleSort alg za " + N + " elemenata : " + watch.Elapsed + '\n');
                Console.WriteLine("Ukupno iskoriscena memorija : " + memKoriscena + '\n');

                //Console.WriteLine(" POSLE SORTIRANJA : \n");

                //for (int j = 0; j < nizovi[i].Length; j++)
                //{
                //    Console.WriteLine(copy1[j] + " - " + j);
                //}
                //Console.WriteLine();

                RasporediPakete(copy1, M);


                Console.ReadLine();

                //Console.WriteLine(" PRE SORTIRANJA : \n");

                //for (int j = 0; j < nizovi[i].Length; j++)
                //{
                //    Console.WriteLine(copy2[j] + " - " + j);
                //}
                //Console.WriteLine();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();


                memPreFunkcije = GC.GetTotalMemory(false);
                watch.Restart();              
                QuickSort(copy2, 0, copy2.Length - 1);
                watch.Stop();
                memPosleFunkcije = GC.GetTotalMemory(false);
                memKoriscena = memPosleFunkcije - memPreFunkcije;
                Console.WriteLine("Vreme izvrsenja QuickSort alg za " + N + " elemenata : " + watch.Elapsed + '\n');
                Console.WriteLine("Ukupno iskoriscena memorija : " + memKoriscena + '\n');
                //Console.WriteLine(" POSLE SORTIRANJA : \n");

                //for (int j = 0; j < nizovi[i].Length; j++)
                //{
                //    Console.WriteLine(copy2[j] + " - " + j);
                //}
                //Console.WriteLine();


                RasporediPakete(copy2, M);

                Console.ReadLine();

                //Console.WriteLine(" PRE SORTIRANJA : \n");

                //for (int j = 0; j < nizovi[i].Length; j++)
                //{
                //    Console.WriteLine(copy3[j] + " - " + j);
                //}
                //Console.WriteLine();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();


                watch.Restart();
                memPreFunkcije = GC.GetTotalMemory(false);                                
                RadixSort(copy3);
                memPosleFunkcije = GC.GetTotalMemory(false);
                watch.Stop();                
                memKoriscena = memPosleFunkcije - memPreFunkcije;
                Console.WriteLine("Vreme izvrsenja RadixSort alg za " + N + " elemenata : " + watch.Elapsed + '\n');
                Console.WriteLine("Ukupno iskoriscena memorija : " + memKoriscena + '\n');
                //Console.WriteLine(" POSLE SORTIRANJA : \n");

                //for (int j = 0; j < nizovi[i].Length; j++)
                //{
                //    Console.WriteLine(copy3[j] + " - " + j);
                //}
                //Console.WriteLine();



                RasporediPakete(copy3, M);

                Console.ReadLine();

                N *= 10;
            }

            Console.WriteLine("#################### KRAJ ####################" + '\n');
            Console.ReadLine();

        }
    }
}
