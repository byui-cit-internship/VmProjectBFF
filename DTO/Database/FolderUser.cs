namespace VmProjectBFF.DTO.Database
{
    public class FolderUser
    {
        // Primary Key
        public int FolderUserId { get; set; }

        // Not Null
        // Links to Folder
        public int FolderId { get; set; }

        // Not Null
        // Links to User
        public int UserId { get; set; }
    }
}
