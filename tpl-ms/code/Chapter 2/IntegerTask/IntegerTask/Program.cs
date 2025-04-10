// Este programa demuestra la ejecución de una tarea que devuelve un valor entero aleatorio utilizando la clase Task en C#.
// Versión de C#: 7.3
// Objetivo .NET: .NET Framework 4.8

using System;
using System.Threading.Tasks;

namespace ParallelProgramming.Parte3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inicia una tarea que devuelve un valor entero aleatorio
            var TaskA = Task<int>.Factory.StartNew(() =>
            {
                Random random = new Random();
                return random.Next(1, 101); // Genera un número aleatorio entre 1 y 100
            });

            try
            {
                // Espera a que la tarea finalice
                TaskA.Wait();

                // Imprime el resultado de la tarea
                Console.WriteLine(TaskA.Result);
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

