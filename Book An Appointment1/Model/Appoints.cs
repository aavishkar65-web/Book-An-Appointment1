namespace Book_An_Appointment1.Model
{
    public class Appoints
    {
    }
    public class FacilityOutput
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<FacilityItem> Data { get; set; } = new();
    }

    public class FacilityItem
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string HospitalLocation { get; set; }
    }
}
