using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using static MTRwebapp.Models.CustomModels;

namespace MTRwebapp.Classes
{
    public static class Dynamo
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);

        public static List<DriverLocation> FindDriverLocationBetween(int driverid, string from, string to)
        {
            List<DriverLocation> returnval = new List<DriverLocation>();
            var request = new QueryRequest
            {
                TableName = "driver_location_history",
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "driverid = :v_driverid and createdon between :v_start and :v_end",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":v_driverid", new AttributeValue {
                     N = driverid.ToString()
                 }},
                {":v_start", new AttributeValue {
                     S = from
                 }},
                {":v_end", new AttributeValue {
                     S = to
                 }}
            }
            };

            var response = client.Query(request);

            Console.WriteLine("\nNo. of reads used (by query in FindRepliesPostedWithinTimePeriod) {0}",
                      response.ConsumedCapacity.CapacityUnits);
            foreach (Dictionary<string, AttributeValue> item
                 in response.Items)
            {
                returnval.Add(new DriverLocation { lat = item["location"].M["lat"].S, lng = item["location"].M["lng"].S, createdon = item["createdon"].S });
                Console.Write(item);
            }
            return returnval;

        }

        private static void PrintDocument(Document document)
        {
            //   count++;
            Console.WriteLine();
            foreach (var attribute in document.GetAttributeNames())
            {
                string stringValue = null;
                var value = document[attribute];
                if (value is Primitive)
                    stringValue = value.AsPrimitive().Value.ToString();
                else if (value is PrimitiveList)
                    stringValue = string.Join(",", (from primitive
                                    in value.AsPrimitiveList().Entries
                                                    select primitive.Value).ToArray());
                Console.WriteLine("{0} - {1}", attribute, stringValue);
            }
        }


    }
}