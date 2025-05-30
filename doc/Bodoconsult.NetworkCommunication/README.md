Bodoconsult.NetworkCommunication
==============

# What does the library

Bodoconsult.NetworkCommunication is a library with basic functionality for setting up a client server network communication based on TCP/IP and a self-defined low level byte message protocol. 

>	[Setting up messaging configuration: IDataMessagingConfig](#setting-up-messaging-configuration-idatamessagingconfig)

>	[Defining your device communication protocol: Simple Device Communication Protocol (SDCP) as sample](#defining-your-device-communication-protocol-simple-device-communication-protocol-sdcp-as-sample)

>	[Define message limiting bytes: DeviceCommunicationBasics](#define-message-limiting-bytes-devicecommunicationbasics)

>	[Implement a data message splitter splitting the incoming byte stream into potential messages: IDataMessageSplitter](#define-message-limiting-bytes-devicecommunicationbasics)

>	[Implement your message types: IDataMessage](#implement-your-message-types-idatamessage)

>	[Implement a handshake validator: IHandshakeDataMessageValidator](#implement-a-handshake-validator-ihandshakedatamessagevalidator)

>	[Implement a data message validator: IDataMessageValidator](#implement-a-data-message-validator-idatamessagevalidator)

>	[Implement a message forwarder for received messages: IDataMessageProcessor](#implement-a-message-forwarder-for-received-messages-idatamessageprocessor)

>	[]()

>	[]()

# How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Setting up messaging configuration: IDataMessagingConfig

``` csharp
/// <summary>
/// Config file for one the client-server network communication with one client device
/// </summary>
public class TestDataMessagingConfig: IDataMessagingConfigTcpIp
{

	/// <summary>
	/// A readable string for identitying the device used for logging
	/// </summary>
	public string LoggerId => "TestDevice";

	/// <summary>
	/// Current socket to use
	/// </summary>
	public ISocketProxy SocketProxy { get; set; }

	/// <summary>
	/// Data message procssing package
	/// </summary>
	public IDataMessageProcessingPackage DataMessageProcessingPackage { get; set; }

	/// <summary>
	/// Update data message processing package
	/// </summary>
	public UpdateDataMessageProcessingPackageDelegate UpdateDataMessageProcessingPackageDelegate { get; set; }

	/// <summary>
	/// Current general logger
	/// </summary>
	public IAppLoggerProxy AppLogger { get; set; }

	/// <summary>
	/// Current monitor logger
	/// </summary>
	public IAppLoggerProxy MonitorLogger { get; set; }

	/// <summary>
	/// A delegate for a method returning true if the communications is online or false if offline
	/// </summary>
	/// <returns>A delegate</returns>
	public CheckIfCommunicationIsOnlineDelegate CheckIfCommunicationIsOnlineDelegate { get; set; }

	/// <summary>
	/// A delegate for a method returning true if the device is or false if not
	/// </summary>
	/// <returns>true if the device is ready else false</returns>
	public CheckIfDeviceIsReadyDelegate CheckIfDeviceIsReadyDelegate { get; set; }

	/// <summary>
	/// Request a closing of the current communication connection from the business logic delegate
	/// </summary>
	public RaiseComDevCloseRequestDelegate RaiseComDevCloseRequestDelegate { get; set; }

	/// <summary>
	/// Delegate for handling central exception handling in <see cref="IDuplexIo"/> implementations.
	/// Set internally normally. Public implementation intended for testing purposes.
	/// </summary>
	public DuplexIoErrorHandlerDelegate DuplexIoErrorHandlerDelegate { get; set; }

	/// <summary>
	/// Message not sent delegate
	/// </summary>
	public RaiseDataMessageNotSentDelegate RaiseDataMessageNotSentDelegate { get; set; }

	/// <summary>
	/// Message sent delegate
	/// </summary>
	public RaiseDataMessageSentDelegate RaiseDataMessageSentDelegate { get; set; }

	/// <summary>
	/// Delegate fired on comm level if a data message has been received. Should be used in <see cref="ICommunicationHandler"/> impls to implement there handshake responses and then forward to the next layer
	/// </summary>
	public RaiseDataMessageReceivedDelegate RaiseCommLayerDataMessageReceivedDelegate { get; set; }

	/// <summary>
	/// Delegate raised on app level if data message was received
	/// </summary>
	public RaiseDataMessageReceivedDelegate RaiseAppLayerDataMessageReceivedDelegate { get; set; }

	/// <summary>
	/// Delegate raised if a device message does not fit the expectations (length, content, ...)
	/// </summary>
	public RaiseUnexpectedDataMessageReceivedDelegate RaiseUnexpectedDataMessageReceivedDelegate { get; set; }

	/// <summary>
	/// IP address of the device
	/// </summary>
	public string IpAddress { get; set; }

	/// <summary>
	/// Port to use for device communication
	/// </summary>
	public int Port { get; set; }
}
```

# Defining your device communication protocol: Simple Device Communication Protocol (SDCP) as sample

For the usage in the test there will be implemented the following Simple Device Communication Protocol (SDCP).

The SDCP knows two basic types of messages:

- **Data messages** starting with STX and ending with ETX. Everything between STX and ETX is handled as datablock
- **Handshake**: a 1-byte-message with either ACK (message received successfully), NACK (message NOT received successfully) or CAN (device not ready)

# Define message limiting bytes: DeviceCommunicationBasics

The static class DeviceCommunicationBasics holds default values used by the communication layer like timeouts, message start tokens and handshake start tokens. If the default settings are not fitting to your requirements, adjust the values as required.

``` csharp
/// <summary>
/// A holder class for basic device communication values
/// </summary>
public static class DeviceCommunicationBasics
{
	public static int MaxSendAttemptCount { get; set; } =  4;


	#region Command timeouts

	/// <summary>
	/// The default timeout in milliseconds
	/// </summary>
	public static int DefaultTimeout { get; set; } = 2000;

	/// <summary>
	/// Timeout for a PING sent to the tower
	/// </summary>
	public static int PingTimeout { get; set; } = 500;

	/// <summary>
	/// Timeout for waiting for a handshake like ACK, NACK or CAN
	/// </summary>
	public static int WaitForAckTimeout { get; set; } = 12000;

	#endregion

	#region Message tokens

	/// <summary>
	/// All tokens a message can start with
	/// </summary>
	public static List<byte> MessageStartTokens = new() {
		Ack,
		Can,
		Nack,
		Stx
	};

	/// <summary>
	/// All tokens a handshake message can start with
	/// </summary>
	public static List<byte> HandshakeMessageStartTokens = new() {
		Ack,
		Can,
		Nack
	};

	#endregion

	#region Constants

	public const byte Stx = 2;      // 0x02

	public const byte Etx = 3;      // 0x03

	public const byte Ack = 6;      // 0x06

	public const byte Nack = 21;    // 0x15

	public const byte Can = 24;     // 0x18

	public const byte NullByte = 0x0;

	#endregion
}
```

# Implement a data message splitter splitting the incoming byte stream into potential messages: IDataMessageSplitter

The following implementation of IDataMessageSplitter shows how the byte stream is splitted for the SDCP protocol. 

``` csharp
/// <summary>
/// Implementation for <see cref="IDataMessageSplitter"/> for SDCP protocol
/// </summary>
public class SdcpDataMessageSplitter : IDataMessageSplitter
{

    // Array pool is okay as shared instance here
    private static readonly ArrayPool<byte> ArrayPool = ArrayPool<byte>.Shared;

    /// <summary>
    /// Length of handshake messages
    /// </summary>
    protected int HandshakeLength = 1;

    /// <summary>
    /// Main method for TCP/IP message receiving: split the inbound byte stream in commands to process later
    /// </summary>
    /// <param name="buffer">Receiving buffer</param>
    /// <param name="command">The received command. May have a length of zero. Then no valid message was received so far</param>
    /// <returns>True if a command was successfuly extract from the buffer else false</returns>
    public virtual bool TryReadCommand(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> command)
    {
        var result = TryReadCommandInternal(ref buffer, out command);

        // Check for nulls string the

        command = DataMessageHelper.CheckCommandForNullAtTheEnd(command);

        // Now copy the command if required
        if (command.Length <= 0)
        {
            return result;
        }
        var array = ArrayPool.Rent((int)command.Length);

        command.CopyTo(array);
        command = new ReadOnlySequence<byte>(array).Slice(0, command.Length);

        ArrayPool.Return(array);

        return result;
    }

    private bool TryReadCommandInternal(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> command)
    {
        //Debug.Print($"TryReadCommand: {GetStringFromArray(buffer.ToArray())}");

        if (buffer.Length == 0)
        {
            command = default;
            return false;
        }

        var firstByte = buffer.Slice(0, 1).FirstSpan[0];

        // First byte is no message start: remove byte until next message start
        while (true)
        {
            // First byte is message start byte
            if (DeviceCommunicationBasics.MessageStartTokens.Contains(firstByte))
            {
                break;
            }

            // Remove byte if no message start byte
            buffer = buffer.Slice(1);

            if (buffer.Length == 0)
            {
                command = default;
                return false;
            }

            firstByte = buffer.Slice(0, 1).FirstSpan[0];

        }

        // No other
        if (buffer.Length < 1)
        {
            command = default;
            return false;
        }

        // First token is not a message start token
        if (!DeviceCommunicationBasics.MessageStartTokens.Contains(firstByte))
        {
            // Handshake with length 1?
            if (HandshakeLength == 1)
            {
                command = buffer.Slice(0, 1);
                buffer = buffer.Slice(1);
                return true;
            }

            // Check if there is a message start token following: if yes return invalid message
            for (var i = 1; i < HandshakeLength; i++)
            {
                var nextByte = buffer.Slice(i, 1).FirstSpan[0];
                if (!DeviceCommunicationBasics.MessageStartTokens.Contains(nextByte))
                {
                    continue;
                }
                command = buffer.Slice(0, i);
                buffer = buffer.Slice(i);
                return true;
            }


            command = default;
            return false;
        }

        // Handshake
        if (DeviceCommunicationBasics.HandshakeMessageStartTokens.Contains(firstByte))
        {

            var nextByte = buffer.Slice(1, 1).FirstSpan[0];

            if (!DeviceCommunicationBasics.MessageStartTokens.Contains(nextByte))
            {
                command = buffer.Slice(0, HandshakeLength);
                buffer = buffer.Slice(HandshakeLength);
                return true;
            }

            command = buffer.Slice(0, 1);
            buffer = buffer.Slice(1);
            return true;
        }

        // Data message
        if (firstByte != DeviceCommunicationBasics.Stx)
        {
            command = default;
            buffer = buffer.Slice(1);
            return false;
        }

        // Find end of message ETX
        int etxPos;
        var etxFound = false;
        for (etxPos = 0; etxPos < buffer.Length; etxPos++)
        {
            if (buffer.Slice(etxPos, 1).FirstSpan[0] == DeviceCommunicationBasics.Etx)
            {
                etxFound = true;
                break;
            }
        }

        if (!etxFound)
        {
            command = default;
            return false;
        }

        command = buffer.Slice(0, etxPos + 1);
        return true;
    }

    /// <summary>
    /// Compute the datablock length depending on firmware version
    /// </summary>
    /// <param name="messageBytes">Raw data as byte array</param>
    /// <returns>Length of the datablock</returns>
    public int ComputeDataLength(ref ReadOnlySequence<byte> messageBytes)
    {
        // Not needed for SDCP
        return 0;
    }

}
```
See the tests in the the repo for this class for how to test IDataMessageSplitter implementations:

``` csharp
[TestFixture]
internal class SdcpDataMessageSplitterTests
{
	private readonly SdcpDataMessageSplitter _splitter = new();

	[Test]
	public void TryReadCommand_NoValidMessage_NullReturned()
	{
		// Arrange 
		var data = new byte[] { 0x99,  0x99 };
		var ros = new ReadOnlySequence<byte>(data);

		// Act  
		var result = _splitter.TryReadCommand(ref ros, out var command);

		// Assert
		Assert.That(result, Is.False);
	}

	...
	
}
```

# Implement your message types: IDataMessage

``` csharp
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
```

# Implement a handshake validator: IHandshakeDataMessageValidator

A handshake validator checks first in method IsHandshakeForSentMessage() if a received handshake is for a certain sent message. Second it sets the ProcessExecutionResult for the responsible send process (ISendPacketProcess):

``` csharp
/// <summary>
/// Implementation of <see cref="IHandshakeDataMessageValidator"/> for SDCP protocol
/// </summary>
public class SdcpHandshakeDataMessageValidator : IHandshakeDataMessageValidator
{
	/// <summary>
	/// Is a received message a handshake for a sent message
	/// </summary>
	/// <param name="sentMessage">Sent message</param>
	/// <param name="handshakeMessage">Received handshake message</param>
	/// <returns>True if the message was the handshake for the sent message</returns>
	public DataMessageValidatorResult IsHandshakeForSentMessage(IDataMessage sentMessage, IDataMessage handshakeMessage)
	{

		if (sentMessage is not SdcpDataMessage)
		{
			return new DataMessageValidatorResult(false, "No SDCP data message sent");
		}

		if (handshakeMessage is not HandshakeMessage)
		{
			return new DataMessageValidatorResult(false, "Received message is NOT a handshake message");
		}

		return new DataMessageValidatorResult(true, string.Empty);
	}

	/// <summary>
	/// Handle the received handshake and sets the ProcessExecutionResult for the responsible send process <see cref="ISendPacketProcess"/>
	/// </summary>
	/// <param name="context">Current send message process</param>
	/// <param name="handshake">Received handshake</param>
	public void HandleHandshake(ISendPacketProcess context, IDataMessage handshake)
	{
		if (handshake == null)
		{
			context.ProcessExecutionResult = OrderExecutionResultState.Error;
			return;
		}

		if (handshake is not HandshakeMessage hs)
		{
			//todo result wrong message?
			context.ProcessExecutionResult = OrderExecutionResultState.NoResponseFromDevice;
			context.DataMessagingConfig.MonitorLogger?.LogWarning($"Message {context.Message.MessageId}: No handshake received. Current Sent Attempt Count > MaxRepeatCount. No ResponseFromTower! ");
			return;
		}

		switch (hs.HandshakeMessageType)
		{
			case HandShakeMessageType.Ack:
				context.ProcessExecutionResult = OrderExecutionResultState.Successful;
				context.CurrentSendAttempsCount = 0;
				context.DataMessagingConfig.MonitorLogger?.LogDebug($"Message {context.Message.MessageId}: ACK received [{hs.HandshakeMessageType:X2}]");
				break;

			case HandShakeMessageType.Nack:
				context.ProcessExecutionResult = OrderExecutionResultState.Nack;
				context.DataMessagingConfig.MonitorLogger?.LogWarning($"Message {context.Message.MessageId} : NAK received [ {hs.HandshakeMessageType:X2}]");
				break;

			case HandShakeMessageType.Can:
				context.ProcessExecutionResult = OrderExecutionResultState.Can;
				//IMPORTANT clear
				context.CurrentSendAttempsCount = 0;
				context.DataMessagingConfig.MonitorLogger?.LogWarning($"Message {context.Message.MessageId}: CAN received");
				break;
			default:
				context.ProcessExecutionResult = OrderExecutionResultState.Error;
				break;
		}
	}
}
```

# Implement a data message validator: IDataMessageValidator

A data message validator checks in method IsMessageValid() if the received message is basically a valid message. No detailed checks for datablock content etc. at this point.

``` csharp
/// <summary>
/// SDCP protocol implementation of <see cref="IDataMessageValidator"/>
/// </summary>
public class SdcpDataMessageValidator : IDataMessageValidator
{
	public DataMessageValidatorResult IsMessageValid(IDataMessage dataMessage)
	{
		// Update mode message or raw message: always valid
		if (dataMessage is RawDataMessage)
		{
			return new DataMessageValidatorResult(true, "Message is valid");
		}

		// No SDCP data message: always valid
		if (dataMessage is not SdcpDataMessage)
		{
			return new DataMessageValidatorResult(false, "Message is NOT a valid SDCP message");
		}

		return new DataMessageValidatorResult(true, "Message is a valid SDCP message");
	}
}
```
# Implement a message forwarder for received messages: IDataMessageProcessor

The IDataMessageProcessor implementations forward the received messages to the business logic handling them. Normally data messages are forwarded to business logic via IDataMessagingConfig.RaiseDataMessageReceivedDelegate delegate invoking. Handshake messages are forwarded normally to a IWaitStateManager instance handling the already sent and for a handshake waiting data messages.

``` csharp
/// <summary>
/// Current implementation of <see cref="IDataMessageProcessor"/> for SDCP protocol.
/// Should invoke IDataMessagingConfig.RaiseDataMessageReceivedDelegate for data messages and IDataMessagingConfig.DataMessageProcessingPackage.WaitStateManager?.OnHandshakeReceived for handshakes
/// </summary>
public class SdcpDataMessageProcessor : IDataMessageProcessor
{

    private readonly AutoResetEvent _stopped = new(false);

    private const int TimeOut = 2000;

    public readonly IDataMessagingConfig Config;

    /// <summary>
    /// Default ctor
    /// </summary>
    public SdcpDataMessageProcessor(IDataMessagingConfig config)
    {
        Config = config;
    }

    /// <summary>
    /// Process the message
    /// </summary>
    /// <param name="message">Message to process</param>
    public void ProcessMessage(IDataMessage message)
    {
        // handshake received
        if (message is HandshakeMessage handShake)
        {
            ProcessHandshakes(handShake);
            return;
        }

        // Tower data message received
        if (message is SdcpDataMessage dataMessage)
        {
            AsyncHelper.FireAndForget2(() => Config.RaiseCommLayerDataMessageReceivedDelegate?.Invoke(dataMessage)).ContinueWith(Callback);
        }

        // No valid message
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private void ProcessHandshakes(HandshakeMessage handShake)
    {
        // fire and forget but let CallBack() be run at the end
        AsyncHelper.FireAndForget2(() =>
                Config.DataMessageProcessingPackage.WaitStateManager?.OnHandshakeReceived(handShake))
            .ContinueWith(Callback);
        _stopped.WaitOne(TimeOut);
        //Config.MonitorLogger?.LogInformation($"received handshake message [{hs.HandshakeMessageType:X2}]");

    }

    private void Callback(IAsyncResult ar)
    {
        _stopped.Set();
    }
}
```

``` csharp

```

``` csharp

```

``` csharp

```

``` csharp

```

``` csharp

```


# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

