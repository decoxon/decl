using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Modulo : BinaryOperator
    {
        public Modulo(IExpression leftOperand, IExpression rightOperand, string format = "{0} + {1}")
            : base(leftOperand, rightOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return modulo(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
        }

        private ExpressionResult modulo(ExpressionResult leftOperand, ExpressionResult rightOperand)
        {
            if ((leftOperand.Type == ExpressionType.Number && rightOperand.Type == ExpressionType.Number))
            {
                if (Decimal.TryParse(leftOperand.Value, out decimal leftDecimal) && Decimal.TryParse(rightOperand.Value, out decimal rightDecimal))
                {
                    result = new ExpressionResult(ExpressionType.Number, (leftDecimal % rightDecimal).ToString(), new List<ExpressionResult>() { leftOperand, rightOperand });
                    return result;
                }
            }

            throw new Exception(String.Format("Cannot modulo expressions of type {0} and {1}", leftOperand.Type, rightOperand.Type));
        }
    }
}
