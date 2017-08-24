using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal class Division : BinaryOperator
    {
        public Division(IExpression leftOperand, IExpression rightOperand, string format = "{0} / {1}")
            : base(leftOperand, rightOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return divide(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
        }

        private ExpressionResult divide(ExpressionResult leftOperand, ExpressionResult rightOperand)
        {
            if ((leftOperand.Type == ExpressionType.Number && rightOperand.Type == ExpressionType.Number))
            {
                if (Decimal.TryParse(leftOperand.Value, out decimal leftDecimal) && Decimal.TryParse(rightOperand.Value, out decimal rightDecimal))
                {
                    result = new ExpressionResult(this.GetType().Name, ExpressionType.Number, (leftDecimal / rightDecimal).ToString(), new List<ExpressionResult>() { leftOperand, rightOperand });
                    return result;
                }
            }

            throw new Exception(String.Format("Cannot divide expressions of type {0} and {1}", leftOperand.Type, rightOperand.Type));
        }
    }
}
