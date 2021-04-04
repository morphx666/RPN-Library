using System;
using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(DropN))]
    public class DropN : Function {
        public DropN() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(DropN).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            int v1 = int.Parse(stack.Pop());
            if(stack.Count < v1) {
                stack.Push(v1.ToString());
                throw new ArgumentException("Too few arguments");
            }
            while(--v1 >= 0) stack.Pop();
        }
    }
}