using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Album: BaseEntity
    {
        public Guid AlbumId { get; set; }
        public string? AlbumName { get; set; }
        public string? Description { get; set; }
        public string? AvatarLink { get; set; }
        public Guid? AuthId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public IFormFileCollection? PictureFiles { get; set; }
        public IList<Picture> Pictures { get; set; }
        public int TotalPictures { get; set; }
        public int TotalViews { get; set; }
    }
}
