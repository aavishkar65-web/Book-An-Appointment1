using Newtonsoft.Json;

namespace Book_An_Appointment1.Model
{
    public class Appoints
    {
    }
    public class FacilityOutput
    {
        //[JsonProperty("status")]
        //public bool Success { get; set; }

        //[JsonProperty("message")]
        //public string Message { get; set; }

        //[JsonProperty("data")]
        //public List<FacilityItem> Data { get; set; } = new();

        [JsonProperty("data")]
        public List<FacilityItem> Data { get; set; } = new();

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class FacilityItem
    {
        //public int FacilityId { get; set; }
        //public string FacilityName { get; set; }
        //public string FacilityCode { get; set; }
        //public string HospitalLocation { get; set; }


        [JsonProperty("facilityId")]
        public int FacilityId { get; set; }

        [JsonProperty("name")]   // ✅ FIX
        public string FacilityName { get; set; }

        [JsonProperty("facilityCode")]
        public string FacilityCode { get; set; }

        [JsonProperty("hospitalLocationId")]  // ✅ FIX
        public int HospitalLocationId { get; set; }
    }
}
