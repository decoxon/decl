using System;
using System.Collections.Generic;
using System.Text;

namespace declang
{
    /// <summary>
    /// The types of expressions recognised by the <see cref="Parsing"/>.
    /// </summary>
    public enum ExpressionType
    {
        Variable,
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Parens,
        DiceRoll,
        Number,
        Truth,
        Assignment,
        Word,
        LessThan,
        GreaterThan,
        Ignore,
        Negation,
        NotEqual,
        Equal,
        TestCase,
        TestCaseCheck,
        Or,
        And,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Modulo,
        Thing,
        Accessor
    }
}
