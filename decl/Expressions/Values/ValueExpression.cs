using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
  internal abstract class ValueExpression : IExpression
  {
    protected IExpressionResult result;
    public IExpressionResult Result => result;

    public abstract IExpressionResult Evaluate(Thing context);

    public virtual Variable ToVariable()
    {
      throw new Exception(String.Format("Value expression {0} cannot be treated as a variable", result));
    }
  }
}
