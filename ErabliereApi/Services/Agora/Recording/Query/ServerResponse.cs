namespace ErabliereApi.Services.Agora.Recording.Query
{
    public class ServerResponse
    {
        public List<File> fileList { get; set; }
        public ushort status { get; set; }
        public DateTime sliceStartTime { get; set; }
    }
}
