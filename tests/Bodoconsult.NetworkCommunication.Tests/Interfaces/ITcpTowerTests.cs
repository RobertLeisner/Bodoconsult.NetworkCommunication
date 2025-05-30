// Copyright (c) Mycronic. All rights reserved.


using System.Net;
using Bodoconsult.App.Interfaces;
using Bodoconsult.NetworkCommunication.Interfaces;
using Bodoconsult.NetworkCommunication.Tests.Infrastructure;

namespace Bodoconsult.NetworkCommunication.Tests.Interfaces
{
    public interface ITcpTowerTests
    {

        /// <summary>
        /// Current TCP/IP server to send data to the socket
        /// </summary>
        TcpServer Server { get; set; }

        /// <summary>
        /// Current IP address to use
        /// </summary>
        IPAddress IpAddress { get; set; }

        /// <summary>
        /// Device communication data
        /// </summary>
        ITcpDataMessagingConfig DataMessagingConfig { get; set; }

        /// <summary>
        /// General log file
        /// </summary>
        IAppLoggerProxy Logger { get; set; }

        /// <summary>
        /// Current <see cref="ISocketProxy"/> implementation to use
        /// </summary>
        ISocketProxy Socket { get; set; }

    }
}
