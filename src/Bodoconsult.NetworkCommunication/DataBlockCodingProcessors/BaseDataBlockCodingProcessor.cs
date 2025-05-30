// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.DataBlockCodingProcessors;

/// <summary>
/// Base class for handling of the usage of the correct datablock codec for processing a datablock 
/// </summary>
public abstract class BaseDataBlockCodingProcessor : IDataBlockCodingProcessor

{
    protected readonly Dictionary<char, IDataBlockCodec> DatablockCodecs = new();

    /// <summary>
    /// Create a byte array to send to tower from datablock
    /// </summary>
    /// <param name="data">Tower message data to send</param>
    /// <param name="dataBlock">Current datablock instance</param>
    public void FromDataBlockToBytes(List<byte> data, IDataBlock dataBlock)
    {

        //throw new NotImplementedException();

        var result = new DataBlockCodecResult();

        var dataBlockCodec = GetDatablockCodecCanBeNull(dataBlock.DataBlockType);
        if (dataBlockCodec == null)
        {
            result.ErrorMessage = $"No codec available for {dataBlock.DataBlockType}";
            return;
        }
        dataBlockCodec.EncodeDataBlock(data, dataBlock);
        return;

    }


    /// <summary>
    /// Get the correct codec for a given datablock
    /// </summary>
    /// <param name="datablockType">Type of the datablock</param>
    /// <returns>Codec or null if no fitting codec was found</returns>
    public IDataBlockCodec GetDatablockCodecCanBeNull(char datablockType)
    {
        DatablockCodecs.TryGetValue(datablockType, out IDataBlockCodec dataBlockCodec);
        return dataBlockCodec;
    }

    /// <summary>
    /// Decode a given datablock as byte array to the related internal datablock object
    /// </summary>
    /// <param name="dataBlockBytes">Received datablock as byte array</param>
    /// <returns>Datablock object or null if no fitting codec was found</returns>
    public IDataBlock FromBytesToDataBlock(Memory<byte> dataBlockBytes)
    {
        if (dataBlockBytes.Length == 0)
        {

            return null;
        }

        var dataBlockType = Convert.ToChar(dataBlockBytes[..1].Span[0]);
        var dataBlockCodec = GetDatablockCodecCanBeNull(dataBlockType);
        if (dataBlockCodec != null)
        {
            var dataBlock = dataBlockCodec.DecodeDataBlock(dataBlockBytes);
            return dataBlock;
        }
        return null;
    }
}