using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class ThingLiteral : ValueExpression
    {
        private IDictionary<string, IExpression> contents;

        public ThingLiteral(IDictionary<string, IExpression> value)
        {
            contents = value;
        }

        public override IExpressionResult Evaluate(Thing context)
        {
            Dictionary<string, IExpressionResult> result = new Dictionary<string, IExpressionResult>();

            foreach(KeyValuePair<string, IExpression> kvp in contents)
            {
                result[kvp.Key] = kvp.Value.Evaluate(context);
            }

            return new Thing("ThingLiteral", ExpressionType.Thing, "", null, result);
        }
    }
}
