using declang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace declang.Expressions
{
    internal class Parens : SyntaxExpression
    {
        private IExpression innerExpression;

        public Parens(IExpression innerExpression)
        {
            this.innerExpression = innerExpression;
        }

        public override IExpressionResult Evaluate(Thing context)
        {
            result = innerExpression.Evaluate(context);
            return result;
        }

        public override string ToString()
        {
            return String.Format(ExpressionDefinitions.GetToStringFormatForType(this.GetType()), innerExpression.ToString());
        }
    }
}
