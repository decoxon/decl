using System;
using System.Collections.Generic;
using System.Text;
using declang.Parsing;
using declang.Expressions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("test declang")]

namespace declang
{
  public static class DECL
  {

    public static IExpressionResult Evaluate(string expression, IDictionary<string, IExpressionResult> context = null)
    {
      try
      {
        var statements = new List<string>(expression.Split(Environment.NewLine.ToCharArray()));
        context = new Thing(context ?? new Dictionary<string, IExpressionResult>());
        return Parser.Parse(expression).Run(new Thing(context));
      }
      catch (Exception e)
      {
        throw new Exception("Exception ocurred during execution", e);
      }
    }
  }
}
