﻿using System.Net;
using JetBrains.Annotations;

namespace McNNTP.Server
{
    public class ConnectionMetadata
    {
        public IPAddress RemoteAddress { get; set; }
        public int RemotePort { get; set; }
        [CanBeNull]
        public string AuthenticatedUsername { get; set; }
    }
}
