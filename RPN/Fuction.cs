﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPN {
    public abstract class Function {
        public string ErrorFunction { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public int ArgumentCount { get; init; }
        public string[] Symbols { get; init; }
        public  bool Execute(Stack<string> stack) {
            try {
                ExecuteInternal(stack);
                return true;
            } catch(Exception ex) {  // FIXME: This is kind of pointless.
                                     // We should be able to handle errors such as:
                                     // - To few arguments
                                     // - Invalid argument types
                                     // - Overflow / Division by Zero / etc...

                ErrorFunction = Symbols[0];
                ErrorMessage = ex.Message;
                return false;
            }
        }
        public abstract void ExecuteInternal(Stack<string> stack);

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