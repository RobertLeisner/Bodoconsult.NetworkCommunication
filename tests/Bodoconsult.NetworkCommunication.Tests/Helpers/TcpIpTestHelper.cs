﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Net;
using Bodoconsult.NetworkCommunication.TcpIp;
using Bodoconsult.NetworkCommunication.TcpIp.Transport;
using Bodoconsult.NetworkCommunication.Tests.Infrastructure;
using Bodoconsult.NetworkCommunication.Tests.Interfaces;

namespace Bodoconsult.NetworkCommunication.Tests.Helpers
{
    public static class TcpIpTestHelper
    {
        /// <summary>
        /// Initialize the IP communication
        /// </summary>
        public static void InitServer(ITcpTowerTests testSetup)
        {
            testSetup.DataMessagingConfig = new DefaultDataMessagingConfig();

            testSetup.IpAddress = IPAddress.Parse(testSetup.DataMessagingConfig.IpAddress);

            testSetup.Server?.Dispose();

            testSetup.Server = new TcpServer(testSetup.IpAddress, testSetup.DataMessagingConfig.Port);
            testSetup.Server.StartServer();
        }

        public static void InitSocket(ITcpTowerTests testSetup)
        {
            // Soft reset server
            testSetup.Server.ResetClientSocket();
            //testSetup.SmdTower.AppLogger = testSetup.Logger;
            //testSetup.SmdTower.MonitorLogger = testSetup.Logger;

            testSetup.Server.WaitForConnections().GetAwaiter().GetResult();


            // Close socket if necessary
            try
            {
                testSetup.Socket?.Close();
            }
            catch
            {
                // Do nothing
            }


            // Load socket
            var socket = new AsyncTcpIpSocketProxy();
            testSetup.Socket = socket;

            var ipLocalEndPoint = new IPEndPoint(testSetup.IpAddress, testSetup.DataMessagingConfig.Port);
            testSetup.Socket.Connect(ipLocalEndPoint).Wait();

            testSetup.Logger = TestDataHelper.GetFakeAppLoggerProxy();
        }

        /// <summary>
        /// Reset only the server
        /// </summary>
        /// <param name="testSetup"></param>
        public static void ResetServer(ITcpTowerTests testSetup)
        {
            // Soft reset server
            testSetup.Server.ResetClientSocket();
            testSetup.Server.WaitForConnections().GetAwaiter().GetResult();

        }

        //public static void InitFakeSocket(ITcpTowerTests testSetup)
        //{
        //    testSetup.Socket = new FakeTcpIpSocketProxy();
        //}
    }
}
