namespace VmProjectBFF.DTO.Database
{
    public class Group
    {
        // Primary Key
        public int GroupId { get; set; }

        // Not Null
        public int CanvasGroupId { get; set; }

        // Not Null
        public string GroupName { get; set; }

        // Not Null
        // Links to Section
        public int SectionId { get; set; }

        // Not Null
        // Links to Folder
        public int FolderId { get; set; }
    }
}
