#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Location.Google.Maps.Directions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using ChilliSource.Mobile.Location.Google;
using Newtonsoft.Json.Converters;

namespace ChilliSource.Mobile.Location
{
    /// <summary>
    /// A step is the most atomic unit of a direction's route, 
    /// containing a single step describing a specific, single 
    /// instruction on the journey. E.g. "Turn left at W. 4th St." 
    /// The step not only describes the instruction but also 
    /// contains distance and duration information relating 
    /// to how this step relates to the following step
    /// </summary>
	public class Step
	{
		private string _instructionsProperty;

        /// <summary>
        /// Distance covered by the step as a value in meteres and a human-readable text representation
        /// </summary>
		public TextValue Distance { get; set; }

        /// <summary>
        /// Time needed to travel the step as a value in seconds and a human-readable text representation
        /// </summary>
        public TextValue Duration { get; set; }

        /// <summary>
        /// Location of starting point of this step
        /// </summary>
		[JsonProperty("start_location")]
        public GoogleCoordinate StartLocation { get; set; }

        /// <summary>
        /// Location of the end point of this step
        /// </summary>
        [JsonProperty("end_location")]
		public GoogleCoordinate EndLocation { get; set; }

        /// <summary>
        /// Type of travel, e.g. driving, walking, transit
        /// </summary>
		[JsonProperty("travel_mode")]
        [JsonConverter(typeof(StringEnumConverter))]                
        public TravelMode TravelMode { get; set; }

        /// <summary>
        /// Formatted instructions for this step, presented as an HTML text string
        /// </summary>
		[JsonProperty("html_instructions")]
		public string Instructions
		{
			get
			{
				return _instructionsProperty;
			}
			set
			{
				//Decode HTML String
				_instructionsProperty = Regex.Replace(value, "<[^>]+>", String.Empty);
			}
		}
	}
}
