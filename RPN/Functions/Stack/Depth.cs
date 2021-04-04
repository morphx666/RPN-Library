using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(Depth))]
    public class Depth : Function {
        public Depth() {
            base.ArgumentCount = 0;
            base.Symbols = new string[] { nameof(Depth).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            stack.Push(stack.Count.ToString());
        }
    }
}