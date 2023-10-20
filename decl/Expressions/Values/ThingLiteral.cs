using declang.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class ThingLiteral : ValueExpression
    {
        private IDictionary<string, IExpression> contents;

        public ThingLiteral(string value)
        {
            contents = parseLiteral(value);
        }

        public override IExpressionResult Evaluate(Thing context)
        {
            var result = new Dictionary<string, IExpressionResult>();

            foreach(var kvp in contents)
            {
                result[kvp.Key] = kvp.Value.Evaluate(context);
            }

            return new Thing("ThingLiteral", ExpressionType.Thing, "", null, result);
        }

        /// <summary>
        /// Build a dictionary based on passed in string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static IDictionary<string, IExpression> parseLiteral(string value)
        {
            var result = new Dictionary<string, IExpression>();
            value = value.Trim();

            if (value.Length > 0)
            {
                var propertyLiterals = new List<string>(value.Split(new char[1] { ',' }));

                foreach (var propertyLiteral in propertyLiterals)
                {
                    parsePropertyString(propertyLiteral, out var property, out IExpression propertyValue);
                    result.Add(property, propertyValue);
                }
            }

            return result;
        }

        private static void parsePropertyString(string literal, out string property, out IExpression propertyValue)
        {
            var firstColon = literal.IndexOf(':');

            if (firstColon == -1)
            {
                throw new Exception(String.Format("Missing colon in property literal {0}", literal));
            }

            property = literal.Substring(0, firstColon).Trim();
            propertyValue = Parser.ParseSingleStatement(literal.Substring(firstColon + 1));
        }
    }
}
