namespace PathLabAPI.Dto
{
    public class ReportDto
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }= string.Empty;          
        public string DoctorName { get; set; }= string.Empty;
        public string TestName { get; set; } = string.Empty;
    }
}
