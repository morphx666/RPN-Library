using System;
using System.Collections.Generic;

namespace RPN.Functions.Trigonometry {
    [RPNFunctionAttr(nameof(Tan))]
    public class Tan : Function {
        public Tan() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Tan).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            stack.Push(Math.Tan(v1).ToString());
        }
    }
}