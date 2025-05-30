// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.NetworkCommunication.EnumAndStates;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.Messages
{
    /// <summary>
    /// Represents a handshake message
    /// </summary>
    public sealed class HandshakeMessage : BaseHandShakeDataMessage, IHandShakeDataMessage
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="messageType">Current message type</param>
        public HandshakeMessage(MessageTypeEnum messageType)
        {
            MessageType = messageType;
        }

        /// <summary>
        /// Typpe of handshake as byte value
        /// </summary>
        public byte HandshakeMessageType { set; get; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return HandshakeMessageType switch
            {
                6 => "HandshakeMessage ACK",
                21 => "HandshakeMessage NAK",
                24 => "HandshakeMessage CAN",
                _ => "HandshakeMessage Unknown"
            };
        }

        /// <summary>
        /// Create an info string for logging
        /// </summary>
        /// <returns>Info string</returns>
        public override string ToInfoString()
        {
            return HandshakeMessageType switch
            {
                6 => "HandshakeMessage ACK",
                21 => "HandshakeMessage NAK",
                24 => "HandshakeMessage CAN",
                _ => "HandshakeMessage Unknown"
            };
        }

        /// <summary>
        /// Create a short info string for logging
        /// </summary>
        /// <returns>Info string</returns>
        public override string ToShortInfoString()
        {
            return HandshakeMessageType switch
            {
                6 => "HandshakeMessage ACK",
                21 => "HandshakeMessage NAK",
                24 => "HandshakeMessage CAN",
                _ => "HandshakeMessage Unknown"
            };
        }
    }
}