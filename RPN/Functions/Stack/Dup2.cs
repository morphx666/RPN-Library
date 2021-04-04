using System.Collections.Generic;

namespace RPN.Functions.Stack {
    [RPNFunctionAttr(nameof(Dup2))]
    public class Dup2 : Function {
        public Dup2() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Dup2).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            stack.Push("2 DUPN");
        }
    }
}