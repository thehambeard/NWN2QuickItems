using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;

namespace NWN2QuickItems.Utility
{
    public class GradientConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Gradient gradient = (Gradient)value;
            var colorKeys = gradient.colorKeys.Select(k => new { k.time, k.color.r, k.color.g, k.color.b, k.color.a }).ToArray();
            var alphaKeys = gradient.alphaKeys.Select(k => new { k.time, k.alpha }).ToArray();

            serializer.Serialize(writer, new { colorKeys, alphaKeys });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = serializer.Deserialize<dynamic>(reader);
            Gradient gradient = new Gradient();

            gradient.colorKeys = obj.colorKeys.ToObject<GradientColorKey[]>();
            gradient.alphaKeys = obj.alphaKeys.ToObject<GradientAlphaKey[]>();

            return gradient;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(Gradient);
    }

    public class MinMaxGradientConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var minMaxGradient = (ParticleSystem.MinMaxGradient)value;

            serializer.Serialize(writer, new
            {
                mode = minMaxGradient.mode.ToString(),
                color = minMaxGradient.color,
                colorMax = minMaxGradient.colorMax,
                colorMin = minMaxGradient.colorMin,
                gradient = minMaxGradient.gradient != null ? SerializeGradient(minMaxGradient.gradient) : null,
                gradientMax = minMaxGradient.gradientMax != null ? SerializeGradient(minMaxGradient.gradientMax) : null,
                gradientMin = minMaxGradient.gradientMin != null ? SerializeGradient(minMaxGradient.gradientMin) : null
            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = serializer.Deserialize<dynamic>(reader);
            var minMaxGradient = new ParticleSystem.MinMaxGradient();

            switch ((string)obj.mode)
            {
                case "Color":
                    minMaxGradient = new ParticleSystem.MinMaxGradient(obj.color.ToObject<Color>());
                    break;
                case "Gradient":
                    minMaxGradient = new ParticleSystem.MinMaxGradient(DeserializeGradient(obj.gradient));
                    break;
                case "TwoColors":
                    minMaxGradient = new ParticleSystem.MinMaxGradient(obj.colorMin.ToObject<Color>(), obj.colorMax.ToObject<Color>());
                    break;
                case "TwoGradients":
                    minMaxGradient = new ParticleSystem.MinMaxGradient(DeserializeGradient(obj.gradientMin), DeserializeGradient(obj.gradientMax));
                    break;
            }

            return minMaxGradient;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ParticleSystem.MinMaxGradient);
        }

        private object SerializeGradient(Gradient gradient)
        {
            return new
            {
                colorKeys = gradient.colorKeys.Select(k => new { k.time, k.color.r, k.color.g, k.color.b, k.color.a }).ToArray(),
                alphaKeys = gradient.alphaKeys.Select(k => new { k.time, k.alpha }).ToArray()
            };
        }

        private Gradient DeserializeGradient(dynamic obj)
        {
            Gradient gradient = new Gradient();
            gradient.colorKeys = obj.colorKeys.ToObject<GradientColorKey[]>();
            gradient.alphaKeys = obj.alphaKeys.ToObject<GradientAlphaKey[]>();
            return gradient;
        }
    }
}
