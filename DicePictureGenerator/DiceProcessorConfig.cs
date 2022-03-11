using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePictureGenerator
{
    public class DiceProcessorConfig
    {
        public DiceTypes DiceTypes { get; set; }
        public Bitmap Bitmap { get; set; }
        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; }

    }
}
