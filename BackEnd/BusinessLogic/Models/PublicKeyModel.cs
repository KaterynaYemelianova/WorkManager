using Newtonsoft.Json;

using System;
using System.Text.RegularExpressions;

namespace BusinessLogic.Models
{
    public class PublicKeyModel : IEquatable<PublicKeyModel>
    {
        private Regex ModulusPattern = new Regex("<Modulus>(.+?)</Modulus>");
        private Regex ExponentPattern = new Regex("<Exponent>(.+?)</Exponent>");

        [JsonProperty("modulus")]
        public string Modulus { get; private set; }

        [JsonProperty("exponent")]
        public string Exponent { get; private set; }

        public PublicKeyModel(string xml)
        {
            Modulus = ModulusPattern.Match(xml).Groups[1].Value;
            Exponent = ExponentPattern.Match(xml).Groups[1].Value;
        }

        public PublicKeyModel(string modulus, string exponent)
        {
            Modulus = modulus;
            Exponent = exponent;
        }

        public override int GetHashCode()
        {
            return Modulus.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PublicKeyModel);
        }

        public bool Equals(PublicKeyModel other)
        {
            return other != null && Modulus == other.Modulus && Exponent == other.Exponent;
        }
    }
}
