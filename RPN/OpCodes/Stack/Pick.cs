using System;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Pick))]
    public class Pick : OpCode {
        public Pick() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Pick).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int v1 = int.Parse(rpn.Pop());
            if(v1 > rpn.Count) throw new Exception("Too Few Arguments");
            string[] stk = new string[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Push(stk[v1 - 1]);
        }
    }
}