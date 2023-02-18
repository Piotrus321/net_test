using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace net_test
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        public static void Main(string[] args)
        {
            using var dbContext = new AccessDatabaseContext();
            DbSet<TableRecord> tableRecords = dbContext.Table1;
            Console.WriteLine("DB content:");
            foreach (TableRecord record in tableRecords)
            {
                Console.WriteLine(record);
            }

            using var memoryStream = new MemoryStream();
            {
                using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
                using var csvWriter = new CsvHelper.CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(tableRecords);
            }
            {
                memoryStream.Position = 0;
                using var streamReader = new StreamReader(memoryStream);
                string csvContent = streamReader.ReadToEnd();
                Console.WriteLine("\nCsv content:");
                Console.WriteLine(csvContent);
            }
        }
    }
}
