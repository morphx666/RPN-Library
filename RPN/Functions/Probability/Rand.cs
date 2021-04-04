using System;
using System.Collections.Generic;

namespace RPN.Functions.Probability {
    [RPNFunctionAttr(nameof(Rand))]
    public class Rand : Function {
        private static Random rnd;

        public Rand() {
            base.ArgumentCount = 0;
            base.Symbols = new string[] { nameof(Rand).ToUpper() };
            rnd = new Random();
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = rnd.NextDouble();
            stack.Push(v1.ToString());
        }
    }
}