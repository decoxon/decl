using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Truth : ValueExpression
    {
        public const string TRUE = "true";
        public const string FALSE = "false";

        bool value;

        public Truth(string value)
        {
            if(value.Equals(TRUE, StringComparison.CurrentCultureIgnoreCase))
            {
                this.value = true;
            }
            else if (value.Equals(FALSE, StringComparison.CurrentCultureIgnoreCase))
            {
                this.value = false;
            }
            else
            {
                throw new Exception(String.Format("Invalid truth value provided: {0}", value));
            }
        }
        
        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            result = new ExpressionResult(ExpressionType.Truth, value ? TRUE : FALSE);
            return result;
        }
    }
}
