using Newtonsoft.Json;
using VmProjectBFF.DTO.VCenter;

namespace VmProjectBFF.Services
{
    public interface IVCenterRepository
    {
        public IVCenterHttpClient VCenterHttpClient { get; }

        // GET's
        public List<ContentLibrary> GetContentLibraries();
        public List<string> GetContentLibraryIds();
        public ContentLibrary GetContentLibraryById(string contentLibraryId);
        public List<Folder> GetFolders();
        public List<VmTemplate> GetTemplatesByContentLibraryId(string contentLibraryId);
        public List<string> GetTemplateIdsInContentLibrary(string contentLibraryId);
        public VmTemplate GetTemplateByVCenterId(string vCenterId);
        public List<Pool> GetResourceGroups();

        // POST's
        public string NewVmInstanceByTemplateId(
            string vCenterTemplateId,
            Deploy deploy);
    }
}
