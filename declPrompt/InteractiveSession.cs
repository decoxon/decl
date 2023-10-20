using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using declang;
using declang.Expressions;

namespace declPrompt
{
  class InteractiveSession
  {
    private IDictionary<string, IExpressionResult> context;
    private bool outputDetail = false;

    public InteractiveSession(IDictionary<string, IExpressionResult> context = null)
    {
      this.context = context ?? new Dictionary<string, IExpressionResult>();
    }

    public void Start()
    {
      Console.WriteLine("decl Interactive Mode");
      Console.WriteLine("=====================");

      string statement = null;
      statement = getStatement();
      while (statement != "exit")
      {
        try
        {
          executeStatement(statement);
        }
        catch (Exception e)
        {
          outputException(e);
        }
        statement = getStatement();
      }
    }

    private void outputException(Exception e)
    {
      Console.WriteLine("Exception Occurred");
      Console.WriteLine("=====================");
      Console.WriteLine(e.ToString());
      Console.WriteLine("=====================");
    }

    private void executeStatement(string statement)
    {
      switch (statement)
      {
        case "reset":
          context = new Dictionary<string, IExpressionResult>();
          Console.WriteLine("Context Reset");
          break;
        case "context":
          outputContext();
          break;
        case "toggle detail":
          outputDetail = !outputDetail;
          Console.WriteLine("Detailed output " + (outputDetail ? "on" : "off"));
          break;
        default:
          outputResult(DECL.Evaluate(statement, context));
          break;
      }
    }

    private void outputResult(IExpressionResult result)
    {
      Console.WriteLine(result);
      if (outputDetail)
      {
        var previousColour = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        outputResultTree(result);
        Console.ForegroundColor = previousColour;
      }
    }

    private void outputContext()
    {
      Console.WriteLine("Current context:");
      foreach (var kvp in context)
      {
        Console.WriteLine(String.Format("    {0}: {1} ({2})", kvp.Key, kvp.Value.Value, kvp.Value.Type.ToString()));
      }
    }

    private string getStatement()
    {
      var previousColour = Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write("> ");
      var command = Console.ReadLine();
      Console.ForegroundColor = previousColour;
      return command;

    }

    private void outputResultTree(IExpressionResult result)
    {
      Console.WriteLine("Result Tree:");
      Console.WriteLine(result.ToResultDetailString());
    }
  }
}
