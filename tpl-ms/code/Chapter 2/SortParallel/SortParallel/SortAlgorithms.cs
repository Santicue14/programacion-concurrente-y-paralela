using System;
using System.Collections.Generic;

namespace SortParallel
{
    /// <summary>
    /// Clase que contiene los algoritmos de ordenación.
    /// </summary>
    public static class SortAlgorithms
    {
        /// <summary>
        /// Ordena una lista de enteros utilizando el algoritmo de ordenación por burbuja.
        /// </summary>
        /// <param name="list">La lista de enteros a ordenar.</param>
        public static void BubbleSort(List<int> list)
        {
            int count = list.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        int temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Ordena una lista de enteros utilizando el algoritmo de ordenación por inserción.
        /// </summary>
        /// <param name="list">La lista de enteros a ordenar.</param>
        public static void InsertionSort(List<int> list)
        {
            int count = list.Count;
            for (int i = 1; i < count; i++)
            {
                int key = list[i];
                int j = i - 1;
                while (j >= 0 && list[j] > key)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }
    }
}

