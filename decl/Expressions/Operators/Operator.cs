using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class Operator : IExpression
    {
        protected string format;
        protected string operatorString;

        public Operator(string operatorString = "", string format = "")
        {
            this.operatorString = operatorString;
            this.format = format;
        }

        public abstract ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context);
    }
}
