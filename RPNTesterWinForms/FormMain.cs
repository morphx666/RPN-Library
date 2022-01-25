using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;
using static RPNTesterWinForms.HPFont;

namespace RPNTesterWinForms {

    public partial class FormMain : Form {
        public int PixelSize { get; set; } = 2;

        public FormMain() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw, true);

            HPFont f = new();

            // Testing HP bitmap fonts
            this.Paint += (object s, PaintEventArgs e) => {
                Graphics g = e.Graphics;

                g.Clear(Color.White);

                //PrintAllChars(g, f);
                PrintText(g, "Hello World!", f, FontSizes.Small, Color.Black, Color.White, 10, 10);
                PrintText(g, "Hello World!", f, FontSizes.Medium, Color.Black, Color.White, 10, PixelSize * f.FontHeight(FontSizes.Small) + 10);
                PrintText(g, "Hello World!", f, FontSizes.Large, Color.Black, Color.White, 10, PixelSize * (f.FontHeight(FontSizes.Small) + f.FontHeight(FontSizes.Medium)) + 10 * 2, 100);
            };
        }

        private void PrintText(Graphics g, string text, HPFont f, FontSizes fs, Color fg, Color bg, int x, int y, int w = -1, int h = -1) {
            BmpChar[] bcs = f.GetString(fs, text);

            int ox = x;
            int oy = y;

            for(int i = 0; i < bcs.Length; i++) {
                PrintChar(g, bcs[i], fg, bg, ox, oy);
                ox += bcs[i].Width * PixelSize - PixelSize;

                if(w != -1 && ox >= w) {
                    ox = x;
                    oy += PixelSize * f.FontHeight(fs);
                }

                if(h != -1 && oy >= h) {
                    oy = y;
                    ox = x;
                }
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
    }
}