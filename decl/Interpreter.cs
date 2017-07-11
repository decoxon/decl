using System;
using System.Collections.Generic;
using System.Text;
using decl.Parser;

namespace decl
{
    public class Interpreter
    {
        public Interpreter() { }

        public int Evaluate (string expression)
        {
            try
            {
                return Parser.Parser.GetExpressionTree(expression).Evaluate();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
