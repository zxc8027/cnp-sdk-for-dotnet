/*
 * Zachary Cook
 *
 * Class for storing and sending 1 to many batch requests.
 * Due to time constraints, the backwards-compatibility wrapping
 * methods were not added. For a final release, they should be added.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    /*
     * Class for representing time.
     */
    public class CNPTime
    {
        /*
         * Returns the current time using a given format.
         */
        public virtual string GetCurrentTime(string format)
        {
            return DateTime.Now.ToString(format);
        }
    }
    
    /*
     * Class for file contents.
     */
    public class CNPFile
    {
        /*
         * Creates a random file.
         */
        public virtual string CreateRandomFile(string fileDirectory,string fileName,string fileExtension,CNPTime cnpTime)
        {
            string filePath = null;
            if (string.IsNullOrEmpty(fileName))
            {
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                fileName = cnpTime.GetCurrentTime("MM-dd-yyyy_HH-mm-ss-ffff_") + RandomGen.NextString(8);
                filePath = fileDirectory + fileName + fileExtension;

                using (var fs = new FileStream(filePath,FileMode.Create))
                {
                }
            }
            else
            {
                filePath = fileDirectory + fileName;
            }

            return filePath;
        }

        /*
         * Appends a line to a file.
         */
        public virtual string AppendLineToFile(string filePath, string lineToAppend)
        {
            using (var fs = new FileStream(filePath, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(lineToAppend);
            }

            return filePath;
        }

        /*
         * Appends another file to th efile.
         */
        public virtual string AppendFileToFile(string filePathToAppendTo, string filePathToAppend)
        {

            using (var fs = new FileStream(filePathToAppendTo, FileMode.Append))
            using (var fsr = new FileStream(filePathToAppend, FileMode.Open))
            {
                var buffer = new byte[16];

                var bytesRead = 0;

                do
                {
                    bytesRead = fsr.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, bytesRead);
                }
                while (bytesRead > 0);
            }

            File.Delete(filePathToAppend);

            return filePathToAppendTo;
        }

        /*
         * Creates a directory it doesn't exist.
         */
        public virtual void CreateDirectory(string destinationFilePath)
        {
            var destinationDirectory = Path.GetDirectoryName(destinationFilePath);

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }
        }
    }
    
    /*
     * Class for generating random numbers.
     */
    public static class RandomGen
    {
        private static RNGCryptoServiceProvider globalRandomNumberGenerator = new RNGCryptoServiceProvider();
        private static Random localRandomNumberGenerator;
        
        /*
         * Returns the next random integer.
         */
        public static int NextInt()
        {
            // Get the random generator.
            var inst = localRandomNumberGenerator;
            if (inst == null)
            {
                var buffer = new byte[8];
                globalRandomNumberGenerator.GetBytes(buffer);
                localRandomNumberGenerator = new Random(BitConverter.ToInt32(buffer, 0));
            }

            // Return the next random number.
            return localRandomNumberGenerator.Next();
        }

        /*
         * Returns the a random string of a given length.
         */
        public static string NextString(int length)
        {
            var result = "";

            // Add random characters.
            for (var i = 0; i < length; i++)
            {
                result += Convert.ToChar(NextInt() % ('Z' - 'A') + 'A');
            }

            // Return the random string.
            return result;
        }
    }
    
    /*
     * Class for storing and sending batch requests.
     */
    public class CnpBatch
    {
        private authentication authentication;
        private ConfigManager config;
        private Communications communication;
        private int numOfCnpBatchRequest = 0;
        private int numOfRFRRequest = 0;
        public string finalFilePath = null;
        private string batchFilePath = null;
        private string requestDirectory;
        private string responseDirectory;
        private CNPTime cnpTime;
        private CNPFile cnpFile;
        private RFRRequest rfrRequest;
        private List<CnpBatchRequest> batchRequests = new List<CnpBatchRequest>();
        
        /*
         * Construct a CNP Batch using the built-in configuration.
         */
        public CnpBatch()
        {
            this.config = new ConfigManager();
            this.InitializeRequest();
        }

        /*
         * Construct a CNP Batch using the given configuration.
         */
        public CnpBatch(ConfigManager config)
        {
            this.config = config;
            this.InitializeRequest();
        }

        /*
         * Construct a CNP Batch using the given configuration.
         */
        [Obsolete("Deprecated in-favor of CnpBatch(ConfigManager config)")]
        public CnpBatch(Dictionary<string,string> config)
        {
            this.config = new ConfigManager(config);
            this.InitializeRequest();
        }

        /*
         * Initializes the request.
         */
        private void InitializeRequest()
        {
            // Create the communications.
            this.communication = new Communications();

            // Create the authentication.
            this.authentication = new authentication();
            this.authentication.user = this.config.GetValue("username");
            this.authentication.password = this.config.GetValue("password");

            // Determine the directories.
            requestDirectory = Path.Combine(this.config.GetValue("requestDirectory"),"Requests") + Path.DirectorySeparatorChar;
            responseDirectory = Path.Combine(this.config.GetValue("responseDirectory"),"Responses") + Path.DirectorySeparatorChar;

            // Create the time and file.
            this.cnpTime = new CNPTime();
            this.cnpFile = new CNPFile();
        }

        public authentication GetAuthenication()
        {
            return this.authentication;
        }

        public string GetRequestDirectory()
        {
            return this.requestDirectory;
        }

        public string GetResponseDirectory()
        {
            return this.responseDirectory;
        }

        public void SetCommunication(Communications communication)
        {
            this.communication = communication;
        }

        public Communications GetCommunication()
        {
            return this.communication;
        }

        public void SetCnpTime(CNPTime cnpTime)
        {
            this.cnpTime = cnpTime;
        }

        public CNPTime GetCnpTime()
        {
            return this.cnpTime;
        }

        public void SetCnpFile(CNPFile cnpFile)
        {
            this.cnpFile = cnpFile;
        }

        public CNPFile GetCnpFile()
        {
            return this.cnpFile;
        }

        // Add a single batch to batch request.
        public void AddBatch(CnpBatchRequest cnpBatchRequest)
        {
            // Throw an exception if an RFRrequest already exsits.
            if (this.numOfRFRRequest != 0)
            {
                throw new CnpOnlineException("Can not add a batch request to a batch with an RFRrequest!");
            }
            
            // Add batchRequest xml element into cnpRequest xml element.
            this.batchRequests.Add(cnpBatchRequest);
            this.numOfCnpBatchRequest++;
        }

        public void addRFRRequest(RFRRequest rfrRequest)
        {
            // Throw an exception for non-RFRrequests and existing RFRrequests
            if (this.numOfCnpBatchRequest != 0)
            {
                throw new CnpOnlineException("Can not add an RFRRequest to a batch with requests!");
            }
            else if (this.numOfRFRRequest >= 1)
            {
                throw new CnpOnlineException("Can not add more than one RFRRequest to a batch!");
            }

            // Add the request.
            this.rfrRequest = rfrRequest;
            this.numOfRFRRequest++;
        }

        /*
         * Sends the file.
         */
        public string SendToCnp()
        {
            var useEncryption =  config.GetValue("useEncryption");
            var vantivPublicKeyId = config.GetValue("vantivPublicKeyId");
            
            // Serialize the file.
            var requestFilePath = this.Serialize();
            var batchRequestDir = requestDirectory;
            var finalRequestFilePath = requestFilePath;
            
            // Encrypt the file.
            if ("true".Equals(useEncryption))
            {
                batchRequestDir = Path.Combine(requestDirectory, "encrypted");
                Console.WriteLine(batchRequestDir);
                finalRequestFilePath =
                    Path.Combine(batchRequestDir, Path.GetFileName(requestFilePath) + ".encrypted");
                cnpFile.CreateDirectory(finalRequestFilePath);
                PgpHelper.EncryptFile(requestFilePath, finalRequestFilePath, vantivPublicKeyId);
            }
            
            // Drop off the file.
            communication.FtpDropOff(batchRequestDir, Path.GetFileName(finalRequestFilePath), config);
            
            // Return the file name.
            return Path.GetFileName(finalRequestFilePath);
        }
        
        /*
         * Waits for a response for the batch request.
         */
        public void BlockAndWaitForResponse(string fileName,int timeOut)
        {
            this.communication.FtpPoll(fileName,timeOut,config);
        }

        /*
         * Retrieves a response.
         */
        public cnpResponse ReceiveFromCnp(string batchFileName)
        {
            var useEncryption =  config.GetValue("useEncryption");
            var pgpPassphrase = config.GetValue("pgpPassphrase");
            cnpFile.CreateDirectory(this.responseDirectory);
            
            var responseFilePath = Path.Combine(this.responseDirectory, batchFileName);
            var batchResponseDir = this.responseDirectory;
            var finalResponseFilePath = responseFilePath;

            // Decrypt the file.
            if ("true".Equals(useEncryption))
            {
                batchResponseDir = Path.Combine(responseDirectory, "encrypted");
                finalResponseFilePath =
                    Path.Combine(batchResponseDir, batchFileName);
                cnpFile.CreateDirectory(finalResponseFilePath);
            }
            communication.FtpPickUp(finalResponseFilePath, config, batchFileName);

            if ("true".Equals(useEncryption))
            {
                responseFilePath = responseFilePath.Replace(".encrypted", "");
                PgpHelper.DecryptFile(finalResponseFilePath, responseFilePath, pgpPassphrase);
            }

            // Deserialize and return the response.
            var cnpResponse = VersionedXMLDeserializer.Deserialize<cnpResponse>(File.ReadAllText(responseFilePath),this.config.GetVersion());
            return cnpResponse;
        }

        /*
         * Serialize the batch into temp xml file, and return the path to it.
         */
        public string SerializeBatchRequestToFile(batchRequest cnpBatchRequest, string filePath)
        {
            // Create cnpRequest xml file if not exist.
            // Otherwise, the xml file created, thus storing some batch requests.
            filePath = cnpFile.CreateRandomFile(requestDirectory, Path.GetFileName(filePath),"_temp_cnpRequest.xml", cnpTime);
            
            // Serializing the batchRequest creates an xml for that batch request and returns the path to it.
            var tempFilePath = cnpBatchRequest.Serialize(this.config.GetVersion());
            
            // Append the batch request xml just created to the accummulating cnpRequest xml file.
            cnpFile.AppendFileToFile(filePath, tempFilePath);
            
            // Return the path to temp xml file.
            return filePath;
        }

        /*
         * Serializes an RFR request file.
         */
        public string SerializeRFRRequestToFile(RFRRequest rfrRequest,string filePath)
        {
            filePath = cnpFile.CreateRandomFile(requestDirectory, Path.GetFileName(filePath), "_temp_cnpRequest.xml", cnpTime);
            var tempFilePath = rfrRequest.Serialize(this.config.GetVersion());

            cnpFile.AppendFileToFile(filePath, tempFilePath);

            return filePath;
        }

        /*
         * Serailizes the request.
         */
        public string Serialize()
        {
            var xmlHeader = "<?xml version='1.0' encoding='utf-8'?>";

            // Populate the request.
            var request = new cnpRequest();
            request.authentication = authentication;
            request.RFRRequest = this.rfrRequest;
            request.numBatchRequests = this.batchRequests.Count;
            foreach (var batchRequest in this.batchRequests)
            {
                request.batchRequest.Add(batchRequest.GetBatchRequest());
            }
            
            // Create the Session file.
            finalFilePath = cnpFile.CreateRandomFile(requestDirectory,Path.GetFileName(finalFilePath), ".xml", cnpTime);
            cnpFile.AppendLineToFile(finalFilePath,request.Serialize(this.config.GetVersion()));
            var filePath = finalFilePath;
            finalFilePath = null;

            return filePath;
        }
    }
}