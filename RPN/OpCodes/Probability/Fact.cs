using RPN.OpCodes.Special;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Fact))]
    public class Fact : OpCode {
        public Fact() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Fact).ToUpper(), "!" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"{Symbols[0]}({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                rpn.Push(SpecialFunctions.Fact(d1).ToString(), dataType);
            }

            return true;
        }
    }
}