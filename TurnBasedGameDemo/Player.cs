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

        public Uri SwordsmanImageUri { get; }
        public Uri ArcherImageUri { get; }
        public Uri PeasantImageUri { get; }

        public Player() { }

        public Player(
            string swordsmanImageUri,
            string archerImageUri,
            string peasantImageUri)
        {
            UnitStacks = new List<UnitStack>();
            SwordsmanImageUri = new Uri(swordsmanImageUri);
            SwordsmanImage = new BitmapImage(SwordsmanImageUri);
            ArcherImageUri = new Uri(archerImageUri);
            ArcherImage = new BitmapImage(ArcherImageUri);
            PeasantImageUri = new Uri(peasantImageUri);
            PeasantImage = new BitmapImage(PeasantImageUri);
        }

        public Player(
            Uri swordsmanImageUri,
            Uri archerImageUri,
            Uri peasantImageUri)
        {
            UnitStacks = new List<UnitStack>();
            SwordsmanImageUri = swordsmanImageUri;
            SwordsmanImage = new BitmapImage(SwordsmanImageUri);
            ArcherImageUri = archerImageUri;
            ArcherImage = new BitmapImage(ArcherImageUri);
            PeasantImageUri = peasantImageUri;
            PeasantImage = new BitmapImage(PeasantImageUri);
        }
    }
}
