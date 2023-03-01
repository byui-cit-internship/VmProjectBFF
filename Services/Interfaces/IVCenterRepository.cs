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
        public VmTemplateMetadata GetTemplateMetadata(string itemId);
        public ContentLibrary GetContentLibraryById(string contentLibraryId);
        public List<Folder> GetFolders();
        public List<VmTemplate> GetTemplatesByContentLibraryId(string contentLibraryId);
        public List<string> GetTemplateIdsInContentLibrary(string contentLibraryId);
        public VmTemplate GetTemplateByVCenterId(string vCenterId);
        public List<Pool> GetResourcePools();
        public List<VmNetworkInfo> GetVmNetworkInfo(string vmInstanceVcenterId);

        // POST's
        public NewVmInstance NewVmInstanceByTemplateId(
            string vCenterTemplateId,
            DeployContainer deployContainer);

        public void StartVm (string vmInstanceId);
        
    }
}
