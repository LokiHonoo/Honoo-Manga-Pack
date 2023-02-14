using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using System.Collections.Generic;
using System.IO;

namespace Honoo.MangaPack
{
    public class ImageRenderListener : IEventListener
    {
        private readonly string _dir;
        private readonly IList<EntrySettings> _entries = new List<EntrySettings>();
        private readonly bool _useMemory;
        private int _error = 0;
        private int _index = 0;

        public ImageRenderListener(bool useMemory, string dir)
        {
            _useMemory = useMemory;
            _dir = dir;
        }

        public int Error => _error;

        internal IList<EntrySettings> Entries => _entries;

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
                        if (_useMemory)
                        {
                            string safe = _index.ToString() + "." + imageObject.IdentifyImageFileExtension();
                            MemoryStream temp = new(imageObject.GetImageBytes());
                            temp.Seek(0, SeekOrigin.Begin);
                            _entries.Add(new EntrySettings(safe, null, temp));
                        }
                        else
                        {
                            string safe = _index.ToString() + "." + imageObject.IdentifyImageFileExtension();
                            string file = Path.Combine(_dir, safe);
                            File.WriteAllBytes(file, imageObject.GetImageBytes());
                            _entries.Add(new EntrySettings(safe, file, null));
                        }
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