using System.Collections.Generic;

static class ListExtensions {
    public static string JoinToString(this List<string> items) =>
        string.Join(", ", items);
}
