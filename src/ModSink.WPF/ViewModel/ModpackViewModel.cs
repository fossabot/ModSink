﻿using System.Linq;
using Humanizer;
using Humanizer.Bytes;
using ModSink.Core;
using ModSink.Core.Models.Repo;
using ReactiveUI;
using Splat;

namespace ModSink.WPF.ViewModel
{
    public class ModpackViewModel : ReactiveObject
    {
        public ModpackViewModel(Modpack modpack)
        {
            Modpack = modpack;
            Size = ByteSize.FromBytes(
                Modpack.Mods
                    .SelectMany(m => m.Mod.Files)
                    .Select(f => f.Value.Length)
                    .Aggregate((sum, a) => sum + a)).Humanize("G03");
            Download = ReactiveCommand.CreateFromTask(async () =>
            {
                var cs = Locator.Current.GetService<IModSink>().Client;
                await cs.ScheduleMissingFilesDownload(Modpack);
            }, null, RxApp.TaskpoolScheduler);
        }

        public Modpack Modpack { get; }

        public ReactiveCommand Download { get; }

        public string Size { get; }
    }
}