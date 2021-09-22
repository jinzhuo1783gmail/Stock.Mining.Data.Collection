using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace Stock.Extraction.Library
{
    public interface IExtraction
    {
        string Path { get; set; }
        OriginalSource OriginalSource { get; set; }

        public Task SetSource(string path, OriginalSource originalSource);

        public Task<List<string>> ExtractText(string tableName);

        public Task<Dictionary<string, List<JObject>>> ExtractTable();

    }
}
