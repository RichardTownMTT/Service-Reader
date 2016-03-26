using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Service_Reader
{
    public class ImageCache
    {
        public Image storedImage { get; set; }
        public string imageId { get; set; }

        internal static void addImage(string image1Url, ImageCache[] cachedSheetImages)
        {
            
        }
    }
}
