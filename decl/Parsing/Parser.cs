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

        public static IExpression ParseSingleStatement(string statement, bool throwOnMultiline = true)
        {
            if (String.IsNullOrEmpty(statement))
            {
                throw new ArgumentNullException("expression");
            }

            List<List<Token>> statements = Tokeniser.Tokenise(statement);

            if(statements.Count < 1)
            {
                throw new Exception(String.Format("No statements provided: {0}", statement));
            }

            if(statements.Count > 1 && throwOnMultiline)
            {
                throw new Exception(String.Format("Multiple statements provided to ParseSingleStatement: {0}", statement));
            }

            return createExpressionTree(statements[0]);
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
                case ExpressionType.Thing:
                    return new ThingLiteral(parseThingString(tokens[selectedToken].Value));
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
                case ExpressionType.Accessor:
                    return new Accessor(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
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
                    return new Assignment(
                        leftOperand.ToVariable(),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.TestCase:
                    IExpression firstOperand = createExpressionTree(tokens.GetRange(0, selectedToken));

                    int testCaseCheckLocation = selectedToken + 1;

                    while (testCaseCheckLocation < tokens.Count - 1 && tokens[testCaseCheckLocation].Type != ExpressionType.TestCaseCheck)
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


        /// <summary>
        /// Build a dictionary based on passed in string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static IDictionary<string, IExpression> parseThingString(string value)
        {
            Dictionary<string, IExpression> result = new Dictionary<string, IExpression>();
            value = value.Trim();

            if (value.Length > 0)
            {
                List<string> propertyLiterals = new List<string>(value.Split(new char[1] { ',' }));

                foreach (string propertyLiteral in propertyLiterals)
                {
                    parsePropertyString(propertyLiteral, out string property, out IExpression propertyValue);
                    result.Add(property, propertyValue);
                }
            }

            return result;
        }

        private static void parsePropertyString(string literal, out string property, out IExpression propertyValue)
        {
            int firstColon = literal.IndexOf(':');

            if(firstColon == -1)
            {
                throw new Exception(String.Format("Missing colon in property literal {0}", literal));
            }

            property = literal.Substring(0, firstColon).Trim();
            propertyValue = ParseSingleStatement(literal.Substring(firstColon + 1));
        }
    }
}
