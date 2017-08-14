﻿using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal class Subtraction : BinaryOperator
    {
        public Subtraction(IExpression leftOperand, IExpression rightOperand)
            : base(" - ", leftOperand, rightOperand) {  }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return subtract(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
        }

        private ExpressionResult subtract(ExpressionResult leftOperand, ExpressionResult rightOperand)
        {
            if ((leftOperand.Type == ExpressionType.Number && rightOperand.Type == ExpressionType.Number))
            {
                if (Decimal.TryParse(leftOperand.Value, out decimal leftDecimal) && Decimal.TryParse(rightOperand.Value, out decimal rightDecimal))
                {
                    return new ExpressionResult(ExpressionType.Number, (leftDecimal - rightDecimal).ToString());
                }
            }

            throw new Exception(String.Format("Cannot subtract expressions of type {0} and {1}", leftOperand.Type, rightOperand.Type));
        }
    }
}