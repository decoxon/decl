using System;
using System.Collections.Generic;
using System.Text;
using declang.Expressions;

namespace declang
{
    public class ExpressionResult
    {
        private string operationType;
        private ExpressionType type;
        private string value;
        private IEnumerable<ExpressionResult> componentResults;

        public string OperationType => operationType;
        public ExpressionType Type => type;
        public string Value => value;
        public IEnumerable<ExpressionResult> ComponentResults => componentResults;

        public ExpressionResult(string operationType, ExpressionType type, string value, IEnumerable<ExpressionResult> componentResults = null)
        {
            this.operationType = operationType;
            this.type = type;
            this.value = value;
            this.componentResults = componentResults ?? new List<ExpressionResult>();
        }

        public override string ToString()
        {
            return value;
        }

        public virtual string ToResultDetailString(int depth = 0)
        {
            string spacing = GetSpacingString(depth);

            StringBuilder detailString = GetBasicValueStringBuilder(spacing);

            AddComponentResultDetailStrings(detailString, depth + 1);

            return detailString.ToString();
        }

        protected StringBuilder GetBasicValueStringBuilder(string spacing)
        {
            StringBuilder detailString = new StringBuilder();
            detailString.Append(spacing);
            detailString.Append(operationType);
            detailString.Append("(");
            detailString.Append(type.ToString());
            detailString.Append("): ");
            detailString.AppendLine(value);
            return detailString;
        }

        protected void AddComponentResultDetailStrings(StringBuilder detailString, int depth)
        {
            foreach (ExpressionResult componentResult in componentResults)
            {
                detailString.Append(componentResult.ToResultDetailString(depth));
            }
        }

        protected string GetSpacingString(int depth)
        {
            string spacing = "";
            for (int i = 0; i < depth; i++)
            {
                spacing += "  ";
            }
            spacing += "-";
            return spacing;
        }
    }
}
