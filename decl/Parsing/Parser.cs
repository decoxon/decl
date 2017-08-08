using declang.Expressions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace declang.Parsing
{
    internal static class Parser
    {
        /// <summary>
        /// Defines the valid characters for expressions
        /// </summary>
        public static Dictionary<ExpressionType, char[]> ExpressionCharacters = new Dictionary<ExpressionType, char[]>
        {
            {ExpressionType.Number, new char[11] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'} },
            {ExpressionType.Variable, new char[53]
                {
                    'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                    '_'
                }
            },
            {ExpressionType.Addition,       new char[1]  {'+'} },
            {ExpressionType.Subtraction,    new char[1]  {'-'} },
            {ExpressionType.Multiplication, new char[1]  {'*'} },
            {ExpressionType.Division,       new char[1]  {'/'} },
            {ExpressionType.Assignment,     new char[1]  {'='} },
            {ExpressionType.Parens,         new char[2]  {'(', ')'} }
        };

        private static char[] validIdentifierCharacters = new char[63]
        {
             '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
             'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
             'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
             '_'
        };

        /// <summary>
        /// Defines the precedence of each type of expression.
        /// </summary>
        public static Dictionary<ExpressionType, int> ExpressionPrecedence = new Dictionary<ExpressionType, int>
        {
            {ExpressionType.Variable,       4 },
            {ExpressionType.Number,         4 },
            {ExpressionType.Parens,         4 },
            {ExpressionType.Multiplication, 3 },
            {ExpressionType.Division,       3 },
            {ExpressionType.DiceRoll,       3 },
            {ExpressionType.Addition,       2 },
            {ExpressionType.Subtraction,    2 },
            {ExpressionType.Assignment,     1 },
        };

        /// <summary>
        /// Create an expression tree, ready to be evaluated, for the given expression.
        /// </summary>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>The completed expression tree.</returns>
        public static IExpression GetExpressionTree(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return createExpressionTree(tokeniseExpression(expression));
        }

        /// <summary>
        /// Recursively creates an expression tree for the provided list of tokens.
        /// </summary>
        /// <param name="tokens">The list of Token objects representing the expression created by <see cref="tokeniseExpression(string)"/></param>
        /// <exception cref="Exception">Thrown if a Token has an <see cref="ExpressionType"/> that is not recognised.</exception>
        /// <returns>The completed expression tree.</returns>
        private static IExpression createExpressionTree(List<Token> tokens)
        {
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
                case ExpressionType.Parens:
                    return new Parens(createExpressionTree(tokeniseExpression(tokens[selectedToken].Value)));
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
                case ExpressionType.DiceRoll:
                    return new DiceRoll(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Assignment:
                    IExpression leftOperand = createExpressionTree(tokens.GetRange(0, selectedToken));

                    if ((leftOperand as Variable) == null)
                    {
                        throw new Exception(String.Format("Invalid left operand for assignment: {0}", leftOperand.ToString()));
                    }

                    return new Assignment(
                        leftOperand as Variable,
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                default:
                    throw new Exception(String.Format("Unrecognised expression type '{0}'.", tokens[selectedToken].Type.ToString()));
            }
        }

        /// <summary>
        /// Converts a string expression into a list of Tokens to be used by <see cref="createExpressionTree(List{Token})"/> 
        /// by looking at each character of the string in turn.
        /// </summary>
        /// <param name="expression">The expression to be tokenised.</param>
        /// <returns>The list of Tokens for the given expression.</returns>
        private static List<Token> tokeniseExpression(string expression)
        {
            List<Token> tokens = new List<Token>();
            string tokenValue;
            int endOfToken;
            int numDecimalPoints = 0;

            for (int currentCharacter = 0; currentCharacter < expression.Length; currentCharacter++)
            {
                if(expression[currentCharacter] == ' ')
                {
                    continue;
                }

                ExpressionType type = getCharacterType(expression[currentCharacter]);

                switch (type)
                {
                    case ExpressionType.Number:
                        // Numbers can be multiple characters so we need to find the end of the number.
                        endOfToken = currentCharacter;
                        while (endOfToken < expression.Length && getCharacterType(expression[endOfToken]) == ExpressionType.Number)
                        {
                            // Count decimal points and throw exception if there are more than one.
                            if(expression[endOfToken] == '.')
                            {
                                numDecimalPoints++;
                            }

                            if(numDecimalPoints > 1)
                            {
                                throw new Exception(String.Format("Too many decimal points in expression {0}.", expression));
                            }

                            endOfToken++;
                        }
                        endOfToken--;

                        tokenValue = expression.Substring(currentCharacter, endOfToken - currentCharacter + 1);
                        tokens.Add(new Token(type, tokenValue, ExpressionPrecedence[type]));
                        currentCharacter = endOfToken;
                        break;
                    case ExpressionType.Variable:
                        ExpressionType tokenType = type;
                        endOfToken = currentCharacter;

                        // Check for DiceRoll operator
                        if ((expression[currentCharacter] == 'd' || expression[currentCharacter] == 'D') 
                            && getCharacterType(expression[currentCharacter + 1]) == ExpressionType.Number)
                        {
                            tokenType = ExpressionType.DiceRoll;

                            // To allow users to say "d6" instead of having to type out "1d6", we'll add a 1 when
                            // there isn't a number present before the D operator because it is a binary operator and
                            // therefore requires two operands.
                            if(tokens.Count == 0 || tokens[tokens.Count - 1].Type != ExpressionType.Number)
                            {
                                tokens.Add(new Token(ExpressionType.Number, "1", ExpressionPrecedence[ExpressionType.Number]));
                            }
                        }
                        else
                        {
                            // Identifiers can be multiple characters so we need to find the end of the identifier.
                            while (endOfToken < expression.Length && isValidNonInitialIdentifierCharacter(expression[endOfToken]))
                            {
                                endOfToken++;
                            }
                            endOfToken--;
                        }

                        tokenValue = expression.Substring(currentCharacter, endOfToken - currentCharacter + 1);
                        tokens.Add(new Token(tokenType, tokenValue, ExpressionPrecedence[tokenType]));
                        currentCharacter = endOfToken;
                        break;
                    case ExpressionType.Parens:
                        // Look for matching closing paren
                        int endOfParen = currentCharacter + 1;
                        int nestingLevel = 0;

                        for (; endOfParen < expression.Length; endOfParen++)
                        {
                            if (expression[endOfParen] == ')')
                            {
                                if (nestingLevel == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    nestingLevel--;
                                }
                            }

                            if (expression[endOfParen] == '(')
                            {
                                nestingLevel++;
                            }
                        }

                        tokenValue = expression.Substring(currentCharacter + 1, endOfParen - currentCharacter - 1);
                        tokens.Add(new Token(type, tokenValue, ExpressionPrecedence[type]));
                        currentCharacter = endOfParen;
                        break;
                    case ExpressionType.Addition:
                    case ExpressionType.Subtraction:
                    case ExpressionType.Multiplication:
                    case ExpressionType.Division:
                    case ExpressionType.DiceRoll:
                    case ExpressionType.Assignment:
                        tokens.Add(new Token(type, expression.Substring(currentCharacter, 1), ExpressionPrecedence[type]));
                        break;
                }
            }

            return tokens;
        }

        private static Dictionary<ExpressionType, string> identifierPatterns = new Dictionary<ExpressionType, string>
        {
            {ExpressionType.Variable, @"^[a-zA-Z][a-zA-Z0-9]*$" },
            {ExpressionType.DiceRoll, @"^\d*[dD]{1}\d+$" },
            {ExpressionType.Number,   @"^\d+\.\d+$" },
        };

        private static ExpressionType getCharacterType(char c)
        {
            foreach (KeyValuePair<ExpressionType, char[]> type in ExpressionCharacters)
            {
                for (int i = 0; i < type.Value.Length; i++)
                {
                    if (c == type.Value[i])
                    {
                        return type.Key;
                    }
                }
            }

            throw new Exception(String.Format("Invalid character '{0}' in expression.", c));
        }

        private static bool isValidNonInitialIdentifierCharacter(char character)
        {
            for (var i = 0; i < validIdentifierCharacters.Length; i++)
            {
                if (character == validIdentifierCharacters[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
