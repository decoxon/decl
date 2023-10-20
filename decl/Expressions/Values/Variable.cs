using System;
using System.Collections.Generic;

namespace declang.Expressions
{
  class Variable : ValueExpression
  {
    private string variableName;

    public string Name => variableName;

    public Variable(string variableName)
    {
      this.variableName = variableName;
    }

    public override IExpressionResult Evaluate(Thing context)
    {
      return context.GetValue(variableName);
    }

    public override Variable ToVariable()
    {
      return this;
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
