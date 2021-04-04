using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(Rot))]
    public class Rot : Function {
        public Rot() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Rot).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            string[] stk = new string[stack.Count];
            stack.CopyTo(stk, 0);
            stack.Clear();
            for(int i = stk.Length - 2; i >= 0; i--) stack.Push(stk[i]);
            stack.Push(stk[^1]);
        }
    }
}