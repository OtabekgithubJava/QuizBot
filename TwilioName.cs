using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Reflection.Metadata;


namespace Quizbot
{
    public class TwilioName
    {
        public string PhoneNumber { get; set; }
        
        public TwilioName(string phoneNumber)
        {
            this.PhoneNumber = phoneNumber;
        }
        public void GetMethod()
        {
            Console.WriteLine("hello");

            var twilioConfigString = File.ReadAllText("/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/jsconfig1.json");
            var twilioConfig = JsonSerializer.Deserialize<TwilioConfig>(twilioConfigString);
            
            TwilioClient.Init(twilioConfig.AccountSID, twilioConfig.AuthToken);

            var message = MessageResource.Create(
                body: "Your code:\n- 34267",
                from: new Twilio.Types.PhoneNumber("+17163511673"),
                to: new Twilio.Types.PhoneNumber($"{this.PhoneNumber}")
            );
            Console.WriteLine(message.Status);
        }
    }
}