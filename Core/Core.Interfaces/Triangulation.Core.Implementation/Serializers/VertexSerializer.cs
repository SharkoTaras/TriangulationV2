using System;
using System.Text.RegularExpressions;
using Triangulation.Core.Implementation.Extensions;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Utils;

namespace Triangulation.Core.Implementation.Serializers
{
    public class VertexSerializer : ISerializer<Vertex>
    {
        public Vertex Deserialize(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }

            var numbers = new Regex(@"\s*(\s*\d\s*,\s*\d\s*)\s*").Match(str).Value.Split(',');
            double x;
            double y;
            try
            {
                x = numbers[0].ToDouble();
                y = numbers[1].ToDouble();
            }
            catch (ArgumentNullException e)
            {
                throw new FormatException("String has incorrect format.\nShould be (x,y)", e);
            }

            return new Vertex(x, y);
        }

        public string Serialize(Vertex obj)
        {
            if (obj is null)
            {
                return null;
            }

            return $"({obj.X},{obj.Y})";
        }
    }
}
