using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuCore.CommandLine;

namespace FubuDocsRunner
{

    public class FubuDocsExecutor : CommandExecutor
    {
        
    }

    class Program
    {
        private static bool success;

        public static int Main(string[] args)
        {
            // Try to magically determine the FubuMvcPackage folder here
            try
            {
                var factory = new CommandFactory();
                factory.RegisterCommands(typeof(Program).Assembly);

                var executor = new CommandExecutor(factory);
                success = executor.Execute(args);
            }
            catch (CommandFailureException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ResetColor();
                return 1;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + ex);
                Console.ResetColor();
                return 1;
            }

            return success ? 0 : 1;
        }
    }
}
