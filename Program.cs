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
        /// <returns></returns>
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
                else if(numeroValido == 0){
                    ImprimirResultado();
                    Console.WriteLine("Digite CRTL + C, para encerar a aplicação!");
                    string vazia = Console.ReadLine(); // Pausa da aplicação.
                }
                else{
                    try{
                        Console.Clear();
                    }
                    catch (Exception){
                        Console.WriteLine("Candidato inexistente, digite novamente.");
                    }
                    finally{
                        ImprimirInicio();
                        ImprimirUrnaVotacao();
                    }
                }
            }
            return numeroValido;
        }

        static void AdicionarVotos(int numeroCandidato, int valor = 1)
        {
            if (numeroCandidato != 0 && numeroCandidatos.Contains(numeroCandidato)){
                foreach (var numero in numeroCandidatos.Where(x => x.Equals(numeroCandidato)))
                    votosValidos[numeroCandidato] += valor;
            }
        }

        /*
            BLOCO DE IMPRESSÕES.
        */

        private static void TelaInicial(){
            ImprimirInicio();
            while(ImprimirUrnaVotacao() != 0){
                ImprimirUrnaVotacao();
            }
        }

        private static void ImprimirInicio(){
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Eleições 2024 - Sejam bem-vindos!");
            Console.ResetColor();
        }
        
        private static int ImprimirUrnaVotacao(){
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Escolha seu candidato, por gentileza.");
            int numeroCandidato = ValidarEntrada();
            AdicionarVotos(numeroCandidato);
            Console.ResetColor();
            return numeroCandidato;
        }

        private static void ImprimirResultado(){
            
            var vencedor = votosValidos.Aggregate((x, y) => x.Value > y.Value ? x : y);
            // Desempatar...
            // Exibe os resultados da eleição.
            foreach (var candidato in votosValidos)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Candidato: {candidato.Key}, Valor: {candidato.Value}");
            }

            Console.WriteLine($"O Candidato com número: {vencedor.Key}, venceu com: {vencedor.Value}votos! ");
            Console.ResetColor();
        }

        /// <summary>
        /// Função adicional para tirar igualdade entre candidatos.
        /// </summary> <summary>
        private static int Desempatar()
        {
            int desempate =0, maximoValorVotos = votosValidos.Values.Max();
            
            // Agrupando por valores e selecionando apenas os valores repetidos
            var valores = votosValidos
                .GroupBy(pair => pair.Value)  // Agrupa pelos valores
                .Where(group => group.Count() > 1); // Seleciona grupos com mais de uma ocorrência

            // Exibindo as chaves e valores duplicados
            foreach (var group in valores)
            {
                var maxPair = group.First(pair => pair.Value == maximoValorVotos);
                desempate = maxPair.Value + 1;
            }

            return desempate;
        }
    }
}
