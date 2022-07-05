using System;
using System.Collections.Generic;
using Nop.Services.Shipping.Tracking;

namespace NopBrasil.Plugin.Shipping.Correios
{
    public class CorreiosShipmentTracker : IShipmentTracker
    {
        private readonly CorreiosSettings _correiosSettings;

        public CorreiosShipmentTracker(CorreiosSettings correiosSettings)
        {
            this._correiosSettings = correiosSettings;
        }

        public virtual bool IsMatch(string trackingNumber)
        {
            throw new NotImplementedException("");
        }

        public virtual string GetUrl(string trackingNumber)
        {
            string url = "http://www.linkcorreios.com.br/{0}";
            url = string.Format(url, trackingNumber);
            return url;
        }

        public virtual IList<ShipmentStatusEvent> GetShipmentEvents(string trackingNumber)
        {
            return new List<ShipmentStatusEvent>();
        }
    }

}