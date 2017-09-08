using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class ExpressionDefinition
    {
        public ExpressionType Type { get; set; }
        public int Precedence { get; set; }
        public char[] TriggerCharacters { get; set; }

        public bool IsTriggerCharacter(char c)
        {
            for(var i = 0; i < TriggerCharacters.Length; i++)
            {
                if (c == TriggerCharacters[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
