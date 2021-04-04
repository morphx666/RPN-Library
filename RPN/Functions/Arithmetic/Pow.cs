using System;
using System.Collections.Generic;

namespace RPN.Functions.Arithmetic {
    [RPNFunctionAttr(nameof(Pow))]
    public class Pow : Function {
        public Pow() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { "^" };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            double v2 = double.Parse(stack.Pop());
            stack.Push(Math.Pow(v2, v1).ToString());
        }
    }
}