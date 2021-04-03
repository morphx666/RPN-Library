using System;
using System.Collections.Generic;

namespace RPN.Functions.Arithmetic {
    [RPNFunctionAttr(nameof(Sqrt))]
    public class Sqrt : Function {
        public Sqrt() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Sqrt).ToUpper(), "√" };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            stack.Push(Math.Sqrt(v1).ToString());
        }
    }
}