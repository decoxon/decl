using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
  internal interface IExpression
  {
    IExpressionResult Result { get; }
    IExpressionResult Evaluate(Thing context);
    Variable ToVariable();
  }
}
