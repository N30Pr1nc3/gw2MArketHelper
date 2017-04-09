using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace gwtrade
{
    class VM
    {
        public List<item> items { get; private set; } = new List<item>();

        public VM()
        {
            this.items.Add(new item(19697));
            this.items.Add(new oreItem(19698, new item(19682), new item(12832),new item(12807),new item(12819),new item(12813),new item(12825)));
            this.items.Add(new item(19700));
            this.items.Add(new item(19701));
            this.items.Add(new item(19702));
            this.items.Add(new item(19703));
            foreach (item item in this.items)
            {
                item.refresh();
            }
        }

        public static string APICall(string call)
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(call);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Display the content.
            return reader.ReadToEnd();
        }
    }

    class item
    {
        public List<offer> buys { get; set; } = new List<offer>();
        public List<offer> sells { get; set; } = new List<offer>();
        public int buy { get { if (this.buys.Count > 0) { return this.buys[0].unit_price; } else { return -1; } } }
        public int sell { get { if (this.sells.Count > 0) { return this.sells[0].unit_price; } else { return -1; } } }
        public int id { get; set; }
        public string name { get; set; }

        public item(int _id)
        {
            this.id = _id;
            this.apicall(string.Concat("https://api.guildwars2.com/v2/items/", this.id, "?lang=de"));
        }
        
        public virtual void refresh()
        {
            this.buys.Clear();
            this.sells.Clear();
            this.apicall(string.Concat("https://api.guildwars2.com/v2/commerce/listings/", this.id));
        }

        private void apicall(string call)
        {
            try
            {
                JsonConvert.PopulateObject(VM.APICall(call), this);
            }
            catch
            {

            }
        }
    }

    class oreItem : item
    {

        public item barren;
        public item filigran;
        public item haken;
        public item kette;
        public item rahmen;
        public item ring;

        public string c1 { get { return string.Concat("(",this.barren.buy,")",Math.Floor(this.barren.buy*0.85)-this.buy*2,"|",this.barren.buys[0].quantity); } }
        public string c2 { get { return string.Concat("(",this.filigran.buy,")",Math.Floor(this.filigran.buy * 0.85) - this.buy *4, "|", this.filigran.buys[0].quantity); } }
        public string c3 { get { return string.Concat("(",this.haken.buy,")",Math.Floor(this.haken.buy * 0.85) - this.buy *4, "|", this.haken.buys[0].quantity); } }
        public string c4 { get { return string.Concat("(",this.kette.buy,")",Math.Floor(this.kette.buy * 0.85) - this.buy *8, "|", this.kette.buys[0].quantity); } }
        public string c5 { get { return string.Concat("(",this.rahmen.buy,")",Math.Floor(this.rahmen.buy * 0.85) - this.buy *4, "|", this.rahmen.buys[0].quantity); } }
        public string c6 { get { return string.Concat("(", this.ring.buy, ")", Math.Floor(this.ring.buy*0.85)-this.buy*6, "|", this.ring.buys[0].quantity); } }
        
        public oreItem(int _id, item barren, item filigran, item haken, item kette, item rahmen, item ring) : base(_id)
        {
            this.barren = barren;
            this.filigran = filigran;
            this.haken = haken;
            this.kette = kette;
            this.rahmen = rahmen;
            this.ring = ring;
        }

        public override void refresh()
        {
            base.refresh();
            this.barren.refresh();
            this.filigran.refresh();
            this.haken.refresh();
            this.kette.refresh();
            this.rahmen.refresh();
            this.ring.refresh();
        }
    }



    class offer
    {
        public int listings { get; set; } = 0;
        public int unit_price { get; set; } = 0;
        public int quantity { get; set; } = 0;
    }

}
