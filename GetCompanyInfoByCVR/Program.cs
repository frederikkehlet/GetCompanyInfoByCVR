using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GetCompanyInfoByCVR
{
    class Program
    {
        static readonly string authString = ConfigurationManager.AppSettings.Get("authString");

        static async Task Main(string[] args)
        {
            try
            {
                Console.Clear();
                Console.Write("CVR number: ");
                string cvr = Console.ReadLine();

                Regex regex = new Regex(@"[0-9]{8}");

                if (regex.IsMatch(cvr) && cvr.Length == 8)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string apiUrl = @"http://distribution.virk.dk/cvr-permanent/virksomhed/_search";
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);

                        Query query = new Query(new Term(cvr));
                        List<string> source = new List<string>() { "Vrvirksomhed.virksomhedMetadata" };
                        Request req = new Request(source, query);
                        string content = JsonConvert.SerializeObject(req);

                        HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(content, Encoding.UTF8, "application/json"));
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject<dynamic>(result);

                        if (json.hits.total == 1) 
                            DisplayCompanyInfo(json);
                        else 
                            throw new Exception($"Could not find a company with CVR number {cvr}.");
                    }
                }
                else
                {
                    throw new Exception($"CVR number {cvr} is not a valid CVR number.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public static void DisplayCompanyInfo(dynamic json)
        {
            dynamic root = json.hits.hits[0]._source.Vrvirksomhed.virksomhedMetadata;

            string output =
                $"Navn: {root.nyesteNavn.navn}\n" +
                $"Binavne:\n{ListToString(root.nyesteBinavne)}" +
                $"Virksomhedform: {root.nyesteVirksomhedsform.langBeskrivelse}\n" +
                $"Branche: {root.nyesteHovedbranche.branchetekst}\n" +
                $"Status: {root.sammensatStatus}\n" +
                $"Stiftelsesdato: {root.stiftelsesDato}\n" +
                $"Kontaktoplysninger:\n{ListToString(root.nyesteKontaktoplysninger)}" +
                $"Adresse: \n" +
                $"  Vej: {root.nyesteBeliggenhedsadresse.vejnavn}\n" +
                $"  Nummer: {root.nyesteBeliggenhedsadresse.husnummerFra}\n" +
                $"  Postnummer: {root.nyesteBeliggenhedsadresse.postnummer}\n" +
                $"  Postdistrikt: {root.nyesteBeliggenhedsadresse.postdistrikt}";

            Console.WriteLine(output);
        }

        public static string ListToString(dynamic jsonArray)
        {
            string output = "";
            foreach (var item in jsonArray)
            {
                output += $"  {item}\n";
            }
            return output;
        }
    }
}
