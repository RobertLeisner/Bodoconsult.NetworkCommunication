// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.NetworkCommunication.Interfaces;
using Bodoconsult.NetworkCommunication.TcpIp.Sending;

namespace Bodoconsult.NetworkCommunication.Delegates
{

    #region Data messaging basics

    public delegate bool DuplexIoIsWorkInProgressDelegate();

    public delegate void DuplexIoSetNotInProgressDelegate();

    /// <summary>
    /// Delegate for handling central exception handling in <see cref="IDuplexIo"/> implementations
    /// </summary>
    public delegate void DuplexIoErrorHandlerDelegate(Exception e);

    /// <summary>
    /// Delegate for unregistering a wait state from a <see cref="IWaitStateManager"/> implementation
    /// </summary>
    /// <param name="state">wait state to unregister</param>
    public delegate void UnregisterWaitStateDelegate(SendPacketProcess state);

    /// <summary>
    /// A delegate for a method returning true if the communications is online or false if offline
    /// </summary>
    /// <returns>True if the device communication is online else false</returns>
    public delegate bool CheckIfCommunicationIsOnlineDelegate();

    /// <summary>
    /// A delegate for a method returning true if the device is or false if not
    /// </summary>
    /// <returns>true if the device is ready else false</returns>
    public delegate bool CheckIfDeviceIsReadyDelegate();

    /// <summary>
    /// Data message not sent delegate
    /// </summary>
    public delegate void  RaiseDataMessageNotSentDelegate(ReadOnlyMemory<byte> message, string reason);

    /// <summary>
    /// Message sent delegate
    /// </summary>
    public delegate void RaiseDataMessageSentDelegate(ReadOnlyMemory<byte> message);

    /// <summary>
    /// Delegate fired if a handshake has been received by the wait state manager
    /// </summary>
    /// <param name="message">Handshake message</param>
    public delegate void RaiseDataMessageHandshakeReceivedDelegate(IDataMessage message);

    /// <summary>
    /// Update the data message processing package
    /// </summary>
    public delegate void UpdateDataMessageProcessingPackageDelegate();

    /// <summary>
    /// Delegate for delivering a received data message to the app next level
    /// </summary>
    public delegate void RaiseDataMessageReceivedDelegate(IDataMessage message);

    /// <summary>
    /// Unexpected data message received delegate
    /// </summary>
    /// <param name="message"></param>
    public delegate void RaiseUnexpectedDataMessageReceivedDelegate(IDataMessage message);

    /// <summary>
    /// Request a closing of the current communication connection from the business logic delegate
    /// </summary>
    /// <param name="requestSource">Source location of the request</param>
    public delegate void RaiseComDevCloseRequestDelegate(string requestSource);

    #endregion

    // ****** Delegates for sender and reciever ****** 
    
    // ****** Receiver delegates ****** 
    
    public delegate void RaisedeviceMessageNotReceivedDelegate(IDataMessage message);
    public delegate void RaisedeviceMessageCorruptedDelegate(byte messageBlockAndRc, string reason);

    // ****** Sender delegates ****** 


}
