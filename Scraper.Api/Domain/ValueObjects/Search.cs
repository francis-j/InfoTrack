using Domain.Types;

namespace Domain.ValueObjects;

public class Search(string keywords, string url)
{
    public string Keywords { get; } = keywords;
    public string Url { get; } = url;
    public DateTime CreatedDate { get; } = DateTime.UtcNow;
    public string Result { get; private set; } = "";

    public void SetResult(string result)
    {
        Result = result;
    }
}