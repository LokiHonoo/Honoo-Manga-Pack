using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using System.IO;

namespace Honoo.MangaPack.Models
{
    public class ImageRenderListener : IEventListener
    {
        private readonly string _dir;
        private readonly int _pad;
        private int _error = 0;
        private int _index = 0;

        public ImageRenderListener(int pages, string dir)
        {
            _pad = pages.ToString().Length + 1;
            _dir = dir;
        }

        public int Error => _error;

        public void EventOccurred(IEventData data, EventType type)
        {
            if (data is ImageRenderInfo imageData)
            {
                try
                {
                    PdfImageXObject imageObject = imageData.GetImage();
                    if (imageObject != null)
                    {
                        _index++;
                        string safe = _index.ToString().PadLeft(_pad, '0') + "." + imageObject.IdentifyImageFileExtension();
                        string file = Path.Combine(_dir, safe);
                        File.WriteAllBytes(file, imageObject.GetImageBytes());
                    }
                }
                catch
                {
                    _error++;
                }
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return null!;
        }
    }
}