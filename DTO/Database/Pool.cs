namespace VmProjectBFF.DTO.Database
{
    public class Pool
    {
        // Primary Key
        public int PoolId { get; set; }

        // Not Null
        public int CanvasPoolId { get; set; }

        // Not Null
        public string PoolName { get; set; }

        // Not Null
        // Links to Section
        public int SectionId { get; set; }

        // Not Null
        // Links to Folder
        public int FolderId { get; set; }
    }
}
