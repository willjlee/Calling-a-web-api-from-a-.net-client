using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//non-default:
using System.Net.Http;
using System.Net.Http.Headers;


namespace apiclient
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://contacttestthing.azurewebsites.net/"); //set base address

            //add an accept header for json (tells server what format to use)
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //add a contact
            var newperson = new Contact() { Name = "newname", Address = "100 street st", City = "city", State="State", Zip=11111, Email="a@b.c", Twitter="twitter"};
            Uri newpersonUri = null;

            HttpResponseMessage response = client.PostAsJsonAsync("api/contacts", newperson).Result;
            if (response.IsSuccessStatusCode)
            {
                newpersonUri = response.Headers.Location;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }


            //get all contacts
            response = client.GetAsync("api/contacts").Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {

                Console.WriteLine("call api/contacts");
                // Parse the response body. Blocking!
                var contacts = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result;
                foreach (var p in contacts)
                {
                    Console.WriteLine("{0}\t{1},{2}", p.Name, p.Address, p.City);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Get a contact by ID
            response = client.GetAsync("api/contacts/1").Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("call api/contacts/1");
                // Parse the response body. Blocking!
                var contact = response.Content.ReadAsAsync<Contact>().Result;
                Console.WriteLine("{0}\t{1},{2}", contact.Name, contact.Address, contact.City);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Delete the contact added in the beginning
            response = client.DeleteAsync(newpersonUri).Result;
            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);


            Console.ReadLine();//hold console open
        }
    }
}
