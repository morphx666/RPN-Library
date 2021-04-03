using System;
using System.Collections.Generic;

namespace RPN.Functions.Trigonometry {
    [RPNFunctionAttr(nameof(Cos))]
    public class Cos : Function {
        public Cos() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Cos).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            stack.Push(Math.Cos(v1).ToString());
        }
    }
}