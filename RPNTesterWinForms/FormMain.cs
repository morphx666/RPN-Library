using RPN;
using RPN.OpCodes;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static RPN.OpCodes.OpCode;
using static RPN.RPNStack;
using static RPNTesterWinForms.HPFont;

namespace RPNTesterWinForms {

    public partial class FormMain : Form {
        private readonly RPNStack rpn = new();
        private readonly HPFont hpf = new();
        private readonly Color hpGreen = Color.FromArgb(155, 177, 164);
        private int PixelSize = 2;
        private int w;
        private int h;

        public FormMain() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw, true);

            SetUISize();
        }

        private void FormMain_Load(object sender, EventArgs e) {
            // Testing HP bitmap fonts
            this.Paint += (object s, PaintEventArgs e) => {
                Graphics g = e.Graphics;

                g.Clear(hpGreen);

                //PrintAllChars(g, hpf);

                PrintStack(g);
                PrintVariables(g);
            };
        }

        private void PrintVariables(Graphics g) {
            string n;
            int l;
            int rw = 5 * hpf.FontWidth(FontSizes.Small) * PixelSize + PixelSize;
            int rh = hpf.FontHeight(FontSizes.Small) * PixelSize;
            int sw = (w - rw * 6) / 5;

            for(int i = 0; i < 6; i++) {
                if(i < rpn.Variables.Count) {
                    n = rpn.Variables[i].Name.Length <= 5 ? rpn.Variables[i].Name : rpn.Variables[i].Name[..5];
                } else {
                    n = "     ";
                }
                l = (5 - n.Length) / 2;
                n = i == 2 ? "MMMMM" : "ABCDE";

                int tw;
                while(true) {
                    tw = hpf.FontSmallWidth(n) * PixelSize;
                    if(tw > rw) {
                        n = n[0..^1];
                    } else {
                        break;
                    }
                }

                g.FillRectangle(Brushes.Black,
                       i * (rw + sw), rh * 8,
                       rw, rh
                );

                g.Clip = new Region(new Rectangle(i * (rw + sw), rh * 8, rw, rh));
                PrintText(g, $"{"".PadLeft(l)}{n}{"".PadRight(5 - l - n.Length)}", hpf, FontSizes.Small, hpGreen, Color.Black,
                        0, 8,
                        i * (rw + sw) + ((rw + 2 * PixelSize) - tw) / 2, PixelSize);
                g.ResetClip();
            }
        }

        private void PrintStack(Graphics g) {
            int max = 4;
            StackItem[] stk = new StackItem[max];
            Array.Copy(rpn.Stack.ToArray(), 0, stk, 0, Math.Min(max, rpn.Stack.Count));

            string tmp;
            string idx;
            for(int i = max - 1; i >= 0; i--) {
                idx = (i + 1).ToString();
                tmp = stk[i] == null ? "" : tmp = stk[i].AsString();
                tmp = tmp.Length <= rpn.ColumnWidth - (idx.Length + 1) ?
                        tmp :
                        "…" + tmp[(tmp.Length - rpn.ColumnWidth + idx.Length + 2)..];

                if(tmp != "" && stk[i].Type == Types.Infix) {
                    foreach(OpCode oc in rpn.OpCodes.Where(o => o.SpaceArguments)) {
                        string s = oc.Symbols[0];
                        if(tmp.Contains(s)) {
                            int p = tmp.IndexOf(s);
                            tmp = $"{tmp[0..p]} {s} {tmp[(p + s.Length)..]}";
                            break;
                        }
                    }
                }

                PrintText(g, $"Row 1", hpf, FontSizes.Medium, Color.Black, hpGreen, 0, 0);
                PrintText(g, $"Row 2", hpf, FontSizes.Medium, Color.Black, hpGreen, 0, 1);

                PrintText(g, $"{idx}: {tmp.PadLeft(rpn.ColumnWidth)}", hpf, FontSizes.Medium, Color.Black, hpGreen, 0, (max - i) + 1);
            }
        }

        private void PrintText(Graphics g, string text, HPFont f, FontSizes fs, Color fg, Color bg, int c, int r, int xOff = 0, int yOff = 0) {
            BmpChar[] bcs = f.GetString(fs, text);

            c *= c * f.FontWidth(fs) * PixelSize;
            r *= f.FontHeight(fs) * PixelSize;

            c += xOff;
            r += yOff;

            int oc = c;
            int or = r;

            for(int i = 0; i < bcs.Length; i++) {
                PrintChar(g, bcs[i], fg, bg, oc, or);
                oc += bcs[i].Width * PixelSize;

                //if(w != -1 && oc >= w) {
                //    oc = c;
                //    or += PixelSize * f.FontHeight(fs);
                //}

                //if(h != -1 && or >= h) {
                //    oc = c;
                //    or = r;
                //}
            }
        }

        private void PrintAllChars(Graphics g, HPFont f) {
            g.Clear(Color.White);

            int xo = 0;
            int yo = 0;
            int ps = 3;

            for(int i = 0; i < 256; i++) {
                BmpChar bc = f.GetChar(FontSizes.Small, i);

                PrintChar(g, bc, Color.Black, Color.LightGray, xo, yo);

                int ncw = (bc.Width + 3) * ps;
                int nxo = xo + ncw;
                if(nxo + ncw >= this.DisplayRectangle.Width) {
                    xo = 0;
                    yo += (bc.Height + 3) * ps;
                } else {
                    xo = nxo;
                }
            }
        }

        private void PrintChar(Graphics g, BmpChar bc, Color fg, Color bg, int x, int y) {
            using(SolidBrush bbg = new(bg)) {
                g.FillRectangle(bbg, x, y, bc.Width * PixelSize, bc.Height * PixelSize);

                using(SolidBrush bfg = new(fg)) {
                    for(int xo = 0; xo < bc.Width; xo++) {
                        for(int yo = 0; yo < bc.Height; yo++) {
                            g.FillRectangle(bc.Bits[xo, yo] == 0 ? bbg : bfg,
                                        x + xo * PixelSize,
                                        y + yo * PixelSize,
                                        PixelSize, PixelSize);
                        }
                    }
                }
            }
        }

        private void SetUISize() {
            // Max width for Medium: 22 chars
            // Max width for Small: 33 chars
            w = 22 * hpf.FontWidth(FontSizes.Medium) * PixelSize;

            // Max height for Medium: 6 chars + 1 row of Small
            h = (6 * hpf.FontHeight(FontSizes.Medium) + hpf.FontHeight(FontSizes.Small)) * PixelSize;

            this.ClientSize = new Size(w, h);
        }
    }
}