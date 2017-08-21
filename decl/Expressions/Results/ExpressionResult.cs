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
        private IEnumerable<ExpressionResult> componentResults;

        public ExpressionType Type => type;
        public string Value => value;
        public IEnumerable<ExpressionResult> ComponentResults => componentResults;

        public ExpressionResult(ExpressionType type, string value, IEnumerable<ExpressionResult> componentResults = null)
        {
            this.type = type;
            this.value = value;
            this.componentResults = componentResults ?? new List<ExpressionResult>();
        }

        public override string ToString()
        {
            return value;
        }
    }
}
