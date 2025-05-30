// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Interfaces;

namespace Bodoconsult.NetworkCommunication.TcpIp
{
    /// <summary>
    /// Helepr class for <see cref="TcpIpDuplexIoReceiver"/>
    /// </summary>
    public class DummyMemory: IResetable
    {
        
        /// <summary>
        /// Memory instance
        /// </summary>
        public Memory<byte> Memory { get; set; }

        /// <summary>
        /// Reset the class to default values
        /// </summary>
        public void Reset()
        {
            Memory = null;
        }
    }
}