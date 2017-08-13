using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Word : IExpression
    {
        private string value;

        public Word(string value)
        {
            this.value = value;
        }

        ExpressionResult IExpression.Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return new ExpressionResult(ExpressionType.Word, value);
        }
    }
}
