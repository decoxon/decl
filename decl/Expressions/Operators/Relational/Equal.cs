﻿using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Equal : BinaryOperator
    {
        public Equal(IExpression leftOperand, IExpression rightOperand, string format = "{1}{0}{2}")
            : base(" == ", leftOperand, rightOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            ExpressionResult left = LeftOperand.Evaluate(context);
            ExpressionResult right = RightOperand.Evaluate(context);

            if (left.Type == right.Type)
            {
                string result = left.Value.Equals(right.Value) ? "true" : "false";

                return new ExpressionResult(ExpressionType.Truth, result);
            }

            throw new Exception(String.Format("Cannot compare expressions of type {0} and {1}", left.Type.ToString(), right.Type.ToString()));
        }
    }
}
