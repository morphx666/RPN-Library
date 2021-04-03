using System;
using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Sqrt))]
    public class Sqrt : Function {
        public Sqrt() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Sqrt).ToUpper(), "√" };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            v1 = Math.Sqrt(v1);
            stack.Push(v1.ToString());
        }
    }
}