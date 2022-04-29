namespace vmProjectBFF.DTO
{
    public class Folder
    {
        // Primary Key
        public int FolderId { get; set; }
        
        // Not Null
        public string VcenterFolderId { get; set; }
        
        // Nullable
        public string FolderDescription { get; set; }
    }
}
