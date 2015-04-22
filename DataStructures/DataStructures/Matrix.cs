using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DataStructures
{
    public class Matrix
    {
        public int[][] Values { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Max4SequenceProduct()
        {
            int? maxProduct = null;
            var m = 4;

            //horizontals
            for (var i = 0; i < Height - 1; i++)
            {
                var j = 0;
                var product = 1;
                while (j + m - 1 < Width)
                {
                    for (var k = 0; k < m - 1; k++)
                    {
                        product = product*Values[i][j + k];
                    }
                    j++;
                }
                maxProduct = !maxProduct.HasValue ? product : Math.Max(product, maxProduct.Value);
            }

            //verticals
            for (var i = 0; i < Width - 1; i++)
            {
                var j = 0;
                var product = 1;
                while (j + m - 1 < Height)
                {
                    for (var k = 0; k < m - 1; k++)
                    {
                        product = product * Values[j + k][i];
                    }
                    j++;
                }
                maxProduct = !maxProduct.HasValue ? product : Math.Max(product, maxProduct.Value);
            }

            //diagonals from bottom row

            for (var i = 0; i < Width - 1; i++)
            {
                var i2 = i;
                var j = 0;
                var product = 1;
                while (j + m - 1 < Width && i + m - 1 < Height)
                {
                    for (var k = 0; k < m - 1; k++)
                    {
                        product = product * Values[i2 + k][j + k];
                    }
                    j++;
                    i2++;
                }
                maxProduct = !maxProduct.HasValue ? product : Math.Max(product, maxProduct.Value);
            }


            //diagonals from left row
            for (var i = 0; i < Height - 1; i++)
            {
                var i2 = i;
                var j = 0;
                var product = 1;
                while (j + m - 1 < Width && i + m - 1 < Height)
                {
                    for (var k = 0; k < m - 1; k++)
                    {
                        product = product * Values[j + k][i2 + k];
                    }
                    j++;
                    i2++;
                }
                maxProduct = !maxProduct.HasValue ? product : Math.Max(product, maxProduct.Value);
            }


            return maxProduct.Value;
        }
    }

   

    [TestFixture]
    public class MatrixTests
    {
        
    }
}
