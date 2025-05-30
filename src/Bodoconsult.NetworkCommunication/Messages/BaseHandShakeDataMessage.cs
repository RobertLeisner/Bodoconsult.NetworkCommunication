// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.NetworkCommunication.EnumAndStates;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.Messages;

/// <summary>
/// Base class for <see cref="IDataMessage"/> implementations
/// </summary>
public class BaseHandShakeDataMessage: IDataMessage
{
    public BaseHandShakeDataMessage()
    {
        MessageId = DateTime.Now.ToFileTimeUtc();
    }

    /// <summary>
    /// A unique ID to identify the message
    /// </summary>
    public long MessageId { get;  }

    /// <summary>
    /// The message type of the message
    /// </summary>
    public MessageTypeEnum MessageType { get; set; }

    /// <summary>
    /// Is waiting for acknowledgement by the device required for the message
    /// </summary>
    public bool WaitForAcknowledgement => false;

    /// <summary>
    /// Should a acknowledgement be sent if the message is received
    /// </summary>
    public bool AnswerWithAcknowledgement => false;

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
    public virtual string ToInfoString()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Create a short info string for logging
    /// </summary>
    /// <returns>Info string</returns>
    public virtual string ToShortInfoString()
    {
        throw new NotSupportedException();
    }

}