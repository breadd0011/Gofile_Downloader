using System.Text.Json.Serialization;

namespace GofileDownloader.Models
{
    public class GofileDataModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public GofileData Data { get; set; }
        public class GofileData
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("createTime")]
            public int CreateTime { get; set; }

            [JsonPropertyName("totalDownloadCount")]
            public int TotalDownloadCount { get; set; }

            [JsonPropertyName("totalSize")]
            public int TotalSize { get; set; }

            [JsonPropertyName("childrenIds")]
            public List<string> ChildrenIds { get; set; }

            [JsonPropertyName("children")]
            public Dictionary<string, Child> Children { get; set; }
        }
        public class Child
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("parentFolder")]
            public string ParentFolder { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("createTime")]
            public int CreateTime { get; set; }

            [JsonPropertyName("size")]
            public int Size { get; set; }

            [JsonPropertyName("downloadCount")]
            public int DownloadCount { get; set; }

            [JsonPropertyName("md5")]
            public string Md5 { get; set; }

            [JsonPropertyName("mimetype")]
            public string Mimetype { get; set; }

            [JsonPropertyName("servers")]
            public List<string> Servers { get; set; }

            [JsonPropertyName("serverSelected")]
            public string ServerSelected { get; set; }

            [JsonPropertyName("link")]
            public string Link { get; set; }

            [JsonPropertyName("thumbnail")]
            public string Thumbnail { get; set; }
        }
    }
}
