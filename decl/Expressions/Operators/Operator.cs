using System;
using System.Collections.Generic;
using System.Text;
using declang.Parsing;

namespace declang.Expressions
{
  internal abstract class Operator : IExpression
  {
    protected string format;
    protected IExpressionResult result = null;

    public Operator()
    {
      this.format = ExpressionDefinitions.GetToStringFormatForType(this.GetType());
    }

    public IExpressionResult Result { get { return result; } }

    public abstract IExpressionResult Evaluate(Thing context);

    public virtual Variable ToVariable()
    {
      throw new Exception(String.Format("Operator {0} cannot be treated as a variable", String.Format(format, "", "")));
    }
  }
}
