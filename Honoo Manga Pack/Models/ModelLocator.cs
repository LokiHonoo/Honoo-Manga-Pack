namespace Honoo.MangaPack.Models

{
    internal static class ModelLocator
    {
        internal static ADSettings ADSettings { get; } = new ADSettings();
        internal static MainSettings MainSettings { get; } = new MainSettings();
        internal static PasswordSettings PasswordSettings { get; } = new PasswordSettings();
        internal static TagSettings TagSettings { get; } = new TagSettings();
    }
}