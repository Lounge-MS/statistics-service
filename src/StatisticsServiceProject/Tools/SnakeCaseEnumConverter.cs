using System.Text;

namespace StatisticsServiceProject.Tools;

public class SnakeCaseEnumConverter : IEnumConverter
{
    public string ConvertToString(Enum enumerable)
    {
        string text = enumerable.ToString();

        if (text.Length < 2)
            return text;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for (int i = 1; i < text.Length; ++i)
        {
            char c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}