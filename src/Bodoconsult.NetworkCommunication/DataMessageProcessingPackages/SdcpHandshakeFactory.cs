// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.NetworkCommunication.EnumAndStates;
using Bodoconsult.NetworkCommunication.Interfaces;
using Bodoconsult.NetworkCommunication.Messages;

namespace Bodoconsult.NetworkCommunication.DataMessageProcessingPackages;

public class SdcpHandshakeFactory : IDataMessageHandshakeFactory
{
    /// <summary>
    /// Get an ACK handshake message
    /// </summary>
    /// <param name="message">Current message received</param>
    /// <returns>ACK handshake message to send</returns>
    public IDataMessage GetAckResponse(IDataMessage message)
    {
        var ack = new HandshakeMessage( MessageTypeEnum.Sent)
        {
            HandshakeMessageType = HandShakeMessageType.Ack,
        };

        return ack;
    }

    /// <summary>
    /// Get a NAK handshake message
    /// </summary>
    /// <param name="message">Current message received</param>
    /// <returns>NAK handshake message to send</returns>
    public IDataMessage GetNakResponse(IDataMessage message)
    {
        var nak = new HandshakeMessage(MessageTypeEnum.Sent)
        {
            HandshakeMessageType = HandShakeMessageType.Nack,
        };
        return nak;
    }
}