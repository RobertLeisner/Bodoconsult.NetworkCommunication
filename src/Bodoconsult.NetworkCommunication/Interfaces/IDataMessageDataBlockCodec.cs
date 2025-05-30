﻿

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.NetworkCommunication.Interfaces
{
    /// <summary>
    /// This class is implemented by classes used to encode or decode
    /// bytes contained in the datablock of a incoming <see cref="IDataMessage"/>
    /// </summary>
    public interface IDataMessageDataBlockCodec
    {

        /// <summary>
        /// Method decodes an incoming bytes array to an instance of Datablock object
        /// Method is used while receiving bytes from device
        /// </summary>
        /// <param name="datablockBytes">Datablock bytes received</param>
        /// <returns>Datablock object</returns>
        IDataMessageDataBlock DecodeDataBlock(Memory<byte> datablockBytes);

        /// <summary>
        /// Method encode an instance of Datablock in bytes array.
        /// Method is called when a message is sent to the device
        /// </summary>
        /// <param name="data">The array as list to add the datablock to</param>
        /// <param name="datablock">Current datablock object</param>
        /// <returns>a byte array with datablock infos</returns>
        void EncodeDataBlock(List<byte> data, IDataBlock datablock);
    }
}