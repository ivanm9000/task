using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivan_Mihalj_Task.MODEL;
using System.IO;
using System.Reflection;

namespace Ivan_Mihalj_Task
{
    class Program
    {
        static void Main(string[] args)
        {

            // Default values

            string pathTags = @"..\..\DOCUMENTS\tags.txt";
            string pathData = @"..\..\DOCUMENTS\data.csv";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--data-file" && (i+1)<args.Length)
                {
                    pathData = args[i + 1];
                }
                if (args[i] == "--tags-file" && (i + 1) < args.Length)
                {
                    pathTags = args[i + 1];
                }
            }
            

            string productName = "MilkaOreo";

            string fullProductName = productName;

            var tagLines = File.ReadLines(pathTags);
            var dataLines = File.ReadLines(pathData);

            if (!(File.Exists(pathData)) || !(File.Exists(pathTags)))
            {
                System.Environment.Exit(1);
            }

            string companyPrefix = "";
            string itemReference = "";
            string serialReference = "";
            int companyPrefixLength , itemReferenceLength , counterFailed = 0;

            string str, partition, binary;

            HexNumber hex = new HexNumber();
            Parser parser = new Parser();
            LinkedList<string> productList = new LinkedList<string>();

            productName = parser.normalizeString(productName);

            foreach (var line in dataLines)
            {
                str = parser.normalizeString(line);
                string[] words = str.Split(';');

                if (productName == words[3])
                {
                    companyPrefix = words[0];
                    itemReference = words[2];
                    break;
                }
            }

            foreach (var line in tagLines)
            {
                if (hex.isHex(line) && line.Length == 24)
                {
                    // Half of 96 bits length, because it is bigger then int64

                    int halfOfBinaryLength = 48;
                    binary = parser.parseToBigBinary(line.Substring(0, 12), line.Substring(12, 12), halfOfBinaryLength, halfOfBinaryLength);

                    if (line.Substring(0, 2) != "30")
                    {
                        // It is hexadecimal and it's length is 24 hexadecimal chars, but it does not begin with "30" so it is not SGTIN-96 code

                       // failedProduct.AddLast(binary);
                        counterFailed++;
                    }
                    else
                    {
                        string companyPrefixBinary = "";
                        string itemReferenceBinary = "";
                        int startFirst = 14;
                        int startSecond = 0;

                        partition = binary.Substring(11, 3);

                        // Get company prefix length based on partition if it does not have code in [0,6] , it is not SGTIN-96
                        companyPrefixLength = parser.getCompanyPrefixLength(partition);

                        if (companyPrefixLength != 0)
                        {
                            itemReferenceLength = 44 - companyPrefixLength;

                            startSecond = startFirst + companyPrefixLength;

                            companyPrefixBinary = parser.parseToBinary(companyPrefix, companyPrefixLength);
                            itemReferenceBinary = parser.parseToBinary(itemReference, itemReferenceLength);

                            if (binary.Substring(startFirst, companyPrefixLength) == companyPrefixBinary && binary.Substring(startSecond, itemReferenceLength) == itemReferenceBinary)
                            {
                                productList.AddLast(binary);
                            }
                        }
                        else
                        {
                            // It is hexadecimal, length 24 hexadecimal chars, begins with "30" but partition code is not in [0,6]

                            //failedProduct.AddLast(line);
                            counterFailed++;
                        }
                    }
                }
                else
                {
                   // failedProduct.AddLast(line);
                    counterFailed++;
                }
            }
            Console.WriteLine("Number of invalid SGTIN-96 EPCs : {0}", counterFailed);
            Console.WriteLine("Number of product {0} : {1}", fullProductName, productList.Count());
            Console.WriteLine("\n---- Serial numbers of all {0} products ----\n", fullProductName);

            foreach (var item in productList)
            {
                serialReference = item.Substring(58, 38);
                Console.WriteLine(serialReference);
            }
            Console.ReadKey();
        }
    }
}
