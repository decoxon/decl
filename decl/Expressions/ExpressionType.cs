﻿using System;
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
    }
}