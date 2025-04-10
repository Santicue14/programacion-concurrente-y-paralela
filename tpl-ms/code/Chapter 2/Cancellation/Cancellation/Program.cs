/// <summary>
/// Este programa demuestra cómo cancelar una tarea utilizando la clase CancellationToken en C#.
/// </summary>
/// <remarks>
/// Versión de C#: 7.3
/// Objetivo .NET: .NET Framework 4.8
/// </remarks>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation
{
    class Program
    {
        /// <summary>
        /// Método que realiza una espera activa utilizando Thread.SpinWait.
        /// </summary>
        static void CalculateTax() { Thread.SpinWait(int.MaxValue); }

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        static void Main(string[] args)
        {
            // Crea un CancellationTokenSource para gestionar la cancelación de la tarea
            CancellationTokenSource cancellationSource = new CancellationTokenSource();
            CancellationToken token = cancellationSource.Token;

            try
            {
                // Inicia una tarea que se puede cancelar
                Task TaskA = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        CalculateTax();
                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                        }
                    }
                }, token);

                // Espera un segundo antes de cancelar la tarea
                Thread.Sleep(1000);
                cancellationSource.Cancel();

                // Espera a que la tarea finalice
                TaskA.Wait();
            }
            catch (AggregateException ex)
            {
                // Maneja las excepciones agregadas
                Console.WriteLine(ex.InnerException.Message);
            }

            // Mensaje para indicar que el programa ha finalizado
            Console.WriteLine("Presiona enter para salir");
            Console.ReadLine();
        }
    }
}

