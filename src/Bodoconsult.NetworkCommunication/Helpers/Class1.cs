//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using System.Buffers;
//using System.Text;
//using Bodoconsult.NetworkCommunication.Interfaces;

//namespace Bodoconsult.NetworkCommunication.Helpers
//{
//    /// <summary>
//    /// Helper class for SMD tower messages
//    /// </summary>
//    public static class DataMessageHelper
//    {
//        /// <summary>
//        /// Unmask the block value byte
//        /// </summary>
//        /// <param name="receivedByte">Byte received</param>
//        /// <returns>Unmasked block value</returns>
//        public static byte DoUnmaskBlockValueReceived(byte receivedByte)
//        {
//            var wordValue = BitConverter.ToUInt16(BitConverter.GetBytes((short)receivedByte), 0);
//            byte value = 15;
//            var actualBlockValue = (byte)(wordValue & value);
//            return actualBlockValue;
//        }

//        /// <summary>
//        /// Get the serial number out of the byte array
//        /// </summary>
//        /// <param name="rawMessageData">Raw data as byte array</param>
//        /// <returns></returns>
//        public static string DoGetTowerSnReceived(byte[] rawMessageData)
//        {
//            return $"00{rawMessageData[2]}{rawMessageData[3]}{rawMessageData[4]}{rawMessageData[5]}";
//        }

//        /// <summary>
//        /// Get the command out of the byte array
//        /// </summary>
//        /// <param name="rawMessageData">Raw data as byte array</param>
//        /// <returns></returns>
//        public static string DoGetCommandReceived(byte[] rawMessageData)
//        {
//            return Encoding.ASCII.GetString(new[] { rawMessageData[6] });
//        }

//        /// <summary>
//        /// Split the received bytes in separate messages
//        /// </summary>
//        /// <returns>List with all received messages as byte array</returns>
//        public static IList<byte[]> SplitReceiveBuffer(byte[] buffer)
//        {

//            var result = new List<byte[]>();

//            var messageIndexes = new List<int>();

//            for (var aindex = 0; aindex < buffer.Length; aindex++)
//            {
//                var x = buffer[aindex];

//                var isNewMessage = DeviceCommunicationBasics.MessageStartTokens.Contains(x);

//                if (isNewMessage)
//                {
//                    messageIndexes.Add(aindex);
//                }

//            }

//            for (var bindex = 0; bindex < messageIndexes.Count; bindex++)
//            {
//                var start = messageIndexes[bindex];

//                int ende;

//                if (bindex == messageIndexes.Count - 1)
//                {
//                    ende = buffer.Length - 1;
//                }
//                else
//                {
//                    ende = messageIndexes[bindex + 1] - 1;
//                }

//                var len = ende - start + 1;
//                var data = new byte[len];
//                Buffer.BlockCopy(buffer, start, data, 0, len);
//                result.Add(data);
//            }

//            return result;

//        }

//        /// <summary>
//        /// Remove null bytes at the end of a message
//        /// </summary>
//        /// <param name="message">Current message to check</param>
//        /// <returns>Trimmed message</returns>
//        public static byte[] RemoveNullByteAtTheEnd(byte[] message)
//        {

//            if (message[^1] > 0 || message.Length <= 2)
//            {
//                return message;
//            }
//            int i;

//            for (i = message.Length - 1; i >= 0; i--)
//            {
//                if (message[i] > 0)
//                {
//                    break;
//                }
//            }

//            if (i < 0)
//            {
//                return Array.Empty<byte>();
//            }

//            if (i == message.Length - 1)
//            {
//                return message;
//            }

//            var newMessage = new byte[i + 1];
//            Buffer.BlockCopy(message, 0, newMessage, 0, i + 1);
//            return newMessage;
//        }


//        /// <summary>
//        /// Check if command has 0x0 bytes at the end and remove them if case of
//        /// </summary>
//        /// <param name="command">Comman to check</param>
//        /// <returns>Cleaned command</returns>
//        public static ReadOnlySequence<byte> CheckCommandForNullAtTheEnd(in ReadOnlySequence<byte> command)
//        {

//            if (command.Length == 0)
//            {
//                return command;
//            }

//            if (command.Length == 2)
//            {
//                return command;
//            }

//            if (command.Slice(command.Length - 1, 1).First.Span[0] != 0)
//            {
//                return command;
//            }

//            for (var i = command.Length - 1; i >= 0; i--)
//            {
//                if (command.Slice(i, 1).First.Span[0] != 0)
//                {
//                    return command.Slice(0, i);
//                }
//            }

//            return command;
//        }

//        /// <summary>
//        /// Get a 4-digits long tower serial number with leading zeros if needed
//        /// </summary>
//        /// <param name="towerSn">Tower serial number to check</param>
//        /// <returns>Correct 4-digit tower serial number</returns>
//        public static string GetCorrectTowerSn(string towerSn)
//        {
//            if (towerSn.Length > 4)
//            {
//                towerSn = towerSn.Substring(towerSn.Length - 4, 4);
//            }

//            var towerSnInt = Convert.ToInt16(towerSn);

//            return towerSnInt.ToString("0000");
//        }

//        /// <summary>
//        /// Add the tower serial number in a length of 4 digits to a list of bytes
//        /// </summary>
//        /// <param name="data">List of bytes to add the tower serial number to</param>
//        /// <param name="towerSn">Current tower serial number</param>
//        public static void AddCorrectTowerSn(List<byte> data, string towerSn)
//        {
//            if (towerSn.Length > 4)
//            {
//                towerSn = towerSn.Substring(towerSn.Length - 4, 4);
//            }

//            towerSn = Convert.ToInt16(towerSn).ToString("0000");

//            foreach (var s in towerSn)
//            {
//                data.Add(Convert.ToByte(s));
//            }
//        }

//        /// <summary>
//        /// Get a datablock as StSys internal string separated with commas
//        /// </summary>
//        /// <param name="data">Current datablock data</param>
//        /// <returns>Current datablock as string with commas as separator</returns>
//        public static string GetDataBlockAsStSysString(Memory<byte> data)
//        {
//            var result = new StringBuilder();
//            if (data is not { Length: > 0 })
//            {
//                return result.ToString();
//            }

//            foreach (var byteValue in data.Span)
//            {
//                result.Append($"{byteValue},");
//            }
//            return result.ToString()[..^1];
//        }
//    }
//}
