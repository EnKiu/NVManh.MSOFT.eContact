using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Picture: BaseEntity
    {
        public Guid? PictureId { get; set; }
        public string? Description { get; set; }
        public string? UrlPath { get; set; }
        public Guid? AlbumId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

    }
}
