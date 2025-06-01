// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.NetworkCommunication.DataMessages;
using Bodoconsult.NetworkCommunication.EnumAndStates;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.DataMessageProcessingPackages;

/// <summary>
/// Factory for creating handshakes for EDCP protocol to sent to the client
/// </summary>
public class EdcpHandshakeFactory : IDataMessageHandshakeFactory
{
    /// <summary>
    /// Get an ACK handshake message
    /// </summary>
    /// <param name="message">Current message received</param>
    /// <returns>ACK handshake message to send</returns>
    public IDataMessage GetAckResponse(IDataMessage message)
    {
        if (message is not EdcpDataMessage em)
        {
            var can = new EdcpHandshakeMessage(MessageTypeEnum.Sent)
            {
                HandshakeMessageType = HandShakeMessageType.Can,
            };
            return can;
        }

        var ack = new EdcpHandshakeMessage(MessageTypeEnum.Sent)
        {
            HandshakeMessageType = HandShakeMessageType.Ack,
            BlockCode = em.BlockCode
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
        if (message is not EdcpDataMessage em)
        {
            var can = new EdcpHandshakeMessage(MessageTypeEnum.Sent)
            {
                HandshakeMessageType = HandShakeMessageType.Can,
            };
            return can;
        }

        var nak = new EdcpHandshakeMessage(MessageTypeEnum.Sent)
        {
            HandshakeMessageType = HandShakeMessageType.Nack,
            BlockCode = em.BlockCode
        };
        return nak;
    }

    /// <summary>
    /// Get a CAN handshake message
    /// </summary>
    /// <param name="message">Current message received</param>
    /// <returns>CAN handshake message to send</returns>
    public IDataMessage GetCanResponse(IDataMessage message)
    {
        if (message is not EdcpDataMessage em)
        {
            var can1 = new EdcpHandshakeMessage(MessageTypeEnum.Sent)
            {
                HandshakeMessageType = HandShakeMessageType.Can,
            };
            return can1;
        }

        var can = new EdcpHandshakeMessage(MessageTypeEnum.Sent)
        {
            HandshakeMessageType = HandShakeMessageType.Can,
            BlockCode = em.BlockCode
        };
        return can;
    }
}