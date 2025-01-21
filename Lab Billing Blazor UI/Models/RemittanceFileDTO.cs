public class RemittanceFileDTO
{
    public int RemittanceId { get; set; }
    public string FileName { get; set; }
    public DateTime ProcessedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }
}
