﻿using RPN.OpCodes.Logic;
using System.Collections.Generic;

namespace RPN.OpCodes.Memory {
    [OpCodeAttr(nameof(Rcl))]
    internal class Rcl : OpCode {
        public Rcl() {
            ArgumentCount = 1;
            Symbols = new string[] { "RCL" };
            DataTypes = new Types[] { Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 5;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            Types type = OpCode.InferType(v1);

            foreach(var v in rpn.Vars) {
                if(v.Name == v1) {
                    rpn.Push(v.Value, v.Type);
                    return true;
                }
            }

            ErrorFunction = "RCL";
            ErrorMessage = "Undefined Name";
            return false;
        }
    }
}