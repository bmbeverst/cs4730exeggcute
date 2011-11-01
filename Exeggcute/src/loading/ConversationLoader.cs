using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Exeggcute.src.text;

namespace Exeggcute.src.loading
{
    class ConversationLoader : LoadedInfo
    {
        public Conversation Load(string filename)
        {
            Data data = new Data(filename);
            DataSection infoSection = data[0];
            currentField = infoSection.Tag;
            if (!matches("info")) throw new ParseError("Info section must come first in {0}", filename);
            ConversationInfo info = new ConversationInfo(filename, infoSection.Tokens);

            DataSection textSection = data[1];
            return new Conversation(info.Font, textSection.GetJoinedLines(), info.Rate.Value);
        }
    }

}
