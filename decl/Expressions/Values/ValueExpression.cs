using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class ValueExpression : IExpression
    {
        protected ExpressionResult result;
        ExpressionResult IExpression.Result => result;

        public abstract ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context);
    }
}
