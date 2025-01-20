using System.Text;

static string Rot13(string message)
{
    char[] chars = message.ToCharArray();
    StringBuilder sb = new StringBuilder();
    foreach (var c in chars)
    {
        if ('a' <= c && c <= 'z') sb.Append(Rot('a', 'z', c, 13));
        else if ('A' <= c && c <= 'Z') sb.Append(Rot('A', 'Z', c, 13));
        else sb.Append(c);
    }

    return sb.ToString();
}

static char Rot(char min, char max, char current, int offset)
{
    int withOffset = current + offset;
    if (withOffset <= max)
    {
        return (char)withOffset;
    }

    return (char)(min + withOffset - max -1);
}

Console.WriteLine(Rot13("test"));