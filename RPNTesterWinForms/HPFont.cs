using System;
using System.Drawing;

namespace RPNTesterWinForms {
    public class HPFont {
        public struct BmpChar {
            public int[,] Bits;
            public int Width { get; init; }
            public int Height { get; init; }

            public BmpChar(int[,] bits, int w, int h) {
                this.Bits = bits;
                this.Width = w;
                this.Height = h;
            }
        }

        public enum FontSizes {
            Small = 0,
            Medium = 1,
            Large = 2
        }

        private class BmpFont {
            public BmpChar[] Chars = new BmpChar[256];

            public BmpFont(Bitmap bmp, int x, int y, int w, int h) {
                int ox = x;
                int oy = y;

                if(w != 0) {
                    for(int i = 0; i < 256;) {
                        int[,] bits = new int[w, h];

                        for(int sx = ox; sx < ox + w; sx++)
                            for(int sy = oy; sy < oy + h; sy++)
                                bits[sx - ox, sy - oy] = bmp.GetPixel(sx, sy).R == 0 ? 1 : 0;

                        Chars[i] = new BmpChar(bits, w, h);

                        if(++i % 16 == 0) {
                            ox = x;
                            oy += h;
                        } else {
                            ox += w;
                        }
                    }
                } else { // Font Size 1 (small) is not monospace
                    int i = 0;

                    w = 18;
                    h = 18;
                    int[,] bits = new int[w, h];

                    (int X, int Y, int S, string C)[] merge = { (264, y +  1 * h, 3, "..."),
                                                                (  x, y +  2 * h, 4, " "),
                                                                (102, y +  2 * h, 4, "''"),
                                                                (  x, y + 10 * h, 4, " "),
                                                                (180, y + 10 * h, 2, "''"),
                                                                (168, y + 11 * h, 2, ".:"),
                                                              };

                    while(i < 255) {
                        int emptyRowCount = 0;
                        int cw = 0;
                        for(int sx = ox; sx < ox + w; sx++) {
                            if(ox + cw >= bmp.Width) {
                                ox = x;
                                oy += h;
                                break;
                            }

                            bool emptyRow = true;
                            for(int sy = oy; sy < oy + h; sy++) {
                                int b = bmp.GetPixel(sx, sy).R == 0 ? 1 : 0;
                                bits[sx - ox, sy - oy] = b;
                                if(b != 0) emptyRow = false;
                            }
                            cw++;
                            if(emptyRow) {
                                if(++emptyRowCount == 3) {
                                    bool isEmpty = true;
                                    for(int n = 0; n < w; n++) {
                                        for(int m = 0; m < h; m++) {
                                            if(bits[n, m] != 0) {
                                                isEmpty = false;
                                                break;
                                            }
                                        }
                                        if(!isEmpty) break;
                                    }

                                    bool resetBits = true;
                                    if(i > 0) {
                                        for(int s = 0; s < merge.Length; s++) {
                                            if(sx >= merge[s].X && sx < merge[s].X + Chars[i - 1].Width && oy == merge[s].Y) {
                                                if(--merge[s].S == 0) {
                                                    isEmpty = false;
                                                } else {
                                                    isEmpty = true;
                                                    resetBits = false;
                                                }
                                                emptyRowCount = 0;
                                                break;
                                            }
                                        }
                                    }

                                    if(!isEmpty) Chars[i++] = new BmpChar((int[,])bits.Clone(), cw, h);
                                    if(resetBits) {
                                        if(ox + cw >= bmp.Width) {
                                            ox = x;
                                            oy += h;
                                            break;
                                        } else {
                                            ox += cw;
                                        }

                                        Array.Clear(bits, 0, bits.Length);
                                        cw = 0;
                                        emptyRowCount = 0;
                                    }
                                }
                            } else {
                                emptyRowCount = 0;
                            }
                        }
                    }
                }
                bmp.Dispose();
            }
        }

        private readonly BmpFont[] fonts = new BmpFont[3];

        public HPFont() { // https://www.drehersoft.com/mapping-hp48-text-to-unicode/
            fonts[(int)FontSizes.Small] = new BmpFont((Bitmap)Bitmap.FromFile("Fonts/hp-font1.png"), 84, 71, 0, 0);
            fonts[(int)FontSizes.Medium] = new BmpFont((Bitmap)Bitmap.FromFile("Fonts/hp-font2.png"), 96, 78, 18, 24);
            fonts[(int)FontSizes.Large] = new BmpFont((Bitmap)Bitmap.FromFile("Fonts/hp-font3.png"), 102, 80, 18, 30);
        }

        public BmpChar GetChar(FontSizes fontSize, int charIndex) {
            return fonts[(int)fontSize].Chars[charIndex];
        }

        public BmpChar[] GetString(FontSizes fontSize, string text) {
            BmpChar[] str = new BmpChar[text.Length];

            for(int i = 0; i < text.Length; i++) {
                str[i] = GetChar(fontSize, text[i]);
            }

            return str;
        }
    }
}