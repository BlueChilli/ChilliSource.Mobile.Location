namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// Identifies each section in the description of a returned <see cref="Prediction"/>. 
    /// A section is generally terminated with a comma.
    /// </summary>
    public class Term
    {
        /// <summary>
        /// Start position of this term in the description
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Text of the term
        /// </summary>
        public string Value { get; set; }
    }
}
