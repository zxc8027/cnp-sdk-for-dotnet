/*
 * Zachary Cook
 *
 * Wrapper for the batchRequst object.
 * Due to time constraints, the backwards-compatibility wrapping
 * methods were not added. For a final release, they should be added.
 */

using System;
using System.ComponentModel;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    public class CnpBatchRequest
    {
        private ConfigManager config;
        private batchRequest batchRequest;
        
        /*
         * Creates a batch request wrapper.
         */
        public CnpBatchRequest() : this(new ConfigManager())
        {
        }

        /*
         * Creates a batch request wrapper.
         */
        public CnpBatchRequest(ConfigManager config)
        {
            this.config = config;
            this.batchRequest = new batchRequest();
        }
        
        /*
         * Adds a transaction.
         */
        public void AddTransaction(cnpTransactionInterface transaction)
        {
            // Set the report group.
            if (transaction is transactionTypeWithReportGroup && ((transactionTypeWithReportGroup) transaction).reportGroup == null)
            {
                ((transactionTypeWithReportGroup) transaction).reportGroup = this.config.GetValue("reportGroup");
            }
            if (transaction is transactionTypeWithReportGroupAndPartial && ((transactionTypeWithReportGroupAndPartial) transaction).reportGroup == null)
            {
                ((transactionTypeWithReportGroupAndPartial) transaction).reportGroup = this.config.GetValue("reportGroup");
            }
            
            // Add the transaction.
            if (transaction is recurringTransactionType)
            {
                this.batchRequest.recurringTransaction.Add((recurringTransactionType) transaction);
            }
            else if (transaction is transactionType)
            {
                this.batchRequest.transaction.Add((transactionType) transaction);
            }
            
            // Add the sum.
            var type = typeof(batchRequest);
            var numberSumName1 = "num" + transaction.GetType().Name.Substring(0,1).ToUpper() + transaction.GetType().Name.Substring(1);
            var numberSumName2 = numberSumName1 + "s";
            var numberSumMember1 = type.GetProperty(numberSumName1);
            var numberSumMember2 = type.GetProperty(numberSumName2);
            if (numberSumMember1 != null)
            {
                numberSumMember1.SetValue(this.batchRequest,(int) numberSumMember1.GetValue(this.batchRequest) + 1);
            }
            if (numberSumMember2 != null)
            {
                numberSumMember2.SetValue(this.batchRequest,(int) numberSumMember2.GetValue(this.batchRequest) + 1);
            }
            
            // Add the amount.
            var amountSumName = transaction.GetType().Name + "Amount";
            var amountSumMember = type.GetProperty(amountSumName);
            var amountTransactionMember = transaction.GetType().GetProperty("amount");
            if (amountSumMember != null && amountTransactionMember != null && amountTransactionMember.GetValue(transaction) != null)
            {
                var existingAmount = (int) amountSumMember.GetValue(this.batchRequest);
                var amount = (int) amountTransactionMember.GetValue(transaction);
                amountSumMember.SetValue(this.batchRequest,existingAmount  + amount);
            }
        }
        /*
         * Returns the enclosed batch request.
         */
        public batchRequest GetBatchRequest()
        {
            return this.batchRequest;
        }
    }
}