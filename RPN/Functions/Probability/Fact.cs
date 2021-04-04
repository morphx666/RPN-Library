using RPN.Functions.Special;
using System;
using System.Collections.Generic;

namespace RPN.Functions.Probability {
    [RPNFunctionAttr(nameof(Fact))]
    public class Fact : Function {
        public Fact() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { "!", nameof(Fact).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            stack.Push(SpecialFunctions.Fact(v1).ToString());
        }
    }
}