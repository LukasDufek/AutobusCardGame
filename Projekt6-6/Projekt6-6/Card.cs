using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt6_6
{
    public class Card
    {
        string popis;
        string symbol;
        int hodnota;
        char znak;

        public Card(string _popis, string _symbol, int _hodnota, char _znak)
        {
            this.popis = _popis;
            this.symbol = _symbol;
            this.hodnota = _hodnota;
            this.znak = _znak;
        }

        public string getPopis()
        {
            return popis;
        }
        public string getSymbol()
        {
            return symbol;
        }
        public int getHodnota()
        {
            return hodnota;
        }

        public char getZnak()
        {
            return znak;
        }

        public string getNazevKarty()
        {
            return ""+popis + znak;
        }
    }
}
