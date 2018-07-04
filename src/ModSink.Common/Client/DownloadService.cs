﻿using System;
using System.IO;
using System.Net;
using DynamicData;
using ReactiveUI;

namespace ModSink.Common.Client
{
    public class DownloadService : ReactiveObject
    {
        private readonly IDownloader downloader;
        private readonly IObservable<IChangeSet<QueuedDownload>> downloadQueue;
        private int simultaneousDownloads;

        public DownloadService(IDownloader downloader, IObservable<IChangeSet<QueuedDownload>> downloadQueue,
            DirectoryInfo tempDownloadsDirectory)
        {
            this.downloader = downloader;
            SimultaneousDownloads = 5;
            this.downloadQueue = downloadQueue;
            QueuedDownloads = downloadQueue.AsObservableList();
            ActiveDownloads = this.downloadQueue
                .Top(SimultaneousDownloads)
                .Transform(qd => new ActiveDownload(qd, tempDownloadsDirectory, downloader))
                .AsObservableList();
        }

        public IObservableList<QueuedDownload> QueuedDownloads { get; }

        public IObservableList<ActiveDownload> ActiveDownloads { get; }

        public int SimultaneousDownloads
        {
            get => simultaneousDownloads;
            set
            {
                this.RaiseAndSetIfChanged(ref simultaneousDownloads, value);
                if (ServicePointManager.DefaultConnectionLimit < value)
                    ServicePointManager.DefaultConnectionLimit = value;
            }
        }
    }
}