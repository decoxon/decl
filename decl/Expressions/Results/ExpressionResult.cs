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
        private List<ExpressionResult> componentResults;

        public ExpressionType Type => type;
        public string Value => value;
        public IReadOnlyList<ExpressionResult> ComponentResults => componentResults;

        public ExpressionResult(ExpressionType type, string value, IEnumerable<ExpressionResult> componentResults = null)
        {
            this.type = type;
            this.value = value;
            this.componentResults = componentResults == null ? new List<ExpressionResult>(componentResults) : new List<ExpressionResult>();
        }

        public override string ToString()
        {
            return value;
        }
    }
}
