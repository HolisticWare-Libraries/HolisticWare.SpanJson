﻿using CuteAnt;
using SpanJson.Document;
using SpanJson.Internal;
using SpanJson.Linq;
using SpanJson.Serialization;
using Xunit;

namespace SpanJson.Tests
{
    public class PolymorphicallyTests : TestFixtureBase
    {
        [Fact]
        public void GetElementType()
        {
            var type = typeof(Dictionary<int, string>);
            var elementType = JsonClassInfo.GetElementType(type, null, null);
            Assert.Equal(typeof(string), elementType);
            elementType = JsonClassInfo.GetElementType(typeof(byte[]), null, null);
            Assert.Equal(typeof(byte), elementType);
        }

        [Fact]
        public void Utf16PolymorphicProperties_Err()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };
            var utf16Json = JsonSerializer.Generic.Utf16.Serialize(drawing);
            Assert.Throws<JsonParserException>(() => JsonSerializer.Generic.Utf16.Deserialize<Drawing>(utf16Json));
        }

        [Fact]
        public void Utf16PolymorphicProperties()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };
            var utf16Json = JsonComplexSerializer.Default.SerializeObject(drawing);
            var deserialized = JsonComplexSerializer.Default.Deserialize<Drawing>(utf16Json);
            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);
        }

        [Fact]
        public void Utf8PolymorphicProperties_Err()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };
            var utf8Json = JsonSerializer.Generic.Utf8.Serialize(drawing);
            Assert.Throws<JsonParserException>(() => JsonSerializer.Generic.Utf8.Deserialize<Drawing>(utf8Json));
        }

        [Fact]
        public void Utf8PolymorphicProperties()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };
            var utf8Json = JsonComplexSerializer.Default.SerializeObjectToUtf8Bytes(drawing);
            var deserialized = JsonComplexSerializer.Default.Deserialize<Drawing>(utf8Json);
            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);
        }

        [Fact]
        public void PolymorphicLinq_Err()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };
            var jObj = JObject.FromObject(drawing);
            Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => jObj.ToObject<Drawing>());
        }

        [Fact]
        public void PolymorphicLinq()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };
            var jObj = JObject.FromPolymorphicObject(drawing);
            var deserialized = jObj.ToPolymorphicObject<Drawing>();
            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);
        }

        [Fact]
        public void Utf16PolymorphicLinq()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };

            var jsPool = JToken.PolymorphicSerializerPool;
            var js = jsPool.Take();
            var utf16Json = Newtonsoft.Json.Linq.JObject.FromObject(drawing, js).ToString();
            jsPool.Return(js);

            var jObj = JObject.Parse(utf16Json);
            var deserialized = jObj.ToPolymorphicObject<Drawing>();

            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);

            jObj = JObject.FromObject(JsonSerializer.Generic.Utf16.Deserialize<dynamic>(utf16Json.ToCharArray()));
            deserialized = jObj.ToPolymorphicObject<Drawing>();

            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);
        }

        [Fact]
        public void Utf8PolymorphicLinq()
        {
            var drawing = new Drawing
            {
                Shapes = new Shape[]
                {
                    new Square { Size = 10 },
                    new Square { Size = 20 },
                    new Circle { Radius = 5 }
                }
            };

            var jsPool = JToken.PolymorphicSerializerPool;
            var js = jsPool.Take();
            var utf16Json = Newtonsoft.Json.Linq.JObject.FromObject(drawing, js).ToString();
            var utf8Json = TextEncodings.UTF8NoBOM.GetBytes(utf16Json);
            jsPool.Return(js);

            var jObj = JObject.Parse(utf8Json);
            var deserialized = jObj.ToPolymorphicObject<Drawing>();

            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);

            jObj = JObject.FromObject(JsonSerializer.Generic.Utf8.Deserialize<dynamic>(utf8Json));
            deserialized = jObj.ToPolymorphicObject<Drawing>();

            Assert.NotNull(deserialized);
            Assert.Equal(3, deserialized.Shapes.Count);
            Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
            Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
            Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
            Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
            Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
            Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);

            using (var doc = JsonDocument.Parse(utf8Json))
            {
                jObj = JObject.FromObject(doc);
                deserialized = jObj.ToPolymorphicObject<Drawing>();

                Assert.NotNull(deserialized);
                Assert.Equal(3, deserialized.Shapes.Count);
                Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
                Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
                Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
                Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
                Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
                Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);
            }

            using (var doc = JsonDocument.Parse(utf8Json))
            {
                jObj = JObject.FromObject(doc.RootElement);
                deserialized = jObj.ToPolymorphicObject<Drawing>();

                Assert.NotNull(deserialized);
                Assert.Equal(3, deserialized.Shapes.Count);
                Assert.Equal(typeof(Square), deserialized.Shapes[0].GetType());
                Assert.Equal(10, ((Square)deserialized.Shapes[0]).Size);
                Assert.Equal(typeof(Square), deserialized.Shapes[1].GetType());
                Assert.Equal(20, ((Square)deserialized.Shapes[1]).Size);
                Assert.Equal(typeof(Circle), deserialized.Shapes[2].GetType());
                Assert.Equal(5, ((Circle)deserialized.Shapes[2]).Radius);
            }
        }

        [Fact]
        public static void IsPolymorphically()
        {
            Assert.False(JsonMetadata.IsPolymorphic<int>());
            Assert.False(JsonMetadata.IsPolymorphic<Uri>());
            Assert.False(JsonMetadata.IsPolymorphic<Guid>());
            Assert.False(JsonMetadata.IsPolymorphic<CombGuid>());
            Assert.False(JsonMetadata.IsPolymorphic<LayerSettings>());
            Assert.False(JsonMetadata.IsPolymorphic<IList<string>>());
        }

        public class LayerSettings
        {
            public IList<string> Roles { get; set; }
            public string[] Zones { get; set; }
        }

        public class Drawing
        {
            public Drawing()
            {
                Shapes = new List<Shape>();
            }

            [JsonPolymorphism]
            public IList<Shape> Shapes { get; set; }
        }

        public abstract class Shape
        {
            public int Id { get; set; }
        }

        public class Square : Shape
        {
            public int Size { get; set; }
        }

        public class Circle : Shape
        {
            public int Radius { get; set; }
        }
    }
}
