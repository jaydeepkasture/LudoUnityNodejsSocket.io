using System;
using System.Collections.Generic;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /* initialize an array */
                Dictionary<string, string> paytmParams = new Dictionary<string, string>();

                /* add parameters in Array */
                paytmParams.Add("MID", "YOUR_MID_HERE");
                paytmParams.Add("ORDER_ID", "YOUR_ORDER_ID_HERE");

                /**
                * Generate checksum by parameters we have
                * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
                */
                String paytmChecksum = Paytm.Checksum.generateSignature(paytmParams, "YOUR_MERCHANT_KEY");
                bool verifySignature = Paytm.Checksum.verifySignature(paytmParams, "YOUR_MERCHANT_KEY", paytmChecksum);

                Console.WriteLine("generateSignature Returns: " + paytmChecksum);
                Console.WriteLine("verifySignature Returns: " + verifySignature + Environment.NewLine);


                /* initialize JSON String */
                string body = "{\"mid\":\"YOUR_MID_HERE\",\"orderId\":\"YOUR_ORDER_ID_HERE\"}";

                /**
                * Generate checksum by parameters we have in body
                * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
                */

                paytmChecksum = Paytm.Checksum.generateSignature(body, "YOUR_MERCHANT_KEY");
                verifySignature = Paytm.Checksum.verifySignature(body, "YOUR_MERCHANT_KEY", paytmChecksum);

                Console.WriteLine("generateSignature Returns: " + paytmChecksum);
                Console.WriteLine("verifySignature Returns: " + verifySignature + Environment.NewLine);
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.StackTrace);
            }
        }
    }
}
