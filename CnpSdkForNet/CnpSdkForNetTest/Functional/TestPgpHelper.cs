/*
 * Zachary Cook
 *
 * Tests the PgpHelper methods.
 */

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestPgpHelper
    {
        private string testDir;
        private string merchantPublickeyId;
        private string passphrase;
        private string vantivPublicKeyId;

        /*
         * Deletes a file if it exists.
         */
        private static void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
        
        [OneTimeSetUp]
        public void SetUp()
        {
            // Ignore the tests if PGP functional tests are disabled.
            EnvironmentVariableTestFlags.RequirePGPFunctionalTestsEnabled();
            
            // Create the test directory.
            this.testDir = Path.Combine(Path.GetTempPath(),"NET" + CnpVersion.CurrentCNPXMLVersion,"testPgp");
            if (!Directory.Exists(this.testDir))
            {
                Directory.CreateDirectory(this.testDir);
            }
            
            // Get the environment variables.
            this.merchantPublickeyId = Environment.GetEnvironmentVariable("merchantPublicKeyId");
            Console.WriteLine("Merchant Public Key ID:" + this.merchantPublickeyId);
            this.passphrase = Environment.GetEnvironmentVariable("pgpPassphrase");
            Console.WriteLine("Pgp Passphrase:" + this.passphrase);
            this.vantivPublicKeyId = Environment.GetEnvironmentVariable("vantivPublicKeyId");
            Console.WriteLine(("Public Key ID:" + this.vantivPublicKeyId));

        }

        /*
         * Tests encrypting and decrypting a file.
         */
        [Test]
        public void TestEncryptionDecryption()
        {
            // Delete the existing file.
            var testFilepath = Path.Combine(this.testDir,"test_pgp.txt");
            DeleteFile(testFilepath);

            // Write a new file.
            string[] lines = {"The purpose of this file is to test 'PgpHelper.EncryptFile and PgpHelper.DecryptFile' methods", 
                "Second Line", 
                "Third Line", 
                "!@#$%^&*()_+-=[]{}/><;':~"};
            File.WriteAllLines(testFilepath, lines);
            
            // Encrypt the file.
            var encryptedFilePath = testFilepath.Replace(".txt", ".asc");
            DeleteFile(encryptedFilePath);
            PgpHelper.EncryptFile(testFilepath, encryptedFilePath,this.merchantPublickeyId);
            
            // Assert that the encrypted file has been created.
            var entries = Directory.EnumerateFiles(this.testDir);
            Assert.True(entries.Contains(encryptedFilePath));
            
            // Decrypt the file.
            var decryptedFilePath = Path.Combine(this.testDir,"test_pgp_decrypted.txt");
            PgpHelper.DecryptFile(encryptedFilePath,decryptedFilePath,this.passphrase);
            
            // Assert that the decrypted file has been created.
            entries = Directory.EnumerateFiles(this.testDir);
            Assert.True(entries.Contains(decryptedFilePath));
            
            // Assert the file was encrypted and decrypted correctly.
            var original = File.ReadAllLines(testFilepath);
            var decrypted = File.ReadAllLines(decryptedFilePath);
            Assert.AreEqual(original, decrypted);
        }

        /*
         * Tests encrypting a file with an invalid public key.
         */
        [Test]
        public void TestInvalidPublicKeyId()
        {
            // Delete the existing file.
            var testFilepath = Path.Combine(this.testDir,"test_pgp.txt");
            DeleteFile(testFilepath);

            // Write a new file.
            string[] lines = {"The purpose of this file is to test 'PgpHelper.EncryptFile and PgpHelper.DecryptFile' methods", 
                "Second Line", 
                "Third Line", 
                "!@#$%^&*()_+-=[]{}/><;':~"};
            File.WriteAllLines(testFilepath, lines);
            
            var encryptedFilePath = testFilepath.Replace(".txt", ".asc");
            DeleteFile(encryptedFilePath);
            try
            {
                PgpHelper.EncryptFile(testFilepath, encryptedFilePath, "BadPublicKeyId");
                Assert.Fail("CnpOnline exception expected but was not thrown");
            }
            catch (CnpOnlineException e)
            {
                Assert.True(e.Message.Contains("Please make sure that the recipient Key ID is correct and is added to your gpg keyring"),"Actual error message: " + e.Message);
            }
        }

        /*
         * Tests attempting to encrypt a file that doesn't exist.
         */
        [Test]
        public void TestNonExistentFileToEncrypt()
        {
            var testFilepath = "bad_file_path";
            var encryptedFilePath = Path.Combine(this.testDir,"test_pgp.asc");
            
            // Assert that encrypting a file throws the correct exception.
            try
            {
                PgpHelper.EncryptFile(testFilepath,encryptedFilePath,this.merchantPublickeyId);
                Assert.Fail("CnpOnline exception expected but was not thrown");
            }
            catch (CnpOnlineException e)
            {
                Assert.True(e.Message.Contains("Please make sure the input file exists and has read permission."),"Actual error message: " + e.Message);
            }
        }
        
        /*
         * Tests attempting to decrypt a file that doesn't exist.
         */
        [Test]
        public void TestNonExistentFileToDecrypt()
        {
            var encryptedFilePath = "bad_file_path";
            var decryptedFilePath = Path.Combine(this.testDir,"test_pgp_decrypted.txt");
            
            // Assert that encrypting a file throws the correct exception.
            try
            {
                PgpHelper.DecryptFile(encryptedFilePath, decryptedFilePath,this.passphrase);
                Assert.Fail("CnpOnline exception expected but was not thrown");
            }
            catch (CnpOnlineException e)
            {
                Assert.True(e.Message.Contains("Please make sure the input file exists and has read permission."),"Actual error message: " + e.Message);
            }
        }

        /*
         * Tests attempting to decrypt with an incorrect passphrase.
         */
        [Test]
        public void TestInvalidPassphrase()
        {
            // Delete the existing file.
            var testFilepath = Path.Combine(this.testDir,"test_pgp.txt");
            DeleteFile(testFilepath);

            // Write a new file.
            string[] lines = {"The purpose of this file is to test 'PgpHelper.EncryptFile and PgpHelper.DecryptFile' methods", 
                "Second Line", 
                "Third Line", 
                "!@#$%^&*()_+-=[]{}/><;':~"};
            File.WriteAllLines(testFilepath, lines);
            
            // Encrypt the file.
            var encryptedFilePath = testFilepath.Replace(".txt", ".asc");
            DeleteFile(encryptedFilePath);
            PgpHelper.EncryptFile(testFilepath, encryptedFilePath,this.merchantPublickeyId);
            
            // Assert that decrypting a file throws the correct exception.
            var decryptedFilePath = Path.Combine(this.testDir,"test_pgp_decrypted.txt");
            try
            {
                PgpHelper.DecryptFile(encryptedFilePath, decryptedFilePath, "bad_passphrase");
                Assert.Fail("CnpOnline exception expected but was not thrown");
            }
            catch (CnpOnlineException e)
            {
                Console.WriteLine(e.Message);
                Assert.True(e.Message.Contains("Please make sure that the passphrase is correct."),"Actual error message: " + e.Message);
            }            
        }

        /*
         * Tests attempting to decrypt with no private key.
         */
        [Test]
        public void TestNoSecretKeyToDecrypt()
        {
            // Delete the existing file.
            var testFilepath = Path.Combine(this.testDir,"test_pgp.txt");
            DeleteFile(testFilepath);

            // Write a new file.
            string[] lines = {"The purpose of this file is to test 'PgpHelper.EncryptFile and PgpHelper.DecryptFile' methods", 
                "Second Line", 
                "Third Line", 
                "!@#$%^&*()_+-=[]{}/><;':~"};
            File.WriteAllLines(testFilepath, lines);
            
            // Encrypt the file.
            var encryptedFilePath = testFilepath.Replace(".txt", ".asc");
            DeleteFile(encryptedFilePath);
            PgpHelper.EncryptFile(testFilepath,encryptedFilePath,this.vantivPublicKeyId);
            
            // Assert that decrypting a file throws the correct exception.
            var decryptedFilePath = Path.Combine(this.testDir,"test_pgp_decrypted.txt");
            try
            {
                PgpHelper.DecryptFile(encryptedFilePath, decryptedFilePath,this.passphrase);
                Assert.Fail("CnpOnline exception expected but was not thrown");
            }
            catch (CnpOnlineException e)
            {
                Assert.True(e.Message.Contains("Please make sure that your merchant secret key is added to your gpg keyring."),"Actual error message: " + e.Message);
            }         
        }
    }
}