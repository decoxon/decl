using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class Operator : IExpression
    {
        protected string format;
        protected IExpressionResult result = null;

        public Operator(string format = "")
        {
            this.format = format;
        }

        public IExpressionResult Result { get { return result; } }

        public abstract IExpressionResult Evaluate(Thing context);

        public virtual Variable ToVariable()
        {
            throw new Exception(String.Format("Operator {0} cannot be treated as a variable", format));
        }
    }
}
