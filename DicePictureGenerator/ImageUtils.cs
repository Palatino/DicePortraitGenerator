using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DicePictureGenerator
{
    public static class ImageUtils
    {
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap BlackAndWhiteImage(Bitmap image)
        {
            Bitmap grayScale = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < grayScale.Height; y++)
                for (int x = 0; x < grayScale.Width; x++)
                {
                    Color c = image.GetPixel(x, y);

                    int gs = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);

                    grayScale.SetPixel(x, y, Color.FromArgb(gs, gs, gs));
                }
            return grayScale;
        }

        public static double GetAspectRatio(Bitmap image)
        {
            return (double)image.Width / image.Height;
        }
    }
}