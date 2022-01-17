using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAppLicenseValidation
{
    internal class LicenseKeyFileData
    {
        public string key { get; set; }
    }
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();
        const string PRODUCTID = "a765ff6f-8fca-4929-9835-ba94e708b27b";

        private static async Task<bool> CheckLicenseRequest(string licenseKey)
        {
            try
            {
                Uri url = new Uri($"https://localhost:5001/api/License/{PRODUCTID}/{licenseKey}/verify");

                var streamTask = client.GetStreamAsync(url);
                var result = await JsonSerializer.DeserializeAsync<bool>(await streamTask);
                
                return result;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        
        private static async Task<bool> CheckLicense()
        {
            string rootDir = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location.Replace("\\bin\\Debug", ""));
            if (!Directory.Exists($"{rootDir}\\keys"))
            {
                Directory.CreateDirectory($"{rootDir}\\keys");
                Console.WriteLine("Please input your license!");
                string licenseKey = "";
                while (true)
                {
                    licenseKey = Console.ReadLine();
                    
                    if (await CheckLicenseRequest(licenseKey))
                    {
                        break;
                    }
                    
                    Console.WriteLine("Incorrect license key please specify another!");
                }

                File.WriteAllText($"{rootDir}\\keys\\license.json", "{\"key\": \""+licenseKey+"\"}");
                return true;
            }
            else
            {
                if (File.Exists($"{rootDir}\\keys\\license.json"))
                {
                    string json = System.IO.File.ReadAllLines($"{rootDir}\\keys\\license.json")[0];
                    LicenseKeyFileData licenseKeyFileData = JsonSerializer.Deserialize<LicenseKeyFileData>(json);
                    if (!await CheckLicenseRequest(licenseKeyFileData.key))
                    {
                        Console.WriteLine("Incorrect license installed!");
                        return false;
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("Please input your license!");
                    string licenseKey = "";
                    while (true)
                    {
                        licenseKey = Console.ReadLine();
                    
                        if (await CheckLicenseRequest(licenseKey))
                        {
                            break;
                        }
                    
                        Console.WriteLine("Incorrect license key please specify another!");
                    }

                    File.WriteAllText($"{rootDir}\\keys\\license.json", "{\"key\": \""+licenseKey+"\"}");
                    return true;
                }
            }

            return true;
        }
        
        public async static Task Main(string[] args)
        {
            if (!await CheckLicense())
            {
                return;
            }
            
            Console.WriteLine("Hey paying user!");
            Console.WriteLine("Press enter to close ...");
            Console.ReadLine();
        }
    }
}