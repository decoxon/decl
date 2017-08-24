using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Word : ValueExpression
    {
        private string value;

        public Word(string value)
        {
            this.value = value;
        }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            result = new ExpressionResult(this.GetType().Name, ExpressionType.Word, value);
            return result;
        }
    }
}
