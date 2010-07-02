using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;



namespace Font
{
    public partial class UIForm : Form
    {
        public UIForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.Text = "字体\n";
            richTextBox1.Font = fontDialog1.Font;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.AppendText(openFileDialog1.FileName);
                richTextBox1.AppendText("\n");
            }
            else
                openFileDialog1.FileName = "";
        }

        public static void Main()
        {
            Application.Run(new UIForm());
        }

        public static Bitmap to8bppgrey(Bitmap srcbmp)
        {
            Bitmap bmp = new Bitmap(srcbmp.Width, srcbmp.Height, PixelFormat.Format8bppIndexed);

            //设定实例BitmapData相关信息   
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            BitmapData data = srcbmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //锁定bmp到系统内存中   
            BitmapData data2 = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            //获取位图中第一个像素数据的地址   
            IntPtr ptr = data.Scan0;
            IntPtr ptr2 = data2.Scan0;

            int numBytes = data.Stride * data.Height;
            int numBytes2 = data2.Stride * data2.Height;

            int n2 = data2.Stride - bmp.Width; //// 显示宽度与扫描线宽度的间隙   

            byte[] rgbValues = new byte[numBytes];
            byte[] rgbValues2 = new byte[numBytes2];
            //将bmp数据Copy到申明的数组中   
            Marshal.Copy(ptr, rgbValues, 0, numBytes);
            Marshal.Copy(ptr2, rgbValues2, 0, numBytes2);

            int n = 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width * 3; x += 3)
                {
                    int i = data.Stride * y + x;

                    double value = rgbValues[i + 2] * 0.299 + rgbValues[i + 1] * 0.587 + rgbValues[i] * 0.114; //计算灰度   

                    rgbValues2[n] = (byte)value;

                    n++;
                }
                n += n2; //跳过差值   
            }

            //将数据Copy到内存指针   
            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            Marshal.Copy(rgbValues2, 0, ptr2, numBytes2);

            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度   
            ColorPalette tempPalette;
            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                tempPalette = tempBmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                tempPalette.Entries[i] = Color.FromArgb(i, i, i);
            }

            bmp.Palette = tempPalette;


            //从系统内存解锁bmp   
            srcbmp.UnlockBits(data);
            bmp.UnlockBits(data2);

            return bmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "") return;

            int COL_COUNT = int.Parse(textBox2.Text);
            int ROW_COUNT = int.Parse(textBox1.Text);
            int CHAR_ROW_PIXEL = int.Parse(textBox3.Text);
            int CHAR_COL_PIXEL = int.Parse(textBox4.Text);

            int img_min_width = int.Parse(textBox8.Text);
            int img_min_height = int.Parse(textBox9.Text);

            img_min_width = CHAR_ROW_PIXEL * ROW_COUNT > img_min_width ? CHAR_ROW_PIXEL * ROW_COUNT : img_min_width;
            img_min_height = CHAR_COL_PIXEL * COL_COUNT > img_min_height ? CHAR_COL_PIXEL * COL_COUNT : img_min_height;

            Bitmap img = new Bitmap(img_min_width ,img_min_height);
            Bitmap slice = new Bitmap(32, 32);
            Graphics g_slice = Graphics.FromImage(slice);
            Graphics g = Graphics.FromImage(img);

            g_slice.Clear(Color.White);
            g_slice.InterpolationMode = InterpolationMode.NearestNeighbor;
            g_slice.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g_slice.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g_slice.ScaleTransform(1.0f, float.Parse(textBox5.Text));
            g.Clear(Color.White);

            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Near;

            StreamReader reader = null;
            reader = new StreamReader(openFileDialog1.FileName,Encoding.Default);
            int filecount=1,count = 0;
            string line = reader.ReadLine();
            while( line != null)
            {
                string[] words = line.Split('=');

                if (words[1] != "")
                {
                    int offset_x = (count % ROW_COUNT) * CHAR_ROW_PIXEL;
                    int offset_y = (count / ROW_COUNT) * CHAR_COL_PIXEL;
                    int drawstr_offset_x = int.Parse(textBox6.Text);
                    int drawstr_offset_y = int.Parse(textBox7.Text);
                    g_slice.Clear(Color.White);
                    g_slice.DrawString(words[1], fontDialog1.Font, Brushes.Black,drawstr_offset_x,drawstr_offset_y);
                    g.DrawImage(slice, offset_x, offset_y);
                    count++;
                }

                if (count == ROW_COUNT * COL_COUNT)
                {
                    Bitmap dstimg;
                    if (checkBox1.Checked)
                        dstimg = to8bppgrey(img);
                    else
                        dstimg = img;
                    dstimg.Save(filecount.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);

                    if (checkBox1.Checked & checkBox2.Checked)
                    {
                        BitmapData raw = null;
                        byte[] rawImage = null;
                        Rectangle rect = new Rectangle(0, 0, dstimg.Width, dstimg.Height);
                        raw = dstimg.LockBits(rect,
                            System.Drawing.Imaging.ImageLockMode.ReadWrite,
                          dstimg.PixelFormat);
                        int size = raw.Height * raw.Stride;
                        rawImage = new byte[size];
                        Marshal.Copy(raw.Scan0, rawImage, 0, size);
                        dstimg.UnlockBits(raw);
                        System.IO.FileStream Fs = new System.IO.FileStream(filecount.ToString() + ".dat", System.IO.FileMode.Create);
                        Fs.Write(rawImage, 0, size);
                        Fs.Close();
                    }

                    g.Clear(Color.White);
                    count = 0;
                    filecount++;
                }
                line = reader.ReadLine();
            }

            if (count != 0)
            {
                Bitmap dstimg;
                if (checkBox1.Checked)
                    dstimg = to8bppgrey(img);
                else
                    dstimg = img;
                dstimg.Save(filecount.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);

                if (checkBox1.Checked & checkBox2.Checked)
                {
                    BitmapData raw = null;
                    byte[] rawImage = null;
                    Rectangle rect = new Rectangle(0, 0, dstimg.Width, dstimg.Height);
                    raw = dstimg.LockBits(rect,
                        System.Drawing.Imaging.ImageLockMode.ReadWrite,
                      dstimg.PixelFormat);
                    int size = raw.Height * raw.Stride;
                    rawImage = new byte[size];
                    Marshal.Copy(raw.Scan0, rawImage, 0, size);
                    dstimg.UnlockBits(raw);
                    System.IO.FileStream Fs = new System.IO.FileStream(filecount.ToString() + ".dat", System.IO.FileMode.Create);
                    Fs.Write(rawImage, 0, size);
                    Fs.Close();
                }

                g.Clear(Color.White);
                count = 0;
                filecount++;
            }

            g.Dispose();
            img.Dispose();
            g_slice.Dispose();
            slice.Dispose();
            richTextBox1.AppendText("OK!\n");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) checkBox1.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) checkBox1.Checked = true;
        }
    }
}