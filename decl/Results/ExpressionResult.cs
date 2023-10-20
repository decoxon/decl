using System;
using System.Collections.Generic;
using System.Text;
using declang.Expressions;

namespace declang
{
    public class ExpressionResult : IExpressionResult
    {
        private string operationType;
        private ExpressionType type;
        private string value;
        private IEnumerable<IExpressionResult> componentResults;

        public string OperationType => operationType;
        public ExpressionType Type => type;
        public virtual string Value => value;
        public IEnumerable<IExpressionResult> ComponentResults => componentResults;

        public ExpressionResult(string operationType, ExpressionType type, string value, IEnumerable<IExpressionResult> componentResults = null)
        {
            this.operationType = operationType;
            this.type = type;
            this.value = value;
            this.componentResults = componentResults ?? new List<IExpressionResult>();
        }

        public override string ToString()
        {
            return value;
        }

        public virtual string ToResultDetailString(int depth = 0)
        {
            var spacing = GetSpacingString(depth);

            StringBuilder detailString = GetBasicValueStringBuilder(spacing);

            AddComponentResultDetailStrings(detailString, depth + 1);

            return detailString.ToString();
        }

        protected StringBuilder GetBasicValueStringBuilder(string spacing)
        {
            var detailString = new StringBuilder();
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
            var spacing = "";
            for (var i = 0; i < depth; i++)
            {
                spacing += "  ";
            }
            spacing += "-";
            return spacing;
        }

        public IExpressionResult As(ExpressionType destType)
        {
            if(Type == destType)
            {
                return this;
            }

            ExpressionDefinition def = ExpressionDefinitions.GetDefinition(Type);

            if (!def.CastTo.ContainsKey(destType))
            {
                throw new InvalidCastException($"Cannot cast {Type.ToString()} to {destType.ToString()}");
            }

            return def.CastTo[destType](this);
        }
    }
}
