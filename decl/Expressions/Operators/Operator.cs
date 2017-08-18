using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class Operator : IExpression
    {
        protected string format;
        protected ExpressionResult result = null;

        public Operator(string format = "")
        {
            this.format = format;
        }

        public ExpressionResult Result { get { return result; } }

        public abstract ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context);
    }
}
