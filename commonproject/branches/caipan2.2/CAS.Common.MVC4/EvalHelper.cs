using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace CAS.Common.MVC4
{
    public class EvalHelper
    {
        public static object Eval(string expression)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            //这里为生成的动态代码
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("      public class DynamicCodeEval ");
            sb.Append(Environment.NewLine);
            sb.Append("      {");
            sb.Append(Environment.NewLine);
            sb.Append("          public object Eval()");
            sb.Append(Environment.NewLine);
            sb.Append("          {");
            sb.Append(Environment.NewLine);
            sb.Append("               return " + expression + ";"); //其实就是一个简单的表达式，如果要复杂的大家可以根据自己的情况改动
            sb.Append(Environment.NewLine);
            sb.Append("          }");
            sb.Append(Environment.NewLine);
            sb.Append("      }");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            string code = sb.ToString();

            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, code);
            //这里是反射了
            Assembly objAssembly = cr.CompiledAssembly;
            object objDynamicCodeEval = objAssembly.CreateInstance("DynamicCodeGenerate.DynamicCodeEval");
            MethodInfo objMI = objDynamicCodeEval.GetType().GetMethod("Eval");
            var result = Convert.ToDouble(objMI.Invoke(objDynamicCodeEval, null));
            return result;
        }
    }
}
