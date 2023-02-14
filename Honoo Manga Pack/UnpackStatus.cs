using System.IO;

namespace Honoo.MangaPack
{
    internal enum UnpackStatus
    {
        Unhandled,
        Unpacked,
        IsEmpty,
        NotZip,
        NotPdf,
        PasswordInvalid,
        PdfUnsupported
    }
}