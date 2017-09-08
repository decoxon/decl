using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class LessThan : BinaryOperator
    {
        public LessThan(IExpression leftOperand, IExpression rightOperand, string format = "{0} < {1}")
            : base(leftOperand, rightOperand, format) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult left = LeftOperand.Evaluate(context);
            IExpressionResult right = RightOperand.Evaluate(context);

            if (left.Type == ExpressionType.Number && right.Type == ExpressionType.Number)
            {
                result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, (Decimal.Parse(left.Value) < Decimal.Parse(right.Value) ? "true" : "false"), new List<IExpressionResult>() { left, right });
                return result;
            }

            throw new Exception(String.Format("Cannot compare expressions of type {0} and {1}", left.Type.ToString(), right.Type.ToString()));
        }
    }
}
