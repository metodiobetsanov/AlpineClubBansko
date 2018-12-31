namespace AlpineClubBansko.Services.Extensions
{
    public static class StringExtensions
    {
        public static string StorySubstring(this string word)
        {
            var count = 150;

            if (word.Length > (count + 50))
            {
                var start = word.Substring(0, count);
                var index = word.Substring(count).IndexOf(" ");
                var end = word.Substring(count, index);

                return $"{start}{end}...";
            }

            return word;
        }

        public static string StorySubstring(this string word, int count)
        {
            if (word.Length > (count + 50))
            {
                var start = word.Substring(0, count);
                var index = word.Substring(count).IndexOf(" ");
                var end = word.Substring(count, index);

                return $"{start}{end}...";
            }

            return word;
        }
    }
}