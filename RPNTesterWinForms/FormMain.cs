using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RPNTesterWinForms.HPFont;

namespace RPNTesterWinForms {

    // https://www.drehersoft.com/mapping-hp48-text-to-unicode/
    public partial class FormMain : Form {
        HPFont f = new();

        public FormMain() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw, true);

            // Testing HP bitmap fonts
            this.Paint += (object s, PaintEventArgs e) => {
                Graphics g = e.Graphics;

                g.Clear(Color.White);

                int xo = 0;
                int yo = 0;
                int ps = 3;

                for(int i = 0; i < 256; i++) {
                    BmpChar bc = f.GetChar(FontSizes.Small, i);

                    g.FillRectangle(Brushes.LightGray, xo, yo, bc.Width * ps, bc.Height * ps);

                    for(int x = 0; x < bc.Width; x++) {
                        for(int y = 0; y < bc.Height; y++) {
                            g.FillRectangle(bc.Bits[x, y] == 0 ? Brushes.Transparent : Brushes.Black,
                                        xo + x * ps,
                                        yo + y * ps,
                                        ps, ps);
                        }
                    }

                    int ncw = (bc.Width + 3) * ps;
                    int nxo = xo + ncw;
                    if(nxo + ncw >= this.DisplayRectangle.Width) {
                        xo = 0;
                        yo += (bc.Height + 3) * ps;
                    } else {
                        xo = nxo;
                    }
                }
            };
        }
    }
}
