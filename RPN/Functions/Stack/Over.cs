using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(Over))]
    public class Over : Function {
        public Over() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Over).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            string v1 = stack.Pop();
            string v2 = stack.Pop();
            stack.Push(v2);
            stack.Push(v1);
            stack.Push(v2);
        }
    }
}