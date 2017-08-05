using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using declang;
using declang.Expressions;

namespace declPrompt
{
    class InteractiveSession
    {
        private IDictionary<string, ExpressionResult> context;

        public InteractiveSession(IDictionary<string, ExpressionResult> context = null)
        {
            this.context = context ?? new Dictionary<string, ExpressionResult>();
        }

        public void Start()
        {
            Console.WriteLine("decl Interactive Mode");
            Console.WriteLine("=====================");

            string statement = null;
            statement = getStatement();
            while (statement != "exit")
            {
                executeStatement(statement);
                statement = getStatement();
            }
        }

        private void executeStatement(string statement)
        {
            switch (statement)
            {
                case "reset":
                    context = new Dictionary<string, ExpressionResult>();
                    break;
                case "current context":
                    outputContext();
                    break;
                default:
                    outputResult(decl.Evaluate(statement, context));
                    break;
            }
        }

        private void outputResult(ExpressionResult result)
        {
            Console.Write("<<<");
            Console.WriteLine(result);
        }

        private void outputContext()
        {
            Console.WriteLine("<<< Current context:");
            foreach(KeyValuePair<string, ExpressionResult> kvp in context)
            {
                Console.WriteLine(String.Format("<<< {0}: {1} ({2})", kvp.Key, kvp.Value.Value, kvp.Value.Type.ToString()));
            }
        }

        private string getStatement()
        {
            Console.Write(">>>");
            return Console.ReadLine();
        }
    }
}
