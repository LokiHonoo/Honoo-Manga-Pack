﻿using Honoo.MangaPack.Models;
using System.IO;

namespace Honoo.MangaPack.Classes
{
    internal sealed class RuntimePackSettings
    {
        internal RuntimePackSettings()
        {
            this.WorkDirectly = Path.Combine(ModelLocator.MainSettings.WorkDirectly, "Packs");
            this.ResetName = ModelLocator.MainSettings.ResetName;
            this.MoveToRecycleBin = ModelLocator.MainSettings.MoveToRecycleBin;
            this.DeltetAD = ModelLocator.MainSettings.DeleteAD;
            this.ADs = [];
            foreach (var ad in ModelLocator.ADSettings.ADs)
            {
                this.ADs.Add(ad[0]);
            }
            this.AddTopTitle = ModelLocator.MainSettings.AddTopTitle;
            this.AddTag = ModelLocator.MainSettings.AddTag;
            this.SelectedTag = ModelLocator.MainSettings.SelectedTag;
        }

        internal bool AddTag { get; }
        internal bool AddTopTitle { get; }
        internal HashSet<string> ADs { get; }
        internal bool DeltetAD { get; }
        internal bool ExecuteAtDrop { get; }
        internal bool MoveToRecycleBin { get; }
        internal bool ResetName { get; }
        internal string SelectedTag { get; }

        internal string WorkDirectly { get; }
    }
}