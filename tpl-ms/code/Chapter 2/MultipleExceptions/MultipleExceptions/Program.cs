/// <summary>
/// Este programa demuestra el manejo de múltiples excepciones en tareas paralelas utilizando la clase Task en C#.
/// </summary>
/// <remarks>
/// Versión de C#: 7.3
/// Objetivo .NET: .NET Framework 4.8
/// </remarks>

using System;
using System.IO;
using System.Threading.Tasks;

namespace MultipleExceptions
{
    class Program
    {
        /// <summary>
        /// Método que lanza una excepción con el mensaje "TaskA Exception".
        /// </summary>
        static void MethodA() { throw new Exception("TaskA Exception"); }

        /// <summary>
        /// Método que lanza una excepción con el mensaje "TaskB Exception".
        /// </summary>
        static void MethodB() { throw new Exception("TaskB Exception"); }

        /// <summary>
        /// Método que lanza una excepción con el mensaje "TaskC Exception".
        /// </summary>
        static void MethodC() { throw new Exception("TaskC Exception"); }

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        static void Main(string[] args)
        {
            string logFilePath = "exceptions.log";

            try
            {
                // Inicia las tareas que lanzan excepciones
                var TaskA = Task.Factory.StartNew(MethodA);
                var TaskB = Task.Factory.StartNew(MethodB);
                var TaskC = Task.Factory.StartNew(MethodC);

                // Espera a que todas las tareas finalicen
                Task.WaitAll(new Task[] { TaskA, TaskB, TaskC });
            }
            catch (AggregateException ae)
            {
                // Maneja las excepciones agregadas
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                    // Escribe el mensaje de la excepción en el archivo de log
                    File.AppendAllText(logFilePath, $"{DateTime.Now}: {ex.Message}{Environment.NewLine}");
                }
            }

            // Mensaje para indicar que el programa ha finalizado
            Console.WriteLine("Presiona enter para salir");
            Console.ReadLine();
        }
    }
}


