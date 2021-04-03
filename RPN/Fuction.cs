using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPN {
    public abstract class Function {
        public int ArgumentCount { get; init; }
        public string[] Symbols { get; init; }
        public abstract bool Execute(Stack<string> stack);

        public static List<Function> GetAvailableFunctions() {
            List<Function> functions = new();

            Type functionType = typeof(Function);
            Assembly asm = Assembly.GetExecutingAssembly();
            Type fa = typeof(RPNFunctionAttr);

            foreach(Type t in asm.GetTypes())
                if(t.GetCustomAttribute(fa) != null)
                    functions.Add((Function)(t.Assembly.CreateInstance(t.FullName)));

            return functions;
        }
    }
}