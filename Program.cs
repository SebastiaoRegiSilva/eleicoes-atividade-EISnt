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
            { 1, 0},
            { 2, 0},
            { 3, 0},
            { 4, 0}
        };

        /// <summary>
        /// Organizar o votos dos usuários. 
        /// </summary>
        static IEnumerable<(Dictionary<int, int>, int )> ordemVotos = new List<(Dictionary<int, int>, int)>();

        /// <summary>
        /// Organizar os votos dos usuários/candidatos. 
        /// </summary>
        static int ordemDoVoto = 0;    

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
                    AcessoPresidencial();
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

        static int AdicionarVotos(int numeroCandidato, int valor = 1)
        {
            if (numeroCandidato != 0 && numeroCandidatos.Contains(numeroCandidato))
            {
                foreach (var numero in numeroCandidatos.Where(x => x.Equals(numeroCandidato))){
                    votosValidos[numeroCandidato] += valor; 
                    ordemDoVoto ++;
                }
            }

            return ordemDoVoto;
        }

        #region IMPRESSÕES.

        private static void TelaInicial()
        {
            ImprimirInicio();
            while (ImprimirUrnaVotacao().Item1 != 0)
                ImprimirUrnaVotacao();
        }

        private static void ImprimirInicio()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Eleições 2024 - Sejam bem-vindos!");
            Console.ResetColor();
        }

        private static (int, int) ImprimirUrnaVotacao()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Escolha seu candidato, por gentileza.");
            int numeroCandidato = ValidarEntrada();
            int posicaoVoto = AdicionarVotos(numeroCandidato);
            Console.ResetColor();
            return (numeroCandidato, posicaoVoto);
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
            // O primeiro candidato que recebeu mais quantidade de votos, ele venceu!
            if(Desempatar().Item3){
                Console.WriteLine("Entrou no IF");
                Console.WriteLine($"\nO Candidato: {Desempatar().Item1}, venceu com: {Desempatar().Item2} votos! \nParabéns por ter sido o primeiro alcançar a quantidade máxima de votos! ");
            }
            else{
                Console.WriteLine("Entrou no ELSE");
                Console.WriteLine($"\nO Candidato: {vencedor.Key}, venceu com: {vencedor.Value} votos! \nParabéns por ter sido o primeiro alcançar a quantidade máxima de votos! ");
            }
            
            Console.ResetColor();
            Console.WriteLine("Pressione qualquer tecla para encerrar a eleição...");
            Console.ReadKey();
            Environment.Exit(0); 
            // Acabar com a aplicação.
        }
        #endregion

        #region MELHORIAS
        /// <summary>
        /// Função adicional para tirar igualdade entre candidatos.
        /// </summary>
        private static (int, int, bool) Desempatar()
        {
            int desempate = 0, maximoValorVotos = votosValidos.Values.Max(), numCandidato = 0;
            bool empate = false;

            // Agrupando por valores e selecionando apenas os valores repetidos.
            var valores = votosValidos
                .GroupBy(pair => pair.Value)  // Agrupa pelos valores.
                .Where(group => group.Count() > 1) // Seleciona grupos com mais de uma ocorrência.
                .OrderByDescending(o => o.Key); // Ordenar por chave.
                // BUG quando há ocorrencia de outros valores parecidos. Impedindo entrar no ELSE da impressão de resultados.

            KeyValuePair<int, int> maxPair;
            // Exibindo as chaves e valores duplicados
            foreach (var group in valores)
            {
                maxPair = group.FirstOrDefault(pair => pair.Value == maximoValorVotos);
                desempate = maxPair.Value;
                numCandidato = maxPair.Key;
                empate = true;
                break;
            }

            if(desempate <= maximoValorVotos)
                desempate = maximoValorVotos;  

            return (numCandidato, desempate, empate);
        }
        #endregion

        #region ACESSO PRESIDENCIAL
        static void AcessoPresidencial()
        {
            Console.WriteLine("Bem-vindo presidente! Digite sua senha de acesso:");
            if (!ControleDeAcesso())
                AcessoComPadrao();
        }

        private static bool ControleDeAcesso()
        {

            var contadorDeAcesso = 3; // Limite de erros de senha.
            bool acessoSenha = false;
            for (int i = 0; i < contadorDeAcesso; i++)
            {
                // Chama a função para ler a senha oculta
                string senha = LerSenhaOculta();

                // Verifica se a senha está correta
                if (senha == "senhaPresidente")
                {
                    Console.WriteLine("\nSenha correta! Acessando ao resultado...");
                    System.Threading.Thread.Sleep(2000);
                    ImprimirResultado();
                    acessoSenha = true;
                    break;
                }
                else
                    Console.WriteLine("\nSenha incorreta! Acesso negado.");
            }
            return acessoSenha;
        }

        private static void AcessoComPadrao()
        {
            Console.WriteLine("Bem-vindo presidente! Digite sua senha de acesso:");
            Console.WriteLine("\nAcesse com a senha padrão - 987456.");
            // Chama a função para ler a senha oculta
            string senha = LerSenhaOculta();

            // Verifica se a senha está correta
            if (senha == "987456")
            {
                Console.WriteLine("\nSenha correta! Acessando ao resultado...");
                System.Threading.Thread.Sleep(2000);
                ImprimirResultado();
            }
            else
            {
                Console.WriteLine("\nSenha incorreta! Acesso negado.");
                Console.WriteLine("\n Tente novamente, com a senha padrão - 987456.");
                AcessoComPadrao();
            }
        }

        static string LerSenhaOculta()
        {
            string senha = string.Empty;
            ConsoleKey key;

            do
            {
                // Lê a tecla pressionada sem exibir no console.
                var tecla = Console.ReadKey(intercept: true);
                key = tecla.Key;

                // Se não for Enter, adiciona o caractere à senha.
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
        #endregion
    }
}