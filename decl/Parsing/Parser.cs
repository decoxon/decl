using declang.Expressions;
using System;
using System.Collections.Generic;

namespace declang.Parsing
{
    internal static class Parser
    {
        /// <summary>
        /// Create an expression tree, ready to be evaluated, for the given expression.
        /// </summary>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>The completed expression tree.</returns>
        public static Script Parse(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return new Script(createExpressionTrees(Tokeniser.Tokenise(expression)));
        }

        private static List<IExpression> createExpressionTrees(List<List<Token>> statements)
        {
            if (statements.Count <= 0)
            {
                return new List<IExpression>() { new Word("") };
            }

            List<IExpression> expressions = new List<IExpression>();

            foreach (List<Token> statement in statements)
            {
                expressions.Add(createExpressionTree(statement));
            }

            return expressions;
        }

        /// <summary>
        /// Recursively creates an expression tree for the provided list of tokens.
        /// </summary>
        /// <param name="tokens">The list of Token objects representing the expression created by <see cref="tokeniseExpression(string)"/></param>
        /// <exception cref="Exception">Thrown if a Token has an <see cref="ExpressionType"/> that is not recognised.</exception>
        /// <returns>The completed expression tree.</returns>
        private static IExpression createExpressionTree(List<Token> tokens)
        {
            if (tokens.Count <= 0)
            {
                return new Word("");
            }

            // Find the right-most, lowest precedence token
            int selectedToken = tokens.Count - 1;

            for (int currentTokenIndex = tokens.Count - 1; currentTokenIndex >= 0; currentTokenIndex--)
            {
                if (tokens[currentTokenIndex].Precedence < tokens[selectedToken].Precedence)
                {
                    selectedToken = currentTokenIndex;
                }
            }

            switch (tokens[selectedToken].Type)
            {
                case ExpressionType.Number:
                    return new Number(tokens[selectedToken].Value);
                case ExpressionType.Variable:
                    return new Variable(tokens[selectedToken].Value);
                case ExpressionType.Truth:
                    return new Truth(tokens[selectedToken].Value);
                case ExpressionType.Word:
                    return new Word(tokens[selectedToken].Value);
                case ExpressionType.Parens:
                    List<List<Token>> innerExpression = Tokeniser.Tokenise(tokens[selectedToken].Value);

                    if (innerExpression.Count <= 0)
                    {
                        throw new Exception(String.Format("Empty Parens expression"));
                    }

                    if (innerExpression.Count > 1)
                    {
                        throw new Exception(String.Format("Cannot have multiple statements in parentheses: {0}", tokens[selectedToken].Value));
                    }

                    return new Parens(createExpressionTree(innerExpression[0]));
                case ExpressionType.Addition:
                    return new Addition(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Subtraction:
                    return new Subtraction(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Multiplication:
                    return new Multiplication(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Division:
                    return new Division(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Modulo:
                    return new Modulo(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.DiceRoll:
                    return new DiceRoll(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.LessThan:
                    return new LessThan(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.GreaterThan:
                    return new GreaterThan(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Equal:
                    return new Equal(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.NotEqual:
                    return new NotEqual(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.And:
                    return new And(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Or:
                    return new Or(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.GreaterThanOrEqual:
                    return new Or(
                        new GreaterThan(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))),
                        new Equal(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))));
                case ExpressionType.LessThanOrEqual:
                    return new Or(
                        new LessThan(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))),
                        new Equal(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))));
                case ExpressionType.Negation:
                    return new Negation(createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Assignment:
                    IExpression leftOperand = createExpressionTree(tokens.GetRange(0, selectedToken));

                    if ((leftOperand as Variable) == null)
                    {
                        throw new Exception(String.Format("Invalid left operand for assignment: {0}", leftOperand.ToString()));
                    }

                    return new Assignment(
                        leftOperand as Variable,
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.TestCase:
                    IExpression firstOperand = createExpressionTree(tokens.GetRange(0, selectedToken));

                    int testCaseCheckLocation = selectedToken + 1;

                    while (testCaseCheckLocation < tokens.Count && tokens[testCaseCheckLocation].Type != ExpressionType.TestCaseCheck)
                    {
                        testCaseCheckLocation++;
                    }

                    if (testCaseCheckLocation - selectedToken < 2 || tokens[testCaseCheckLocation].Type != ExpressionType.TestCaseCheck)
                    {
                        throw new Exception("No check or case found.");
                    }

                    IExpression secondOperand = createExpressionTree(tokens.GetRange(selectedToken + 1, testCaseCheckLocation - selectedToken - 1));
                    IExpression thirdOperand = new TestCaseCheck(tokens[testCaseCheckLocation].Value);

                    return new TestCase(firstOperand, secondOperand, thirdOperand);
                default:
                    throw new Exception(String.Format("Unrecognised expression type '{0}'.", tokens[selectedToken].Type.ToString()));
            }
        }


    }
}
