// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.NetworkCommunication.Interfaces;
using Bodoconsult.NetworkCommunication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodoconsult.NetworkCommunication.DataMessaging.DataMessageValidators
{
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
}
