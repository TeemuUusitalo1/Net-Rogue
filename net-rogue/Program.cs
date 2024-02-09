namespace net_rogue
{

    internal class Program

    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Input a number 1-10");
            string input = Console.ReadLine();
            if (input != null)
            {
                try
                {
                    int number = System.Convert.ToInt32(input);
                    // If Convert.ToInt32 fails, the rest of try
                    // block is not executed
                    Console.WriteLine("Got number: " + number);
                    if (number < 1 || number > 10)
                    {
                        Console.WriteLine("The number is not in requested range.");
                    }
                }
                catch (FormatException fe)
                {
                    Console.WriteLine("Cannot convert text \"" + input + "\" to integer.");
                    Console.WriteLine(fe.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unhandled exception: " + e.ToString());
                }
            }
            else
            {
                Console.WriteLine("No input received.");
            }
        }

    }
}
