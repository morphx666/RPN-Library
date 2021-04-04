using System;
using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(Roll))]
    public class Roll : Function {
        public Roll() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Roll).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            int v1 = int.Parse(stack.Pop());
            if(v1 > stack.Count) throw new Exception("Too few arguments");
            string[] stk = new string[stack.Count];
            stack.CopyTo(stk, 0);
            stack.Clear();
            for(int i = stk.Length - 1; i >= 0; i--) {
                if((v1 - 1) != i) stack.Push(stk[i]);
            }
            stack.Push(stk[v1 - 1]);
        }
    }
}