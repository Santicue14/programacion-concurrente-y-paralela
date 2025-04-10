/// <summary>
/// Este programa demuestra el manejo de excepciones en una tarea que provoca una división por cero utilizando la clase Task en C#.
/// </summary>
/// <remarks>
/// Versión de C#: 7.3
/// Objetivo .NET: .NET Framework 4.8
/// </remarks>

using System;
using System.Threading.Tasks;

namespace ParallelProgramming.Parte4
{
    class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        static void Main(string[] args)
        {
            Task TaskA = null;

            try
            {
                // Inicia una tarea que provoca una división por cero
                TaskA = Task.Factory.StartNew(() =>
                {
                    int a = 5, b = 0;
                    a /= b; // Esto provocará una excepción DivideByZeroException
                });

                // Espera a que la tarea finalice
                TaskA.Wait();
            }
            catch (AggregateException ae)
            {
                // Imprime el estado de la tarea
                Console.WriteLine("Task has " + TaskA.Status.ToString());

                // Maneja las excepciones agregadas
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Exception detail: {ex.Message}");
                }
            }

            // Mensaje para indicar que el programa ha finalizado
            Console.WriteLine("Presiona enter para salir");
            Console.ReadLine();
        }
    }
}

