using UnityEngine;


public static class ExtensionsColor
{
    public static Color Randomize(this ref Color self, float saturationMin, float brightnessMin, float brightnessMax)
    {
        float saturation, brightness;
        do
        {
            for (int i = 0; i < 3; i++)
                self[i] = Random.Range(0, 256) / 255f;

            brightness = self.maxColorComponent;
            saturation = brightness == 0 ? 0 : (1f - Mathf.Min(self.r, self.g, self.b) / brightness);
        }
        while (saturation <= saturationMin || brightness <= brightnessMin || brightness > brightnessMax);

        self.a = 1f;

        return self;
    }

    public static bool IsSimilar(this Color self, Color other, float variance = 0.025f) => Mathf.Abs(self.r - other.r) < variance && Mathf.Abs(self.g - other.g) < variance && Mathf.Abs(self.b - other.b) < variance;

    public static Color Brightness(this Color self, float brightness)
    {
        for (int i = 0; i < 3; i++)
            self[i] = Mathf.Clamp01(self[i] * brightness);

        self.a = 1f;
        return self;
    }

    public static Color SetAlpha(this Color self, float alpha)
    {
        self.a = alpha;
        return self;
    }
}
