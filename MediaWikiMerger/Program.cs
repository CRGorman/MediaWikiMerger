using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaWikiMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> arguments = args.ToList();
            string Output = arguments.Last();
            arguments.Remove(Output);
            
            List<MediaWikiTable> tables = new List<MediaWikiTable>();
            List<String> tableFileList = new List<string>(arguments);

            tableFileList.ForEach(x =>
            { tables.Add(ParseTable(x)); }
            );

            OutputTable(MergeTables(tables), Output);
        }

        static MediaWikiTable ParseTable(string fileName)
        {
            MediaWikiTable mediaWikiTable = new MediaWikiTable();

            TextReader textReader = new StreamReader(fileName);
            string currentLine;
            string currentRow = "";
            while ((currentLine = textReader.ReadLine()) != null)
            {
                if (currentLine.Trim() == "|-")
                {
                    if (currentRow != "") { mediaWikiTable.MediaWikiTableRows.Add(new MediaWikiTableRow(currentRow)); }
                    currentRow = "";
                }
                else
                {
                    currentRow += currentLine;
                }
            }
            if (currentRow != "") { mediaWikiTable.MediaWikiTableRows.Add(new MediaWikiTableRow(currentRow)); }
            return mediaWikiTable;
        }

        static MediaWikiTable MergeTables(List<MediaWikiTable> mediaWikiTables)
        {
            MediaWikiTable MergedTable = new MediaWikiTable();
            mediaWikiTables.ForEach(x =>
            {
                MergedTable.MediaWikiTableRows.AddRange(x.MediaWikiTableRows);
            });

            MergedTable.MediaWikiTableRows.Sort(SortCells);

            return MergedTable;
        }

        private static int SortCells(MediaWikiTableRow x, MediaWikiTableRow y)
        {
            int returnHolder = x.MediaWikiTableCells.First().WithoutFormatting.CompareTo(y.MediaWikiTableCells.First().WithoutFormatting);
            return returnHolder;
        }

        static void OutputTable(MediaWikiTable outputTable, string outputFile)
        {
            TextWriter textWriter = new StreamWriter(outputFile);
            textWriter.WriteLine("{|");
            outputTable.MediaWikiTableRows.ForEach(x =>
            {
                textWriter.WriteLine("|-");
                if (!String.IsNullOrWhiteSpace(x.MediaWikiTableAnchor))
                {                    
                    textWriter.WriteLine(x.MediaWikiTableAnchor.Trim());
                }
                x.MediaWikiTableCells.ForEach(y =>
                {
                    if (y.Raw.Trim().StartsWith("align=") || y.Raw.Trim().StartsWith("data-sort-value="))
                    {
                        textWriter.Write("| " + y.Raw.Trim() + " ");
                    }
                    else
                    {
                        textWriter.WriteLine("| " + y.Raw.Trim());
                    }
                });
            });
            textWriter.WriteLine("|}");
            textWriter.Close();
        }
    }
}
