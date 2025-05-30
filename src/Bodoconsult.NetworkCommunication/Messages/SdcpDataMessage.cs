﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.App.Helpers;
using Bodoconsult.NetworkCommunication.EnumAndStates;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.Messages
{
    /// <summary>
    /// Basic implementation of <see cref="IDataMessage"/> for SDCP protocol
    /// </summary>
    public class SdcpDataMessage: IDataMessage
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public SdcpDataMessage()
        {
            MessageId = DateTime.Now.ToFileTimeUtc();
        }

        /// <summary>
        /// A unique ID to identify the message
        /// </summary>
        public long MessageId { get; }

        /// <summary>
        /// The message type of the message
        /// </summary>
        public MessageTypeEnum MessageType { get; set; } = MessageTypeEnum.Received;

        /// <summary>
        /// Is waiting for acknowledgement by the device required for the message
        /// </summary>
        public bool WaitForAcknowledgement { get; set; }

        /// <summary>
        /// Should a acknowledgement be sent if the message is received
        /// </summary>
        public bool AnswerWithAcknowledgement { get; set; }

        /// <summary>
        /// Current raw message data as byte array
        /// </summary>
        public Memory<byte> RawMessageData { get; set; }

        /// <summary>
        /// Current raw message data as clear text
        /// </summary>
        public string RawMessageDataClearText { get; set; }

        /// <summary>
        /// Create an info string for logging
        /// </summary>
        /// <returns>Info string</returns>
        public string ToInfoString()
        {
            return $"SdcpDataMessage ID {MessageId} {MessageType.ToString()} {ArrayHelper.GetStringFromArrayCsharpStyle(RawMessageData)}";
        }

        public string ToShortInfoString()
        {
            return $"SdcpDataMessage ID {MessageId} {MessageType.ToString()}";
        }
    }
}
