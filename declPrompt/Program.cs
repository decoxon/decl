using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using declang;
using System.IO;


namespace declPrompt
{
  class Program
  {
    static void Main(string[] args)
    {
      var arguments = new Arguments();
      var parser = new CommandLine.Parser(with => with.MutuallyExclusive = true);

      if (parser.ParseArguments(args, arguments))
      {
        if (arguments.IsInteractiveModeRequested)
        {
          var session = new InteractiveSession();
          session.Start();
        }
        else if (!String.IsNullOrEmpty(arguments.FilePath))
        {
          if (!File.Exists(arguments.FilePath))
          {
            Console.WriteLine(String.Format("No file at this location: {0}", arguments.FilePath));
          }

          Console.WriteLine(DECL.Evaluate(File.ReadAllText(arguments.FilePath)));
        }
        else if (!String.IsNullOrEmpty(arguments.Statement))
        {
          Console.WriteLine(DECL.Evaluate(arguments.Statement));
        }
      }
    }
  }
}
