using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using declang;


namespace declPrompt
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments Arguments = new Arguments();
            if(CommandLine.Parser.Default.ParseArguments(args, Arguments))
            {
                if (Arguments.IsInteractiveModeRequested)
                {
                    InteractiveSession session = new InteractiveSession();
                    session.Start();
                }
                else if (!String.IsNullOrEmpty(Arguments.Statement))
                {
                    Console.WriteLine(decl.Evaluate(Arguments.Statement));
                }
            }
        }
    }
}
