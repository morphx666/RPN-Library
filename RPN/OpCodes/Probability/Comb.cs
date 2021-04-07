using System;
using RPN.OpCodes.Special;
using static RPN.RPNStack;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Comb))]
    public class Comb : OpCode {
        public Comb() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Comb).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            StackItem v1 = rpn.Pop();
            StackItem v2 = rpn.Pop();

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"{Symbols[0]}({v2.Token},{v1.Token})", dataType);
            } else {
                double d1 = double.Parse(v1.Token);
                double d2 = double.Parse(v2.Token);
                if(d2 < d1) {
                    rpn.Push(v2.Token, v2.Type);
                    rpn.Push(v1.Token, v1.Type);
                    throw new ArgumentException("Bad argument value");
                }
                rpn.Push((SpecialFunctions.Fact(d2) / (SpecialFunctions.Fact(d1) * SpecialFunctions.Fact(d2 - d1))).ToString(), dataType);
            }
        }
    }
}