using System;

namespace InterfaceActivityBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleInitialMessage();
            while (true)
            {
                try
                {
                    ResolveMessage(args);
                    ConsoleWait();
                    break;
                }
                catch (Exception exception)
                {
                    ConsoleException(exception);
                }
                finally
                {
                    args = null;
                }
            }
        }        

        private static void ResolveMessage(string[] args)
        {
            string path = ReadPath(args);
            ICanonicResolver resolver = new ClaroCanonicResolver(path);
            resolver.Resolve();
        }

        private static string ReadPath(string[] args)
        {
            string path = string.Empty;
            if (args != null && args.Length > 0)
            {
                path = args[0];                
            }
            else
            {
                ConsoleReadPath();
                path = Console.ReadLine(); // i.e. "C:\\Users\\ezozkme\\Desktop\\Book2.xlsx";
            }

            return path;
        }

        private static void ConsoleInitialMessage()
        {
            Console.WriteLine();
            Console.WriteLine("******************************************************************");
            Console.WriteLine("Interface Activity Builder 2018 Command Prompt");
            Console.WriteLine("Copright (c) 2018 Mehmet Ozkaya");
            Console.WriteLine("******************************************************************");
            Console.WriteLine();
        }

        private static void ConsoleReadPath()
        {            
            Console.Write("Please insert excel path : ");
        }

        private static void ConsoleException(Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(exception.Message);
            Console.WriteLine();
        }

        private static void ConsoleWait()
        {
            Console.Read();
        }
    }
}
