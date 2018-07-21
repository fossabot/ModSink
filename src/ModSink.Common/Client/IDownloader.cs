﻿using System;
using System.IO;
using System.Reactive.Subjects;

namespace ModSink.Common.Client
{
    public interface IDownloader
    {
        IConnectableObservable<DownloadProgress> Download(Uri source, Lazy<Stream> destination, ulong expectedLength = 0);
    }
}