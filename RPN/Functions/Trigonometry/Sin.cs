using System;
using System.Collections.Generic;

namespace RPN.Functions.Trigonometry {
    [RPNFunctionAttr(nameof(Sin))]
    public class Sin : Function {
        public Sin() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Sin).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            stack.Push(Math.Sin(v1).ToString());
        }
    }
}