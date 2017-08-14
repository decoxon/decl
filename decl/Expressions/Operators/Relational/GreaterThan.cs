using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class GreaterThan : BinaryOperator
    {
        public GreaterThan(IExpression leftOperand, IExpression rightOperand, string format = "{1}{0}{2}")
            : base(" > ", leftOperand, rightOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            ExpressionResult left = LeftOperand.Evaluate(context);
            ExpressionResult right = RightOperand.Evaluate(context);

            if (left.Type == ExpressionType.Number && right.Type == ExpressionType.Number)
            {
                string result = Decimal.Parse(left.Value) > Decimal.Parse(right.Value) ? "true" : "false";

                return new ExpressionResult(ExpressionType.Truth, result);
            }

            throw new Exception(String.Format("Cannot compare expressions of type {0} and {1}", left.Type.ToString(), right.Type.ToString()));
        }
    }
}
