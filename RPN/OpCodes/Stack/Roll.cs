using System;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Roll))]
    public class Roll : OpCode {
        public Roll() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Roll).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int v1 = int.Parse(rpn.Pop());
            if(v1 > rpn.Count) throw new Exception("Too Few Arguments");
            string[] stk = new string[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Clear();
            for(int i = stk.Length - 1; i >= 0; i--) {
                if((v1 - 1) != i) rpn.Push(stk[i]);
            }
            rpn.Push(stk[v1 - 1]);
        }
    }
}