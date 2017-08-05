using System;
using System.Collections.Generic;
using System.Text;
using declang.Expressions;

namespace declang
{
    public class ExpressionResult
    {
        private ExpressionType type;
        private string value;

        public ExpressionType Type => type;
        public string Value => value;

        public ExpressionResult(ExpressionType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}
