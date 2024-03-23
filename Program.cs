namespace AsyncMatrixMultiplexor
{
    public class Program
    {
        public static object o = new object();
        public static readonly int ProcessorCount = Environment.ProcessorCount - 2;
        public static async Task Main(string[] args)
        {

            var a = new Matrix(1000, 2000);
            var b = new Matrix(2000, 3000);
            var equation = new Equation(a, b);
            await equation.Prepare(100);
            PrintSys($"Вычисление произведения матриц будет происходить на {ProcessorCount} логических ядрах.");
            var solve = equation.SolveAsync(ProcessorCount);
            var printPlus = Task.Run(() => { Print("+", 20, ConsoleColor.Green, solve); });
            var printMinus = Task.Run(() => { Print("-", 40, ConsoleColor.Red, solve); });
            Task.WaitAny(solve, printMinus, printPlus);
            Console.ReadLine();
        }

        public static Task Print(string str, int delayInMs, ConsoleColor color, Task until)
        {
            while (true)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                lock (o)
                {
                    Console.ForegroundColor = color;
                    Console.Write($"({str} / поток {threadId}) ");
                }
                Task.Delay(delayInMs).Wait();
                if (until.IsCompleted)
                {
                    break;
                }
            }
            return Task.CompletedTask;
        }

        public static void PrintSys(string str)
        {
            lock (o)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n" + "\n" + str);
            }
        }

    }
}
