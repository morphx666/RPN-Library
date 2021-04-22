using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;
using static RPNTesterWinForms.HPFont;

namespace RPNTesterWinForms {

    public partial class FormMain : Form {
        public int PixelSize { get; set; } = 2;
        public FontSizes FontSize { get; set; } = FontSizes.Small;

        private readonly HPFont hpf = new();

        public FormMain() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw, true);

            // Testing HP bitmap fonts
            this.Paint += (object s, PaintEventArgs e) => {
                Graphics g = e.Graphics;

                PrintAllChars(g);
                PrintText(g, "Hello World!", Color.Black, Color.White, 10, 10);
            };
        }

        private void PrintText(Graphics g, string text, Color fg, Color bg, int x, int y) {
            g.Clear(Color.White);

            BmpChar[] bcs = hpf.GetString(FontSize, text);

            for(int i = 0; i < bcs.Length; i++) {
                PrintChar(g, bcs[i], fg, bg, x, y);
                x += bcs[i].Width * PixelSize - PixelSize;
            }

        }

        private void PrintAllChars(Graphics g) {
            g.Clear(Color.White);

            int xo = 0;
            int yo = 0;
            int ps = 3;

            for(int i = 0; i < 256; i++) {
                BmpChar bc = hpf.GetChar(FontSize, i);

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