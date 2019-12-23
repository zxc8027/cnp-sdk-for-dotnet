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
         * Creates a request object to send.
         */
        private cnpOnlineRequest CreateRequest(cnpTransactionInterface transactionObject)
        {
            // Create the request.
            var request = CreateCnpOnlineRequest();

            // Add the report group.
            if (transactionObject is transactionTypeWithReportGroup && ((transactionTypeWithReportGroup) transactionObject).reportGroup == null)
            {
                ((transactionTypeWithReportGroup) transactionObject).reportGroup = this.config.GetValue("reportGroup");
            }
            
            // Add the element.
            request.AddAdditionalElement(transactionObject.Serialize(this.config.GetVersion()));

            // Return the request.
            return request;
        }
        
        /*
         * Sends a Authorize request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<authorizationResponse>(cnpTransactionInterface transaction)")]
        public authorizationResponse Authorize(authorization transaction)
        {
            return this.SendTransaction<authorizationResponse>(transaction);
        }
        
        /*
         * Sends a Authorize request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<authorizationResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<authorizationResponse> AuthorizeAsync(authorization transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<authorizationResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a AuthReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<authReversalResponse>(cnpTransactionInterface transaction)")]
        public authReversalResponse AuthReversal(authReversal transaction)
        {
            return this.SendTransaction<authReversalResponse>(transaction);
        }
        
        /*
         * Sends a AuthReversal request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<authReversalResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<authReversalResponse> AuthReversalAsync(authReversal transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<authReversalResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a GiftCardAuthReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<giftCardAuthReversalResponse>(cnpTransactionInterface transaction)")]
        public giftCardAuthReversalResponse GiftCardAuthReversal(giftCardAuthReversal transaction)
        {
            return this.SendTransaction<giftCardAuthReversalResponse>(transaction);
        }
        
        /*
         * Sends a GiftCardAuthReversal request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<giftCardAuthReversalResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<giftCardAuthReversalResponse> GiftCardAuthReversalAsync(giftCardAuthReversal transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<giftCardAuthReversalResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a Capture request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<captureResponse>(cnpTransactionInterface transaction)")]
        public captureResponse Capture(capture transaction)
        {
            return this.SendTransaction<captureResponse>(transaction);
        }
        
        /*
         * Sends a Capture request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<captureResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<captureResponse> CaptureAsync(capture transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<captureResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a GiftCardCapture request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<giftCardCaptureResponse>(cnpTransactionInterface transaction)")]
        public giftCardCaptureResponse GiftCardCapture(giftCardCapture transaction)
        {
            return this.SendTransaction<giftCardCaptureResponse>(transaction);
        }
        
        /*
         * Sends a GiftCardCapture request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<giftCardCaptureResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<giftCardCaptureResponse> GiftCardCaptureAsync(giftCardCapture transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<giftCardCaptureResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a CaptureGivenAuth request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<captureGivenAuthResponse>(cnpTransactionInterface transaction)")]
        public captureGivenAuthResponse CaptureGivenAuth(captureGivenAuth transaction)
        {
            return this.SendTransaction<captureGivenAuthResponse>(transaction);
        }
        
        /*
         * Sends a CaptureGivenAuth request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<captureGivenAuthResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<captureGivenAuthResponse> CaptureGivenAuthAsync(captureGivenAuth transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<captureGivenAuthResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a Credit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<creditResponse>(cnpTransactionInterface transaction)")]
        public creditResponse Credit(credit transaction)
        {
            return this.SendTransaction<creditResponse>(transaction);
        }
        
        /*
         * Sends a Credit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<creditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<creditResponse> CreditAsync(credit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<creditResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a GiftCardCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<giftCardCreditResponse>(cnpTransactionInterface transaction)")]
        public giftCardCreditResponse GiftCardCredit(giftCardCredit transaction)
        {
            return this.SendTransaction<giftCardCreditResponse>(transaction);
        }
        
        /*
         * Sends a GiftCardCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<giftCardCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<giftCardCreditResponse> GiftCardCreditAsync(giftCardCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<giftCardCreditResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a EcheckCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<echeckCreditResponse>(cnpTransactionInterface transaction)")]
        public echeckCreditResponse EcheckCredit(echeckCredit transaction)
        {
            return this.SendTransaction<echeckCreditResponse>(transaction);
        }
        
        /*
         * Sends a EcheckCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<echeckCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<echeckCreditResponse> EcheckCreditAsync(echeckCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<echeckCreditResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a EcheckRedeposit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<echeckRedepositResponse>(cnpTransactionInterface transaction)")]
        public echeckRedepositResponse EcheckRedeposit(echeckRedeposit transaction)
        {
            return this.SendTransaction<echeckRedepositResponse>(transaction);
        }
        
        /*
         * Sends a EcheckRedeposit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<echeckRedepositResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<echeckRedepositResponse> EcheckRedepositAsync(echeckRedeposit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<echeckRedepositResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a EcheckSale request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<echeckSalesResponse>(cnpTransactionInterface transaction)")]
        public echeckSalesResponse EcheckSale(echeckSale transaction)
        {
            return this.SendTransaction<echeckSalesResponse>(transaction);
        }
        
        /*
         * Sends a EcheckSale request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<echeckSalesResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<echeckSalesResponse> EcheckSaleAsync(echeckSale transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<echeckSalesResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a EcheckVerification request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<echeckVerificationResponse>(cnpTransactionInterface transaction)")]
        public echeckVerificationResponse EcheckVerification(echeckVerification transaction)
        {
            return this.SendTransaction<echeckVerificationResponse>(transaction);
        }
        
        /*
         * Sends a EcheckVerification request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<echeckVerificationResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<echeckVerificationResponse> EcheckVerificationAsync(echeckVerification transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<echeckVerificationResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a ForceCapture request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<forceCaptureResponse>(cnpTransactionInterface transaction)")]
        public forceCaptureResponse ForceCapture(forceCapture transaction)
        {
            return this.SendTransaction<forceCaptureResponse>(transaction);
        }
        
        /*
         * Sends a ForceCapture request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<forceCaptureResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<forceCaptureResponse> ForceCaptureAsync(forceCapture transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<forceCaptureResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a Sale request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<saleResponse>(cnpTransactionInterface transaction)")]
        public saleResponse Sale(sale transaction)
        {
            return this.SendTransaction<saleResponse>(transaction);
        }
        
        /*
         * Sends a Sale request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<saleResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<saleResponse> SaleAsync(sale transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<saleResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a RegisterToken request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<registerTokenResponse>(cnpTransactionInterface transaction)")]
        public registerTokenResponse RegisterToken(registerTokenRequestType transaction)
        {
            return this.SendTransaction<registerTokenResponse>(transaction);
        }
        
        /*
         * Sends a RegisterToken request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<registerTokenResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<registerTokenResponse> RegisterTokenAsync(registerTokenRequestType transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<registerTokenResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a Void request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<voidResponse>(cnpTransactionInterface transaction)")]
        public voidResponse DoVoid(voidTxn transaction)
        {
            return this.SendTransaction<voidResponse>(transaction);
        }
        
        /*
         * Sends a Void request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<voidResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<voidResponse> DoVoidAsync(voidTxn transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<voidResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a EcheckVoid request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<echeckVoidResponse>(cnpTransactionInterface transaction)")]
        public echeckVoidResponse EcheckVoid(echeckVoid transaction)
        {
            return this.SendTransaction<echeckVoidResponse>(transaction);
        }
        
        /*
         * Sends a EcheckVoid request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<echeckVoidResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<echeckVoidResponse> EcheckVoidAsync(echeckVoid transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<echeckVoidResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a UpdateCardValidationNumOnToken request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<updateCardValidationNumOnTokenResponse>(cnpTransactionInterface transaction)")]
        public updateCardValidationNumOnTokenResponse UpdateCardValidationNumOnToken(updateCardValidationNumOnToken transaction)
        {
            return this.SendTransaction<updateCardValidationNumOnTokenResponse>(transaction);
        }
        
        /*
         * Sends a UpdateCardValidationNumOnToken request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<updateCardValidationNumOnTokenResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<updateCardValidationNumOnTokenResponse> UpdateCardValidationNumOnTokenAsync(updateCardValidationNumOnToken transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<updateCardValidationNumOnTokenResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a CancelSubscription request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<cancelSubscriptionResponse>(cnpTransactionInterface transaction)")]
        public cancelSubscriptionResponse CancelSubscription(cancelSubscription transaction)
        {
            return this.SendTransaction<cancelSubscriptionResponse>(transaction);
        }
        
        /*
         * Sends a UpdateSubscription request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<updateCardValidationNumOnTokenResponse>(cnpTransactionInterface transaction)")]
        public updateSubscriptionResponse UpdateSubscription(updateSubscription transaction)
        {
            return this.SendTransaction<updateSubscriptionResponse>(transaction);
        }
        
        /*
         * Sends a Activate request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<activateResponse>(cnpTransactionInterface transaction)")]
        public activateResponse Activate(activate transaction)
        {
            return this.SendTransaction<activateResponse>(transaction);
        }
        
        /*
         * Sends a Deactivate request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<deactivateResponse>(cnpTransactionInterface transaction)")]
        public deactivateResponse Deactivate(deactivate transaction)
        {
            return this.SendTransaction<deactivateResponse>(transaction);
        }
        
        /*
         * Sends a Load request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<loadResponse>(cnpTransactionInterface transaction)")]
        public loadResponse Load(load transaction)
        {
            return this.SendTransaction<loadResponse>(transaction);
        }
        
        /*
         * Sends a Unload request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<unloadResponse>(cnpTransactionInterface transaction)")]
        public unloadResponse Unload(unload transaction)
        {
            return this.SendTransaction<unloadResponse>(transaction);
        }
        
        /*
         * Sends a BalanceInquiry request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<balanceInquiryResponse>(cnpTransactionInterface transaction)")]
        public balanceInquiryResponse BalanceInquiry(balanceInquiry transaction)
        {
            return this.SendTransaction<balanceInquiryResponse>(transaction);
        }
        
        /*
         * Sends a BalanceInquiry request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<balanceInquiryResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<balanceInquiryResponse> BalanceInquiryAsync(updateCardValidationNumOnToken transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<balanceInquiryResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a CreatePlan request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<createPlanResponse>(cnpTransactionInterface transaction)")]
        public createPlanResponse CreatePlan(createPlan transaction)
        {
            return this.SendTransaction<createPlanResponse>(transaction);
        }
        
        /*
         * Sends a UpdatePlan request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<updatePlanResponse>(cnpTransactionInterface transaction)")]
        public updatePlanResponse UpdatePlan(updatePlan transaction)
        {
            return this.SendTransaction<updatePlanResponse>(transaction);
        }
        
        /*
         * Sends a RefundReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<refundReversalResponse>(cnpTransactionInterface transaction)")]
        public refundReversalResponse RefundReversal(refundReversal transaction)
        {
            return this.SendTransaction<refundReversalResponse>(transaction);
        }
        
        /*
         * Sends a DepositReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<depositReversalResponse>(cnpTransactionInterface transaction)")]
        public depositReversalResponse DepositReversal(depositReversal transaction)
        {
            return this.SendTransaction<depositReversalResponse>(transaction);
        }
        
        /*
         * Sends a ActivateReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<activateReversalResponse>(cnpTransactionInterface transaction)")]
        public activateReversalResponse ActivateReversal(activateReversal transaction)
        {
            return this.SendTransaction<activateReversalResponse>(transaction);
        }
        
        /*
         * Sends a DeactivateReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<deactivateReversalResponse>(cnpTransactionInterface transaction)")]
        public deactivateReversalResponse DeactivateReversal(deactivateReversal transaction)
        {
            return this.SendTransaction<deactivateReversalResponse>(transaction);
        }
        
        /*
         * Sends a ActivateReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<loadReversalResponse>(cnpTransactionInterface transaction)")]
        public loadReversalResponse LoadReversal(loadReversal transaction)
        {
            return this.SendTransaction<loadReversalResponse>(transaction);
        }
        
        /*
         * Sends a DeactivateReversal request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<unloadReversalResponse>(cnpTransactionInterface transaction)")]
        public unloadReversalResponse UnloadReversal(unloadReversal transaction)
        {
            return this.SendTransaction<unloadReversalResponse>(transaction);
        }

        /*
         * Sends a QueryTransaction request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<cnpTransactionInterfaceWithReportGroup>(cnpTransactionInterface transaction)")]
        public transactionTypeWithReportGroup QueryTransaction(queryTransaction transaction)
        {
            return this.SendTransaction<transactionTypeWithReportGroup>(transaction);
        }
        
        /*
         * Sends a QueryTransaction request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<cnpTransactionInterfaceWithReportGroup>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<transactionTypeWithReportGroup> QueryTransactionAsync(queryTransaction transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<transactionTypeWithReportGroup>(transaction,cancellationToken);
        }
        
        /*
         * Sends a FraudCheck request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<payFacCreditResponse>(cnpTransactionInterface transaction)")]
        public fraudCheckResponse FraudCheck(fraudCheck transaction)
        {
            return this.SendTransaction<fraudCheckResponse>(transaction);
        }
        
        /*
         * Sends a FastAccessFunding request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<fastAccessFundingResponse>(cnpTransactionInterface transaction)")]
        public fastAccessFundingResponse FastAccessFunding(fastAccessFunding transaction)
        {
            return this.SendTransaction<fastAccessFundingResponse>(transaction);
        }

        /*
         * Sends a PayFacCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<payFacCreditResponse>(cnpTransactionInterface transaction)")]
        public payFacCreditResponse PayFacCredit(payFacCredit transaction)
        {
            return this.SendTransaction<payFacCreditResponse>(transaction);
        }
        
        /*
         * Sends a PayFacCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<payFacCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<payFacCreditResponse> PayFacCreditAsync(payFacCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<payFacCreditResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a PayFacDebit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<payFacDebitResponse>(cnpTransactionInterface transaction)")]
        public payFacDebitResponse PayFacDebit(payFacDebit transaction)
        {
            return this.SendTransaction<payFacDebitResponse>(transaction);
        }
        
        /*
         * Sends a PayFacDebit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<payFacDebitResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<payFacDebitResponse> PayFacDebitAsync(payFacDebit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<payFacDebitResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a PhysicalCheckCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<physicalCheckCreditResponse>(cnpTransactionInterface transaction)")]
        public physicalCheckCreditResponse PhysicalCheckCredit(physicalCheckCredit transaction)
        {
            return this.SendTransaction<physicalCheckCreditResponse>(transaction);
        }
        
        /*
         * Sends a PhysicalCheckCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<physicalCheckCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<physicalCheckCreditResponse> PhysicalCheckCreditAsync(physicalCheckCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<physicalCheckCreditResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a PhysicalCheckDebit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<physicalCheckDebitResponse>(cnpTransactionInterface transaction)")]
        public physicalCheckDebitResponse PhysicalCheckDebit(physicalCheckDebit transaction)
        {
            return this.SendTransaction<physicalCheckDebitResponse>(transaction);
        }
        
        /*
         * Sends a PhysicalCheckDebit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<physicalCheckDebitResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<physicalCheckDebitResponse> PhysicalCheckDebitAsync(physicalCheckDebit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<physicalCheckDebitResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a ReserveCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<reserveCreditResponse>(cnpTransactionInterface transaction)")]
        public reserveCreditResponse ReserveCredit(reserveCredit transaction)
        {
            return this.SendTransaction<reserveCreditResponse>(transaction);
        }
        
        /*
         * Sends a ReserveCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<reserveCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<reserveCreditResponse> ReserveCreditAsync(reserveCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<reserveCreditResponse>(transaction,cancellationToken);
        }

        /*
         * Sends a ReserveDebit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<reserveDebitResponse>(cnpTransactionInterface transaction)")]
        public reserveDebitResponse ReserveDebit(reserveDebit transaction)
        {
            return this.SendTransaction<reserveDebitResponse>(transaction);
        }
        
        /*
         * Sends a ReserveDebit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<reserveDebitResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<reserveDebitResponse> ReserveDebitAsync(reserveDebit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<reserveDebitResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a SubmerchantCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<submerchantCreditResponse>(cnpTransactionInterface transaction)")]
        public submerchantCreditResponse SubmerchantCredit(submerchantCredit transaction)
        {
            return this.SendTransaction<submerchantCreditResponse>(transaction);
        }
        
        /*
         * Sends a SubmerchantCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<submerchantCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<submerchantCreditResponse> SubmerchantCreditAsync(submerchantCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<submerchantCreditResponse>(transaction,cancellationToken);
        }

        /*
         * Sends a SubmerchantDebit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<submerchantDebitResponse>(cnpTransactionInterface transaction)")]
        public submerchantDebitResponse SubmerchantDebit(submerchantDebit transaction)
        {
            return this.SendTransaction<submerchantDebitResponse>(transaction);
        }
        
        /*
         * Sends a SubmerchantDebit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<submerchantDebitResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<submerchantDebitResponse> SubmerchantDebitAsync(submerchantDebit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<submerchantDebitResponse>(transaction,cancellationToken);
        }

        /*
         * Sends a VendorCredit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<vendorCreditResponse>(cnpTransactionInterface transaction)")]
        public vendorCreditResponse VendorCredit(vendorCredit transaction)
        {
            return this.SendTransaction<vendorCreditResponse>(transaction);
        }
        
        /*
         * Sends a VendorCredit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<vendorCreditResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<vendorCreditResponse> VendorCreditAsync(vendorCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<vendorCreditResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a VendorDebit request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<vendorDebitResponse>(cnpTransactionInterface transaction)")]
        public vendorDebitResponse VendorDebit(vendorDebit transaction)
        {
            return this.SendTransaction<vendorDebitResponse>(transaction);
        }
        
        /*
         * Sends a VendorDebit request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<vendorDebitResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<vendorDebitResponse> VendorDebitAsync(vendorDebit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<vendorDebitResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a TranslateToLowValueTokenRequest request.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransaction<translateToLowValueTokenResponse>(cnpTransactionInterface transaction)")]
        public translateToLowValueTokenResponse TranslateToLowValueTokenRequest(vendorCredit transaction)
        {
            return this.SendTransaction<translateToLowValueTokenResponse>(transaction);
        }
        
        /*
         * Sends a TranslateToLowValueTokenRequest request asynchronously.
         */
        [Obsolete("Deprecated in favor of CnpOnline.SendTransactionAsync<translateToLowValueTokenResponse>(cnpTransactionInterface transaction,CancellationToken cancellationToken)")]
        public Task<translateToLowValueTokenResponse> TranslateToLowValueTokenRequestAsync(vendorCredit transaction,CancellationToken cancellationToken)
        {
            return this.SendTransactionAsync<translateToLowValueTokenResponse>(transaction,cancellationToken);
        }
        
        /*
         * Sends a transaction request.
         */
        public T SendTransaction<T>(cnpTransactionInterface transaction)
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
        public async Task<T> SendTransactionAsync<T>(cnpTransactionInterface transaction, CancellationToken cancellationToken)
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