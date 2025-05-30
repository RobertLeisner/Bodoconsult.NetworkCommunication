// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.NetworkCommunication.Interfaces
{
    public interface IDataBlockCodingProcessor
    {
        /// <summary>
        /// Create a byte array to send to device from datablock
        /// </summary>
        /// <param name="data">device message data to send</param>
        /// <param name="dataBlock">Current datablock instance</param>
        /// <param name="firmware">Current firmware version</param>
        void FromDataBlockToBytes(List<byte> data, IDataBlock dataBlock, uint firmware);

        /// <summary>
        /// Get the correct codec for a given datablock
        /// </summary>
        /// <param name="datablockType">Type of the datablock</param>
        /// <returns>Codec or null if no fitting codec was found</returns>
        IDataBlockCodec GetDatablockCodecCanBeNull(char datablockType);

        /// <summary>
        /// Decode a given datablock as byte array to the related internal datablock object
        /// </summary>
        /// <param name="dataBlockBytes">Received datablock as byte array</param>
        /// <param name="firmware">Current firmware version</param>
        /// <returns>Datablock object or null if no fitting codec was found</returns>
        IDataBlock FromBytesToDataBlock(Memory<byte> dataBlockBytes, uint firmware);
    }
}