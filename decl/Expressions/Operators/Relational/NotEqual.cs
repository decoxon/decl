using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
  internal class NotEqual : BinaryOperator
  {
    public NotEqual(IExpression leftOperand, IExpression rightOperand)
        : base(leftOperand, rightOperand) { }

    public override IExpressionResult Evaluate(Thing context)
    {
      IExpressionResult left = LeftOperand.Evaluate(context);
      IExpressionResult right = RightOperand.Evaluate(context).As(left.Type);
      result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, (!left.Value.Equals(right.Value) ? Truth.TRUE : Truth.FALSE), new List<IExpressionResult>() { left, right });
      return result;
    }
  }
}
