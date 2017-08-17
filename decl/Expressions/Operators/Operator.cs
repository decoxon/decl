using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class Operator : IExpression
    {
        protected string format;
        protected string operatorString;
        protected ExpressionResult result = null;

        public Operator(string operatorString = "", string format = "")
        {
            this.operatorString = operatorString;
            this.format = format;
        }

        public ExpressionResult Result { get { return result; } }

        public abstract ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context);
    }
}
