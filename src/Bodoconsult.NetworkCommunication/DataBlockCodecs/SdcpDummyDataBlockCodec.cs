// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.DataBlockCodecs
{
    public class SdcpDummyDataBlockCodec: IDataBlockCodec
    {
        /// <summary>
        /// Method encode an instance of Datablock in bytes array.
        /// Method is called when a message is sent to the device
        /// </summary>
        /// <param name="data">The array as list to add the datablock to</param>
        /// <param name="datablock">Current datablock object</param>
        /// <returns>a byte array with datablock infos</returns>
        public void EncodeDataBlock(List<byte> data, IDataBlock datablock)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method decodes an incoming bytes array to an instance of Datablock object
        /// Method is used while receiving bytes from device
        /// </summary>
        /// <param name="datablockBytes">Datablock bytes received</param>
        /// <returns>Datablock object</returns>
        public IDataBlock DecodeDataBlock(Memory<byte> datablockBytes)
        {
            throw new NotImplementedException();
        }
    }
}
