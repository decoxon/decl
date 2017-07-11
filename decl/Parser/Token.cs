using System;
using System.Collections.Generic;
using System.Text;

namespace decl.Parser
{
    internal class Token
    {
        public ExpressionType Type { get; }
        public string Value { get; }
        public int Precedence { get; }

        public Token(ExpressionType type, string value, int precedence)
        {
            Type = type;
            Value = value;
            Precedence = precedence;
        }
    }
}
