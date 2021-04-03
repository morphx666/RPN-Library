using System;
using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Sqrt))]
    public class Sqrt : Function {
        public Sqrt() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Sqrt).ToUpper(), "√" };
        }

        public override bool Execute(Stack<string> stack) {
            try {
                double v1 = double.Parse(stack.Pop());
                v1 = Math.Sqrt(v1);
                stack.Push(v1.ToString());
                return true;
            } catch {
                return false;
            }
        }
    }
}