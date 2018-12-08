namespace AlpineClubBansko.Services.Extensions
{
    public static class StringExtensions
    {
        public static string StorySubstring(this string word, int count = 200)
        {
            var start = word.Substring(0, count);
            var index = word.Substring(count).IndexOf(" ");
            var end = word.Substring(count, index);

            return $"{start}{end}...";
        }
    }
}