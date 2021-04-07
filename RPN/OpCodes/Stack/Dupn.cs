using System;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(DupN))]
    public class DupN : OpCode {
        public DupN() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(DupN).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int v1 = int.Parse(rpn.Pop());
            if(rpn.Count < v1) {
                rpn.Push(v1.ToString());
                throw new ArgumentException("Too few arguments");
            }
            string[] stk = new string[v1];
            Array.Copy(rpn.ToArray(), rpn.Count - v1, stk, 0, v1);
            while(--v1 >= 0) rpn.Push(stk[v1]);
        }
    }
}