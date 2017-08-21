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
        private ExpressionResult finalResult;

        public Script(IEnumerable<IExpression> expressions)
        {
            this.expressions = new List<IExpression>(expressions);
        }

        public ExpressionResult Run(IDictionary<string, ExpressionResult> context)
        {
            context = context ?? new Dictionary<string, ExpressionResult>();

            foreach (IExpression expression in expressions)
            {
                finalResult = expression.Evaluate(context);
            }

            return finalResult;
        }
    }
}
