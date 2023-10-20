using System;
using System.Collections.Generic;
using System.Text;
using declang.Parsing;

namespace declang.Expressions
{
  internal class Accessor : BinaryOperator
  {
    public Accessor(IExpression leftOperand, IExpression rightOperand)
        : base(leftOperand, rightOperand)
    {
      if (!(LeftOperand.ToVariable() is Variable) || !(RightOperand.ToVariable() is Variable))
      {
        throw new Exception(String.Format("Invalid operands for access operator: {0} {1}", LeftOperand, RightOperand));
      }
    }

    public override IExpressionResult Evaluate(Thing context)
    {
      IExpressionResult container = leftOperand.Evaluate(context).As(ExpressionType.Thing);
      return ((Thing)container).GetValue(((Variable)rightOperand).Name);
    }

    public override Variable ToVariable()
    {
      return new Variable(base.ToString());
    }
  }
}
