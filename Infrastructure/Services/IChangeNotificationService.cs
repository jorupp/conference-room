using System;
using Microsoft.Exchange.WebServices.Data;
using RightpointLabs.ConferenceRoom.Domain.Models;
using RightpointLabs.ConferenceRoom.Domain.Models.Entities;
using RightpointLabs.ConferenceRoom.Infrastructure.Services.ExchangeEWS;

namespace RightpointLabs.ConferenceRoom.Infrastructure.Services
{
    public interface IChangeNotificationService
    {
        void TrackRoom(IRoom room, IExchangeServiceManager exchangeServiceManager, OrganizationEntity organization);
        bool IsTrackedForChanges(IRoom room);
    }
}