namespace EduwardPost.AzureAd.K8s.Auth.SideCar.Extensions {
    public class AzureAdOptions {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
    }
}