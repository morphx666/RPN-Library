using System;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Rand))]
    public class Rand : OpCode {
        private static Random rnd;

        public Rand() {
            Symbols = new string[] { nameof(Rand).ToUpper() };

            rnd = new Random();
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            double d1 = rnd.NextDouble();
            rpn.Push(d1.ToString(), Types.Float);
        }
    }
}