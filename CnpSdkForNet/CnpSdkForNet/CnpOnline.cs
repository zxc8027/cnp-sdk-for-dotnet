/*
 * Zachary Cook
 *
 * Sends Cnp Online (realtime) requests.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    /*
     * Additional functionality for handling responses.
     */
    public partial class cnpOnlineResponse
    {
        private string onlineResponse;
        private XMLVersion parsedVersion;
        
        /*
         * Returns the response object.
         */
        public T GetOnlineResponse<T>()
        {
            return VersionedXMLDeserializer.Deserialize<T>(this.onlineResponse,this.parsedVersion);
        }
        
        /*
         * Parses elements that aren't defined by properties.
         */
        public override void ParseAdditionalElements(XMLVersion version,List<string> elements)
        {
            this.parsedVersion = version;
            if (elements.Count != 0)
            {
                this.onlineResponse = elements[0];
            }
        }
    }
    
    /*
     * Class for sending CNP online requests.
     */
    public class CnpOnline
    {
        public const string XML_HEADER = "<?xml version='1.0' encoding='utf-8'?>";
        public const string LITLE_NAMESPACE = "http://www.litle.com/schema";
        public const string CNP_NAMESPACE = "http://www.vantivcnp.com/schema";
        
        private Communications communication;
        private ConfigManager config;
        
        public event EventHandler HttpAction
        {
            add { this.communication.HttpAction += value; }
            remove { this.communication.HttpAction -= value; }
        }
        
        /*
         * Creates a CNP Online object.
         */
        public CnpOnline(ConfigManager config)
        {
            this.communication = new Communications();
            this.config = config;
        }
        
        /*
         * Creates a CNP Online object.
         */
        public CnpOnline() : this(new ConfigManager())
        {
            
        }
        
        /*
         * Creates a CNP Online object.
         */
        [Obsolete("Deprecated in favor of CnpOnline(ConfigManager config)")]
        public CnpOnline(Dictionary<string,string> config) : this(new ConfigManager(config))
        {
            
        }
    
        /*
         * Sets the Communications object to use.
         */
        public void SetCommunication(Communications communication)
        {
            this.communication = communication;
        }


        /*
        public Task<authorizationResponse> AuthorizeAsync(authorization auth, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.authorizationResponse, auth, cancellationToken);
        }
*/
        
        private cnpOnlineRequest CreateRequest(transactionType transactionObject)
        {
            // Create the request.
            var request = CreateCnpOnlineRequest();

            // Add the report group.
            if (transactionObject is transactionTypeWithReportGroup)
            {
                ((transactionTypeWithReportGroup) transactionObject).reportGroup = this.config.GetValue("reportGroup");
            }
            
            // Add the element.
            request.AddAdditionalElement(transactionObject.Serialize(this.config.GetVersion()));

            // Return the request.
            return request;
        }
/*
        public authorizationResponse Authorize(authorization auth)
        {
            return SendRequest(response => response.authorizationResponse, auth);
        }

        public authReversalResponse AuthReversal(authReversal reversal)
        {
            return SendRequest(response => response.authReversalResponse, reversal);
        }

        public Task<authReversalResponse> AuthReversalAsync(authReversal reversal, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.authReversalResponse, reversal, cancellationToken);
        }

        public giftCardAuthReversalResponse GiftCardAuthReversal(giftCardAuthReversal giftCard)
        {
            return SendRequest(response => response.giftCardAuthReversalResponse, giftCard);
        }

        public Task<giftCardAuthReversalResponse> GiftCardAuthReversalAsync(giftCardAuthReversal giftCard, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.giftCardAuthReversalResponse, giftCard, cancellationToken);
        }

        public Task<captureResponse> CaptureAsync(capture capture, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.captureResponse, capture, cancellationToken);
        }

        public captureResponse Capture(capture capture)
        {
            return SendRequest(response => response.captureResponse, capture);
        }

        public giftCardCaptureResponse GiftCardCapture(giftCardCapture giftCardCapture)
        {
            return SendRequest(response => response.giftCardCaptureResponse, giftCardCapture);
        }

        public Task<giftCardCaptureResponse> GiftCardCaptureAsync(giftCardCapture giftCardCapture, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.giftCardCaptureResponse, giftCardCapture, cancellationToken);
        }

        public Task<captureGivenAuthResponse> CaptureGivenAuthAsync(captureGivenAuth captureGivenAuth, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.captureGivenAuthResponse, captureGivenAuth, cancellationToken);
        }

        public captureGivenAuthResponse CaptureGivenAuth(captureGivenAuth captureGivenAuth)
        {
            return SendRequest(response => response.captureGivenAuthResponse, captureGivenAuth);
        }

        public creditResponse Credit(credit credit)
        {
            return SendRequest(response => response.creditResponse, credit);
        }

        public Task<creditResponse> CreditAsync(credit credit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.creditResponse, credit, cancellationToken);
        }

        public Task<vendorDebitResponse> VendorDebitAsync(vendorDebit vendorDebit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.vendorDebitResponse, vendorDebit, cancellationToken);
        }

        public giftCardCreditResponse GiftCardCredit(giftCardCredit giftCardCredit)
        {
            return SendRequest(response => response.giftCardCreditResponse, giftCardCredit);
        }

        public Task<giftCardCreditResponse> GiftCardCreditAsync(giftCardCredit giftCardCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.giftCardCreditResponse, giftCardCredit, cancellationToken);
        }

        public Task<echeckCreditResponse> EcheckCreditAsync(echeckCredit echeckCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.echeckCreditResponse, echeckCredit, cancellationToken);
        }

        public echeckCreditResponse EcheckCredit(echeckCredit echeckCredit)
        {
            return SendRequest(response => response.echeckCreditResponse, echeckCredit);
        }

        public Task<echeckRedepositResponse> EcheckRedepositAsync(echeckRedeposit echeckRedeposit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.echeckRedepositResponse, echeckRedeposit, cancellationToken);
        }

        public echeckRedepositResponse EcheckRedeposit(echeckRedeposit echeckRedeposit)
        {
            return SendRequest(response => response.echeckRedepositResponse, echeckRedeposit);
        }

        public Task<echeckSalesResponse> EcheckSaleAsync(echeckSale echeckSale, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.echeckSalesResponse, echeckSale, cancellationToken);
        }

        public echeckSalesResponse EcheckSale(echeckSale echeckSale)
        {
            return SendRequest(response => response.echeckSalesResponse, echeckSale);
        }

        public echeckVerificationResponse EcheckVerification(echeckVerification echeckVerification)
        {
            return SendRequest(response => response.echeckVerificationResponse, echeckVerification);
        }

        public Task<echeckVerificationResponse> EcheckVerificationAsync(echeckVerification echeckVerification, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.echeckVerificationResponse, echeckVerification, cancellationToken);
        }

        public forceCaptureResponse ForceCapture(forceCapture forceCapture)
        {
            return SendRequest(response => response.forceCaptureResponse, forceCapture);
        }

        public Task<forceCaptureResponse> ForceCaptureAsync(forceCapture forceCapture, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.forceCaptureResponse, forceCapture, cancellationToken);
        }

        public saleResponse Sale(sale sale)
        {
            return SendRequest(response => response.saleResponse, sale);
        }

        public Task<saleResponse> SaleAsync(sale sale, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.saleResponse, sale, cancellationToken);
        }

        public Task<registerTokenResponse> RegisterTokenAsync(registerTokenRequestType tokenRequest, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.registerTokenResponse, tokenRequest, cancellationToken);
        }

        public registerTokenResponse RegisterToken(registerTokenRequestType tokenRequest)
        {
            return SendRequest(response => response.registerTokenResponse, tokenRequest);
        }

        public voidResponse DoVoid(voidTxn v)
        {
            return SendRequest(response => response.voidResponse, v);
        }

        public Task<voidResponse> DoVoidAsync(voidTxn v, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.voidResponse, v, cancellationToken);
        }

        public echeckVoidResponse EcheckVoid(echeckVoid v)
        {
            return SendRequest(response => response.echeckVoidResponse, v);
        }

        public Task<echeckVoidResponse> EcheckVoidAsync(echeckVoid v, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.echeckVoidResponse, v, cancellationToken);
        }

        public updateCardValidationNumOnTokenResponse UpdateCardValidationNumOnToken(updateCardValidationNumOnToken updateCardValidationNumOnToken)
        {
            return SendRequest(response => response.updateCardValidationNumOnTokenResponse, updateCardValidationNumOnToken);
        }

        public Task<updateCardValidationNumOnTokenResponse> UpdateCardValidationNumOnTokenAsync(updateCardValidationNumOnToken update, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.updateCardValidationNumOnTokenResponse, update, cancellationToken);
        }

        public cancelSubscriptionResponse CancelSubscription(cancelSubscription cancelSubscription)
        {
            return SendRequest(response => response.cancelSubscriptionResponse, cancelSubscription);
        }

        public updateSubscriptionResponse UpdateSubscription(updateSubscription updateSubscription)
        {
            return SendRequest(response => response.updateSubscriptionResponse, updateSubscription);
        }

        public activateResponse Activate(activate activate)
        {
            return SendRequest(response => response.activateResponse, activate);
        }

        public deactivateResponse Deactivate(deactivate deactivate)
        {
            return SendRequest(response => response.deactivateResponse, deactivate);
        }

        public loadResponse Load(load load)
        {
            return SendRequest(response => response.loadResponse, load);
        }

        public unloadResponse Unload(unload unload)
        {
            return SendRequest(response => response.unloadResponse, unload);
        }

        public balanceInquiryResponse BalanceInquiry(balanceInquiry balanceInquiry)
        {
            return SendRequest(response => response.balanceInquiryResponse, balanceInquiry);
        }

        public Task<balanceInquiryResponse> BalanceInquiryAsync(balanceInquiry balanceInquiry, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.balanceInquiryResponse, balanceInquiry, cancellationToken);
        }

        public createPlanResponse CreatePlan(createPlan createPlan)
        {
            return SendRequest(response => response.createPlanResponse, createPlan);
        }

        public updatePlanResponse UpdatePlan(updatePlan updatePlan)
        {
            return SendRequest(response => response.updatePlanResponse, updatePlan);
        }

        public refundReversalResponse RefundReversal(refundReversal refundReversal)
        {
            return SendRequest(response => response.refundReversalResponse, refundReversal);
        }

        public depositReversalResponse DepositReversal(depositReversal depositReversal)
        {
            return SendRequest(response => response.depositReversalResponse, depositReversal);
        }

        public activateReversalResponse ActivateReversal(activateReversal activateReversal)
        {
            return SendRequest(response => response.activateReversalResponse, activateReversal);
        }

        public deactivateReversalResponse DeactivateReversal(deactivateReversal deactivateReversal)
        {
            return SendRequest(response => response.deactivateReversalResponse, deactivateReversal);
        }

        public loadReversalResponse LoadReversal(loadReversal loadReversal)
        {
            return SendRequest(response => response.loadReversalResponse, loadReversal);
        }

        public unloadReversalResponse UnloadReversal(unloadReversal unloadReversal)
        {
            return SendRequest(response => response.unloadReversalResponse, unloadReversal);
        }

        public Task<transactionTypeWithReportGroup> QueryTransactionAsync(queryTransaction queryTransaction, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => (response.queryTransactionResponse ?? (transactionTypeWithReportGroup)response.queryTransactionUnavailableResponse), queryTransaction, cancellationToken);
        }

        public transactionTypeWithReportGroup QueryTransaction(queryTransaction queryTransaction)
        {
            return SendRequest(response =>(response.queryTransactionResponse ?? (transactionTypeWithReportGroup)response.queryTransactionUnavailableResponse), queryTransaction);
        }

        public fraudCheckResponse FraudCheck(fraudCheck fraudCheck)
        {
            return SendRequest(response => response.fraudCheckResponse, fraudCheck);
        }

        public fastAccessFundingResponse FastAccessFunding(fastAccessFunding fastAccessFunding)
        {
            return SendRequest(response => response.fastAccessFundingResponse, fastAccessFunding);
        }
        
        public payFacCreditResponse PayFacCredit(payFacCredit payFacCredit)
        {
            return SendRequest(response => response.payFacCreditResponse, payFacCredit);
        }

        public Task<payFacCreditResponse> PayFacCreditAsync(payFacCredit payFacCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.payFacCreditResponse, payFacCredit, cancellationToken);
        }

        public payFacDebitResponse PayFacDebit(payFacDebit payFacDebit)
        {
            return SendRequest(response => response.payFacDebitResponse, payFacDebit);
        }

        public Task<payFacDebitResponse> PayFacDebitAsync(payFacDebit payFacDebit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.payFacDebitResponse, payFacDebit, cancellationToken);
        }

        public physicalCheckCreditResponse PhysicalCheckCredit(physicalCheckCredit physicalCheckCredit)
        {
            return SendRequest(response => response.physicalCheckCreditResponse, physicalCheckCredit);
        }

        public Task<physicalCheckCreditResponse> PhysicalCheckCreditAsync(physicalCheckCredit physicalCheckCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.physicalCheckCreditResponse, physicalCheckCredit, cancellationToken);
        }

        public physicalCheckDebitResponse PhysicalCheckDebit(physicalCheckDebit physicalCheckDebit)
        {
            return SendRequest(response => response.physicalCheckDebitResponse, physicalCheckDebit);
        }

        public Task<physicalCheckDebitResponse> PhysicalCheckDebitAsync(physicalCheckDebit physicalCheckDebit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.physicalCheckDebitResponse, physicalCheckDebit, cancellationToken);
        }

        public reserveCreditResponse ReserveCredit(reserveCredit reserveCredit)
        {
            return SendRequest(response => response.reserveCreditResponse, reserveCredit);
        }

        public Task<reserveCreditResponse> ReserveCreditAsync(reserveCredit reserveCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.reserveCreditResponse, reserveCredit, cancellationToken);
        }

        public reserveDebitResponse ReserveDebit(reserveDebit reserveDebit)
        {
            return SendRequest(response => response.reserveDebitResponse, reserveDebit);
        }

        public Task<reserveDebitResponse> ReserveDebitAsync(reserveDebit reserveDebit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.reserveDebitResponse, reserveDebit, cancellationToken);
        }

        public submerchantCreditResponse SubmerchantCredit(submerchantCredit submerchantCredit)
        {
            return SendRequest(response => response.submerchantCreditResponse, submerchantCredit);
        }

        public Task<submerchantCreditResponse> SubmerchantCreditAsync(submerchantCredit submerchantCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.submerchantCreditResponse, submerchantCredit, cancellationToken);
        }

        public submerchantDebitResponse SubmerchantDebit(submerchantDebit submerchantDebit)
        {
            return SendRequest(response => response.submerchantDebitResponse, submerchantDebit);
        }

        public Task<submerchantDebitResponse> SubmerchantDebitAsync(submerchantDebit submerchantDebit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.submerchantDebitResponse, submerchantDebit, cancellationToken);
        }

        public vendorCreditResponse VendorCredit(vendorCredit vendorCredit)
        {
            return SendRequest(response => response.vendorCreditResponse, vendorCredit);
        }

        public Task<vendorCreditResponse> VendorCreditAsync(vendorCredit vendorCredit, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.vendorCreditResponse, vendorCredit, cancellationToken);
        }

        public translateToLowValueTokenResponse TranslateToLowValueTokenRequest(translateToLowValueTokenRequest translateToLowValueTokenRequest)
        {
            return SendRequest(response => response.translateToLowValueTokenResponse, translateToLowValueTokenRequest);
        }

        public Task<translateToLowValueTokenResponse> TranslateToLowValueTokenRequestAsync(translateToLowValueTokenRequest translateToLowValueTokenRequest, CancellationToken cancellationToken)
        {
            return SendRequestAsync(response => response.translateToLowValueTokenResponse, translateToLowValueTokenRequest, cancellationToken);
        }
*/

        /*
         * Sends a VendorDebit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<vendorDebitResponse>(transactionRequest transaction)")]
        public vendorDebitResponse VendorDebit(vendorDebit vendorDebit)
        {
            return this.SendTransaction<vendorDebitResponse>(vendorDebit);
        }
        
        /*
         * Sends a VendorDebit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<vendorDebitResponse>(transactionRequest transaction,CancellationToken cancellationToken)")]
        public Task<vendorDebitResponse> VendorDebitAsync(vendorDebit vendorDebit,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<vendorDebitResponse>(vendorDebit,cancellationToken);
        }

        /*
         * Sends a transaction request.
         */
        public T SendTransaction<T>(transactionType transaction)
        {
            // Create the CNP Online request.
            var request = CreateRequest(transaction);
            
            // Send the request and return the response.
            var cnpResponse = this.SendToCnp(request);
            return cnpResponse.GetOnlineResponse<T>();
        }
        
        /*
         * Sends a transaction request asynchronously.
         */
        public async Task<T> SendTransactionAsync<T>(transactionType transaction, CancellationToken cancellationToken)
        {
            // Create the CNP Online request.
            var request = CreateRequest(transaction);

            // Send the request and return the response.
            var cnpResponse = await this.SendToCnpAsync(request,cancellationToken).ConfigureAwait(false);
            return cnpResponse.GetOnlineResponse<T>();
        }
        
        /*
         * Creates a CNP Online request.
         */
        private cnpOnlineRequest CreateCnpOnlineRequest()
        {
            // Create the request.
            var request = new cnpOnlineRequest();
            request.merchantId = this.config.GetValue("merchantId");
            request.version = this.config.GetVersion().ToString();
            request.merchantSdk = CnpVersion.CurrentCNPSDKVersion;
            
            // Add the schema.
            if (this.config.GetVersion() < new XMLVersion(12,0))
            {
                request.SetAdditionalAttribute("xmlns",LITLE_NAMESPACE);
            }
            else
            {
                request.SetAdditionalAttribute("xmlns",CNP_NAMESPACE);
            }
            
            // Create and add the authentication.
            var authentication = new authentication();
            authentication.password = this.config.GetValue("password");
            authentication.user = this.config.GetValue("username");
            request.authentication = authentication;
            
            // Return the request.
            return request;
        }
        
        /*
         * Parses a CNP response.
         */
        private cnpOnlineResponse DeserializeResponse(string xmlResponse)
        {
            try
            {
                // Deserialize the response and output the response. 
                var cnpOnlineResponse = VersionedXMLDeserializer.Deserialize<cnpOnlineResponse>(xmlResponse,this.config.GetVersion());
                if (Convert.ToBoolean(this.config.GetValue("printxml")))
                {
                    
                    Console.WriteLine(cnpOnlineResponse.response);
                    
                }
                
                // If the response isn't "0" (success), throw an exception if an error is valid.
                if (!"0".Equals(cnpOnlineResponse.response))
                {
                    if ("2".Equals(cnpOnlineResponse.response) || "3".Equals(cnpOnlineResponse.response))
                    {
                        throw new CnpInvalidCredentialException(cnpOnlineResponse.message);
                    }
                    else if ("4".Equals(cnpOnlineResponse.response))
                    {
                        throw new CnpConnectionLimitExceededException(cnpOnlineResponse.message);
                    }
                    else if ("5".Equals(cnpOnlineResponse.response))
                    {
                        throw new CnpObjectionableContentException(cnpOnlineResponse.message);
                    }
                    else
                    {
                        throw new CnpOnlineException(cnpOnlineResponse.message);
                    }
                }
                return cnpOnlineResponse;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw new CnpOnlineException("Error validating xml data against the schema", invalidOperationException);
            }
        }

        /*
         * Sends a CNP request.
         */
        private cnpOnlineResponse SendToCnp(cnpOnlineRequest request)
        {
            var xmlRequest = XML_HEADER + request.Serialize(this.config.GetVersion());
            var xmlResponse = this.communication.HttpPost(xmlRequest,config);

            return this.DeserializeResponse(xmlResponse);
        }
        
        /*
         * Sends a CNP request asynchronously.
         */
        private async Task<cnpOnlineResponse> SendToCnpAsync(cnpOnlineRequest request,CancellationToken cancellationToken)
        {
            var xmlRequest = request.Serialize(this.config.GetVersion());
            var xmlResponse = await this.communication.HttpPostAsync(xmlRequest, this.config, cancellationToken).ConfigureAwait(false);
            
            return this.DeserializeResponse(xmlResponse);
        }
    }
}