// Este programa demuestra la ejecución de dos tareas en paralelo y espera a que cualquiera de ellas finalice primero utilizando la clase Task en C#.
// Versión de C#: 7.3
// Objetivo .NET: .NET Framework 4.8

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming.Parte2
{
    class Program
    {
        // Método que realiza una espera activa utilizando Thread.SpinWait
        static void MethodA()
        {
            Thread.SpinWait(int.MaxValue);
        }

        // Método que realiza una espera activa utilizando Thread.SpinWait
        static void MethodB() { Thread.SpinWait(int.MaxValue / 2); }

        static void Main(string[] args)
        {
            // Inicia la tarea que ejecuta MethodA
            var TaskA = Task.Factory.StartNew(MethodA);

            // Inicia la tarea que ejecuta MethodB
            var TaskB = Task.Factory.StartNew(MethodB);

            // Imprime los IDs de las tareas
            Console.WriteLine("TaskA id = {0}", TaskA.Id);
            Console.WriteLine("TaskB id = {0}", TaskB.Id);

            var tasks = new Task[] { TaskA, TaskB };

            try
            {
                // Espera a que cualquiera de las tareas finalice
                int whichTask = Task.WaitAny(tasks);
                Console.WriteLine("Task {0} is the gold medal task.", tasks[whichTask].Id);
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

