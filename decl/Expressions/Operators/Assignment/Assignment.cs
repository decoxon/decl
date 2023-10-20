using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
  internal class Assignment : BinaryOperator
  {
    public Assignment(Variable leftOperand, IExpression rightOperand)
        : base(leftOperand, rightOperand) { }

    public override IExpressionResult Evaluate(Thing context)
    {
      result = RightOperand.Evaluate(context);
      Variable v = leftOperand.ToVariable();
      context.SetValue(LeftOperand.ToVariable().Name, result);
      return new ExpressionResult(this.GetType().Name, result.Type, result.Value, new List<IExpressionResult> { result });
    }
  }
}
