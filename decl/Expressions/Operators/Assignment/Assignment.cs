using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Assignment : BinaryOperator
    {
        public Assignment(Variable leftOperand, IExpression rightOperand, string format = "{0} = {1}")
            : base(leftOperand, rightOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            result = RightOperand.Evaluate(context);
            context[((Variable)LeftOperand).Name] = result;
            return new ExpressionResult(this.GetType().Name, result.Type, result.Value, new List<ExpressionResult> { result });
        }
    }
}
