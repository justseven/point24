using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCheckOn
{
    public class Calc24Points
    {

        string m_exp;
        bool m_stop;
        Cell[] m_cell;
        int[] m_express;
        StringWriter m_string;
        public Calc24Points(int n1, int n2, int n3, int n4)
        {
            m_cell = new Cell[8];
            m_cell[0] = new Cell(Cell.Type.Number, n1, '?');
            m_cell[1] = new Cell(Cell.Type.Number, n2, '?');
            m_cell[2] = new Cell(Cell.Type.Number, n3, '?');
            m_cell[3] = new Cell(Cell.Type.Number, n4, '?');
            m_cell[4] = new Cell(Cell.Type.Signal, 0, '+');
            m_cell[5] = new Cell(Cell.Type.Signal, 0, '-');
            m_cell[6] = new Cell(Cell.Type.Signal, 0, '*');
            m_cell[7] = new Cell(Cell.Type.Signal, 0, '/');
            m_stop = false;
            m_express = new int[7];
            m_string = new StringWriter();
            m_exp = null;
        }

        public override string ToString()
        {
            if (m_exp == null)
            {
                PutCell(0);
                m_exp = m_string.ToString();
            }
            if (m_exp != "") return m_exp;
            return null;
        }

        /// <summary>
        /// 在第n位置放置一个单元
        /// </summary>
        /// <param name="n"></param>
        void PutCell(int n)
        {
            if (n >= 7)
            {
                if (Calculate())
                {
                    m_stop = true;
                    Formate();
                }
                return;
            }
            int end = 8;
            if (n < 2) end = 4;
            for (int i = 0; i < end; ++i)
            {
                m_express[n] = i;
                if (CheckCell(n)) PutCell(n + 1);
                if (m_stop) break;
            }
        }

        /// <summary>
        /// 检查当前放置是否合理
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        bool CheckCell(int n)
        {
            int nums = 0, sigs = 0;
            for (int i = 0; i <= n; ++i)
            {
                if (m_cell[m_express[i]].Typ == Cell.Type.Number) ++nums;
                else ++sigs;
            }
            if (nums - sigs < 1) return false;
            if (m_cell[m_express[n]].Typ == Cell.Type.Number) //数值不能重复，但是符号可以重复
            {
                for (int i = 0; i < n; ++i) if (m_express[i] == m_express[n]) return false;
            }
            if (n == 6)
            {
                if (nums != 4 || sigs != 3) return false;
                if (m_cell[m_express[6]].Typ != Cell.Type.Signal) return false;
                return true;
            }
            return true;
        }

        /// <summary>
        /// 计算表达式是否为24
        /// </summary>
        /// <returns>返回值true为24，否则不为24</returns>
        bool Calculate()
        {
            double[] dblStack = new double[4];
            int indexStack = -1;
            for (int i = 0; i < 7; ++i)
            {
                if (m_cell[m_express[i]].Typ == Cell.Type.Number)
                {
                    ++indexStack;
                    dblStack[indexStack] = m_cell[m_express[i]].Number;
                }
                else
                {
                    switch (m_cell[m_express[i]].Signal)
                    {
                        case '+':
                            dblStack[indexStack - 1] = dblStack[indexStack - 1] + dblStack[indexStack];
                            break;
                        case '-':
                            dblStack[indexStack - 1] = dblStack[indexStack - 1] - +dblStack[indexStack];
                            break;
                        case '*':
                            dblStack[indexStack - 1] = dblStack[indexStack - 1] * dblStack[indexStack];
                            break;
                        case '/':
                            dblStack[indexStack - 1] = dblStack[indexStack - 1] / dblStack[indexStack];
                            break;
                    }
                    --indexStack;
                }
            }
            if (Math.Abs(dblStack[indexStack] - 24) < 0.1) return true;
            return false;
        }

        /// <summary>
        /// 后缀表达式到中缀表达式
        /// </summary>
        void Formate()
        {
            Cell[] c = new Cell[7];
            for (int i = 0; i < 7; ++i) c[i] = new Cell(m_cell[m_express[i]]);
            int[] cStack = new int[4];
            int indexStack = -1;
            for (int i = 0; i < 7; ++i)
            {
                if (c[i].Typ == Cell.Type.Number)
                {
                    ++indexStack;
                    cStack[indexStack] = i;
                }
                else
                {
                    c[i].Right = c[cStack[indexStack]];
                    --indexStack;
                    c[i].Left = c[cStack[indexStack]];
                    cStack[indexStack] = i;
                }
            }
            ToStringFormate(c[cStack[indexStack]]);
        }

        void ToStringFormate(Cell root)
        {
            if (root.Left.Typ == Cell.Type.Number)
            {
                m_string.Write(root.Left.Number);
                m_string.Write(root.Signal);
            }
            else
            {
                if (root.Priority > root.Left.Priority)
                {
                    m_string.Write("(");
                    ToStringFormate(root.Left);
                    m_string.Write(")");
                }
                else ToStringFormate(root.Left);
                m_string.Write(root.Signal);
            }
            if (root.Right.Typ == Cell.Type.Number) m_string.Write(root.Right.Number);
            else
            {
                if (root.Priority >= root.Right.Priority)
                {
                    m_string.Write("(");
                    ToStringFormate(root.Right);
                    m_string.Write(")");
                }
                else ToStringFormate(root.Right);
            }
        }
    }
}
