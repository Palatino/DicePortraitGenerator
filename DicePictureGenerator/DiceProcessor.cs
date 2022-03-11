using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePictureGenerator
{
    public static class DiceProcessor
    {

        public static Dice[,] ProcessImage(DiceProcessorConfig config)
        {

            int minValue = 1;
            int maxValue;

            if (config.DiceTypes == DiceTypes.BlackOnly || config.DiceTypes == DiceTypes.WhiteOnly)
            {
                maxValue = 6;
            }
            else
            {
                maxValue = 12;
            }
            var resizedImage = ImageUtils.ResizeImage(config.Bitmap, config.OutputWidth, config.OutputHeight);
            var bwImage = ImageUtils.BlackAndWhiteImage(resizedImage);

            Tuple<int, int> minMaxGray = GetGrayExtremeValues(bwImage);
            int[,] result = MapToGreyScale(minValue, maxValue, bwImage, minMaxGray);

            Dice[,] diceArray = MapToDicieArray(result, config.DiceTypes);
            return diceArray;
        }

        private static Dice[,] MapToDicieArray(int[,] result, DiceTypes diceType)
        {
            int length = result.GetLength(0);
            int height = result.GetLength(1);

            var diceArray = new Dice[length, height];
            for (int i = 0; i < height; i++)
            {
                for (int u = 0; u < length; u++)
                {
                    int value = result[u, i];
                    if (diceType == DiceTypes.BlackOnly)
                    {
                        diceArray[u, i] = new Dice(value, DiceColor.Black);
                    }
                    else if(diceType == DiceTypes.WhiteOnly)
                    {
                        diceArray[u, i] = new Dice(Math.Abs(value - 7), DiceColor.White);
                    }
                    else
                    {
                        if (value > 6)
                        {
                            diceArray[u, i] = new Dice(Math.Abs(value - 13), DiceColor.White);
                        }
                        else
                        {
                            diceArray[u, i] = new Dice(value, DiceColor.Black);
                        }
                    }

                }
            }

            return diceArray;
        }

        private static int[,] MapToGreyScale(int minValue, int maxValue, Bitmap bwImage, Tuple<int, int> minMaxGray)
        {
            var result = new int[bwImage.Width, bwImage.Height];
            for (int i = 0; i < bwImage.Height; i++)
            {
                for (int u = 0; u < bwImage.Width; u++)
                {
                    var pixelColor = bwImage.GetPixel(u, i);
                    result[u, i] = RemapValue(pixelColor.R, minMaxGray.Item1, minMaxGray.Item2, minValue, maxValue);
                }
            }

            return result;
        }

        private static Tuple<int,int> GetGrayExtremeValues(Bitmap map)
        {

            int max = int.MinValue;
            int min = int.MaxValue;

            for (int y = 0; y < map.Height; y++)
                for (int x = 0; x < map.Width; x++)
                {
                    Color color = map.GetPixel(x, y);
                    if(max < color.R)
                    {
                        max = color.R;
                    }
                    if(color.R < min)
                    {
                        min= color.R;
                    }
                }
            return new Tuple<int, int>(min,max);
        }
        private static int RemapValue(int value, int previousMin, int previousMax, int outputMin, int outputMax)
        {
            if(value < previousMin || value > previousMax)
            {
                throw new ArgumentOutOfRangeException($"Value provided must be within the minimum and maximum values");
            }

            double newValue =  outputMin + ((((double)value - previousMin) / (previousMax - previousMin)) * (outputMax - outputMin));
            return Convert.ToInt32(newValue);
        }
    }


}
