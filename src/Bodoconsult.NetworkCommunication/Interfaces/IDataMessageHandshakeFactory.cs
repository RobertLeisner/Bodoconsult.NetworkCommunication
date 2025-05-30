

namespace Bodoconsult.NetworkCommunication.Interfaces
{
    public interface IDataMessageHandshakeFactory
    {
        /// <summary>
        /// Get a ACK handshake message
        /// </summary>
        /// <param name="message">Current message received</param>
        /// <returns>ACK handshake message to send</returns>
        IDataMessage GetAckResponse(IDataMessage message);

        /// <summary>
        /// Get a NAK handshake message
        /// </summary>
        /// <param name="message">Current message received</param>
        /// <returns>NAK handshake message to send</returns>
        IDataMessage GetNakResponse(IDataMessage message);
    }
}