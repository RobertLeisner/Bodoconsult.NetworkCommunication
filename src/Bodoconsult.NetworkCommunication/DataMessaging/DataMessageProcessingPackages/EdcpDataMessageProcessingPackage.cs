﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.NetworkCommunication.DataBlockCodecs;
using Bodoconsult.NetworkCommunication.DataBlockCodingProcessors;
using Bodoconsult.NetworkCommunication.DataMessageCodecs;
using Bodoconsult.NetworkCommunication.DataMessageCodingProcessors;
using Bodoconsult.NetworkCommunication.DataMessageProcessingPackages;
using Bodoconsult.NetworkCommunication.DataMessageProcessors;
using Bodoconsult.NetworkCommunication.DataMessageSplitters;
using Bodoconsult.NetworkCommunication.DataMessageValidators;
using Bodoconsult.NetworkCommunication.DataMessaging.DataMessagingConfig;
using Bodoconsult.NetworkCommunication.HandshakeDataMessageValidators;
using Bodoconsult.NetworkCommunication.Interfaces;
using Bodoconsult.NetworkCommunication.TcpIp.Sending;

namespace Bodoconsult.NetworkCommunication.DataMessaging.DataMessageProcessingPackages;

/// <summary>
/// Current implementation of <see cref="IDataMessageProcessingPackage"/> for EDCP protocol
/// </summary>
public class EdcpDataMessageProcessingPackage : IDataMessageProcessingPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public EdcpDataMessageProcessingPackage(IDataMessagingConfig dataMessagingConfig)
    {

        if (dataMessagingConfig is not EdcpDataMessagingConfig)
        {
            throw new ArgumentException("dataMessagingConfig must be or inherit from EdcpDataMessagingConfig");
        }

        DataMessagingConfig = dataMessagingConfig;

        // *******************************
        // Now setup the dependent objects

        // 1. Message splitter
        DataMessageSplitter = new EdcpDataMessageSplitter();

        // 2. Codecs
        DataMessageCodingProcessor = new DefaultDataMessageCodingProcessor();
        LoadCodecs();

        // 3. Internal forwarding
        DataMessageProcessor = new EdcpDataMessageProcessor(dataMessagingConfig);

        // 4. Wait state handler
        WaitStateManager = new DefaultWaitStateManager(dataMessagingConfig);

        // 5. Handshake validator
        HandshakeDataMessageValidator = new EdcpHandshakeDataMessageValidator();

        // 6. Data message validator
        DataMessageValidator = new EdcpDataMessageValidator();

        // 7. Handshake creation factory
        DataMessageHandshakeFactory = new EdcpHandshakeFactory();
    }

    private void LoadCodecs()
    {
        var handShakeCodec = new EdcpHandshakeMessageCodec();
        DataMessageCodingProcessor.MessageCodecs.Add(handShakeCodec);

        var processor = new DefaultDataBlockCodingProcessor();

        // Load your datablock codes here
        processor.LoadDataBlockCodecs('x', new SdcpDummyDataBlockCodec());

        var towerMessageCodec = new EdcpDataMessageCodec(processor);
        DataMessageCodingProcessor.MessageCodecs.Add(towerMessageCodec);

        var rawCodec = new RawDataMessageCodec();
        DataMessageCodingProcessor.MessageCodecs.Add(rawCodec);
    }

    /// <summary>
    /// Current data messaging config
    /// </summary>
    public IDataMessagingConfig DataMessagingConfig { get; }

    /// <summary>
    /// Current data message splitter
    /// </summary>
    public IDataMessageSplitter DataMessageSplitter { get; }

    /// <summary>
    /// Current data message coding processor
    /// </summary>
    public IDataMessageCodingProcessor DataMessageCodingProcessor { get; }

    /// <summary>
    /// Current data message processor for internal forwarding of the received messages
    /// </summary>
    public IDataMessageProcessor DataMessageProcessor { get; }

    /// <summary>
    /// Current wait state manager
    /// </summary>
    public IWaitStateManager WaitStateManager { get; }

    /// <summary>
    /// Current validator impl for handshake messages
    /// </summary>
    public IHandshakeDataMessageValidator HandshakeDataMessageValidator { get; }

    /// <summary>
    /// Current validator impl for data messages
    /// </summary>
    public IDataMessageValidator DataMessageValidator { get; }

    /// <summary>
    /// Factory for creation of handshakes to be sent for received messages
    /// </summary>
    public IDataMessageHandshakeFactory DataMessageHandshakeFactory { get; }
}