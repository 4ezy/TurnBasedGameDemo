using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TurnBasedGameDemo
{
    public class GameField : Canvas
    {
        public int HorzCellsCount { get; set; }
        public int VertCellsCount { get; set; }
        private readonly int _rectSize = 50;

        public GameField() { }

        public GameField(int horzCellsCount, int vertCellsCount)
        {
            HorzCellsCount = horzCellsCount;
            VertCellsCount = vertCellsCount;
            Width = _rectSize * horzCellsCount;
            Height = _rectSize * vertCellsCount;
            GenerateField();
        }

        public void GenerateField()
        {
            if (HorzCellsCount > 0 && VertCellsCount > 0)
            {
                int vertOffset, horzOffset = 0;

                for (int i = 0; i < HorzCellsCount; i++)
                {
                    vertOffset = 0;

                    for (int j = 0; j < VertCellsCount; j++)
                    {
                        Rectangle rectangle = new Rectangle()
                        {
                            Width = _rectSize,
                            Height = _rectSize
                        };

                        Children.Add(rectangle);
                        SetTop(rectangle, vertOffset);
                        SetLeft(rectangle, horzOffset);
                        vertOffset += _rectSize;
                    }

                    horzOffset += _rectSize;
                }
            }
        }
    }
}
