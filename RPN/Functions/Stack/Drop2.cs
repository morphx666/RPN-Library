using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(Drop2))]
    public class Drop2 : Function {
        public Drop2() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Drop2).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            stack.Push("2 DROPN");
        }
    }
}