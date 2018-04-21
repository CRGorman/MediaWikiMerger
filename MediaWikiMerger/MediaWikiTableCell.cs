using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaWikiMerger
{
    class MediaWikiTableCell
    {
        internal string Raw { get; set; }
        internal string WithoutFormatting
        {
            get
            {
                return Raw.Trim('\'').Replace("[", "").Replace("]", "").Replace("<span style=\"display: none\">", "").Replace("<span style=\"display:none\">", "").Replace("data-sort-value=\"", "");
            }
        }
    }
}
