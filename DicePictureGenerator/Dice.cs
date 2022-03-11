using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePictureGenerator
{
    public class Dice
    {
        public int Value { get;  }
        public DiceColor Color { get;  }

        public Dice(int value, DiceColor color)
        {
            Value = value;
            Color = color;
        }

        public override string ToString()
        {
            if(Color == DiceColor.White)
            {
                return "w" + Value.ToString();
            }
            if (Color == DiceColor.Black)
            {
                return "b" + Value.ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
