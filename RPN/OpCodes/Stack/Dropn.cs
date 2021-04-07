using System;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(DropN))]
    public class DropN : OpCode {
        public DropN() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(DropN).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int v1 = int.Parse(rpn.Pop());
            if(rpn.Count < v1) {
                rpn.Push(v1.ToString());
                throw new ArgumentException("Too few arguments");
            }
            while(--v1 >= 0) rpn.Pop();
        }
    }
}