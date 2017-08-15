using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Assignment : BinaryOperator
    {
        public Assignment(Variable leftOperand, IExpression rightOperand)
            : base(" = ", leftOperand, rightOperand)
        {
        }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            ExpressionResult result = RightOperand.Evaluate(context);
            context[((Variable)LeftOperand).Name] = result;
            return result;
        }
    }
}
