using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal class Multiplication : BinaryOperator
    {
        public Multiplication(IExpression leftOperand, IExpression rightOperand)
            : base(" * ", leftOperand, rightOperand) {}

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return multiply(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
        }

        private ExpressionResult multiply(ExpressionResult leftOperand, ExpressionResult rightOperand)
        {
            if ((leftOperand.Type == ExpressionType.Number && rightOperand.Type == ExpressionType.Number))
            {
                if (Decimal.TryParse(leftOperand.Value, out decimal leftDecimal) && Decimal.TryParse(rightOperand.Value, out decimal rightDecimal))
                {
                    result = new ExpressionResult(ExpressionType.Number, (leftDecimal * rightDecimal).ToString());
                    return result;
                }
            }

            throw new Exception(String.Format("Cannot multiply expressions of type {0} and {1}", leftOperand.Type, rightOperand.Type));
        }
    }
}
