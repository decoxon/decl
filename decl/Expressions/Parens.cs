using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace decl.Expressions
{
    internal class Parens : IExpression
    {
        private IExpression innerExpression;

        public Parens(IExpression innerExpression)
        {
            this.innerExpression = innerExpression;
        }

        public int Evaluate()
        {
            return innerExpression.Evaluate();
        }

        public override string ToString()
        {
            return String.Format("({0})", innerExpression.ToString());
        }
    }
}
