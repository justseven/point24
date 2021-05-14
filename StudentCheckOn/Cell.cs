using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCheckOn
{
    public class Cell
    {
        public enum Type
        {
            Number,
            Signal
        }
        public int Number;
        public char Signal;
        public Type Typ;
        public Cell Right;
        public Cell Left;



        /// <summary>
        /// 符号优先级
        /// </summary>
        public int Priority
        {
            get
            {
                if (Typ == Type.Signal)
                {
                    switch (Signal)
                    {
                        case '+': return 0;
                        case '-': return 0;
                        case '*': return 1;
                        case '/': return 1;
                        default: return -1;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// 基本单元构造函数
        /// </summary>
        /// <param name="t">单元类型，数值或符号</param>
        /// <param name="num">数值</param>
        /// <param name="sig">符号</param>
        public Cell(Type t, int num, char sig)
        {
            Right = null;
            Left = null;
            Typ = t;
            Number = num;
            Signal = sig;
        }
        public Cell()
        {
            Right = null;
            Left = null;
            Number = 0;
            Typ = Type.Number;
        }
        public Cell(Cell c)
        {
            Right = null;
            Left = null;
            Number = c.Number;
            Signal = c.Signal;
            Typ = c.Typ;
        }
    }
}
