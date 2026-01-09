using ModelGenerator;
using System;
using System.Threading.Tasks;

internal class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Config path is required");
            Console.ReadLine();
        }
        await new GeneratorApp().RunAsync(args[0]);
    }
}
