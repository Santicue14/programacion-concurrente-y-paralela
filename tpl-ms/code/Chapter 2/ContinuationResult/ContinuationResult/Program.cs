/// <summary>
/// Este programa demuestra el uso de tareas continuas y cómo obtener resultados de tareas en C#.
/// </summary>
/// <remarks>
/// Versión de C#: 7.3
/// Objetivo .NET: .NET Framework 4.8
/// </remarks>

using System;
using System.Threading.Tasks;

namespace ContinuationResult
{
    class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        static void Main(string[] args)
        {
            // Crea una tarea que calcula un resultado
            var calculate = new Task<int>(() =>
            {
                Console.WriteLine("Calculate result.");
                Random random = new Random();
                return random.Next(1, 101); // Genera un número aleatorio entre 1 y 100
            });

            // Crea una tarea continua que utiliza el resultado de la tarea anterior
            var answer = calculate.ContinueWith((predecessor) =>
            {
                if (predecessor.Exception != null)
                {
                    // Maneja las excepciones de la tarea anterior
                    foreach (var ex in predecessor.Exception.InnerExceptions)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("The answer is {0}.", predecessor.Result);
                }
            });

            try
            {
                // Inicia la tarea de cálculo
                calculate.Start();

                // Espera a que ambas tareas finalicen
                Task.WaitAll(calculate, answer);
            }
            catch (AggregateException ae)
            {
                // Maneja las excepciones agregadas
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            // Mensaje para indicar que el programa ha finalizado
            Console.WriteLine("Presiona enter para salir");
            Console.ReadLine();
        }
    }
}


