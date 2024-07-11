using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honoo.MangaPack.Classes
{
    public class ADSearchedEntry(string path, bool isZipped, string crc)
    {
        private readonly string _crc = crc;
        private readonly bool _isZipped = isZipped;
        private readonly string _path = path;
        private int _count = 1;

        public int Count => _count;
        public string Crc => _crc;
        public bool IsZipped => _isZipped;
        public string Path => _path;

        public void Increment()
        {
            _count++;
        }
    }
}