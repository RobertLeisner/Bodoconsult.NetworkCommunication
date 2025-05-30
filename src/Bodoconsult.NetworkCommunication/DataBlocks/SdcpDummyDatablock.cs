// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.NetworkCommunication.Interfaces;

namespace Bodoconsult.NetworkCommunication.DataBlocks
{
    /// <summary>
    /// Dummy data block
    /// </summary>
    public class SdcpDummyDatablock: IDataBlock
    {
        /// <summary>
        /// Data contains the bytes of the Data except the byte representing datablock type
        /// </summary>
        public Memory<byte> Data { get; set; }

        /// <summary>
        /// Code for the datablock type
        /// </summary>
        public char DataBlockType { get; set; }= 'd';
    }
}
