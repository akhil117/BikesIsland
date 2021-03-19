namespace BikesIsland.Configurations.Models
{
    public class Configure
    {
        public CosmosDbSettings CosmosDbSettings { get; set; }
        public BlobStorageSettings BlobStorageSettings { get; set; }

        public ApplicationInsights ApplicationInsights { get; set; }
    }

    public class ApplicationInsights
    {
      public string InstrumentationKey { get; set; }
    }

    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }

    public class CosmosDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BikeContainerName { get; set; }
        public string EnquiryContainerName { get; set; }
        public string BikePartitionKeyPath { get; set; }
        public string EnquiryPartitionKeyPath {get; set;}
        public string BikeReservationContainerName { get; set;}
        public string BikeReservationPartitionKeyPath { get; set; }
    }
}
