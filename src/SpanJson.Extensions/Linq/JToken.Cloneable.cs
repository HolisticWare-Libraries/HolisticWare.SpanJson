namespace SpanJson.Linq
{
    partial class JToken : ICloneable
    {
        object ICloneable.Clone()
        {
            return DeepClone();
        }

        /// <summary>Creates a new instance of the <see cref="JToken"/>. All child tokens are recursively cloned.</summary>
        /// <returns>A new instance of the <see cref="JToken"/>.</returns>
        public JToken DeepClone()
        {
            return CloneToken(settings: null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="JToken"/>. All child tokens are recursively cloned.
        /// </summary>
        /// <param name="settings">A <see cref="JsonCloneSettings"/> object to configure cloning settings.</param>
        /// <returns>A new instance of the <see cref="JToken"/>.</returns>
        public JToken DeepClone(JsonCloneSettings settings)
        {
            return CloneToken(settings);
        }

        internal abstract JToken CloneToken(JsonCloneSettings? settings);

        /// <summary>Compares the values of two tokens, including the values of all descendant tokens.</summary>
        /// <param name="t1">The first <see cref="JToken"/> to compare.</param>
        /// <param name="t2">The second <see cref="JToken"/> to compare.</param>
        /// <returns><c>true</c> if the tokens are equal; otherwise <c>false</c>.</returns>
        public static bool DeepEquals(JToken? t1, JToken? t2)
        {
            return (t1 == t2 || (t1 is not null && t2 is not null && t1.DeepEquals(t2)));
        }

        internal abstract bool DeepEquals(JToken node);
    }
}