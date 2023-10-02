// Aluna: Maria Eduarda S. Ferreira

using System;

namespace RoundRobin
{
    class RoundRobin
    {
        static int[] CalctempoEspera(int[] burstTime, int quantum)
        {
            int numeroElementos = burstTime.Length;
            int[] burstTimeRestante = (int[])burstTime.Clone();
            int[] tempoEspera = new int[numeroElementos];
            for (int i = 0; i < numeroElementos; i++)
            {
                tempoEspera[i] = 0;
            }
            int tempoPassado = 0;
            bool pronto;

            do
            {
                pronto = true;
                // verifica se todos os processos estão prontos
                for (int i = 0; i < numeroElementos; i++)
                {
                    if (burstTimeRestante[i] > 0)
                    {
                        pronto = false;
                        break;
                    }
                }
                for (int i = 0; i < numeroElementos; i++)
                {
                    if (burstTimeRestante[i] != 0)
                    {
                        if (burstTimeRestante[i] > quantum)
                        {
                            tempoPassado += quantum;
                            burstTimeRestante[i] -= quantum;
                        }
                        else
                        {
                            tempoPassado += burstTimeRestante[i];
                            tempoEspera[i] = tempoPassado - burstTime[i];
                            burstTimeRestante[i] = 0;
                        }
                    }
                }
            }
            while (!pronto);
            return tempoEspera;
        }

        static int[] CalctempoResposta(int[] burstTime, int[] tempoEspera)
        {
            int numeroElementos = burstTime.Length;
            int[] tempoResposta = new int[numeroElementos];
            for (int i = 0; i < numeroElementos; i++)
            {
                tempoResposta[i] = burstTime[i] + tempoEspera[i];
            }
            return tempoResposta;
        }

        static void PrintTempoMedio(int[] burstTime, int quantum)
        {
            int n = burstTime.Length;
            int totaltempoEspera = 0;
            int totaltempoResposta = 0;

            // tempo de espera de todos os processos
            int[] tempoEspera = CalctempoEspera(burstTime, quantum);

            // tempo de resposta de todos os processos
            int[] tempoResposta = CalctempoResposta(burstTime, tempoEspera);
            
            Console.WriteLine("Processo\tBurst Time\tTempo de espera\tTempo de Resposta (TAT)");
            Console.WriteLine("=======\t==========\t============\t===============");
            // calcula tempo total de espera e resposta
            for (int i = 0; i < n; i++)
            {
                totaltempoEspera += tempoEspera[i];
                totaltempoResposta += tempoResposta[i];
                Console.WriteLine($"{i}\t\t{burstTime[i]}\t\t{tempoEspera[i]}\t\t{tempoResposta[i]}");
            }

            Console.WriteLine($"\nTempo de espera medio= {(float)totaltempoEspera / (float)n}");
            Console.WriteLine($"Tempo de resposta medio = {(float)totaltempoResposta / (float)n}");
        }

        public static void Main(string[] args)
        {
            int[] burstTime = { 5, 20, 4, 3 };
            int quantum = 2;

            PrintTempoMedio(burstTime, quantum);
        }
    }
}