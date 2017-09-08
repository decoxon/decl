using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal class Multiplication : BinaryOperator
    {
        public Multiplication(IExpression leftOperand, IExpression rightOperand, string format = "{0} * {1}")
            : base(leftOperand, rightOperand, format) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            return multiply(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
        }

        private IExpressionResult multiply(IExpressionResult leftOperand, IExpressionResult rightOperand)
        {
            if ((leftOperand.Type == ExpressionType.Number && rightOperand.Type == ExpressionType.Number))
            {
                if (Decimal.TryParse(leftOperand.Value, out decimal leftDecimal) && Decimal.TryParse(rightOperand.Value, out decimal rightDecimal))
                {
                    result = new ExpressionResult(this.GetType().Name, ExpressionType.Number, (leftDecimal * rightDecimal).ToString(), new List<IExpressionResult>() { leftOperand, rightOperand });
                    return result;
                }
            }

            throw new Exception(String.Format("Cannot multiply expressions of type {0} and {1}", leftOperand.Type, rightOperand.Type));
        }
    }
}
