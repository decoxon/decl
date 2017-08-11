using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Truth : IExpression
    {
        bool value;

        public Truth(string value)
        {
            if(value.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                this.value = true;
            }
            else if (value.Equals("false", StringComparison.CurrentCultureIgnoreCase))
            {
                this.value = false;
            }
            else
            {
                throw new Exception(String.Format("Invalid truth value provided: {0}", value));
            }
        }

        public ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return new ExpressionResult(ExpressionType.Truth, value ? "true" : "false");
        }
    }
}
