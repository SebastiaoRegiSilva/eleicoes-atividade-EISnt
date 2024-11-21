using System;
using System.Collections.Generic;
using System.Linq;

namespace Eleicoes.Atividade.EISnt
{
    class Program
    {
        static List<int> numerocandidato = new List<int> { 1, 2, 3, 4 };
        /// <summary>
        /// Contador de votos.
        /// </summary>
        /// <typeparam name="int">Número do candidato.</typeparam>
        /// <typeparam name="int">Votos computados ao candidato.</typeparam>
        /// <returns></returns>
        static Dictionary<int, int> votosValidos = new Dictionary<int, int>();

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Eleições 2024 - Sejam bem-vindos!");
            Console.ResetColor();
            Console.WriteLine("Selecione seu voto por gentileza.");
            int numeroCandidato = ValidarEntrada(Console.ReadLine());
            
            AdicionarVotos(votosValidos, numeroCandidato);

            // Exibe os resultados da eleição.
            foreach (var candidato in votosValidos)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Candidato: {candidato.Key}, Valor: {candidato.Value}");
                Console.ResetColor();
            }

            Console.ReadKey();
        }

        private static int ValidarEntrada(string? entradaUsuario)
        {
            int numero;
            bool entradaValida = false;

            while (!entradaValida)
            {
                Console.Write("Digite um número entre 1 e 4: ");
                entradaUsuario = Console.ReadLine();

                // Tenta converter a entrada para inteiro
                if (int.TryParse(entradaUsuario, out numero))
                {
                    // Verifica se o número está entre 1 e 4
                    if (numero >= 1 && numero <= 4)
                    {
                        entradaValida = true;
                        Console.WriteLine($"Você digitou um número válido: {numero}");
                    }
                    else
                        Console.WriteLine("O número deve estar entre 1 e 4. Tente novamente.");
                }
                else
                    Console.WriteLine("Entrada inválida. Por favor, digite um número.");
                
            }
        }
        
        static void AdicionarVotos(Dictionary<string, int> votosComputar, int numeroCandidato, int valor = 1)
        {
            if (numeroCandidato != 0 && votosComputar.ElementAt(n => n == numeroCandidato))
            {
                if (votosValidos.ContainsKey(numeroCandidato))
                    // Incrementa o valor a chave existente.
                    votosComputar[numeroCandidato] += valor;
            }
        }
    }
}
