using declang.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Parsing
{
    /// <summary>
    /// Represents a series of expressions to be executed sequentially.
    /// </summary>
    class Script
    {
        private List<IExpression> expressions;
        private IExpressionResult finalResult;

        public Script(IEnumerable<IExpression> expressions)
        {
            this.expressions = new List<IExpression>(expressions);
        }

        public IExpressionResult Run(Thing context = null)
        {
            context = context ?? new Thing();

            foreach (IExpression expression in expressions)
            {
                finalResult = expression.Evaluate(context);
            }

            return finalResult;
        }

        public bool IsSingleExpressionOfType<T>()
        {
            return expressions.Count == 1 && expressions[0] is T;
        }
    }
}
