// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO.Pipelines;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.TcpIp
{
    /// <summary>
    /// High performance TcpIp implementation based on MS pipeline System.Io.Pipelines
    /// </summary>
    public class TcpIpHighPerformanceDuplexIo : BaseTcpIpDuplexIo, IDuplexPipe
    {
        private readonly Pipe _readPipe;
        private readonly Pipe _writePipe;

        /// <summary>
        /// Timeout for polling in milliseconds
        /// </summary>
        public int PollingTimeout { get; set; } = 1000;

        /// <summary>
        /// Default ctor
        /// </summary>
        public TcpIpHighPerformanceDuplexIo(IDataMessagingConfig config, ISendPacketProcessFactory sendPacketProcessFactory) : base(config, sendPacketProcessFactory)
        {
            _readPipe = new Pipe();
            _writePipe = new Pipe();
        }

        /// <summary>
        /// Start the duplex communication
        /// </summary>
        /// <returns>Task</returns>
        public override async Task StartCommunication()
        {
            await Task.Run(async () =>
            {
                if (Receiver == null)
                {
                    Receiver = new TcpIpHighPerformanceDuplexIoReceiver(_readPipe, DataMessagingConfig, PollingTimeout);

                    await Receiver.StartReceiver();

                }

                if (Sender == null)
                {
                    Sender = new TcpIpHighPerformanceDuplexIoSender(_writePipe, DataMessagingConfig, PollingTimeout);

                    await Sender.StartSender();
                }
            });

            IsCommunicationStarted = true;
        }

        /// <summary>
        /// Stop the duplex communication
        /// </summary>
        /// <returns>Task</returns>
        public override async Task StopCommunication()
        {
            if (Sender != null)
            {
                await Sender.StopSender();
            }

            if (Receiver != null)
            {
                await Receiver.StopReceiver();
            }

            IsCommunicationStarted = false;
        }


        /// <summary>
        /// Current implemenation of Dispose()
        /// </summary>
        /// <param name="disposing">Dispong required?</param>
        protected override async Task Dispose(bool disposing)
        {
            await StopCommunication();
        }

        /// <summary>Gets the <see cref="T:System.IO.Pipelines.PipeReader" /> half of the duplex pipe.</summary>
        public PipeReader Input => _readPipe.Reader;

        /// <summary>Gets the <see cref="T:System.IO.Pipelines.PipeWriter" /> half of the duplex pipe.</summary>
        public PipeWriter Output => _writePipe.Writer;
    }
}