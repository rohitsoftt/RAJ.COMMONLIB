using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raj.CommonLib.SFTPProvider.Models.Credential_Models;
using Raj.CommonLib.SFTPProvider.UnitTests.static_data;

namespace Raj.CommonLib.SFTPProvider.UnitTests
{
    [TestClass]
    public class ConnectionTest
    {
        [TestMethod]
        public void TestUsingPassowrd()
        {
            try
            {
                var client = SFTPDetails.GetSFTPClientProvider(true);
                client.Connect();
                Assert.IsTrue(true, "Success");
                client.Disconnect();
            }
            catch(Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }
        [TestMethod]
        public void TestUsingKey()
        {
            try
            {
                var client = SFTPDetails.GetSFTPClientProvider(true,false, true);
                client.Connect();
                Assert.IsTrue(true, "Success");
                client.Disconnect();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }
        [TestMethod]
        public void TestUsingPasswordAndKey()
        {
            try
            {
                var client = SFTPDetails.GetSFTPClientProvider(true, true, true);
                client.Connect();
                Assert.IsTrue(true, "Success");
                client.Disconnect();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }
    }
}
