using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Depth))]
    public class Depth : OpCode {
        public Depth() {
            Symbols = new string[] { nameof(Depth).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            rpn.Push(rpn.Count.ToString());
        }
    }
}