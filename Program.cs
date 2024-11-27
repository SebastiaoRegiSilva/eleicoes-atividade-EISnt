using System;
using System.Collections.Generic;
using System.Linq;

namespace Eleicoes.Atividade.EISnt
{
    class Program
    {
        static List<int> numeroCandidatos = new List<int> {
            1,
            2,
            3,
            4
        };
        /// <summary>
        /// Contador de votos.
        /// </summary>
        /// <typeparam name="int">Número do candidato.</typeparam>
        /// <typeparam name="int">Votos computados ao candidato.</typeparam>
        static Dictionary<int, int> votosValidos = new Dictionary<int, int>{
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 }
        };

        static void Main(string[] args)
        {
            TelaInicial();
        }

        private static int ValidarEntrada()
        {
            int numeroValido = 0;
            bool entradaValida = false;

            while (!entradaValida)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("Digite um número entre 1 e 4:");
                Console.ResetColor();
                string entrada = Console.ReadLine()?.Trim(); // Remove espaços em branco do início e do fim.

                if (int.TryParse(entrada, out numeroValido) && numeroValido >= 1 && numeroValido <= 4)
                    entradaValida = true;
                else if (numeroValido == 0)
                {
                    ImprimirResultado();
                    string vazia = Console.ReadLine(); // Pausa da aplicação.
                }
                else
                {
                    try
                    {
                        Console.Clear();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Candidato inexistente, digite novamente.");
                    }
                    finally
                    {
                        ImprimirInicio();
                        ImprimirUrnaVotacao();
                    }
                }
            }
            return numeroValido;
        }

        static void AdicionarVotos(int numeroCandidato, int valor = 1)
        {
            if (numeroCandidato != 0 && numeroCandidatos.Contains(numeroCandidato))
            {
                foreach (var numero in numeroCandidatos.Where(x => x.Equals(numeroCandidato)))
                    votosValidos[numeroCandidato] += valor;
            }
        }

        #region IMPRESSÕES.

        private static void TelaInicial()
        {
            ImprimirInicio();
            while (ImprimirUrnaVotacao() != 0)
            {
                ImprimirUrnaVotacao();
            }
        }

        private static void ImprimirInicio()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Eleições 2024 - Sejam bem-vindos!");
            Console.ResetColor();
        }

        private static int ImprimirUrnaVotacao()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Escolha seu candidato, por gentileza.");
            int numeroCandidato = ValidarEntrada();
            AdicionarVotos(numeroCandidato);
            Console.ResetColor();
            return numeroCandidato;
        }

        private static void ImprimirResultado()
        {

            var vencedor = votosValidos.Aggregate((x, y) => x.Value > y.Value ? x : y);
            // Exibe os resultados da eleição.
            foreach (var candidato in votosValidos)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Candidato: {candidato.Key}, Valor: {candidato.Value}");
                Console.ResetColor();
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            // O último candidato que recebeu mais quantidade de votos, ele venceu!
            // Implementei o desempate, mas ia complicar ainda mais!  
            Console.WriteLine($"O Candidato com número: {vencedor.Key}, venceu com: {vencedor.Value + 1} votos! ");

            Console.ResetColor();
        }
        #endregion

        #region POSSÍVEIS MELHORIAS
        /// <summary>
        /// Função adicional para tirar igualdade entre candidatos.
        /// </summary>
        private static (int, int, bool) Desempatar()
        {
            int desempate = 0, maximoValorVotos = votosValidos.Values.Max(), numCandidato = 0;
            bool empate = false;

            // Agrupando por valores e selecionando apenas os valores repetidos
            var valores = votosValidos
                .GroupBy(pair => pair.Value)  // Agrupa pelos valores.
                .Where(group => group.Count() > 1); // Seleciona grupos com mais de uma ocorrência.

            // Exibindo as chaves e valores duplicados
            foreach (var group in valores)
            {
                var maxPair = group.FirstOrDefault(pair => pair.Value == maximoValorVotos);
                desempate = maxPair.Value + 1;
                numCandidato = maxPair.Key;
                empate = true;
            }
            return (desempate, numCandidato, empate);
        }
        #endregion

        #region ACESSO PRESIDENCIAL
        static void AcessoPresidencial()
        {
            Console.WriteLine("Bem-vindo presidente! Digite sua senha de acesso:");
            ControleDeAcesso();
            // ACESSO com PADRAO 
            // ACESSO SEM PADRAO
        }

        private static bool ControleDeAcesso()
        {

            var contadorDeAcesso = 3; // Limite de erros de senha.
            bool acesso = false;
            for (int i = 0; i < contadorDeAcesso; i++)
            {
                // Chama a função para ler a senha oculta
                string senha = LerSenhaOculta();

                // Verifica se a senha está correta
                if (senha == "senhaPresidente")
                {
                    Console.WriteLine("\nSenha correta! Acessando a função...");
                    FuncaoRestrita();
                    acesso = true;
                    break;
                }
                else
                    Console.WriteLine("\nSenha incorreta! Acesso negado.");

            }
            // CODIGO chega aqui;
            ImplementarSenhaPadrao();
            return acesso;
        }

        private static int ImplementarSenhaPadrao()
        {
            int passwordDefault = 987456;
            return passwordDefault;
        }

        static string LerSenhaOculta()
        {
            string senha = string.Empty;
            ConsoleKey key;

            do
            {
                // Lê a tecla pressionada sem exibir no console
                var tecla = Console.ReadKey(intercept: true);
                key = tecla.Key;

                // Se não for Enter, adiciona o caractere à senha
                if (key != ConsoleKey.Enter)
                {
                    senha += tecla.KeyChar;
                    Console.Write("*"); // Exibe um asterisco no lugar do caractere.
                }
            }
            while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return senha;
        }

        static void FuncaoRestrita()
        {
            // Exibir resultado da eleição.
            Console.WriteLine("Executando a função restrita...");
            // Insira a lógica da função aqui
        }
        #endregion

    }
}