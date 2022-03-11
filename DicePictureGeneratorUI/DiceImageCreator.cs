using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePictureGeneratorUI
{
    internal static class DiceImageCreator
    {
        internal static Bitmap MergeImagesHorizontally(List<Bitmap> bitmaps)
        {

            Bitmap resultBitmap = new Bitmap(bitmaps.Select(i => i.Width).Sum(), bitmaps.Select(i => i.Height).Max());
            //resultBitmap.SetResolution(72, 72);

            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                for (int i = 0; i < bitmaps.Count; i++)
                {
                    g.DrawImage(bitmaps[i], bitmaps.Take(i).Select(i => i.Width).Sum(), 0, bitmaps[i].Width, bitmaps[i].Height);
                }

            }

            return resultBitmap;
        }
        internal static Bitmap MergeImagesVertically(List<Bitmap> bitmaps)
        {

            Bitmap resultBitmap = new Bitmap(bitmaps.Select(i => i.Width).Max(), bitmaps.Select(i => i.Height).Sum());

            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                for (int i = 0; i < bitmaps.Count; i++)
                {
                    g.DrawImage(bitmaps[i],0, bitmaps.Take(i).Select(i => i.Height).Sum(),bitmaps[i].Width,bitmaps[i].Height);
                }
            }

            return resultBitmap;
        }
        internal static Bitmap CreateCollage(List<List<Bitmap>> bitmaps)
        {
            List<Bitmap> rows = new List<Bitmap>();
            foreach(var row in bitmaps)
            {
                rows.Add(MergeImagesHorizontally(row));
            }

            return MergeImagesVertically(rows);
        }
    }
}
