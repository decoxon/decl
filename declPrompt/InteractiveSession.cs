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
        private bool outputDetail = false;

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
                try
                {
                    executeStatement(statement);
                }
                catch(Exception e)
                {
                    outputException(e);
                }
                statement = getStatement();
            }
        }

        private void outputException(Exception e)
        {
            Console.WriteLine("Exception Occurred");
            Console.WriteLine("=====================");
            Console.WriteLine(e.ToString());
            Console.WriteLine("=====================");
        }

        private void executeStatement(string statement)
        {
            switch (statement)
            {
                case "reset":
                    context = new Dictionary<string, ExpressionResult>();
                    break;
                case "context":
                    outputContext();
                    break;
                case "toggle detail":
                    outputDetail = !outputDetail;
                    Console.WriteLine("<<< Detail output " + (outputDetail ? "on" : "off"));
                    break;
                default:
                    outputResult(DECL.Evaluate(statement, context));
                    break;
            }
        }

        private void outputResult(ExpressionResult result)
        {
            Console.Write("<<<");
            Console.WriteLine(result);
            if (outputDetail)
            {
                outputResultTree(result);
            }
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

        private void outputResultTree(ExpressionResult result)
        {
            Console.WriteLine("<<< Result Tree:");
            Console.WriteLine(result.ToResultDetailString());
        }
    }
}
