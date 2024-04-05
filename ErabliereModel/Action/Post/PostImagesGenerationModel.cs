using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Post;


public class PostImagesGenerationModel
{
    public int? ImageCount { get; set; }
    public string Prompt { get; set; }
    public string? Size { get; set; }
}
