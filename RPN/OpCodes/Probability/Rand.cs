using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Rand))]
    public class Rand : OpCode {
        private static Random rnd;

        public Rand() {
            base.ArgumentCount = 0;
            base.Symbols = new string[] { nameof(Rand).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };

            rnd = new Random();
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            double v1 = rnd.NextDouble();
            stack.Push(v1.ToString());
        }
    }
}