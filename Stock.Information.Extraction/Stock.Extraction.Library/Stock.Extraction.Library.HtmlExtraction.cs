using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Stock.Extraction.Library.Converting;

namespace Stock.Extraction.Library
{
    public class HtmlExtraction : IExtraction
    {

        public string Path { get; set; }
        public OriginalSource OriginalSource { get; set; }

        public ILogger<HtmlExtraction> _logger;
        public HtmlExtraction(ILogger<HtmlExtraction> logger)
        {
            _logger = logger;
        }

        public async Task<Dictionary<string, List<JObject>>> ExtractTable()
        {

            var tables = new Dictionary<string, List<JObject>>();
            if (string.IsNullOrEmpty(Path))
            {
                _logger.LogError($"Invalid Path [{Path}]");
                return null;
            }
            
            if (OriginalSource != OriginalSource.Url)
            {
                _logger.LogError($"Invalid Source [{nameof(OriginalSource.Url)}]");
                return null;
            }

            
            var htmlStr = new System.Net.WebClient().DownloadString(Path);
            
     	    var htmlDoc = new HtmlDocument();
     	    htmlDoc.LoadHtml(htmlStr);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes($"//table");

            if (htmlNodes == null)
            {
                _logger.LogError("No table nodes in html");
                return null;
            }

            foreach (var htmlNode in htmlNodes)
            {
                int i = 1;

                var table = HtmlTableConvertor.HtmlNodeToDataTable(htmlNode, out var error);

                if (string.IsNullOrEmpty(error))
                {
                    while (tables.ContainsKey($"table{i}")) i++;

                    tables.Add($"table{i}", table);
                }
                else {
                    _logger.LogError("unable to create table " +　error);
                }

            }
            


            return tables;

        }

        public async Task<List<string>> ExtractText(string name)
        {
            throw new NotImplementedException();
        }

        public async Task SetSource(string path, OriginalSource originalSource)
        {
            Path = path;
            OriginalSource = originalSource;
        }
    }
}
