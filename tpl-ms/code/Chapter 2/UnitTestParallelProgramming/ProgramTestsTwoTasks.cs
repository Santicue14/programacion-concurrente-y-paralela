using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using ParallelProgramming.Parte1;

namespace ParallelProgramming.Tests
{
    [TestClass]
    public class ProgramTestsTwoTasks
    {
        [TestMethod]
        public void MethodA_PrintsMethodA()
        {
            // Arrange
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.MethodA();

                // Assert
                var result = sw.ToString().Trim();
                Assert.AreEqual("MethodA", result);
            }
        }

        [TestMethod]
        public void MethodB_PrintsMethodB()
        {
            // Arrange
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.MethodB();

                // Assert
                var result = sw.ToString().Trim();
                Assert.AreEqual("MethodB", result);
            }
        }

        [TestMethod]
        public void Main_WaitsForTasksToComplete()
        {
            // Arrange
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader(" "))
                {
                    Console.SetIn(sr);

                    // Act
                    Program.Main(new string[] { });

                    // Assert
                    var result = sw.ToString();
                    StringAssert.Contains(result, "MethodA");
                    StringAssert.Contains(result, "MethodB");
                    StringAssert.Contains(result, "Presiona enter para salir");
                }
            }
        }
    }
}
