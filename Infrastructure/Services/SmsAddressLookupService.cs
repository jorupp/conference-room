﻿using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Exchange.WebServices.Data;
using RightpointLabs.ConferenceRoom.Domain;
using RightpointLabs.ConferenceRoom.Infrastructure.Services.ExchangeEWS;

namespace RightpointLabs.ConferenceRoom.Infrastructure.Services
{
    /// <summary>
    /// This will look up SMS addresses in Exchange based on the 'mobile phone' phone number, which almost no-one has....
    /// Need that thing from Brandon....
    /// </summary>
    public class SmsAddressLookupService : ISmsAddressLookupService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IExchangeServiceManager _exchangeServiceManager;

        public SmsAddressLookupService(IExchangeServiceManager exchangeServiceManager)
        {
            _exchangeServiceManager = exchangeServiceManager;
        }

        public string[] LookupAddresses(string[] emailAddresses)
        {
            var phoneNumbers = _exchangeServiceManager.Execute("", svc =>
            {
                var re = new Regex("[^0-9]");
                return emailAddresses
                    .Select(i => svc.ResolveName(i, ResolveNameSearchLocation.DirectoryOnly, true))
                    .Select(i => i.SingleOrDefault().ChainIfNotNull(ii => ii.Contact))
                    .Where(i => i != null && i.PhoneNumbers.Contains(PhoneNumberKey.MobilePhone))
                    .Select(i => i.PhoneNumbers[PhoneNumberKey.MobilePhone])
                    .Where(i => i != null)
                    .Select(i => re.Replace(i, ""))
                    .Select(i => i.Length == 9 ? "1" + i : i)
                    .ToArray();
            });

            log.DebugFormat("Looked up {0} into {1}", string.Join(", ", emailAddresses), string.Join(", ", phoneNumbers));

            return phoneNumbers;
        }
    }
}