using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Stock.Extraction.Library;


namespace Stock.Information.Extraction.Manager
{
    public class HtmlExtractionManager
    {

        public IExtraction _extraction;
        public HtmlExtractionManager(IExtraction extraction)
        {
            _extraction = extraction;
            _extraction.OriginalSource = OriginalSource.Url;
        }

        public async Task<Dictionary<string, List<JObject>>> ExtractTable(string path)
        {
            _extraction.Path = path;
            return await _extraction.ExtractTable();
        }

    }
}
