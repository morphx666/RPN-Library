using System;
using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(DupN))]
    public class DupN : Function {
        public DupN() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(DupN).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            int v1 = int.Parse(stack.Pop());
            if(stack.Count < v1) {
                stack.Push(v1.ToString());
                throw new ArgumentException("Too few arguments");
            }
            string[] stk = new string[v1];
            Array.Copy(stack.ToArray(), stack.Count - v1, stk, 0, v1);
            while(--v1 >= 0) stack.Push(stk[v1]);
        }
    }
}