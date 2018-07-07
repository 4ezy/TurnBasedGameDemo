using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TurnBasedGameDemo
{
    public class Player
    {
        public List<UnitStack> UnitStacks { get; set; }
        public BitmapImage SwordsmanImage { get; set; }
        public BitmapImage ArcherImage { get; set; }
        public BitmapImage PeasantImage { get; set; }

        public Player(
            string swordsmanImageUri,
            string archerImageUri,
            string peasantImageUri)
        {
            UnitStacks = new List<UnitStack>();
            Uri uri = new Uri(swordsmanImageUri);
            SwordsmanImage = new BitmapImage(uri);
            uri = new Uri(archerImageUri);
            ArcherImage = new BitmapImage(uri);
            uri = new Uri(peasantImageUri);
            PeasantImage = new BitmapImage(uri);
        }
    }
}
