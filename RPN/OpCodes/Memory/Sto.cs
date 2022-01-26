using RPN.OpCodes.Logic;
using System.Collections.Generic;

namespace RPN.OpCodes.Memory {
    [OpCodeAttr(nameof(Sto))]
    internal class Sto : OpCode {
        public Sto() {
            ArgumentCount = 2;
            Symbols = new string[] { "STO" };
            DataTypes = new Types[] { Types.Infix, Types.Any };
            Associativity = Associativities.Left;
            Precedence = 5;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;
            Types type = OpCode.InferType(v2);

            rpn.Variables.Add(new RPNStack.Variable(v1, v2, type));

            return true;
        }
    }
}