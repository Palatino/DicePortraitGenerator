using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp
{
    public static class ExtensionMethods
    {
        public static string ToCsv(this int[,] array )
        {
            StringBuilder sb = new StringBuilder();

            int width = array.GetLength( 0 );
            int height = array.GetLength( 1 );

            for (int i = 0; i < height; i++)
            {
                for(int u=0; u<width; u++)
                {
                    sb.Append( array[u,i]+"," );
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }


    }
}
