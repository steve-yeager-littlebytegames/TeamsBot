using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;

namespace TeamsBotApi.UiHelpers
{
    public class ListBuilder
    {
        private readonly List<string> entries;

        public ListBuilder(IEnumerable<string> text)
        {
            entries = text.ToList();
        }

        public static implicit operator AdaptiveRichTextBlock(ListBuilder listBuilder)
        {
            return new AdaptiveRichTextBlock
            {
                Inlines =
                {
                    new AdaptiveTextRun(string.Join('\r', listBuilder.entries.Select((t, i) => $"{i + 1}. {t}")))
                }
            };
        }
    }
}