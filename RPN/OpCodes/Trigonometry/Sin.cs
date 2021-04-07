﻿using System;

namespace RPN.OpCodes.Trigonometry {
    [OpCodeAttr(nameof(Sin))]
    public class Sin : OpCode {
        public Sin() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Sin).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop().Replace("'", "");
                rpn.Push($"'{Symbols[0]}({v1})'");
            } else {
                double v1 = double.Parse(rpn.Pop());
                rpn.Push(Math.Sin(v1).ToString());
            }
        }
    }
}