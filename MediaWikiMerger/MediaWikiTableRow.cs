using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaWikiMerger
{
    class MediaWikiTableRow
    {
        internal List<MediaWikiTableCell> MediaWikiTableCells { get; set; }
        internal string MediaWikiTableAnchor { get; set; }
        internal MediaWikiTableRow(string Row) { MediaWikiTableCells = ParseRow(Row.Trim()); }
        internal List<MediaWikiTableCell> ParseRow(string Row)
        {
            List<MediaWikiTableCell> parsedRow = new List<MediaWikiTableCell>();
            int currentIndex = 0;
            if (Row.StartsWith("<!--"))
            {
                currentIndex = Row.IndexOf("-->") + 3;
                MediaWikiTableAnchor = Row.Substring(0, currentIndex);
            }
            int nextPipe;
            string chunk;
            bool templateOpen = false;
            while (currentIndex < Row.Length)
            {
                //We need to find the next pipe that isn't part of a template.
                int nextTemplateOpener = Row.IndexOf("{{", currentIndex);
                int nextWikilinkOpener = Row.IndexOf("[[", currentIndex);
                nextPipe = Row.IndexOf('|', currentIndex + 1);

                while (nextTemplateOpener < nextPipe && nextTemplateOpener != -1)
                {
                    int nextTemplateCloser = Row.IndexOf("}}", nextTemplateOpener + 2);
                    nextTemplateOpener = Row.IndexOf("{{", nextTemplateCloser);
                    nextPipe = Row.IndexOf('|', nextTemplateCloser + 1);
                }
                while (nextWikilinkOpener < nextPipe && nextWikilinkOpener != -1)
                {
                    int nextWikilinkCloser = Row.IndexOf("]]", nextWikilinkOpener + 2);
                    nextWikilinkOpener = Row.IndexOf("[[", nextWikilinkCloser);
                    nextPipe = Row.IndexOf('|', nextWikilinkCloser + 1);
                }
                
                if (nextPipe > 0)
                {
                    chunk = Row.Substring(currentIndex, nextPipe - currentIndex);
                    parsedRow.Add(new MediaWikiTableCell() { Raw = chunk.Trim('|').Trim() });
                    currentIndex = nextPipe;
                }
                else
                {
                    chunk = Row.Substring(currentIndex);
                    parsedRow.Add(new MediaWikiTableCell() { Raw = chunk.Trim('|').Trim() });
                    break;
                }
            };

            return parsedRow;
        }
    }
}

