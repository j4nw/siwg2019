using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace GraphToImage
{
    public class Position
    {
        public double posX;
        public double posY;
        public Position() { }
        public Position(double x, double y)
        {
            posX = x;
            posY = y;
        }
    }
    public class PositionProvider
    {
        int cnt = 1;        // ilość  położenie
        int r = 200;        // promień okręgu
        double alfa;        // kąt rozstawnienia
        int quantity = 0;   // ilość punktów
        // Inicjalizacja, jeśli za promień podamy liczbę mniejszą od 0 będzie brana pod uwagę wartość domyślna
        public PositionProvider(int quantity, int radius)
        {
            this.quantity = quantity;
            if(radius > 0)
                r = radius;
            alfa = ReturnAngle();
        }
        // oblicza kąt z jakim rozstawić punkty na okręgu
        private double ReturnAngle()
        {
            return (360f / quantity * Math.PI /180f);
        }
        //zwraca pozycję X
        private double ReturnX(int r, int alfaCnt)
        {
            double x = 0;
            x = r * Math.Cos(alfa * alfaCnt);
            //Console.WriteLine(alfa * cnt);
            return x;
        }
        // zwraca pozycję Y
        private double ReturnY(int r, int alfaCnt)
        {
            double y = 0;
            y = r * Math.Sin(alfa * alfaCnt);
            return y;
        }

        // zwraca kolejną pozycję dla wierzchołka
        public Position ReturnPosition()
        {
            Position outValue = new Position();
            if (cnt <= quantity)
            {
                outValue.posX = ReturnX(r, cnt);
                outValue.posY = ReturnY(r, cnt);
                cnt++;
                return outValue;
            }
            else
            {
                Console.WriteLine("Ilość dostępnych pozycji została wykorzystana!");
                return null;
            }
        }

        public Position ReturnNodePosition(int nr, int nodeRadius)
        {
            Position outValue = new Position();
            outValue.posX = ReturnX(r - nodeRadius / 2, nr);
            outValue.posY = ReturnY(r - nodeRadius / 2, nr);
            return outValue;
        }
    }
}
