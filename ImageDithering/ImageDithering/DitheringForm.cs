using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing;

namespace ImageDithering
{
    public class DitheringForm : Form
    {
        readonly PictureBox original_pb, dithered_pb;
        readonly Button load, save;

        public DitheringForm()
        {
            this.Text = "Floyd–Steinberg Dithering";
            this.BackColor = Color.GhostWhite;
            this.Size = new Size(816, 468);

            // Initialize controls
            original_pb = new PictureBox
            {
                Size = new Size(400, 400),
                Location = new Point(0, 0),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            dithered_pb = new PictureBox
            {
                Size = original_pb.Size,
                Location = new Point(original_pb.Right, original_pb.Top),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            load = new Button
            {
                Location = new Point((original_pb.Width - 100) / 2, original_pb.Bottom),
                Size = new Size(100, 30),
                Text = "Load Image"
            };

            save = new Button
            {
                Location = new Point(load.Left + original_pb.Width, dithered_pb.Bottom),
                Size = new Size(100, 30),
                Text = "Save Image"
            };

            // Events
            load.Click += Load_Image;
            save.Click += Save_Image;

            // Add controls
            Controls.Add(load);
            Controls.Add(save);
            Controls.Add(original_pb);
            Controls.Add(dithered_pb);
        }

        private Bitmap Dithering(Bitmap original)
        {
            int width = original.Width;
            int height = original.Height;
            Bitmap result = new Bitmap(width, height);

            // Initialize DitheringArray with greyscale values for each pixel from original image
            int[,] DitheringArray = GreyScale(original);

            // Floyd–Steinberg dithering algorithm
            int oldpixel, newpixel, quant_error;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    oldpixel = DitheringArray[x, y];
                    newpixel = Quantize(oldpixel);
                    DitheringArray[x, y] = newpixel;
                    result.SetPixel(x, y, Color.FromArgb(newpixel, newpixel, newpixel));

                    quant_error = oldpixel - newpixel;

                    if (x < width - 1)
                        DitheringArray[x + 1, y] += quant_error * 7 / 16;
                    if (x > 0 && y < height - 1)
                        DitheringArray[x - 1, y + 1] += quant_error * 3 / 16;
                    if (y < height - 1)
                        DitheringArray[x, y + 1] += quant_error * 5 / 16;
                    if (x < width - 1 && y < height - 1)
                        DitheringArray[x + 1, y + 1] += quant_error * 1 / 16;
                }

            return result;
        }

        private int Quantize(int value)
        {
            int factor = 1;     // Resultion / number of bounds - 1

            // Quantize the colorvalue to 0 (black) or 255 (white), depending on which bound is closer
            int quantized_value = (int)Math.Round((decimal)factor * value / 255) * (255 / factor);
            return quantized_value;
        }

        private int[,] GreyScale(Bitmap original)
        {
            int width = original.Width;
            int height = original.Height;
            int[,] GreyArray = new int[width, height];

            // Set values for DitheringArray with grey-filtered value for each pixel from original image
            int r, g, b, grey;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Color color = original.GetPixel(x, y);
                    r = color.R;
                    g = color.G;
                    b = color.B;

                    // Grey-value is the average
                    grey = (r + g + b) / 3;
                    GreyArray[x, y] = grey;
                }

            return GreyArray;
        }

        private void Load_Image(object sender, EventArgs ea)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Load image...",
                Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap original = new Bitmap(dialog.FileName);

                original_pb.Image = original;
                original_pb.Refresh();

                dithered_pb.Image = new Bitmap(Dithering(original));
                dithered_pb.Refresh();
            }
            dialog.Dispose();
        }

        private void Save_Image(object sender, EventArgs ea)
        {
            if (dithered_pb.Image == null)
                return;

            SaveFileDialog dialoog = new SaveFileDialog
            {
                Title = "Save image as...",
                Filter = "Image Files | *.png"
            };

            // Dithered image gets saved to a chosen location
            if (dialoog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bm = new Bitmap(dithered_pb.Image);

                bm.Save(dialoog.FileName, ImageFormat.Png);
                bm.Dispose();
            }
            dialoog.Dispose();
        }
    }
}
