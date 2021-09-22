using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HtmlAgilityPack;

using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stock.Extraction.Library.Converting
{
    public static class HtmlTableConvertor
    {
        public static List<JObject> HtmlNodeToDataTable(HtmlNode htmlNode, out string error)
        {

            try
            {
                var table = new List<JObject>();

                var headerNodeCollection = htmlNode.SelectNodes("thead/tr/th");
                if  (headerNodeCollection == null)
                     headerNodeCollection = htmlNode.SelectNodes("tr/th");
                if  (headerNodeCollection == null)
                     headerNodeCollection = htmlNode.SelectNodes("th");


                var listOfColName = new List<string>();


                if (headerNodeCollection != null)
                { 
                    foreach (var header in headerNodeCollection)
                    {
                        var colNameStr = string.IsNullOrEmpty(header.InnerText) ? "Unnamed" : header.InnerText;

                        int i = 1;
                        while (listOfColName.Contains(colNameStr))
                        {
                            colNameStr += $"_{i}";
                            i++;
                        }
                        listOfColName.Add(colNameStr);
                    }
                }

                

                var rowrNodeCollection = htmlNode.SelectNodes("tr");
                if  (rowrNodeCollection == null)
                     rowrNodeCollection = htmlNode.SelectNodes("tbody/tr");

                if (rowrNodeCollection == null)
                {
                    error = $"no data found in the table {htmlNode.InnerHtml.Substring(0, 500)}";
                    return null;
                }

                foreach (var rowNode in rowrNodeCollection)
                {
                    var rowCells = rowNode.SelectNodes("td");

                    if (rowCells != null && rowCells.Count > 0)
                    {
                        if (listOfColName.Count < rowCells.Count)
                        {
                            for (int colCount = listOfColName.Count ; colCount <= rowCells.Count; colCount++)
                            {
                                listOfColName.Add($"ColName_{colCount}");
                            }
                        }

                        var row = new JObject();
                        for (int i = 0; i < rowCells.Count; i++)
                        {
                            
                            if (i >= listOfColName.Count)
                            {
                                error = $"table {listOfColName[0] ?? "unknow"} has less columns[{listOfColName.Count}] has row cells[{rowCells.Count}] ";
                                return null;
                            }

                            row.Add(listOfColName[i], new JValue(rowCells[i].InnerText));
                        }

                        table.Add(row);
                    }
                }

                error = "";
                return table;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }

        }

    }
}
