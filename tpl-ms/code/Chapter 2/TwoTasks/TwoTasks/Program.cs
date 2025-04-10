// Este programa demuestra la ejecución de dos tareas en paralelo.
// Versión de C#: 7.3
// Objetivo .NET: .NET Framework 4.8

using System;
using System.Threading.Tasks;

namespace ParallelProgramming.Parte1
{
    public class Program
    {
        // Método que imprime "MethodA" en la consola
        public static void MethodA()
        {
            Console.WriteLine("MethodA");
        }

        // Método que imprime "MethodB" en la consola
        public static void MethodB()
        {
            Console.WriteLine("MethodB");
        }

        public static void Main(string[] args)
        {
            // Inicia la tarea que ejecuta MethodA
            var TaskA = Task.Factory.StartNew(MethodA);

            // Inicia la tarea que ejecuta MethodB
            var TaskB = Task.Factory.StartNew(MethodB);

            try
            {
                // Espera a que ambas tareas finalicen
                Task.WaitAll(new Task[] { TaskA, TaskB });
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

