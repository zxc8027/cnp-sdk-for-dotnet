/*
 * Zachary Cook
 *
 * Helper methods for GnuGP.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Cnp.Sdk.Properties;

namespace Cnp.Sdk
{
    struct ProcessResult
    {
        public string output;
        public string error;
        public int status;
    }
    
    public class PgpHelper
    {
        private const int Success = 0;
        private static string GpgPath = Settings.Default.gnuPgDir;
        private const string GpgExecutableName = "gpg";
        private const string GpgConfExecutableName = "gpgconf";
        private const string encryptCommandFormat = @"--batch --yes --armor --trust-model always --output {0} --recipient {1} --encrypt {2}";
        private const string decryptCommandFormat = @"--batch --trust-model always --output {0} --passphrase {1} --decrypt {2}";
        private const string decryptCommandFormatPinentryLoop = @"--batch --trust-model always --pinentry-mode loopback --output {0} --passphrase {1} --decrypt {2}";
        private const string importPrivateKeyCommandFormat = @"--import --passphrase-fd 0 --pinentry-mode loopback {0}";
        private const string importPublicKeyCommandFormat = @"--import {0}";
        
        /*
         * Returns the executable path for a given path.
         * This is used for Windows that may or may not hav ".exe" at
         * the end of file names.
         */
        public static string GetExecutablePath(string path)
        {
            // Add ".exe" if the file exists.
            if (!File.Exists(path) && File.Exists(path + ".exe"))
            {
                return path + ".exe";
            }

            // Return the base path.
            return path;
        }
        
        /*
         * Encrypts a file.
         */
        public static void EncryptFile(string inputFileName,string outputFileName,string recipientKeyId)
        {
            // Perform the command.
            var command = string.Format(encryptCommandFormat,outputFileName,recipientKeyId,inputFileName);
            var processResult = ExecuteCommand(command,GpgExecutableName);
            
            // Throw an exception for errors.
            if (processResult.status != Success)
            {
                if (processResult.error.ToLower().Contains("no public key"))
                {
                    throw new CnpOnlineException("Please make sure that the recipient Key ID is correct and is added to your gpg keyring.\n" + processResult.error);
                }
                else if (Regex.IsMatch(processResult.error,string.Format("can't open .{0}", Regex.Escape(inputFileName))))
                {
                    throw new CnpOnlineException("Please make sure the input file exists and has read permission.\n" + processResult.error);
                }
                else
                {
                    throw new CnpOnlineException(processResult.error);
                }
            }
            
            // Log that the encryption occured.
            Console.WriteLine("Encrypted with key id " + recipientKeyId + " successfully!");
        }

        /*
         * Decrypts a file.
         */
        public static void DecryptFile(string inputFileName,string outputFileName,string passphrase)
        {
            // Delete the existing output file if it exists.
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }
            
            // Run the command for GPG >=2.1. If it doesn't work (<2.1), then use 2.0 and earlier.
            // If it works, reset the passphrase so it isn't saved (GPG >=2.1).
            var procResult = ExecuteCommand(string.Format(decryptCommandFormatPinentryLoop,outputFileName,passphrase,inputFileName),GpgExecutableName);
            if (procResult.status != Success && procResult.error.ToLower().Contains("gpg: invalid option \"--pinentry-mode\""))
            {
                procResult = ExecuteCommand(string.Format(decryptCommandFormat,outputFileName,passphrase,inputFileName),GpgExecutableName);
            }
            else
            {
                var result = ExecuteCommand(@"--kill gpg-agent",GpgConfExecutableName);
            }

            // Throw an exception if it failed.
            if (procResult.status != Success)
            {
                if (procResult.error.ToLower().Contains("gpg: public key decryption failed: bad passphrase"))
                {
                    throw new CnpOnlineException("Please make sure that the passphrase is correct.\n" + procResult.error);
                }
                else if (procResult.error.ToLower().Contains("gpg: decryption failed: no secret key"))
                {
                    throw new CnpOnlineException("Please make sure that your merchant secret key is added to your gpg keyring.\n" + procResult.error);
                }
                else if (Regex.IsMatch(procResult.error,string.Format("can't open .{0}", Regex.Escape(inputFileName))))
                {
                    throw new CnpOnlineException("Please make sure the input file exists and has read permission.\n" + procResult.error);
                }
                else
                {
                    throw new CnpOnlineException(procResult.error);
                }
            }
        }

        /*
         * Imports a private key.
         */
        public static string ImportPrivateKey(string keyFilePath,string passphrase)
        {
            // Run the command.
            var procResult = ExecuteCommand(string.Format(importPrivateKeyCommandFormat,keyFilePath),passphrase);
            if (procResult.status != Success)
            {
                throw new CnpOnlineException(procResult.error);
            }

            // Return the key id.
            return ExtractKeyId(procResult.error);
        }
        
        /*
         * Imports a public key.
         */
        public static string ImportPublicKey(string keyFilePath)
        {
            // Run the command.
            var procResult = ExecuteCommand(string.Format(importPublicKeyCommandFormat, keyFilePath),GpgExecutableName);
            if (procResult.status != Success)
            {
                throw new CnpOnlineException(procResult.error);
            }
            
            // Return the key id.
            return ExtractKeyId(procResult.error);
        }
        
        /*
         * Executes a command for an executable.
         */
        private static ProcessResult ExecuteCommand(string command,string executable)
        {
            // Create the process information.
            var path = GetExecutablePath(Path.Combine(GpgPath, executable));
            var procStartInfo = new ProcessStartInfo(path,command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            // Start the process and wait for it to finish.
            // Note: If the standard input's buffer becomes full, the thread will lock.
            var process = new Process { StartInfo = procStartInfo };
            process.Start();
            process.StandardInput.Flush();
            process.WaitForExit();
            
            // Return the result.
            return new ProcessResult
            {
                output = process.StandardOutput.ReadToEnd(),
                error = process.StandardError.ReadToEnd(),
                status = process.ExitCode
            };
        }

        /*
         * Extracts a key id from a string.
         */
        private static string ExtractKeyId(string result)
        {
            return result.Split(':')[1].Split(' ')[2].Substring(8);
        }
    }
}