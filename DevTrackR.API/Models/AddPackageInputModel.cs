namespace DevTrackR.API.Models
{
    public class AddPackageInputModel
    {
        public string Title { get; set; }
        public decimal Weight { get; private set; }  
        public string SenderName {get; set;}  
        public string SenderEmail { get; set; }
        
        
    }
}