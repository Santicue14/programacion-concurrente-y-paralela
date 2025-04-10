/// <summary>
/// Este programa demuestra el manejo de excepciones en tareas paralelas utilizando la clase Parallel en C#.
/// </summary>
/// <remarks>
/// Versión de C#: 7.3
/// Objetivo .NET: .NET Framework 4.8
/// </remarks>

using System;
using System.IO;
using System.Threading.Tasks;

namespace HandleException
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
                // Invoca los métodos en paralelo y maneja las excepciones
                Parallel.Invoke(new Action[] { MethodA, MethodB, MethodC });
            }
            catch (AggregateException ae)
            {
                // Maneja las excepciones agregadas
                ae.Handle(ex =>
                {
                    Console.WriteLine($"Exception detail: {ex.Message}");
                    // Escribe el mensaje de la excepción en el archivo de log
                    File.AppendAllText(logFilePath, $"{DateTime.Now}: {ex.Message}{Environment.NewLine}");
                    return true;
                });
            }

            // Mensaje para indicar que el programa ha finalizado
            Console.WriteLine("Presiona enter para salir");
            Console.ReadLine();
        }
    }
}

