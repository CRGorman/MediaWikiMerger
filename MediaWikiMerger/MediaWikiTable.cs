using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaWikiMerger
{
    internal class MediaWikiTable
    {
        internal List<MediaWikiTableRow> MediaWikiTableRows { get; set; }

        internal MediaWikiTable() { MediaWikiTableRows = new List<MediaWikiTableRow>(); }
    }
}
