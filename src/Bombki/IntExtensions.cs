public static class IntExtensions {
    extension(int value) {
        public int Clamp(int min, int max) =>
            Math.Clamp(value, min, max);
    }
}
