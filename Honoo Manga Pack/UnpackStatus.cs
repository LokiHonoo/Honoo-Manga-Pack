using System.IO;

namespace Honoo.MangaPack
{
    internal enum UnpackStatus
    {
        Unhandled,
        Unpacked,
        IsEmpty,
        PasswordInvalid,
        Failed
    }
}