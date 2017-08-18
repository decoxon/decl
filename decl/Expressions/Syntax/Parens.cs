using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace declang.Expressions
{
    internal class Parens : IExpression
    {
        private IExpression innerExpression;
        private ExpressionResult result;

        public Parens(IExpression innerExpression)
        {
            this.innerExpression = innerExpression;
        }

        public ExpressionResult Result => result;

        public ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            result = innerExpression.Evaluate(context);
            return result;
        }

        public override string ToString()
        {
            return String.Format("({0})", innerExpression.ToString());
        }
    }
}
