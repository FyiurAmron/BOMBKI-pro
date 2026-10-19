public static class History {
    public const int Limit = 100;
    public static readonly List<string> Lines = [];
    static int _index;
    static string _draft = "";

    public static void Begin() {
        _index = Lines.Count;
        _draft = "";
    }

    public static void Remember(string line) {
        if (string.IsNullOrWhiteSpace(line)) {
            return;
        }

        if (Lines.Count == Limit) {
            Lines.RemoveAt(0);
        }

        Lines.Add(line);
    }

    public static string Up(string current) {
        if (!Lines.Any()) {
            return current;
        }

        if (_index == Lines.Count) {
            _draft = current;
        }

        if (_index > 0) {
            _index--;
        }

        return Lines[_index];
    }

    public static string Down(string current) {
        if (_index >= Lines.Count) {
            return current;
        }

        _index++;
        return _index < Lines.Count ? Lines[_index] : _draft;
    }
}
