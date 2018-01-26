using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class LessThan : BinaryOperator
    {
        public LessThan(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult left = LeftOperand.Evaluate(context).As(ExpressionType.Number);
            IExpressionResult right = RightOperand.Evaluate(context).As(ExpressionType.Number);
            result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, (Decimal.Parse(left.Value) < Decimal.Parse(right.Value) ? Truth.TRUE : Truth.FALSE), new List<IExpressionResult>() { left, right });
            return result;
        }
    }
}
