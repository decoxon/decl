using System;
using System.Collections.Generic;

namespace declang.Expressions
{
  internal class Multiplication : BinaryOperator
  {
    public Multiplication(IExpression leftOperand, IExpression rightOperand)
        : base(leftOperand, rightOperand) { }

    public override IExpressionResult Evaluate(Thing context)
    {
      return multiply(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
    }

    private IExpressionResult multiply(IExpressionResult leftOperand, IExpressionResult rightOperand)
    {
      IExpressionResult left = leftOperand.As(ExpressionType.Number);
      IExpressionResult right = rightOperand.As(ExpressionType.Number);
      var leftDecimal = Decimal.Parse(left.Value);
      var rightDecimal = Decimal.Parse(right.Value);
      result = new ExpressionResult(this.GetType().Name, ExpressionType.Number, (leftDecimal * rightDecimal).ToString("G29"), new List<IExpressionResult>() { left, right });
      return result;
    }
  }
}
