/*
 * Zachary Cook
 *
 * Handles online and batch communications.s
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Renci.SshNet.Common;

namespace Cnp.Sdk
{
    public class Communications
    {
        public const string CONTENT_TYPE_TEXT_XML_UTF8 = "text/xml; charset=UTF-8";
        
        private static readonly object SynLock = new object();

        public event EventHandler HttpAction;
        
        /*
         * Invoked when an Http method is performed.
         */
        public void OnHttpAction(RequestType requestType,string xmlPayload,bool neuterAccountNumbers,bool neuterCredentials)
        {
            if (HttpAction != null)
            {
                // Neuter the account numbers and credentials.
                if (neuterAccountNumbers)
                {
                    NeuterXml(ref xmlPayload);
                }
                if (neuterCredentials)
                {
                    NeuterUserCredentials(ref xmlPayload);
                }

                // Perform the action.
                HttpAction(this,new HttpActionEventArgs(requestType,xmlPayload));
            }
        }
        
        /*
         * Validates a server certificate.
         */
        public static bool ValidateServerCertificate(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            // If there are no error, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // Return false (certificate error).
            Console.WriteLine("Certificate error: " + sslPolicyErrors);
            return false;
        }

        /*
         * Neuters the account numbers of an input XML string.
         */
        public void NeuterXml(ref string inputXml)
        {
            const string pattern1 = "(?i)<number>.*?</number>";
            const string pattern2 = "(?i)<accNum>.*?</accNum>";
            const string pattern3 = "(?i)<track>.*?</track>";
            const string pattern4 = "(?i)<accountNumber>.*?</accountNumber>";

            // Create the RegEx patterns.
            var rgx1 = new Regex(pattern1);
            var rgx2 = new Regex(pattern2);
            var rgx3 = new Regex(pattern3);
            var rgx4 = new Regex(pattern4);
            
            // Replace the strings.
            inputXml = rgx1.Replace(inputXml, "<number>xxxxxxxxxxxxxxxx</number>");
            inputXml = rgx2.Replace(inputXml, "<accNum>xxxxxxxxxx</accNum>");
            inputXml = rgx3.Replace(inputXml, "<track>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</track>");
            inputXml = rgx4.Replace(inputXml, "<accountNumber>xxxxxxxxxxxxxxxx</accountNumber>");
        }

        /*
         * Neuters the user crednetials of an input XML string.
         */
        public void NeuterUserCredentials(ref string inputXml)
        {
            const string pattern1 = "(?i)<user>.*?</user>";
            const string pattern2 = "(?i)<password>.*></password>";

            // Create the RegEx patterns.
            var rgx1 = new Regex(pattern1);
            var rgx2 = new Regex(pattern2);
            
            // Replace the strings.
            inputXml = rgx1.Replace(inputXml,"<user>xxxxxx</user>");
            inputXml = rgx2.Replace(inputXml,"<password>xxxxxxxx</password>");
        }

        /*
         * Logs a message to a log file..
         */
        public void Log(string logMessage,string logFile,bool neuterAccountNumbers,bool neuterCredentials)
        {
            lock (SynLock)
            {
                // Neuter the account numbers and credentials.
                if (neuterAccountNumbers)
                {
                    NeuterXml(ref logMessage);
                }
                if (neuterCredentials)
                {
                    NeuterUserCredentials(ref logMessage);
                }
                
                // Write the log.
                using (var logWriter = new StreamWriter(logFile, true))
                {
                    var time = DateTime.Now;
                    logWriter.WriteLine(time.ToString(CultureInfo.InvariantCulture));
                    logWriter.WriteLine(logMessage + "\r\n");
                }
            }
        }
        
        /*
         * Creates a web request to send.
         */
        public HttpWebRequest CreateWebRequest(string xmlRequest,ConfigManager config)
        { 
            // Get the log file.
            string logFile = null;
            if (!string.IsNullOrEmpty(config.GetValue("logFile")))
            {
                logFile = config.GetValue("logFile");
            }
            
            // Get the rest of the configuration values.
            var neuterAccountNumbers = "true".Equals(config.GetValue("neuterAccountNums"));
            var neuterUserCredentials = "true".Equals(config.GetValue("neuterUserCredentials"));
            var printXml = "true".Equals(config.GetValue("printxml"));

            // Get the request target information.
            var url = config.GetValue("url");
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            var request = (HttpWebRequest)WebRequest.Create(url);

            // Output the request and log file.
            if (printXml)
            {
                Console.WriteLine(xmlRequest);
                Console.WriteLine(logFile);
            }

            // Log the request.
            if (logFile != null)
            {
                this.Log(xmlRequest,logFile, neuterAccountNumbers,neuterUserCredentials);
            }

            // Set up the request.
            request.ContentType = CONTENT_TYPE_TEXT_XML_UTF8;
            request.Method = "POST";
            request.ServicePoint.MaxIdleTime = 8000;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = true;
            if (!string.IsNullOrEmpty(config.GetValue("proxyHost")) && !string.IsNullOrEmpty(config.GetValue("proxyPort")))
            {
                var proxy = new WebProxy(config.GetValue("proxyHost"),int.Parse(config.GetValue("proxyPort")))
                {
                    BypassProxyOnLocal = true
                };
                request.Proxy = proxy;
            }
            
            // Set the timeout (only effective for non-async requests.
            try {
                request.Timeout = Convert.ToInt32(config.GetValue("timeout"));
            }
            catch (FormatException e) {
                request.Timeout = 60000;
            }

            // Invoke the event.
            this.OnHttpAction(Communications.RequestType.Request,xmlRequest,neuterAccountNumbers,neuterUserCredentials);
            
            // Return the request.
            return request;
        }

        /*
         * Sends an async HTTP Post request.
         */
        public virtual async Task<string> HttpPostAsync(string xmlRequest,ConfigManager config,CancellationToken cancellationToken)
        {
            // Get the log file.
            string logFile = null;
            if (!string.IsNullOrEmpty(config.GetValue("logFile")))
            {
                logFile = config.GetValue("logFile");
            }
            
            // Get the rest of the configuration values.
            var neuterAccountNumbers = "true".Equals(config.GetValue("neuterAccountNums"));
            var neuterUserCredentials = "true".Equals(config.GetValue("neuterUserCredentials"));
            var printXml = "true".Equals(config.GetValue("printxml"));
            
            // Get the request.
            var request = this.CreateWebRequest(xmlRequest,config);

            // Submit the request.
            using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
            {
                writer.Write(xmlRequest);
            }

            // Read the response.
            string xmlResponse = null;
            var response = await request.GetResponseAsync().ConfigureAwait(false);
            var httpResponse = (HttpWebResponse)response;
            try
            {
                // Read the XML.
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    xmlResponse = (await reader.ReadToEndAsync().ConfigureAwait(false)).Trim();
                }
                if (printXml)
                {
                    Console.WriteLine(xmlResponse);
                }

                // Invoke the event.
                this.OnHttpAction(Communications.RequestType.Response, xmlResponse,neuterAccountNumbers,neuterUserCredentials);

                // Log the response.
                if (logFile != null)
                {
                    this.Log(xmlResponse,logFile,neuterAccountNumbers,neuterUserCredentials);
                }
            } catch (WebException webException)
            {
                
            }

            // Return the response.
            return xmlResponse;
        }
        
        /*
         * Sends an async HTTP Post request.
         */
        [Obsolete("Deprecated in favor of Communications.HttpPostAsync(string xmlRequest,ConfigManager config,CancellationToken cancellationToken)")]
        public virtual Task<string> HttpPostAsync(string xmlRequest,Dictionary<string, string> config,CancellationToken cancellationToken)
        {
            return this.HttpPostAsync(xmlRequest,new ConfigManager(config),cancellationToken);
        }

        /*
         * Sends an HTTP Post request.
         */
        public virtual string HttpPost(string xmlRequest,ConfigManager config)
        {
            // Get the log file.
            string logFile = null;
            if (!string.IsNullOrEmpty(config.GetValue("logFile")))
            {
                logFile = config.GetValue("logFile");
            }
            
            // Get the rest of the configuration values.
            var neuterAccountNumbers = "true".Equals(config.GetValue("neuterAccountNums"));
            var neuterUserCredentials = "true".Equals(config.GetValue("neuterUserCredentials"));
            var printXml = "true".Equals(config.GetValue("printxml"));
            
            // Get the request.
            var request = this.CreateWebRequest(xmlRequest,config);

            // Submit the request.
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(xmlRequest);
            }

            // Read the response.
            string xmlResponse = null;
            try
            {
                var resp = request.GetResponse();
                
                // Read the response.
                using (var reader = new StreamReader(resp.GetResponseStream()))
                {
                    xmlResponse = reader.ReadToEnd().Trim();
                }
                if (printXml)
                {
                    Console.WriteLine(xmlResponse);
                }
                
                // Invoke the event.
                this.OnHttpAction(Communications.RequestType.Response, xmlResponse,neuterAccountNumbers,neuterUserCredentials);

                // Log the response.
                if (logFile != null)
                {
                    this.Log(xmlResponse,logFile,neuterAccountNumbers,neuterUserCredentials);
                }
            } catch (WebException webException)
            {
                
            }
            
            // Return the XML response.
            return xmlResponse;
        }
        
        /*
         * Sends an async HTTP Post request.
         */
        [Obsolete("Deprecated in favor of Communications.HttpPost(string xmlRequest,ConfigManager config)")]
        public virtual string HttpPost(string xmlRequest,Dictionary<string, string> config)
        {
            return this.HttpPost(xmlRequest,new ConfigManager(config));
        }
        
        /*
         * Connects an FTP client.
         */
        public SftpClient FtpConnect(ConfigManager config)
        {
            // Get the configuration
            var url = config.GetValue("sftpUrl");
            var username = config.GetValue("sftpUsername");
            var password = config.GetValue("sftpPassword");
            
            // Connect the client.
            var sftpClient = new SftpClient(url, username, password);
            try
            {
                sftpClient.Connect();
            }
            catch (SshConnectionException e)
            {
                throw new CnpOnlineException("Error occured while establishing an SFTP connection", e);
            }
            catch (SshAuthenticationException e)
            {
                throw new CnpOnlineException("Error occured while attempting to establish an SFTP connection", e);
            }
            
            // Return the connection.
            return sftpClient;
        }

        /*
         * Copies a file over FTP.
         */
        public virtual void FtpDropOff(string fileDirectory,string fileName,ConfigManager config)
        {
            // Get the configuration.
            var url = config.GetValue("sftpUrl");
            var username = config.GetValue("sftpUsername");
            var password = config.GetValue("sftpPassword");
            var filePath = Path.Combine(fileDirectory, fileName);
            var printXml = config.GetValue("printxml") == "true";
            
            // Print the debug information.
            if (printXml)
            {
                Console.WriteLine("Sftp Url: " + url);
                Console.WriteLine("Username: " + username);
            }

            // Connect the SFTP client.
            var sftpClient = this.FtpConnect(config);

            // Upload the file and disconnect the client.
            try {
                if (printXml) {
                    Console.WriteLine("Dropping off local file " + filePath + " to inbound/" + fileName + ".prg");
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                sftpClient.UploadFile(fileStream, "inbound/" + fileName + ".prg");
                fileStream.Close();
                if (printXml) {
                    Console.WriteLine("File copied - renaming from inbound/" + fileName + ".prg to inbound/" +
                                      fileName + ".asc");
                }

                sftpClient.RenameFile("inbound/" + fileName + ".prg", "inbound/" + fileName + ".asc");
            }
            catch (SshConnectionException e) {
                throw new CnpOnlineException("Error occured while attempting to upload and save the file to SFTP", e);
            }
            catch (SshException e) {
                throw new CnpOnlineException("Error occured while attempting to upload and save the file to SFTP", e);
            }
            finally {
                sftpClient.Disconnect();
            }
        }
        
        /*
         * Copies a file over FTP.
         */
        [Obsolete("Deprecated in favor of Communications.FtpDropOff(string fileDirectory,string fileName,ConfigManager config)")]
        public virtual void FtpDropOff(string fileDirectory, string fileName, Dictionary<string, string> config)
        {
            this.FtpDropOff(fileDirectory,fileName,new ConfigManager(config));
        }
        
        /*
         * Waits for an FTP file to be created.
         */
        public virtual void FtpPoll(string fileName,int timeout,ConfigManager config)
        {
            // Get the configuration
            var printXml = config.GetValue("printxml") == "true";
            fileName += ".asc";
            
            // Print the debug information.
            if (printXml)
            {
                Console.WriteLine("Polling for outbound result file.  Timeout set to " + timeout + "ms. File to wait for is " + fileName);
            }
            
            // Connect the client.
            var sftpClient = this.FtpConnect(config);

            // Wait for the file to appear or the timeout to be reached.
            SftpFileAttributes sftpAttrs = null;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            do
            {
                // Print the debug information.
                if (printXml)
                {
                    Console.WriteLine("Elapsed time is " + stopWatch.Elapsed.TotalMilliseconds);
                }
                
                // Try to get the file attributes.
                try
                {
                    sftpAttrs = sftpClient.Get(Path.Combine("outbound",fileName)).Attributes;
                    if (printXml)
                    {
                        Console.WriteLine("Attrs of file are: " + GetSftpFileAttributes(sftpAttrs));
                    }
                }
                catch (SshConnectionException e)
                {
                    if (printXml)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Thread.Sleep(30000);
                }
                catch (SftpPathNotFoundException e)
                {
                    if (printXml)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Thread.Sleep(30000);
                }
            } while (sftpAttrs == null && stopWatch.Elapsed.TotalMilliseconds <= timeout);
            
            // Close the connections.
            sftpClient.Disconnect();
        }

        /*
         * Waits for an FTP file to be created.
         */
        [Obsolete("Deprecated in favor of Communications.FtpDropOff(string fileName,int timeout,ConfigManager config)")]
        public virtual void FtpPoll(string fileName,int timeout,Dictionary<string, string> config)
        {
            this.FtpPoll(fileName,timeout,new ConfigManager(config));
        }
        
        /*
         * Copes an FTP file to the local file system.
         */
        public virtual void FtpPickUp(string destinationFilePath,ConfigManager config,string fileName)
        {
            // Get the configuration
            var printXml = config.GetValue("printxml") == "true";

            // Connect the client.
            var sftpClient = this.FtpConnect(config);

            // Print the debug information.
            if (printXml) {
                Console.WriteLine("Picking up remote file outbound/" + fileName + ".asc");
                Console.WriteLine("Putting it at " + destinationFilePath);
            }
            
            try {
                // Download the file.
                var downloadStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.ReadWrite);
                sftpClient.DownloadFile(Path.Combine("outbound/",fileName + ".asc"), downloadStream);
                downloadStream.Close();
                if (printXml) {
                    Console.WriteLine("Removing remote file output/" + fileName + ".asc");
                }

                sftpClient.Delete("outbound/" + fileName + ".asc");
            }
            catch (SshConnectionException e) {
                throw new CnpOnlineException("Error occured while attempting to retrieve and save the file from SFTP",
                    e);
            }
            catch (SftpPathNotFoundException e) {
                throw new CnpOnlineException("Error occured while attempting to locate desired SFTP file path", e);
            }
            finally {
                sftpClient.Disconnect();
            }
        }

        /*
         * Copes an FTP file to the local file system.
         */
        [Obsolete("Deprecated in favor of Communications.FtpPickUp(string destinationFilePath,ConfigManager config,string fileName)")]
        public virtual void FtpPickUp(string destinationFilePath, Dictionary<string,string> config, string fileName)
        {
            this.FtpPickUp(destinationFilePath, new ConfigManager(config),fileName);
        }
        
        /*
         * Returns the SFTP file attributes.
         */
        private string GetSftpFileAttributes(SftpFileAttributes sftpAttrs)
        {
            var permissions = sftpAttrs.GetBytes().ToString();
            return "Permissions: " + permissions
                                   + " | UserID: " + sftpAttrs.UserId 
                                   + " | GroupID: " + sftpAttrs.GroupId 
                                   + " | Size: " + sftpAttrs.Size 
                                   + " | LastEdited: " + sftpAttrs.LastWriteTime.ToString();
        }

        /*
         * Enums for a request type.
         */
        public enum RequestType
        {
            Request, Response
        }

        /*
         * Arguments for an Http action.
         */
        public class HttpActionEventArgs : EventArgs
        {
            public RequestType RequestType { get; set; }
            public string XmlPayload;

            public HttpActionEventArgs(RequestType requestType, string xmlPayload)
            {
                RequestType = requestType;
                XmlPayload = xmlPayload;
            }
        }
    }
}